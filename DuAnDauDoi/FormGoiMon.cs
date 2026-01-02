using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DuAnDauDoi.Models;

namespace DuAnDauDoi
{
    public partial class FormGoiMon : Form
    {
        private readonly Random _random = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private Ban _table;
        private Mon _selectedMon;

        public FormGoiMon()
        {
            InitializeComponent();
        }

        public FormGoiMon(Ban table)
        {
            InitializeComponent();
            _table = table;

            // Cấu hình UI
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            lbBAN.Text = $"Số Bàn: {_table.Soban}";

            // Đăng ký sự kiện
            btnXacnhan.Click += BtnXacnhan_Click;
            btnHuy.Click += (s, e) => this.Close();
            lbSL.Click += lbSL_Click; 
            dgvMon.CellClick += dgvMon_CellClick;
            cboLoaimon.SelectedIndexChanged += CboLoaimon_SelectedIndexChanged;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                LoadMonButtonsByTenLoai(null);
            }
        }

        // --- QUẢN LÝ NÚT MÓN ĂN (FLOW LAYOUT PANEL) ---

        private void LoadMonButtonsByTenLoai(string tenLoaiFilter)
        {
            flowLayoutPanel1.Controls.Clear();
            using (var context = new Model1())
            {
                var query = context.Mons.Include(m => m.MaloaiNavigation).AsQueryable();

                if (!string.IsNullOrEmpty(tenLoaiFilter))
                {
                    query = query.Where(m => m.MaloaiNavigation.Tenloai == tenLoaiFilter);
                }

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
                        TextAlign = ContentAlignment.MiddleCenter,
                        Tag = mon,
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

            // Kiểm tra món đã có trong Grid chưa
            DataGridViewRow existingRow = dgvMon.Rows.Cast<DataGridViewRow>()
                .FirstOrDefault(r => !r.IsNewRow && (r.Tag as Mon)?.Mamon == _selectedMon.Mamon);

            if (existingRow != null)
            {
                // NẾU CÓ: Cộng thêm 1
                int currentQty = int.Parse(existingRow.Cells["ColSl"].Value.ToString());
                int newQty = currentQty + 1;
                existingRow.Cells["ColSl"].Value = newQty;
                existingRow.Cells["ColGia"].Value = unitPrice * newQty;

                txtSL.Text = newQty.ToString(); // Đồng bộ số lượng lên ô nhập
            }
            else
            {
                // NẾU CHƯA: Thêm mới dòng với SL = 1
                int rowIndex = dgvMon.Rows.Add(_selectedMon.Tenmon, 1, unitPrice);
                dgvMon.Rows[rowIndex].Tag = _selectedMon;
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
                if (row.Tag is Mon mon)
                {
                    _selectedMon = mon;
                    txtSL.Text = row.Cells["ColSl"].Value.ToString();
                    HighlightSelectedMonButton(mon.Mamon);
                    txtSL.Focus();
                    txtSL.SelectAll();
                }
            }
        }

        private void lbSL_Click(object sender, EventArgs e) // Nút xác nhận sửa số lượng
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
            else
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!");
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMon.CurrentRow == null || dgvMon.CurrentRow.IsNewRow) return;

            dgvMon.Rows.Remove(dgvMon.CurrentRow);
            ResetSelection();
        }

        // --- LƯU DATABASE ---

        private void BtnXacnhan_Click(object sender, EventArgs e)
        {
            if (!dgvMon.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
            {
                MessageBox.Show("Chưa chọn món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new Model1())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var hoadon = db.Hoadons.FirstOrDefault(h => h.Maban == _table.Maban && h.Status == 0);
                        string currentMahd;
                        bool isNewInvoice = false;

                        if (hoadon == null)
                        {
                            currentMahd = GenerateUniqueMahd(db, "HD");
                            hoadon = new Hoadon
                            {
                                Mahd = currentMahd,
                                Ngaylap = DateTime.Now,
                                Status = 0,
                                Maban = _table.Maban,
                                Tongtien = 0
                            };
                            db.Hoadons.Add(hoadon);
                            isNewInvoice = true;
                        }
                        else
                        {
                            currentMahd = hoadon.Mahd;
                        }

                        foreach (DataGridViewRow row in dgvMon.Rows)
                        {
                            if (row.IsNewRow) continue;
                            var mon = row.Tag as Mon;
                            int qtyFromGrid = int.Parse(row.Cells["ColSl"].Value.ToString());

                            var existingCthd = db.Cthds.FirstOrDefault(ct => ct.Mahd == currentMahd && ct.Mamon == mon.Mamon);

                            if (existingCthd != null)
                            {
                                existingCthd.Sl += qtyFromGrid; // Cộng dồn vào hóa đơn cũ
                            }
                            else
                            {
                                string randomSuffix = GenerateRandomString(3);
                                string maCTHD = $"CT{currentMahd.Substring(currentMahd.Length - 3)}{randomSuffix}";
                                if (maCTHD.Length > 10) maCTHD = maCTHD.Substring(0, 10);

                                db.Cthds.Add(new Cthd
                                {
                                    Macthd = maCTHD,
                                    Mahd = currentMahd,
                                    Mamon = mon.Mamon,
                                    Sl = qtyFromGrid,
                                    Khuyenmai = 0
                                });
                            }
                        }

                        db.SaveChanges();

                        // Tính lại tổng tiền
                        var allDetails = db.Cthds.Where(ct => ct.Mahd == currentMahd).ToList();
                        decimal totalAmount = 0;
                        foreach (var item in allDetails)
                        {
                            var dish = db.Mons.Find(item.Mamon);
                            totalAmount += (dish.Giamon ?? 0) * (item.Sl ?? 0);
                        }
                        hoadon.Tongtien = totalAmount;

                        var banToUpdate = db.Bans.Find(_table.Maban);
                        if (banToUpdate != null) banToUpdate.Status = "Có khách";

                        db.SaveChanges();
                        transaction.Commit();

                        MessageBox.Show("Gọi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Lỗi: {ex.Message}");
                    }
                }
            }
        }

        // --- HÀM BỔ TRỢ ---

        private void ResetSelection()
        {
            _selectedMon = null;
            txtSL.Clear();
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c is Button b) b.BackColor = Color.WhiteSmoke;
            }
        }

        private void HighlightSelectedMonButton(int mamon)
        {
            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is Button btn && btn.Tag is Mon m)
                {
                    btn.BackColor = (m.Mamon == mamon) ? Color.DodgerBlue : Color.WhiteSmoke;
                }
            }
        }

        private void CboLoaimon_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMonButtonsByTenLoai(string.IsNullOrEmpty(cboLoaimon.Text) ? null : cboLoaimon.Text);
        }

        private string GenerateRandomString(int length)
        {
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++) stringChars[i] = Chars[_random.Next(Chars.Length)];
            return new string(stringChars);
        }

        private string GenerateUniqueMahd(Model1 context, string prefix = "HD")
        {
            string newMahd;
            do
            {
                newMahd = $"{prefix}{GenerateRandomString(3)}";
            } while (context.Hoadons.Any(h => h.Mahd == newMahd));
            return newMahd;
        }
    }
}