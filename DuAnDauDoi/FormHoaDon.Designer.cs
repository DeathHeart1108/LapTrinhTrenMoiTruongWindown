using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DuAnDauDoi
{
    partial class FormHoaDon
    {
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbBAN = new System.Windows.Forms.Label();
            this.dgvMon = new System.Windows.Forms.DataGridView();
            this.ColMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.lbTongTien = new System.Windows.Forms.Label();
            this.lbHd = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMon)).BeginInit();
            this.SuspendLayout();
            // 
            // lbBAN
            // 
            this.lbBAN.AutoSize = true;
            this.lbBAN.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lbBAN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lbBAN.Location = new System.Drawing.Point(22, 12);
            this.lbBAN.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbBAN.Name = "lbBAN";
            this.lbBAN.Size = new System.Drawing.Size(189, 60);
            this.lbBAN.TabIndex = 0;
            this.lbBAN.Text = "📄 Bàn:";
            // 
            // dgvMon
            // 
            this.dgvMon.AllowUserToAddRows = false;
            this.dgvMon.BackgroundColor = System.Drawing.Color.White;
            this.dgvMon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMon,
            this.ColSl,
            this.ColGia});
            this.dgvMon.EnableHeadersVisualStyles = false;
            this.dgvMon.Location = new System.Drawing.Point(32, 146);
            this.dgvMon.Margin = new System.Windows.Forms.Padding(2);
            this.dgvMon.Name = "dgvMon";
            this.dgvMon.ReadOnly = true;
            this.dgvMon.RowHeadersWidth = 62;
            this.dgvMon.RowTemplate.Height = 35;
            this.dgvMon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMon.Size = new System.Drawing.Size(540, 390);
            this.dgvMon.TabIndex = 1;
            // 
            // ColMon
            // 
            this.ColMon.HeaderText = "Món ";
            this.ColMon.MinimumWidth = 8;
            this.ColMon.Name = "ColMon";
            this.ColMon.ReadOnly = true;
            this.ColMon.Width = 150;
            // 
            // ColSl
            // 
            this.ColSl.HeaderText = "SL";
            this.ColSl.MinimumWidth = 8;
            this.ColSl.Name = "ColSl";
            this.ColSl.ReadOnly = true;
            this.ColSl.Width = 150;
            // 
            // ColGia
            // 
            this.ColGia.HeaderText = "Giá";
            this.ColGia.MinimumWidth = 8;
            this.ColGia.Name = "ColGia";
            this.ColGia.ReadOnly = true;
            this.ColGia.Width = 150;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(373, 631);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(2);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(199, 56);
            this.btnHuy.TabIndex = 6;
            this.btnHuy.Text = "✕ Đóng";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIn.FlatAppearance.BorderSize = 0;
            this.btnIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnIn.ForeColor = System.Drawing.Color.White;
            this.btnIn.Location = new System.Drawing.Point(32, 631);
            this.btnIn.Margin = new System.Windows.Forms.Padding(2);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(225, 56);
            this.btnIn.TabIndex = 7;
            this.btnIn.Text = "🖨️ In Hóa Đơn";
            this.btnIn.UseVisualStyleBackColor = false;
            // 
            // lbTongTien
            // 
            this.lbTongTien.AutoSize = true;
            this.lbTongTien.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lbTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.lbTongTien.Location = new System.Drawing.Point(24, 555);
            this.lbTongTien.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTongTien.Name = "lbTongTien";
            this.lbTongTien.Size = new System.Drawing.Size(306, 48);
            this.lbTongTien.TabIndex = 8;
            this.lbTongTien.Text = "💵 Tổng tiền: 0đ";
            // 
            // lbHd
            // 
            this.lbHd.AutoSize = true;
            this.lbHd.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lbHd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lbHd.Location = new System.Drawing.Point(23, 81);
            this.lbHd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbHd.Name = "lbHd";
            this.lbHd.Size = new System.Drawing.Size(207, 54);
            this.lbHd.TabIndex = 9;
            this.lbHd.Text = " Hóa đơn:";
            // 
            // FormHoaDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(599, 718);
            this.Controls.Add(this.lbHd);
            this.Controls.Add(this.lbTongTien);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.dgvMon);
            this.Controls.Add(this.lbBAN);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormHoaDon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "📄 Hóa Đơn";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lbBAN;
        private DataGridView dgvMon;
        private DataGridViewTextBoxColumn ColMon;
        private DataGridViewTextBoxColumn ColSl;
        private DataGridViewTextBoxColumn ColGia;
        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox txtSL;
        private Label lbSL;
        private Button btnXacnhan;
        private Button btnHuy;
        private Button btnIn;
        private Label lbTongTien;
        private Label lbHd;
    }
}