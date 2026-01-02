using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.Service;
using DAL.Entities;

namespace DuAnDauDoi
{
    public partial class FormHoaDon : Form
    {
        private Ban _table;
        private Hoadon _currentHoadon;
        private string _maHD_LichSu = null;
        private readonly HOADONservice _hoadonService = new HOADONservice();

        public FormHoaDon()
        {
            InitializeComponent();
            btnHuy.Click += (s, e) => Close();
            btnIn.Click += BtnIn_Click;
        }

        // Constructor 1: Mở từ bàn đang chọn (để thanh toán)
        public FormHoaDon(Ban table) : this()
        {
            _table = table ?? throw new ArgumentNullException(nameof(table));
            lbBAN.Text = $"Số Bàn: {_table.Soban}";
            LoadData(isHistory: false);
        }

        // Constructor 2: Mở từ FormLichsu (để xem lại)
        public FormHoaDon(string maHD) : this()
        {
            _maHD_LichSu = maHD;
            LoadData(isHistory: true);
        }

        private void LoadData(bool isHistory)
        {
            dgvMon.Rows.Clear();
            
            if (isHistory)
            {
                _currentHoadon = _hoadonService.GetById(_maHD_LichSu);
            }
            else
            {
                _currentHoadon = _hoadonService.GetUnpaidInvoiceByTable(_table.Maban);
            }

            if (_currentHoadon == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu hóa đơn!");
                return;
            }

            lbHd.Text = $"Hóa Đơn: {_currentHoadon.Mahd}";
            lbBAN.Text = $"Số Bàn: {_currentHoadon.Maban}"; // Note: Maban is int, usually we want Soban but Hoadon only has Maban foreign key. If we want Soban we need to join or load Ban. Hoadon.Ban should be loaded if we include it.

            foreach (var cthd in _currentHoadon.Cthds)
            {
                var mon = cthd.Mon; // Updated from MamonNavigation
                decimal unitPrice = mon?.Giamon ?? 0m;
                decimal lineTotal = unitPrice * (cthd.Sl ?? 0);

                dgvMon.Rows.Add(
                    mon?.Tenmon ?? $"Mã: {cthd.Mamon}",
                    cthd.Sl,
                    lineTotal
                );
            }

            lbTongTien.Text = $"Tổng Tiền: {CalculateTotal():N0} VND";
        }

        private void BtnIn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_maHD_LichSu))
            {
                this.Close();
                return;
            }
            MessageBox.Show("Đã In hóa đơn.");
            this.Close();
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (DataGridViewRow row in dgvMon.Rows)
            {
                if (row.IsNewRow) continue;
                var val = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.Name == "ColGia")?.Value
                          ?? row.Cells[2].Value;

                if (val != null && decimal.TryParse(val.ToString(), out decimal parsed))
                    total += parsed;
            }
            return total;
        }
    }
}