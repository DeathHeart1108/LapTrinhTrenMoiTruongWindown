using DuAnDauDoi.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace DuAnDauDoi
{
    public partial class FormLichsu : Form
    {
        public FormLichsu()
        {
            InitializeComponent();
            this.Load += (s, e) => {
                SetupDataGridView();
                LoadLichSuHoadon();
            };
            txtFind.TextChanged += TxtFind_TextChanged;
            btnHD.Click += BtnHD_Click;
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void LoadLichSuHoadon(string searchText = "")
        {
            try
            {
                using (var db = new Model1())
                {
                    var query = db.Hoadons.Where(h => h.Status == 1); // 1 = Đã thanh toán

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        query = query.Where(h => h.Mahd.Contains(searchText) ||
                                               h.Maban.ToString().Contains(searchText));
                    }

                    var data = query.OrderByDescending(h => h.Ngayxuat)
                        .Select(h => new
                        {
                            MaHD = h.Mahd,
                            NgayLap = h.Ngaylap,
                            NgayThanhToan = h.Ngayxuat,
                            SoBan = h.Maban,
                            TongTien = h.Tongtien,
                            GiamGia = h.KHUYENMAI_HD ?? 0
                        }).ToList();

                    dataGridView1.DataSource = data;

                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
                        dataGridView1.Columns["TongTien"].DefaultCellStyle.Format = "N0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void TxtFind_TextChanged(object sender, EventArgs e)
        {
            LoadLichSuHoadon(txtFind.Text.Trim());
        }

        private void BtnHD_Click(object sender, EventArgs e)
        {
            string maHD = dataGridView1.CurrentRow.Cells["MaHD"].Value.ToString();
            FormHoaDon frm = new FormHoaDon(maHD);
            frm.ShowDialog();
        }
    }
}