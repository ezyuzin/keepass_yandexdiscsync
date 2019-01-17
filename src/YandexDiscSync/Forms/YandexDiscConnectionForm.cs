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

using System;
using System.Linq;
using System.Windows.Forms;
using KeePass.UI;
using KeePassLib.Security;
using YandexDiscSync.Configuration;

namespace YandexDiscSync.Forms
{
    internal partial class VaultConnectionConfigForm : Form
    {
        private const string NO_PASSWORD = @"(no password)";
        private YandexDiscSyncConf _yandexDiscConf = null;

        public void InitEx(YandexDiscSyncConf vaultConf)
        {
            _yandexDiscConf = vaultConf;
        }

        public VaultConnectionConfigForm()
        {
            InitializeComponent();
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            if (_yandexDiscConf == null)
            {
                throw new InvalidOperationException();
            }

            GlobalWindowManager.AddWindow(this);

            string strTitle = "Configure Yandex Disc Sync";
            string strDesc = "Sync database with Yandex Disc";

            this.Text = strTitle;
            BannerFactory.CreateBannerEx(this, pb_Image1, Resources.YandexDiscSync.B48x48_YandexDisc, strTitle, strDesc);

            tb_Username.Text = _yandexDiscConf.Username;
            tb_Password.PasswordChar = '•';
            if (!string.IsNullOrEmpty(_yandexDiscConf.Password?.ReadString()))
                tb_Password.Text = NO_PASSWORD;

            tb_Location.Text = _yandexDiscConf.Location;

            cb_AutoSync.Enabled = true;
            cb_AutoSync.Items.Clear();
            foreach(var value in Enum.GetValues(typeof(AutoSyncMode)))
                cb_AutoSync.Items.Add(value);

            cb_AutoSync.SelectedIndex = Enum.GetValues(typeof(AutoSyncMode))
                .Cast<AutoSyncMode>().ToList()
                .IndexOf(_yandexDiscConf.AutoSyncMode);


            EnableControlsEx();
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalWindowManager.RemoveWindow(this);
        }

        private void OnBtnOk(object sender, EventArgs e)
        {
            _yandexDiscConf.Username = tb_Username.Text;
            if (tb_Password.Text != NO_PASSWORD)
                _yandexDiscConf.Password = new ProtectedString(true, tb_Password.Text);

            _yandexDiscConf.Location = tb_Location.Text;
            _yandexDiscConf.AutoSyncMode = (AutoSyncMode)cb_AutoSync.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }

        private void EnableControlsEx()
        {
            bool bOk = (!string.IsNullOrEmpty(tb_Username.Text));
            bOk &= (!string.IsNullOrEmpty(tb_Password.Text));

            btn_OK.Enabled = bOk;
        }

        private void OnCtrSelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControlsEx();
        }

        private void OnTextBoxChanged(object sender, EventArgs e)
        {
            EnableControlsEx();
        }

        private void btn_PassCreate_Click(object sender, EventArgs e)
        {
            if (tb_Password.PasswordChar == '\0')
            {
                tb_Password.PasswordChar = '•';
            }
            else
            {
                tb_Password.PasswordChar = '\0';
                if (tb_Password.Text == NO_PASSWORD)
                    tb_Password.Text = _yandexDiscConf.Password.ReadString();
            }

        }
    }
}
