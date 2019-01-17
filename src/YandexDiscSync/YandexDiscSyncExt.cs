/*
  SafeVault KeePass Plugin
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
using System.Reflection;
using KeePass.Plugins;

namespace YandexDiscSync
{
    public sealed class YandexDiscSyncExt : Plugin
    {
        private static string _productVersion;
        private YandexDiscSync _sync;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null)
                return false;

            _sync = new YandexDiscSync();
            _sync.Initialize(host);

            return true;
        }

        public override void Terminate()
        {
            _sync?.Terminate();

            _sync = null;
        }

        public static string VersionString()
        {
            if (_productVersion == null)
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                _productVersion = "v" + version.Major + "." + version.Minor + "." + version.Build + "." + version.Revision;
            }
            return _productVersion;
        }
    }
}
