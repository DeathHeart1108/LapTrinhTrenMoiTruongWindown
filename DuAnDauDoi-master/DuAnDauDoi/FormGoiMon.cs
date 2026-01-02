using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.Service;
using DAL.Entities;

namespace DuAnDauDoi
{
    public partial class FormGoiMon : Form
    {
        private Ban _table;
        private Mon _selectedMon;
        private readonly MONservice _monService = new MONservice();
        private readonly HOADONservice _hoadonService = new HOADONservice();

        public FormGoiMon()
        {
            InitializeComponent();
        }

        public FormGoiMon(Ban table)
        {
            InitializeComponent();
            _table = table;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            lbBAN.Text = $"Số Bàn: {_table.Soban}";
            LoadComboBoxLoaiMon();
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
            var mons = _monService.GetByLoai(tenLoaiFilter);

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


        private void BtnXacnhan_Click(object sender, EventArgs e)
        {
            if (_selectedMon != null && dgvMon.CurrentRow != null && !string.IsNullOrWhiteSpace(txtSL.Text))
            {
                if (int.TryParse(txtSL.Text.Trim(), out var newQty))
                {
                    if (newQty <= 0)
                    {
                        dgvMon.Rows.Remove(dgvMon.CurrentRow);
                    }
                    else
                    {
                        var unitPrice = _selectedMon.Giamon ?? 0m;
                        dgvMon.CurrentRow.Cells["ColSl"].Value = newQty;
                        dgvMon.CurrentRow.Cells["ColGia"].Value = unitPrice * newQty;
                    }
                    ResetSelection();
                }
            }
            if (!dgvMon.Rows.Cast<DataGridViewRow>().Any(r => !r.IsNewRow))
            {
                MessageBox.Show("Chưa có món nào trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var items = new List<Tuple<Mon, int>>();
                foreach (DataGridViewRow row in dgvMon.Rows)
                {
                    if (row.IsNewRow) continue;

                    var mon = row.Tag as Mon;
                    if (mon != null)
                    {
                        int qty = int.Parse(row.Cells["ColSl"].Value.ToString());
                        items.Add(new Tuple<Mon, int>(mon, qty));
                    }
                }

                _hoadonService.AddOrder(_table.Maban, items);

                MessageBox.Show("gọi món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
            }
        }


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
            if (cboLoaimon.SelectedIndex != -1)
            {
                string tenLoai = cboLoaimon.Text;
                LoadMonButtonsByTenLoai(tenLoai);
            }
            else
            {
                LoadMonButtonsByTenLoai(null);
            }
        }
        private void LoadComboBoxLoaiMon()
        {
            using (var db = new Model1())
            {
                var listLoai = db.Loaimons.AsNoTracking().ToList();

                listLoai.Insert(0, new Loaimon { Tenloai = "Tất cả các món", Maloai = -1 });

                cboLoaimon.DataSource = listLoai;
                cboLoaimon.DisplayMember = "Tenloai";
                cboLoaimon.ValueMember = "Maloai";

                cboLoaimon.SelectedIndex = 0; 
            }
        }
    }
}