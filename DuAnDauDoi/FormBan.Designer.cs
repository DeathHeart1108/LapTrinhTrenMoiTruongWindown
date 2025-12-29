using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DuAnDauDoi
{
    partial class FormBan
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGoi = new System.Windows.Forms.Button();
            this.BtnSua = new System.Windows.Forms.Button();
            this.btnHD = new System.Windows.Forms.Button();
            this.btnLS = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDb = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGoi
            // 
            this.btnGoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnGoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGoi.FlatAppearance.BorderSize = 0;
            this.btnGoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoi.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnGoi.ForeColor = System.Drawing.Color.White;
            this.btnGoi.Location = new System.Drawing.Point(1080, 20);
            this.btnGoi.Name = "btnGoi";
            this.btnGoi.Size = new System.Drawing.Size(290, 70);
            this.btnGoi.TabIndex = 0;
            this.btnGoi.Text = "🍽️ Gọi Món";
            this.btnGoi.UseVisualStyleBackColor = false;
            // 
            // BtnSua
            // 
            this.BtnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(196)))), ((int)(((byte)(15)))));
            this.BtnSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSua.FlatAppearance.BorderSize = 0;
            this.BtnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSua.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.BtnSua.ForeColor = System.Drawing.Color.White;
            this.BtnSua.Location = new System.Drawing.Point(1080, 110);
            this.BtnSua.Name = "BtnSua";
            this.BtnSua.Size = new System.Drawing.Size(290, 70);
            this.BtnSua.TabIndex = 1;
            this.BtnSua.Text = "✏️ Sửa Món";
            this.BtnSua.UseVisualStyleBackColor = false;
            // 
            // btnHD
            // 
            this.btnHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(89)))), ((int)(((byte)(182)))));
            this.btnHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHD.FlatAppearance.BorderSize = 0;
            this.btnHD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHD.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnHD.ForeColor = System.Drawing.Color.White;
            this.btnHD.Location = new System.Drawing.Point(1080, 200);
            this.btnHD.Name = "btnHD";
            this.btnHD.Size = new System.Drawing.Size(290, 70);
            this.btnHD.TabIndex = 2;
            this.btnHD.Text = "📄 Hóa Đơn";
            this.btnHD.UseVisualStyleBackColor = false;
            this.btnHD.Click += new System.EventHandler(this.btnHD_Click_1);
            // 
            // btnLS
            // 
            this.btnLS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(126)))), ((int)(((byte)(34)))));
            this.btnLS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLS.FlatAppearance.BorderSize = 0;
            this.btnLS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLS.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnLS.ForeColor = System.Drawing.Color.White;
            this.btnLS.Location = new System.Drawing.Point(1080, 380);
            this.btnLS.Name = "btnLS";
            this.btnLS.Size = new System.Drawing.Size(290, 70);
            this.btnLS.TabIndex = 3;
            this.btnLS.Text = "📋 Lịch Sử";
            this.btnLS.UseVisualStyleBackColor = false;
            this.btnLS.Click += new System.EventHandler(this.BtnLS_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(1080, 290);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(290, 70);
            this.btnThanhToan.TabIndex = 4;
            this.btnThanhToan.Text = "💰 Thanh Toán";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(20, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1040, 680);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // btnDb
            // 
            this.btnDb.BackColor = System.Drawing.Color.Brown;
            this.btnDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDb.FlatAppearance.BorderSize = 0;
            this.btnDb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDb.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.btnDb.ForeColor = System.Drawing.Color.White;
            this.btnDb.Location = new System.Drawing.Point(1080, 475);
            this.btnDb.Name = "btnDb";
            this.btnDb.Size = new System.Drawing.Size(290, 70);
            this.btnDb.TabIndex = 6;
            this.btnDb.Text = "🕛 Đặt Bàn";
            this.btnDb.UseVisualStyleBackColor = false;
            this.btnDb.Click += new System.EventHandler(this.btnDb_Click);
            // 
            // FormBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.ClientSize = new System.Drawing.Size(1395, 722);
            this.Controls.Add(this.btnDb);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnThanhToan);
            this.Controls.Add(this.btnLS);
            this.Controls.Add(this.btnHD);
            this.Controls.Add(this.BtnSua);
            this.Controls.Add(this.btnGoi);
            this.Name = "FormBan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "🏠 Quản Lý Nhà Hàng";
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnGoi;
        private Button BtnSua;
        private Button btnHD;
        private Button btnLS;
        private Button btnThanhToan;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnDb;
    }
}
