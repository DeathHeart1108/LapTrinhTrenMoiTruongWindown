using BLL.Service;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DuAnDauDoi
{
    public partial class FormSua : Form
    {
        private Ban _table = null;
        private Mon _selectedMon;
        private Hoadon _currentHoadon;
        private readonly MONservice _monService = new MONservice();
        private readonly HOADONservice _hoadonService = new HOADONservice();

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
            LoadComboBoxLoaiMon();
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
                    Tag = mon,
                    TextAlign = ContentAlignment.MiddleCenter,
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

                _selectedMon = _monService.GetById(mamonId);

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
            _currentHoadon = _hoadonService.GetUnpaidInvoiceByTable(_table.Maban);

            if (_currentHoadon == null) return;

            foreach (var cthd in _currentHoadon.Cthds)
            {
                var mon = cthd.Mon;
                decimal unitPrice = mon?.Giamon ?? 0;
                int idx = dgvMon.Rows.Add(mon?.Tenmon, cthd.Sl, unitPrice * (cthd.Sl ?? 0));
                dgvMon.Rows[idx].Tag = cthd; // Lưu Cthd cũ để giữ liên kết
            }
        }

        private void BtnXacnhan_Click(object sender, EventArgs e)
        {
            var validRows = dgvMon.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).ToList();

            try
            {
                if (_currentHoadon == null) return;

                var items = new List<Tuple<int, int>>();
                foreach (var row in validRows)
                {
                    int mamonId = GetMamonFromTag(row.Tag);
                    int sl = Convert.ToInt32(row.Cells["ColSl"].Value);
                    items.Add(new Tuple<int, int>(mamonId, sl));
                }

                _hoadonService.UpdateOrder(_currentHoadon.Mahd, _table.Maban, items);

                MessageBox.Show("Cập nhật thành công!", "Thông báo");
                this.DialogResult = DialogResult.OK;
                this.Close();
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