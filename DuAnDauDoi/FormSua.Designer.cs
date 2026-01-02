using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DuAnDauDoi
{
    partial class FormSua
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbBAN = new System.Windows.Forms.Label();
            this.dgvMon = new System.Windows.Forms.DataGridView();
            this.ColMon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSL = new System.Windows.Forms.TextBox();
            this.lbSL = new System.Windows.Forms.Label();
            this.btnXacnhan = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.cboLoaimon = new System.Windows.Forms.ComboBox();
            this.btnXoa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMon)).BeginInit();
            this.SuspendLayout();
            // 
            // lbBAN
            // 
            this.lbBAN.AutoSize = true;
            this.lbBAN.BackColor = System.Drawing.Color.Transparent;
            this.lbBAN.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lbBAN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lbBAN.Location = new System.Drawing.Point(22, 15);
            this.lbBAN.Name = "lbBAN";
            this.lbBAN.Size = new System.Drawing.Size(189, 60);
            this.lbBAN.TabIndex = 0;
            this.lbBAN.Text = "✏️ Bàn:";
            // 
            // dgvMon
            // 
            this.dgvMon.AllowUserToAddRows = false;
            this.dgvMon.BackgroundColor = System.Drawing.Color.White;
            this.dgvMon.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMon.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMon.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMon.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMon,
            this.ColSl,
            this.ColGia});
            this.dgvMon.EnableHeadersVisualStyles = false;
            this.dgvMon.Location = new System.Drawing.Point(595, 85);
            this.dgvMon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvMon.Name = "dgvMon";
            this.dgvMon.RowHeadersWidth = 62;
            this.dgvMon.RowTemplate.Height = 35;
            this.dgvMon.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMon.Size = new System.Drawing.Size(511, 370);
            this.dgvMon.TabIndex = 1;
            // 
            // ColMon
            // 
            this.ColMon.HeaderText = "Món ";
            this.ColMon.MinimumWidth = 8;
            this.ColMon.Name = "ColMon";
            this.ColMon.Width = 150;
            // 
            // ColSl
            // 
            this.ColSl.HeaderText = "SL";
            this.ColSl.MinimumWidth = 8;
            this.ColSl.Name = "ColSl";
            this.ColSl.Width = 150;
            // 
            // ColGia
            // 
            this.ColGia.HeaderText = "Giá";
            this.ColGia.MinimumWidth = 8;
            this.ColGia.Name = "ColGia";
            this.ColGia.Width = 150;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(20, 85);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(560, 560);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // txtSL
            // 
            this.txtSL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSL.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.txtSL.Location = new System.Drawing.Point(810, 475);
            this.txtSL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSL.Name = "txtSL";
            this.txtSL.Size = new System.Drawing.Size(296, 50);
            this.txtSL.TabIndex = 3;
            // 
            // lbSL
            // 
            this.lbSL.AutoSize = true;
            this.lbSL.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lbSL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lbSL.Location = new System.Drawing.Point(595, 482);
            this.lbSL.Name = "lbSL";
            this.lbSL.Size = new System.Drawing.Size(192, 38);
            this.lbSL.TabIndex = 4;
            this.lbSL.Text = "📊 Số lượng:";
            this.lbSL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnXacnhan
            // 
            this.btnXacnhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnXacnhan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXacnhan.FlatAppearance.BorderSize = 0;
            this.btnXacnhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXacnhan.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnXacnhan.ForeColor = System.Drawing.Color.White;
            this.btnXacnhan.Location = new System.Drawing.Point(732, 545);
            this.btnXacnhan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXacnhan.Name = "btnXacnhan";
            this.btnXacnhan.Size = new System.Drawing.Size(200, 70);
            this.btnXacnhan.TabIndex = 5;
            this.btnXacnhan.Text = "✓ Xác Nhận";
            this.btnXacnhan.UseVisualStyleBackColor = false;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnHuy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHuy.FlatAppearance.BorderSize = 0;
            this.btnHuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(957, 545);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(149, 70);
            this.btnHuy.TabIndex = 6;
            this.btnHuy.Text = "✕ Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            // 
            // cboLoaimon
            // 
            this.cboLoaimon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoaimon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboLoaimon.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.cboLoaimon.FormattingEnabled = true;
            this.cboLoaimon.Items.AddRange(new object[] {
            "🥗 Món Chay",
            "🍖 Món Mặn",
            "🥤 Nước Ngọt",
            "🍺 Bia",
            "💧 Nước Lọc"});
            this.cboLoaimon.Location = new System.Drawing.Point(595, 15);
            this.cboLoaimon.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboLoaimon.Name = "cboLoaimon";
            this.cboLoaimon.Size = new System.Drawing.Size(511, 46);
            this.cboLoaimon.TabIndex = 8;
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(586, 545);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(119, 70);
            this.btnXoa.TabIndex = 9;
            this.btnXoa.Text = "🗑️Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // FormSua
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1127, 665);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.cboLoaimon);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnXacnhan);
            this.Controls.Add(this.lbSL);
            this.Controls.Add(this.txtSL);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dgvMon);
            this.Controls.Add(this.lbBAN);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormSua";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "✏️ Sửa Món";
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
        private ComboBox cboLoaimon;
        private Button btnXoa;
    }
}