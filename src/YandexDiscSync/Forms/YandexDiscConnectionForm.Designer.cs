/*
  SafeVaultKeyProvider Plugin
  Copyright (C) 2016-2017 Evgeny Zyuzin <evgeny.zyuzin@gmail.com>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 2 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

namespace YandexDiscSync.Forms
{
    partial class VaultConnectionConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.gb_Frame1 = new System.Windows.Forms.GroupBox();
            this.lb_Location = new System.Windows.Forms.Label();
            this.tb_Location = new System.Windows.Forms.TextBox();
            this.btn_PassCreate = new System.Windows.Forms.Button();
            this.cb_AutoSync = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lb_Password = new System.Windows.Forms.Label();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.lb_Label3 = new System.Windows.Forms.Label();
            this.tb_Username = new System.Windows.Forms.TextBox();
            this.pb_Image1 = new System.Windows.Forms.PictureBox();
            this.gb_Frame1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(317, 263);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "&OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.OnBtnOk);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(398, 263);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "&Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // gb_Frame1
            // 
            this.gb_Frame1.Controls.Add(this.lb_Location);
            this.gb_Frame1.Controls.Add(this.tb_Location);
            this.gb_Frame1.Controls.Add(this.btn_PassCreate);
            this.gb_Frame1.Controls.Add(this.cb_AutoSync);
            this.gb_Frame1.Controls.Add(this.label1);
            this.gb_Frame1.Controls.Add(this.lb_Password);
            this.gb_Frame1.Controls.Add(this.tb_Password);
            this.gb_Frame1.Controls.Add(this.lb_Label3);
            this.gb_Frame1.Controls.Add(this.tb_Username);
            this.gb_Frame1.Location = new System.Drawing.Point(12, 66);
            this.gb_Frame1.Name = "gb_Frame1";
            this.gb_Frame1.Size = new System.Drawing.Size(461, 191);
            this.gb_Frame1.TabIndex = 3;
            this.gb_Frame1.TabStop = false;
            this.gb_Frame1.Text = "Yandex Disc Sync";
            // 
            // lb_Location
            // 
            this.lb_Location.AutoSize = true;
            this.lb_Location.Location = new System.Drawing.Point(9, 88);
            this.lb_Location.Name = "lb_Location";
            this.lb_Location.Size = new System.Drawing.Size(51, 13);
            this.lb_Location.TabIndex = 17;
            this.lb_Location.Text = "Location:";
            // 
            // tb_Location
            // 
            this.tb_Location.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_Location.Location = new System.Drawing.Point(85, 85);
            this.tb_Location.Name = "tb_Location";
            this.tb_Location.Size = new System.Drawing.Size(338, 21);
            this.tb_Location.TabIndex = 18;
            // 
            // btn_PassCreate
            // 
            this.btn_PassCreate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_PassCreate.Location = new System.Drawing.Point(426, 46);
            this.btn_PassCreate.Margin = new System.Windows.Forms.Padding(0);
            this.btn_PassCreate.Name = "btn_PassCreate";
            this.btn_PassCreate.Size = new System.Drawing.Size(29, 20);
            this.btn_PassCreate.TabIndex = 16;
            this.btn_PassCreate.Text = "•••";
            this.btn_PassCreate.UseVisualStyleBackColor = true;
            this.btn_PassCreate.Click += new System.EventHandler(this.btn_PassCreate_Click);
            // 
            // cb_AutoSync
            // 
            this.cb_AutoSync.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_AutoSync.FormattingEnabled = true;
            this.cb_AutoSync.Items.AddRange(new object[] {
            "Disabled",
            "Save",
            "Open",
            "Both"});
            this.cb_AutoSync.Location = new System.Drawing.Point(85, 152);
            this.cb_AutoSync.Name = "cb_AutoSync";
            this.cb_AutoSync.Size = new System.Drawing.Size(110, 21);
            this.cb_AutoSync.TabIndex = 15;
            this.cb_AutoSync.SelectedIndexChanged += new System.EventHandler(this.OnCtrSelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Auto Sync:";
            // 
            // lb_Password
            // 
            this.lb_Password.AutoSize = true;
            this.lb_Password.Location = new System.Drawing.Point(9, 48);
            this.lb_Password.Name = "lb_Password";
            this.lb_Password.Size = new System.Drawing.Size(56, 13);
            this.lb_Password.TabIndex = 12;
            this.lb_Password.Text = "Password:";
            // 
            // tb_Password
            // 
            this.tb_Password.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_Password.Location = new System.Drawing.Point(85, 45);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '•';
            this.tb_Password.Size = new System.Drawing.Size(338, 21);
            this.tb_Password.TabIndex = 13;
            this.tb_Password.TextChanged += new System.EventHandler(this.OnTextBoxChanged);
            // 
            // lb_Label3
            // 
            this.lb_Label3.AutoSize = true;
            this.lb_Label3.Location = new System.Drawing.Point(9, 22);
            this.lb_Label3.Name = "lb_Label3";
            this.lb_Label3.Size = new System.Drawing.Size(58, 13);
            this.lb_Label3.TabIndex = 5;
            this.lb_Label3.Text = "Username:";
            // 
            // tb_Username
            // 
            this.tb_Username.Location = new System.Drawing.Point(85, 19);
            this.tb_Username.Name = "tb_Username";
            this.tb_Username.Size = new System.Drawing.Size(338, 20);
            this.tb_Username.TabIndex = 3;
            this.tb_Username.TextChanged += new System.EventHandler(this.OnTextBoxChanged);
            // 
            // pb_Image1
            // 
            this.pb_Image1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pb_Image1.Location = new System.Drawing.Point(0, 0);
            this.pb_Image1.Name = "pb_Image1";
            this.pb_Image1.Size = new System.Drawing.Size(478, 60);
            this.pb_Image1.TabIndex = 0;
            this.pb_Image1.TabStop = false;
            // 
            // VaultConnectionConfigForm
            // 
            this.AcceptButton = this.btn_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_Cancel;
            this.ClientSize = new System.Drawing.Size(478, 298);
            this.Controls.Add(this.gb_Frame1);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.pb_Image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VaultConnectionConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "<>";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.gb_Frame1.ResumeLayout(false);
            this.gb_Frame1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Image1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_Image1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.GroupBox gb_Frame1;
        private System.Windows.Forms.Label lb_Label3;
        private System.Windows.Forms.TextBox tb_Username;
        private System.Windows.Forms.ComboBox cb_AutoSync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_Password;
        private System.Windows.Forms.TextBox tb_Password;
        private System.Windows.Forms.Button btn_PassCreate;
        private System.Windows.Forms.Label lb_Location;
        private System.Windows.Forms.TextBox tb_Location;
    }
}