using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity; // Sử dụng EF6
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DuAnDauDoi
{
    // Map namespace đúng với Model1 của bạn
    using Ban = DuAnDauDoi.Models.Ban;
    using Cthd = DuAnDauDoi.Models.Cthd;
    using Hoadon = DuAnDauDoi.Models.Hoadon;
    using Model1 = DuAnDauDoi.Models.Model1;
    using Mon = DuAnDauDoi.Models.Mon;

    public partial class FormThanhToan : Form
    {
        private Ban _table;
        private string _mahd;
        private decimal _total;

        public FormThanhToan()
        {
            InitializeComponent();
            btnHuy.Click += (s, e) => Close();
            btnThanhToan.Click += BtnThanhToan_Click;
            txtTien.TextChanged += TxtTien_TextChanged;
        }

        public FormThanhToan(Ban table, string mahd = null, decimal total = 0m) : this()
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            _mahd = mahd;
            _total = total;

            // Nếu chưa có tổng tiền, tự động tính từ DB
            if (_total == 0m)
            {
                CalculateTotalFromDatabase();
            }

            lbBAN.Text = $"Số Bàn: {_table.Soban}";
            lbTongTien.Text = $"Tổng Tiền: {_total:N0} VND";
        }

        private void TxtTien_TextChanged(object sender, EventArgs e)
        {
            string text = txtTien.Text.Replace(",", "");
            if (decimal.TryParse(text, out decimal value))
            {
                if (txtTien.Focused)
                {
                    txtTien.Text = value.ToString("N0");
                    txtTien.SelectionStart = txtTien.Text.Length;
                }
            }
        }

        private void CalculateTotalFromDatabase()
        {
            try
            {
                using (var context = new Model1())
                {
                    // EF6: Tìm hóa đơn chưa thanh toán (Status != 1 hoặc Ngayxuat là null)
                    var hoadon = context.Hoadons
                        .Where(h => h.Maban == _table.Maban && (h.Status == 0 || h.Ngayxuat == null))
                        .OrderByDescending(h => h.Ngaylap)
                        .FirstOrDefault();

                    if (hoadon != null)
                    {
                        _mahd = hoadon.Mahd;

                        // Truy vấn tính tổng tiền trong EF6
                        var totalCalculated = (from ct in context.Cthds
                                               join m in context.Mons on ct.Mamon equals m.Mamon
                                               where ct.Mahd == hoadon.Mahd
                                               select new { ct.Sl, m.Giamon })
                                             .ToList() // Thực thi trên RAM để tránh lỗi null/casting phức tạp của SQL
                                             .Sum(item => (decimal)((item.Sl ?? 0) * (item.Giamon ?? 0)));

                        _total = totalCalculated;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tính toán tổng tiền: {ex.Message}");
                _total = 0m;
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra tiền khách đưa
            string rawTien = txtTien.Text.Replace(",", "").Replace(".", "");
            if (!decimal.TryParse(rawTien, out var given) || given <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền khách trả hợp lệ.");
                return;
            }

            // 2. Kiểm tra đủ tiền không
            if (given < _total)
            {
                MessageBox.Show("Tiền khách đưa không đủ để thanh toán.");
                return;
            }

            try
            {
                using (var context = new Model1())
                {
                    // 3. Tìm hóa đơn trong DB
                    Hoadon hoadon = null;
                    if (!string.IsNullOrEmpty(_mahd))
                        hoadon = context.Hoadons.Find(_mahd);

                    if (hoadon == null)
                    {
                        hoadon = context.Hoadons
                            .Where(h => h.Maban == _table.Maban && (h.Status == 0 || h.Ngayxuat == null))
                            .OrderByDescending(h => h.Ngaylap)
                            .FirstOrDefault();
                    }

                    if (hoadon != null)
                    {
                        // 4. Cập nhật Hóa đơn
                        hoadon.Status = 1; // Đã thanh toán
                        hoadon.Ngayxuat = DateTime.Now; // EF6 dùng DateTime
                        hoadon.Tongtien = _total;

                        // 5. Cập nhật Bàn
                        var ban = context.Bans.Find(_table.Maban);
                        if (ban != null)
                        {
                            ban.Status = "Trống"; // Đảm bảo đúng text hiển thị bàn trống
                        }

                        context.SaveChanges();

                        decimal change = given - _total;
                        MessageBox.Show($"Thanh toán thành công!\nTiền thối: {change:N0} VND", "Thông báo");

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu hóa đơn cho bàn này.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}