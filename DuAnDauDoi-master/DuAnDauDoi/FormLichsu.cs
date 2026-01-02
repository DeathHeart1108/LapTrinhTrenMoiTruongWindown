using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL.Service;
using DAL.Entities;

namespace DuAnDauDoi
{
    public partial class FormLichsu : Form
    {
        private readonly HOADONservice _hoadonService = new HOADONservice();

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
                var data = _hoadonService.GetHistory(searchText);
                dataGridView1.DataSource = data;

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["MaHD"].HeaderText = "Mã Hóa Đơn";
                    dataGridView1.Columns["TongTien"].DefaultCellStyle.Format = "N0";
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
            if (dataGridView1.CurrentRow == null) return;
            string maHD = dataGridView1.CurrentRow.Cells["MaHD"].Value.ToString();
            FormHoaDon frm = new FormHoaDon(maHD);
            frm.ShowDialog();
        }
    }
}