/*
  YandexDisc Sync KeePass Plugin
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using KeePass.DataExchange;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.UI;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Serialization;
using YandexDiscSync.Configuration;
using YandexDiscSync.Exceptions;
using YandexDiscSync.Forms;
using YandexDiscSync.Misc;

namespace YandexDiscSync
{
    /// <summary>
    /// main plugin class
    /// </summary>
    public sealed class YandexDiscSync
    {
        public const string PRODUCT_NAME = "Yandex Disc Sync";

        private IPluginHost _host;

        private ToolStripSeparator _tsSeparator;
        private ToolStripMenuItem _tsmiPopup;
        private enum SyncCommand
        {
            SyncLocal = 1,
            SyncRemote = 2,
            Download = 3,
            Upload = 4
        }

        public bool Initialize(IPluginHost host)
        {
            if (host == null) return false;
            _host = host;

            // Get a reference to the 'Tools' menu item container
            ToolStripItemCollection tsMenu = _host.MainWindow.ToolsMenu.DropDownItems;

            // Add a separator at the bottom
            _tsSeparator = new ToolStripSeparator();
            tsMenu.Add(_tsSeparator);

            // Add the popup menu item
            _tsmiPopup = new ToolStripMenuItem();
            _tsmiPopup.Text = @"Yandex Disc Sync";
            tsMenu.Add(_tsmiPopup);

            var tsmiSyncLocal = new ToolStripMenuItem();
            tsmiSyncLocal.Name = SyncCommand.SyncLocal.ToString();
            tsmiSyncLocal.Text = @"Sync Local << Remote";
            tsmiSyncLocal.Click += OnSyncWithYandexDisc;
            _tsmiPopup.DropDownItems.Add(tsmiSyncLocal);

            var tsmiSyncRemote = new ToolStripMenuItem();
            tsmiSyncRemote.Name = SyncCommand.SyncRemote.ToString();
            tsmiSyncRemote.Text = @"Sync Local << >> Remote";
            tsmiSyncRemote.Click += OnSyncWithYandexDisc;
            _tsmiPopup.DropDownItems.Add(tsmiSyncRemote);

            _tsmiPopup.DropDownItems.Add(new ToolStripSeparator());

            var tsmiDownload = new ToolStripMenuItem();
            tsmiDownload.Name = SyncCommand.Download.ToString();
            tsmiDownload.Text = @"Download";
            tsmiDownload.Click += OnSyncWithYandexDisc;
            _tsmiPopup.DropDownItems.Add(tsmiDownload);

            var tsmiUpload = new ToolStripMenuItem();
            tsmiUpload.Name = SyncCommand.Upload.ToString();
            tsmiUpload.Text = @"Upload";
            tsmiUpload.Click += OnSyncWithYandexDisc;
            _tsmiPopup.DropDownItems.Add(tsmiUpload);

            _tsmiPopup.DropDownItems.Add(new ToolStripSeparator());

            var tsmiConfigure = new ToolStripMenuItem();
            tsmiConfigure.Name = "CONFIG";
            tsmiConfigure.Text = @"Configuration...";
            tsmiConfigure.Click += OnConfigure;
            _tsmiPopup.DropDownItems.Add(tsmiConfigure);

            // We want a notification when the user tried to save the
            // current database or opened a new one.
            _host.MainWindow.FileSaved += OnFileSaved;
            _host.MainWindow.FileOpened += OnFileOpened;

            return true; // Initialization successful
        }

        /// <summary>
        /// The <c>Terminate</c> function is called by KeePass when
        /// you should free all resources, close open files/streams,
        /// etc. It is also recommended that you remove all your
        /// plugin menu items from the KeePass menu.
        /// </summary>
        public void Terminate()
        {
            // Remove all of our menu items
            ToolStripItemCollection tsMenu = _host.MainWindow.ToolsMenu.DropDownItems;
            tsMenu.Remove(_tsSeparator);
            tsMenu.Remove(_tsmiPopup);

            // Important! Remove event handlers!
            _host.MainWindow.FileSaved -= OnFileSaved;
            _host.MainWindow.FileOpened -= OnFileOpened;
        }

        /// <summary>
        /// Event handler to implement auto sync on save
        /// </summary>
        private void OnFileSaved(object sender, FileSavedEventArgs e)
        {
            if (e.Success == false)
                return;

            var conf = new YandexDiscSyncConf(_host.Database);

            if (AutoSyncMode.Save != (conf.AutoSyncMode & AutoSyncMode.Save))
                return;

            if (Keys.Shift == (Control.ModifierKeys & Keys.Shift))
            {
                SetStatusText("Shift Key pressed. Auto Sync ignored.");
                return;
            }
            if (!IsSyncConfigured(conf))
            {
                SetStatusText("YandexDiscSync not configured.");
                return;
            }
            SyncWithYandexDisc(SyncCommand.SyncRemote);
        }

        /// <summary>
        /// Event handler to implement auto sync on open
        /// </summary>
        private void OnFileOpened(object sender, FileOpenedEventArgs e)
        {
            var conf = new YandexDiscSyncConf(_host.Database);
            if (AutoSyncMode.Open != (conf.AutoSyncMode & AutoSyncMode.Open))
                return;

            if (Keys.Shift == (Control.ModifierKeys & Keys.Shift))
            {
                SetStatusText("Shift Key pressed. Auto Sync ignored.");
                return;
            }
            if (!IsSyncConfigured(conf))
            {
                SetStatusText("YandexDiscSync not configured.");
                return;
            }
            SyncWithYandexDisc(SyncCommand.SyncLocal);
        }

        private bool IsSyncConfigured(YandexDiscSyncConf conf)
        {
            var required = new[] {
                conf.Username,
                conf.Password.ReadString()
             };

            if (required.Any(string.IsNullOrEmpty))
                return false;

            return true;
        }

        /// <summary>
        /// Event handler for sync menu entries
        /// </summary>
        private void OnSyncWithYandexDisc(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem)sender;
            SyncCommand syncCommand = (SyncCommand)Enum.Parse(typeof(SyncCommand), item.Name);
            SyncWithYandexDisc(syncCommand);
        }

        /// <summary>
        /// Event handler for configuration menu entry
        /// </summary>
        private void OnConfigure(object sender, EventArgs e)
        {
            if (!_host.Database.IsOpen)
            {
                ShowMessageBox("You first need to open a database.");
                return;
            }

            if (_host.Database.IsOpen)
            {
                YandexDiscSyncConf conf = new YandexDiscSyncConf(_host.Database);

                VaultConnectionConfigForm form1 = new VaultConnectionConfigForm();
                form1.InitEx(conf);
                if (DialogResult.OK == UIUtil.ShowDialogAndDestroy(form1))
                {
                    conf.Save();
                    _host.MainWindow.UpdateUI(false, null, true, null, true, null, bSetModified: true);
                }
            }
        }

        private void SyncWithYandexDisc(SyncCommand syncCommand)
        {
            PwDatabase pwDatabase = _host.Database;
            if (!pwDatabase.IsOpen)
            {
                ShowMessageBox("You first need to open a database.");
                return;
            }
            if (!pwDatabase.IOConnectionInfo.IsLocalFile())
            {
                ShowMessageBox("Only databases stored locally or on a network share are supported.\n" +
                               "Save your database locally or on a network share and try again.");
                return;
            }
            if (pwDatabase.Modified)
            {
                ShowMessageBox("Database has not saved changes. Save it first.");
                return;
            }

            YandexDiscSyncConf syncConf = new YandexDiscSyncConf(pwDatabase);
            if (!IsSyncConfigured(syncConf))
            {
                SetStatusText("Sync not Configured.");
                return;
            }
            
            _host.MainWindow.FileSaved -= OnFileSaved; // disable to not trigger when saving ourselves
            _host.MainWindow.FileOpened -= OnFileOpened; // disable to not trigger when opening ourselves
            _host.MainWindow.Enabled = false;

            _lastStatus = "";

            try
            {
                var filename = Path.GetFileNameWithoutExtension(pwDatabase.IOConnectionInfo.Path);
                YandexWebDavClient webClient = new YandexWebDavClient(syncConf);

                if (syncCommand == SyncCommand.Download)
                {
                    try
                    {
                        SyncDownload(pwDatabase, webClient, filename, syncConf);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(ex.Message);
                        SetStatusText("SyncDownload Failed");
                    }
                }
                if (syncCommand == SyncCommand.Upload)
                {
                    try
                    {
                        SyncUpload(pwDatabase, webClient, filename, syncConf);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(ex.Message);
                        SetStatusText("SyncUpload Failed");
                    }
                }

                if (syncCommand == SyncCommand.SyncLocal)
                {
                    try
                    {
                        SyncLocal(pwDatabase, webClient, filename, syncConf);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(ex.Message);
                        SetStatusText("SyncLocal Failed");
                    }
                }

                if (syncCommand == SyncCommand.SyncRemote)
                {
                    try
                    {
                        SyncRemote(pwDatabase, webClient, filename, syncConf);
                    }
                    catch (Exception ex)
                    {
                        ShowMessageBox(ex.Message);
                        SetStatusText("SyncRemote Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox(ex.Message);
                SetStatusText("Unknown Err: " + ex.Message);
            }

            _host.MainWindow.UpdateUI(false, null, true, null, true, null, false);
            _host.MainWindow.Enabled = true;
            _host.MainWindow.FileSaved += OnFileSaved;
            _host.MainWindow.FileOpened += OnFileOpened;
            if (_lastStatus != "")
                SetStatusText(_lastStatus);
        }

        private void SyncDownload(PwDatabase pwDatabase, YandexWebDavClient webClient, string filename, YandexDiscSyncConf storageConf)
        {
            var lastModified = GetRemoteLastModified(filename, webClient);
            if (lastModified != null)
            {
                SetStatusText("Downloading...");
                var dbData = Async.Invoke(() => DownloadFile(filename, webClient));

                ReplaceDatabase(pwDatabase, dbData);
                SetStatusText("Download Done.");
                storageConf.ChangeDatabase(_host.Database);
                storageConf.SyncRemoteLastModified = lastModified.Value.ToString("u");
                storageConf.Save();
                pwDatabase.Save(new NullStatusLogger());
            }
            else
            {
                SetStatusText("Database not found. Nothing to sync");
            }
        }

        private void SyncUpload(PwDatabase pwDatabase, YandexWebDavClient webClient, string databaseUuid,
            YandexDiscSyncConf storageConf)
        {
            var lastModified = GetLastModified(pwDatabase);

            var location = _host.Database.IOConnectionInfo.Path;
            SetStatusText("Saving to Yandex Disc...");
            Async.Invoke(() =>
            {
                webClient.PutFile(
                    databaseUuid,
                    File.ReadAllBytes(location),
                    lastModified,
                    progress => { SetStatusText($"Saving to YandexDisc ({Math.Floor(progress)}%)"); });
            });
            SetStatusText("Successfull save to YandexDisc");

            storageConf.SyncRemoteLastModified = lastModified.ToString("u");
            storageConf.Save();
        }

        private void SyncLocal(PwDatabase pwDatabase, YandexWebDavClient webClient, string filename, YandexDiscSyncConf vaultConf)
        {
            SetStatusText("Check changes...");
            var value = GetRemoteLastModified(filename, webClient);
            if (value == null)
            {
                SetStatusText("Database not found on Yandex Disc. Nothing to sync");
                return;
            }
            var lastModified = value.Value;

            if (vaultConf.SyncRemoteLastModified == lastModified.ToString("u"))
            {
                SetStatusText("No changes");
                return;
            }

            SetStatusText("Downloading...");
            var dbData = Async.Invoke(() => DownloadFile(filename, webClient));

            SetStatusText("Sync...");
            SyncDatabase(pwDatabase, dbData, false);
            SetStatusText("Successfull Sync Local <= Remote");

            vaultConf.SyncRemoteLastModified = lastModified.ToString("u");
            vaultConf.Save();
            pwDatabase.Save(new NullStatusLogger());
        }

        private void SyncRemote(PwDatabase pwDatabase, YandexWebDavClient webClient, string filename, YandexDiscSyncConf vaultConf)
        {
            SetStatusText("Check changes...");
            var localLastModified = GetLastModified(pwDatabase);
            var lastModified = GetRemoteLastModified(filename, webClient);
            if (lastModified != null && lastModified == localLastModified)
            {
                SetStatusText("No changes.");
                return;
            }

            if (lastModified != null)
            {
                SetStatusText("Downloading...");
                var dbData = Async.Invoke(() => DownloadFile(filename, webClient));
                if (dbData != null)
                {
                    SetStatusText("Sync...");
                    SyncDatabase(pwDatabase, dbData, true);
                    _host.MainWindow.Enabled = false;
                    localLastModified = GetLastModified(pwDatabase);
                }
            }

            SetStatusText("Saving to YandexDisc...");
            Async.Invoke(() =>
            {
                webClient.PutFile(
                    filename,
                    File.ReadAllBytes(pwDatabase.IOConnectionInfo.Path),
                    localLastModified,
                    progress => { SetStatusText($"Saving to YandexDisc ({Math.Floor(progress)}%)"); });
            });
            SetStatusText("Successfull Sync Local <=> Remote");

            vaultConf.SyncRemoteLastModified = localLastModified.ToString("u");
            vaultConf.Save();
            pwDatabase.Save(new NullStatusLogger());
        }

        private static DateTime GetLastModified(PwDatabase pwDatabase)
        {
            List<PwUuid> ignore = new List<PwUuid>();
            PwEntry confEntry = YandexDiscSyncConf.GetConfPwEntry(pwDatabase);
            ignore.Add(confEntry?.Uuid);
            ignore.AddRange(pwDatabase.DeletedObjects.Select(m => m.Uuid));

            DateTime lastModified = DateTime.MinValue;
            return GetLastModified(pwDatabase.RootGroup, lastModified, ignore.ToArray()).ToUniversalTime();
        }

        private static DateTime GetLastModified(PwGroup pwGroup, DateTime lastModified, PwUuid[] skip)
        {
            foreach (var group in pwGroup.Groups)
                lastModified = GetLastModified(group, lastModified, skip);

            foreach (var pwEntry in pwGroup.Entries)
            {
                if (skip.Any(uuid => uuid != null && uuid.EqualsValue(pwEntry.Uuid)))
                    continue;

                if (pwEntry.LastModificationTime > lastModified)
                    lastModified = pwEntry.LastModificationTime;

                if (pwEntry.CreationTime > lastModified)
                    lastModified = pwEntry.CreationTime;
            }

            return lastModified;
        }

        private static DateTime? GetRemoteLastModified(string filename, YandexWebDavClient webClient)
        {
            return Async.Invoke(() =>
            {
                if (!webClient.IsFileExist(filename))
                    return null;

                return (DateTime?)webClient.GetLastModified(filename);
            });
        }

        private byte[] DownloadFile(string filename, YandexWebDavClient webClient)
        {
            byte[] databaseData;
            try
            {
                databaseData = webClient.GetFile(filename, progress => { SetStatusText($"Download ({Math.Floor(progress)}%)"); });
            }
            catch (WebException httpEx)
            {
                var response = (HttpWebResponse)httpEx.Response;
                if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    throw new SynchronizeException("Database not found on YandexDisc. Please save it first.");
                }
                throw;
            }
            return databaseData;
        }

        private void CreateBackup(PwDatabase pwDatabase)
        {
            var location = pwDatabase.IOConnectionInfo.Path;
            var folder = System.IO.Path.GetDirectoryName(location) + "\\backup";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            int ix = 0;
            while (true)
            {
                string bakName = folder + "\\" + string.Format("{0}.{1}.bak", Path.GetFileName(location), ix++);
                if (!File.Exists(bakName))
                {
                    File.Copy(location, bakName);
                    break;
                }
            }
        }

        private void ReplaceDatabase(PwDatabase pwDatabase, byte[] dbData)
        {
            CreateBackup(pwDatabase);

            var pwKey = pwDatabase.MasterKey;
            var location = pwDatabase.IOConnectionInfo.Path;
            pwDatabase.Close();

            File.WriteAllBytes(location, dbData);
            try
            {
                // try to open with current MasterKey ...
                _host.Database.Open(IOConnectionInfo.FromPath(location), pwKey, new NullStatusLogger());
            }
            catch (KeePassLib.Keys.InvalidCompositeKeyException)
            {
                // ... MasterKey is different, let user enter the MasterKey
                _host.MainWindow.OpenDatabase(IOConnectionInfo.FromPath(location), null, true);
            }
        }

        /// <summary>
        /// Sync YandexDisc with currently open Database file
        /// </summary>
        /// <returns>Return status of the update</returns>
        private void SyncDatabase(PwDatabase pwDatabase, byte[] dbData, bool forceSave)
        {
            CreateBackup(pwDatabase);

            var location = pwDatabase.IOConnectionInfo.Path;
            var tmpFile = location + ".sync.tmp";
            try
            {
                System.IO.File.WriteAllBytes(tmpFile, dbData);

                IOConnectionInfo connection = IOConnectionInfo.FromPath(tmpFile);

                bool? success = ImportUtil.Synchronize(pwDatabase, _host.MainWindow, connection, false,
                    _host.MainWindow);

                if (!success.HasValue)
                {
                    throw new SynchronizeException(
                        "Synchronization failed.\n\nYou do not have permission to import. Adjust your KeePass configuration.");
                }
                if (!(bool)success)
                    throw new SynchronizeException("Synchronization failed.\n\n" +
                                            "If the error was that master keys (passwords) do not match, use Upload / Download commands instead of Sync " +
                                            "or change the local master key to match that of the remote database.");

                pwDatabase.Save(new NullStatusLogger());
                var mostRecentUsed = _host.MainWindow.FileMruList;
                mostRecentUsed.RemoveItem(tmpFile);
            }
            finally
            {
                if (System.IO.File.Exists(tmpFile))
                    System.IO.File.Delete(tmpFile);
            }
        }

        private static void ShowMessageBox(string message)
        {
            MessageBox.Show(message, PRODUCT_NAME);
        }

        private delegate void SetStatusTextMT();

        private string _lastStatus = string.Empty;
        private void SetStatusText(string message)
        {
            Application.DoEvents();
            _host.MainWindow.Invoke(new SetStatusTextMT(() => {
                _host.MainWindow.SetStatusEx("Yandex Disc Sync: " + message);
            }));
            Application.DoEvents();
            _lastStatus = message;
        }
    }
}
