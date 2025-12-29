using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DuAnDauDoi.Models; // Đảm bảo namespace này đúng với dự án của bạn

namespace DuAnDauDoi
{
    public partial class FormSua : Form
    {
        private Ban _table = null;
        private Mon _selectedMon;
        private Hoadon _currentHoadon;

        public FormSua()
        {
            InitializeComponent();
            btnHuy.Click += (s, e) => Close();
        }

        public FormSua(Ban table) : this()
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            lbBAN.Text = $"Số Bàn: {_table.Soban}";

            // Đăng ký sự kiện
            btnXacnhan.Click += BtnXacnhan_Click;
            lbSL.Click += lbSL_Click; 
            dgvMon.CellClick += dgvMon_CellClick;
            cboLoaimon.SelectedIndexChanged += CboLoaimon_SelectedIndexChanged;

            // Load dữ liệu ban đầu
            LoadMonButtonsByTenLoai(null);
            LoadExistingUnpaidOrder();
        }

        // --- QUẢN LÝ NÚT MÓN ĂN (Click để thêm) ---

        private void CboLoaimon_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMonButtonsByTenLoai(string.IsNullOrEmpty(cboLoaimon.Text) ? null : cboLoaimon.Text);
        }

        private void LoadMonButtonsByTenLoai(string tenLoaiFilter)
        {
            flowLayoutPanel1.Controls.Clear();
            using (var context = new Model1())
            {
                var query = context.Mons.Include(m => m.MaloaiNavigation).AsQueryable();
                if (!string.IsNullOrEmpty(tenLoaiFilter))
                    query = query.Where(m => m.MaloaiNavigation.Tenloai == tenLoaiFilter);

                var mons = query.ToList();
                foreach (var mon in mons)
                {
                    var giaMonText = mon.Giamon.HasValue ? mon.Giamon.Value.ToString("N0") : "0";
                    var monButton = new Button
                    {
                        Width = 120,
                        Height = 80,
                        Margin = new Padding(8),
                        Text = $"{mon.Tenmon}\n{giaMonText} VND",
                        Tag = mon,
                        TextAlign = ContentAlignment.MiddleCenter,
                        BackColor = Color.WhiteSmoke
                    };
                    monButton.Click += MonButton_Click;
                    flowLayoutPanel1.Controls.Add(monButton);
                }
            }
        }

        private void MonButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button clickedButton)) return;
            _selectedMon = clickedButton.Tag as Mon;
            if (_selectedMon == null) return;

            var unitPrice = _selectedMon.Giamon ?? 0m;

            // Tìm món trong Grid (kiểm tra cả Tag là Mon hay Cthd)
            DataGridViewRow existingRow = dgvMon.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => !r.IsNewRow && GetMamonFromTag(r.Tag) == _selectedMon.Mamon);

            if (existingRow != null)
            {
                int currentQty = Convert.ToInt32(existingRow.Cells["ColSl"].Value);
                int newQty = currentQty + 1;
                existingRow.Cells["ColSl"].Value = newQty;
                existingRow.Cells["ColGia"].Value = unitPrice * newQty;
                txtSL.Text = newQty.ToString();
            }
            else
            {
                int idx = dgvMon.Rows.Add(_selectedMon.Tenmon, 1, unitPrice);
                dgvMon.Rows[idx].Tag = _selectedMon;
                txtSL.Text = "1";
            }

            HighlightSelectedMonButton(_selectedMon.Mamon);
        }

        // --- XỬ LÝ TRÊN GRID ---

        private void dgvMon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMon.Rows[e.RowIndex];
                int mamonId = GetMamonFromTag(row.Tag);

                using (var db = new Model1())
                {
                    _selectedMon = db.Mons.Find(mamonId);
                }

                if (_selectedMon != null)
                {
                    txtSL.Text = row.Cells["ColSl"].Value.ToString();
                    HighlightSelectedMonButton(_selectedMon.Mamon);
                    txtSL.Focus();
                    txtSL.SelectAll();
                }
            }
        }

        private void lbSL_Click(object sender, EventArgs e) // Cập nhật số lượng thủ công
        {
            if (_selectedMon == null || dgvMon.CurrentRow == null) return;

            if (int.TryParse(txtSL.Text.Trim(), out var newQty))
            {
                if (newQty <= 0)
                {
                    dgvMon.Rows.Remove(dgvMon.CurrentRow);
                    ResetSelection();
                }
                else
                {
                    var unitPrice = _selectedMon.Giamon ?? 0m;
                    dgvMon.CurrentRow.Cells["ColSl"].Value = newQty;
                    dgvMon.CurrentRow.Cells["ColGia"].Value = unitPrice * newQty;
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMon.CurrentRow == null || dgvMon.CurrentRow.IsNewRow) return;
            string tenMon = dgvMon.CurrentRow.Cells[0].Value?.ToString();
                dgvMon.Rows.Remove(dgvMon.CurrentRow);
                ResetSelection();
        }

        // --- DỮ LIỆU DATABASE ---

        private void LoadExistingUnpaidOrder()
        {
            dgvMon.Rows.Clear();
            using (var context = new Model1())
            {
                // Tìm hóa đơn chưa thanh toán của bàn này
                var hoadon = context.Hoadons
                    .Include(h => h.Cthds.Select(ct => ct.MamonNavigation))
                    .Where(h => h.Maban == _table.Maban && h.Status == 0)
                    .OrderByDescending(h => h.Ngaylap)
                    .FirstOrDefault();

                if (hoadon == null) return;
                _currentHoadon = hoadon;

                foreach (var cthd in hoadon.Cthds)
                {
                    var mon = cthd.MamonNavigation;
                    decimal unitPrice = mon?.Giamon ?? 0;
                    int idx = dgvMon.Rows.Add(mon?.Tenmon, cthd.Sl, unitPrice * (cthd.Sl ?? 0));
                    dgvMon.Rows[idx].Tag = cthd; // Lưu Cthd cũ để giữ liên kết
                }
            }
        }

        private void BtnXacnhan_Click(object sender, EventArgs e)
        {
            var validRows = dgvMon.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();

            try
            {
                using (var context = new Model1())
                {
                    if (_currentHoadon == null) return;

                    var hoadonInDb = context.Hoadons.Include(h => h.Cthds).FirstOrDefault(h => h.Mahd == _currentHoadon.Mahd);
                    if (hoadonInDb == null) return;

                    // 1. Xóa toàn bộ chi tiết cũ (không đổi mã hóa đơn)
                    var oldCthds = context.Cthds.Where(c => c.Mahd == hoadonInDb.Mahd).ToList();
                    context.Cthds.RemoveRange(oldCthds);

                    if (validRows.Count == 0)
                    {
                        // Nếu xóa sạch món -> Hủy luôn hóa đơn và trả bàn về trống
                        context.Hoadons.Remove(hoadonInDb);
                        var ban = context.Bans.Find(_table.Maban);
                        if (ban != null) ban.Status = "Trống";
                    }
                    else
                    {
                        // 2. Thêm lại chi tiết mới từ Grid
                        int i = 1;
                        decimal total = 0;
                        foreach (var row in validRows)
                        {
                            int mamonId = GetMamonFromTag(row.Tag);
                            int sl = Convert.ToInt32(row.Cells["ColSl"].Value);
                            var monObj = context.Mons.Find(mamonId);

                            total += (monObj?.Giamon ?? 0) * sl;

                            context.Cthds.Add(new Cthd
                            {
                                Macthd = hoadonInDb.Mahd + "-" + (i++).ToString("D2"),
                                Mahd = hoadonInDb.Mahd, // Vẫn dùng Mahd cũ
                                Mamon = mamonId,
                                Sl = sl
                            });
                        }
                        hoadonInDb.Tongtien = total;
                    }

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- HÀM TRỢ GIÚP ---

        private int GetMamonFromTag(object tag)
        {
            if (tag is Cthd c) return c.Mamon;
            if (tag is Mon m) return m.Mamon;
            return 0;
        }

        private void ResetSelection()
        {
            _selectedMon = null;
            txtSL.Clear();
            foreach (Control c in flowLayoutPanel1.Controls) c.BackColor = Color.WhiteSmoke;
        }

        private void HighlightSelectedMonButton(int mamonId)
        {
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is Button btn && btn.Tag is Mon m)
                    btn.BackColor = (m.Mamon == mamonId) ? Color.DodgerBlue : Color.WhiteSmoke;
            }
        }
    }
}