using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.Service;
using DAL.Entities;

namespace DuAnDauDoi
{
    public partial class FormThanhToan : Form
    {
        private Ban _table;
        private string _mahd;
        private decimal _total;
        private readonly HOADONservice _hoadonService = new HOADONservice();

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

            // Nếu chưa có tổng tiền, tự động lấy từ DB
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
             var hoadon = _hoadonService.GetUnpaidInvoiceByTable(_table.Maban);
             if (hoadon != null)
             {
                 _mahd = hoadon.Mahd;
                 _total = hoadon.Tongtien ?? 0;
             }
             else
             {
                 _total = 0;
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
                // Find current invoice again if missing
                if (string.IsNullOrEmpty(_mahd))
                {
                    CalculateTotalFromDatabase();
                }

                if (!string.IsNullOrEmpty(_mahd))
                {
                    _hoadonService.PayOrder(_mahd, _total);
                    
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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thanh toán: " + ex.Message);
            }
        }
    }
}