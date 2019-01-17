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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Security;
using KeePassLib.Utility;
using YandexDiscSync.Configuration;
using YandexDiscSync.Exceptions;

namespace YandexDiscSync
{
    internal sealed class YandexDiscSyncConf
    {
        private const string YANDEX_DISC_CONFIGURATION_ENTRY = "YandexDiscSync";

        [ConfStorage(Type = ConfStorageType.PwDatabase, FieldName = "Username")]
        public string Username { get; set; }

        [ConfStorage(Type = ConfStorageType.PwDatabase, FieldName = "Username_Password")]
        public ProtectedString Password { get; set; }

        [ConfStorage(Type = ConfStorageType.PwDatabase, FieldName = "AutoSyncMode")]
        public AutoSyncMode AutoSyncMode { get; set; }

        [ConfStorage(Type = ConfStorageType.PwDatabase, FieldName = "Location")]
        public string Location { get; set; }


        [ConfStorage(Type = ConfStorageType.PwDatabase)]
        public string SyncRemoteLastModified { get; set; }

        private PwDatabase _pwDatabase;
        private string _localFilename;

        public YandexDiscSyncConf(PwDatabase pwDatabase)
        {
            _pwDatabase = pwDatabase;
            ReadFromPwDatabase();
        }

        public void ChangeDatabase(PwDatabase db)
        {
            _pwDatabase = db;
        }

        public void Save()
        {
            try
            {
                SaveToPwDatabase();
            }
            catch (Exception e)
            {
                throw new ConfigurationException(e, "Unable to save settings into PwDatabase:\n" + e.Message);
            }
        }

        private void ReadFromPwDatabase()
        {
            var pwEntry = GetConfPwEntry();
            if (pwEntry != null)
            {
                foreach (var prop in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var pwAttribute = prop.GetCustomAttributes(typeof(ConfStorageAttribute), false)
                        .Cast<ConfStorageAttribute>()
                        .FirstOrDefault(m => m.Type == ConfStorageType.PwDatabase);

                    if (pwAttribute == null)
                        continue;

                    try
                    {
                        var entryName = pwAttribute.FieldName ?? prop.Name;
                        if (prop.PropertyType == typeof(ProtectedString))
                        {
                            prop.SetValue(this, pwEntry.Strings.GetSafe(entryName), null);
                            continue;
                        }
                        if (prop.PropertyType == typeof(string))
                        {
                            prop.SetValue(this, pwEntry.Strings.GetSafe(entryName)?.ReadString(), null);
                            continue;
                        }

                        string svalue = pwEntry.Strings.GetSafe(entryName)?.ReadString() ?? "";
                        prop.SetValue(this, DeserializeObject(svalue, prop), null);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void SaveToPwDatabase()
        {
            if (_pwDatabase == null)
                throw new ConfigurationException("Unable to save setting. PwDatabase not setted");

            if (!_pwDatabase.IsOpen)
                throw new ConfigurationException("Unable to save setting. PwDatabase not opened");

            var pwEntry = GetConfPwEntry(_pwDatabase);

            if (pwEntry == null)
            {
                pwEntry = new PwEntry(true, true);
                pwEntry.Strings.Set( PwDefs.TitleField, new ProtectedString( false, YANDEX_DISC_CONFIGURATION_ENTRY ) );
                pwEntry.Strings.Set( PwDefs.NotesField, new ProtectedString( false, "" ) );

                _pwDatabase.RootGroup.AddEntry(pwEntry, true);
                _pwDatabase.RootGroup.Touch(true);
            }

            foreach (var prop in this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var pwAttribute = prop.GetCustomAttributes(typeof(ConfStorageAttribute), false)
                    .Cast<ConfStorageAttribute>()
                    .FirstOrDefault(m => m.Type == ConfStorageType.PwDatabase);

                if (pwAttribute == null)
                    continue;

                var entryName = pwAttribute.FieldName ?? prop.Name;
                if (prop.PropertyType == typeof(ProtectedString))
                {
                    var newValue = (ProtectedString) prop.GetValue(this, null);
                    if (pwEntry.Strings.GetSafe(entryName)?.ReadString() != newValue?.ReadString())
                    {
                        pwEntry.Strings.Set(entryName, newValue);
                        pwEntry.Touch(true);
                    }
                    continue;
                }
                if (prop.PropertyType == typeof(string))
                {
                    var newValue = (string) prop.GetValue(this, null) ?? "";
                    if (pwEntry.Strings.GetSafe(entryName)?.ReadString() != newValue)
                    {
                        pwEntry.Strings.Set(entryName, new ProtectedString(pwAttribute.IsProtected, newValue));
                        pwEntry.Touch(true);
                    }
                    continue;
                }

                var newValue1 = SerializeObject(prop.GetValue(this, null), prop.PropertyType);
                if (pwEntry.Strings.GetSafe(entryName)?.ReadString() != newValue1)
                {
                    pwEntry.Strings.Set(entryName, new ProtectedString(pwAttribute.IsProtected, newValue1));
                    pwEntry.Touch(true);
                }
            }
        }

        public PwEntry GetConfPwEntry()
        {
            return GetConfPwEntry(_pwDatabase);
        }

        public static PwEntry GetConfPwEntry(PwDatabase pwDatabase)
        {
            var sp = new SearchParameters
            {
                SearchString = YANDEX_DISC_CONFIGURATION_ENTRY,
                ComparisonMode = StringComparison.OrdinalIgnoreCase,
                RespectEntrySearchingDisabled = false,
                SearchInGroupNames = false,
                SearchInNotes = false,
                SearchInOther = false,
                SearchInPasswords = false,
                SearchInTags = false,
                SearchInTitles = true,
                SearchInUrls = true,
                SearchInUserNames = false,
                SearchInUuids = false
            };

            PwObjectList<PwEntry> accounts = new PwObjectList<PwEntry>();
            pwDatabase.RootGroup.SearchEntries(sp, accounts);
            return (accounts.UCount >= 1)
                ? accounts.GetAt(0)
                : null;
        }

        private static string SerializeObject(object value, Type type)
        {
            if (value == null)
                return "";

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.CloseOutput = true;
            xws.Encoding = StrUtil.Utf8;
            xws.Indent = true;
            xws.IndentChars = "\t";
            xws.OmitXmlDeclaration = true;

            using (var ms = new MemoryStream())
            using (var output = new StreamWriter(ms, Encoding.UTF8))
            {
                XmlWriter xw = XmlWriter.Create(output, xws);
                XmlSerializer xs = new XmlSerializer(type);
                xs.Serialize(xw, value);

                ms.Flush();
                ms.Position = 0;
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(ms);
                return xdoc.DocumentElement?.InnerXml;
            }
        }

        private static object DeserializeObject(string svalue, PropertyInfo prop)
        {
            object value = null;
            if (!string.IsNullOrEmpty(svalue))
            {
                var xdoc = string.Format("<{0}>{1}</{0}>", prop.PropertyType.Name, svalue);

                using (var textReader = new StringReader(xdoc))
                {
                    XmlSerializer xs = new XmlSerializer(prop.PropertyType);
                    value = xs.Deserialize(textReader);
                }
            }
            return value;
        }
    }
}
