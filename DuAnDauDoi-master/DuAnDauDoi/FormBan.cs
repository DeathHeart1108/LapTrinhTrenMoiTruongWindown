using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.Service;
using DAL.Entities;

namespace DuAnDauDoi
{
    public partial class FormBan : Form
    {
        private Button _currentSelectedButton = null;
        private readonly BANservice _banService = new BANservice();
        private readonly Color SelectedColor = Color.SkyBlue;
        private readonly Color AvailableColor = Color.LightGray;
        private readonly Color ReservedColor = Color.Yellow;
        private readonly Color OccupiedColor = Color.Red;

        public FormBan()
        {
            InitializeComponent();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = true;
            btnGoi.Click += BtnGoi_CLick;
            btnHD.Click += BtnHD_Click;
            BtnSua.Click += BtnSua_Click;
            btnThanhToan.Click += BtnThanhToan_Click;
            this.Load += (s, e) => CreateSeats();
        }

        private void CreateSeats()
        {
            flowLayoutPanel1.Controls.Clear();
            var tables = _banService.GetAll();
            var sortedTables = tables
            .OrderBy(t => {
                int result;
                return int.TryParse(t.Soban, out result) ? result : 0;
            })
            .ToList();
            foreach (var table in tables)
            {
                Button seatButton = new Button
                {
                    Width = 100,
                    Height = 100,
                    Text = $"{table.Soban}",
                    Tag = table.Maban, 
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Margin = new Padding(10),
                    TextAlign = ContentAlignment.MiddleCenter
                };

                SetButtonColorByStatus(seatButton, table.Status);
                seatButton.Click += SeatButton_Click;
                flowLayoutPanel1.Controls.Add(seatButton);
            }
        }

        private void SetButtonColorByStatus(Button btn, string status)
        {
            if (status == "Trống")
            {
                btn.BackColor = AvailableColor;
                btn.ForeColor = Color.Black;
            }
            else if (status == "Đã đặt bàn")
            {
                btn.BackColor = ReservedColor;
                btn.ForeColor = Color.Black;
            }
            else
            {
                btn.BackColor = OccupiedColor;
                btn.ForeColor = Color.White;
            }
        }

        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Nếu nhấn lại chính bàn đang chọn -> Bỏ chọn
            if (_currentSelectedButton == clickedButton)
            {
                ResetSelection();
            }
            else
            {
                // Hoàn tác màu cho bàn cũ trước đó
                if (_currentSelectedButton != null)
                {
                    RefreshButtonAppearance(_currentSelectedButton);
                }

                _currentSelectedButton = clickedButton;
                clickedButton.BackColor = SelectedColor;

                // Cập nhật nhãn nút Đặt bàn
                int tableId = (int)clickedButton.Tag;
                var table = _banService.GetById(tableId);
                btnDb.Text = (table?.Status == "Đã đặt bàn") ? "🕛 Hủy Đặt" : "🕛 Đặt Bàn";
            }
        }

        // Cập nhật diện mạo bàn từ Database
        private void RefreshButtonAppearance(Button btn)
        {
            if (btn == null) return;
            int tableId = (int)btn.Tag;
            var table = _banService.GetById(tableId);
            if (table != null)
            {
                btn.Text = $"{table.Soban}";
                SetButtonColorByStatus(btn, table.Status);
            }
        }

        private void ResetSelection()
        {
            if (_currentSelectedButton != null)
            {
                RefreshButtonAppearance(_currentSelectedButton);
                _currentSelectedButton = null;
            }
            btnDb.Text = "🕛 Đặt Bàn";
        }


        private void btnDb_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null)
            {
                MessageBox.Show("Vui lòng chọn bàn!");
                return;
            }

            ExecuteTableAction(banToUpdate => {
                if (banToUpdate.Status == "Đã đặt bàn")
                {
                    _banService.UpdateStatus(banToUpdate.Maban, "Trống");
                    MessageBox.Show("Đã hủy đặt bàn!");
                }
                else if (banToUpdate.Status == "Trống")
                {
                    _banService.UpdateStatus(banToUpdate.Maban, "Đã đặt bàn");
                    MessageBox.Show("Đặt bàn thành công!");
                }
                else
                {
                    MessageBox.Show("Bàn đang có khách, không thể thao tác!");
                    return;
                }
                ResetSelection();
            });
        }

        private void BtnGoi_CLick(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                FormGoiMon f = new FormGoiMon(table);
                if (f.ShowDialog() == DialogResult.OK) { 
                    // Refresh UI if needed
                    RefreshButtonAppearance(_currentSelectedButton);
                }
            });
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                 if (table.Status == "Trống")
                {
                    MessageBox.Show("Bàn trống không thể sửa món!");
                    return;
                }
                FormSua f = new FormSua(table);
                f.ShowDialog();
                ResetSelection();
            });
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                if (table.Status == "Trống") return;
                FormThanhToan f = new FormThanhToan(table);
                f.ShowDialog();
                ResetSelection();
            });
        }

        private void BtnHD_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                if (table.Status == "Trống") return;
                FormHoaDon f = new FormHoaDon(table);
                f.ShowDialog();
            });
        }

        private void BtnLS_Click(object sender, EventArgs e)
        {
            FormLichsu f = new FormLichsu();
            f.ShowDialog();
        }

        private void ExecuteTableAction(Action<Ban> action)
        {
            int tableId = (int)_currentSelectedButton.Tag;
            var table = _banService.GetById(tableId);
            if (table != null)
            {
                action(table);
            }
        }

        private void btnHD_Click_1(object sender, EventArgs e)
        {

        }
    }
}