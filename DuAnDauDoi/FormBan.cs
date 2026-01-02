using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DuAnDauDoi.Models;

namespace DuAnDauDoi
{
    public partial class FormBan : Form
    {
        private Button _currentSelectedButton = null;

        // Bảng màu
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

            CreateSeats();
        }

        // 1. Vẽ danh sách bàn
        private void CreateSeats()
        {
            flowLayoutPanel1.Controls.Clear();
            using (var db = new Model1())
            {
                var tables = db.Bans.ToList();
                foreach (var table in tables)
                {
                    Button seatButton = new Button
                    {
                        Width = 100,
                        Height = 100,
                        Text = $"{table.Soban}",
                        Tag = table.Maban, // Lưu ID bàn
                        Font = new Font("Arial", 10, FontStyle.Bold),
                        Margin = new Padding(10),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    SetButtonColorByStatus(seatButton, table.Status);
                    seatButton.Click += SeatButton_Click;
                    flowLayoutPanel1.Controls.Add(seatButton);
                }
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
                using (var db = new Model1())
                {
                    var table = db.Bans.Find(tableId);
                    btnDb.Text = (table?.Status == "Đã đặt bàn") ? "🕛 Hủy Đặt" : "🕛 Đặt Bàn";
                }
            }
        }

        // Cập nhật diện mạo bàn từ Database
        private void RefreshButtonAppearance(Button btn)
        {
            if (btn == null) return;
            int tableId = (int)btn.Tag;
            using (var db = new Model1())
            {
                var table = db.Bans.Find(tableId);
                if (table != null)
                {
                    btn.Text = $"{table.Soban}";
                    SetButtonColorByStatus(btn, table.Status);
                }
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

        // --- CÁC CHỨC NĂNG ---

        private void btnDb_Click(object sender, EventArgs e)
        {
            // Kiểm tra ngay lập tức xem có đang chọn bàn nào không
            if (_currentSelectedButton == null)
            {
                MessageBox.Show("Vui lòng chọn bàn!");
                return;
            }

            int tableId = (int)_currentSelectedButton.Tag;

            using (var db = new Model1())
            {
                var banToUpdate = db.Bans.Find(tableId);
                if (banToUpdate != null)
                {
                    if (banToUpdate.Status == "Đã đặt bàn")
                    {
                        banToUpdate.Status = "Trống";
                        db.SaveChanges();
                        MessageBox.Show("Đã hủy đặt bàn!");
                    }
                    else if (banToUpdate.Status == "Trống")
                    {
                        banToUpdate.Status = "Đã đặt bàn";
                        db.SaveChanges();
                        MessageBox.Show("Đặt bàn thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Bàn đang có khách, không thể thao tác!");
                        return; // Thoát không gọi ResetSelection để người dùng chọn lại
                    }

                    // Chỉ reset khi thực hiện thành công
                    ResetSelection();
                }
            }
        }

        private void BtnGoi_CLick(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                FormGoiMon f = new FormGoiMon(table);
                if (f.ShowDialog() == DialogResult.OK) { /* Cập nhật nếu cần */ }
            });
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            int tableId = (int)_currentSelectedButton.Tag;
            using (var db = new Model1())
            {
                var table = db.Bans.Find(tableId);
                if (table?.Status == "Trống")
                {
                    MessageBox.Show("Bàn trống không thể sửa món!");
                    return;
                }
                FormSua f = new FormSua(table);
                f.ShowDialog();
                ResetSelection();
            }
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (_currentSelectedButton == null) { MessageBox.Show("Vui lòng chọn bàn!"); return; }
            ExecuteTableAction(table => {
                if (table.Status == "Trống") return;
                FormThanhToan f = new FormThanhToan(table);
                f.ShowDialog();
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
            using (var db = new Model1())
            {
                var table = db.Bans.Find(tableId);
                if (table != null)
                {
                    action(table);
                    ResetSelection();
                }
            }
        }

        private void btnHD_Click_1(object sender, EventArgs e)
        {

        }
    }
}