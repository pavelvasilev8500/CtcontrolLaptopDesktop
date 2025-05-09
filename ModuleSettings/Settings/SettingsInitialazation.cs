using ModuleSettings.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSettings.Settings
{
    public static class SettingsInitialazation
    {
        static string _path = $"{Path.GetDirectoryName(Environment.CurrentDirectory.ToString())}\\settings.json";
        public static SettingsModel Get()
        {
            var settings = File.ReadAllText(_path, Encoding.UTF8);
            return JsonConvert.DeserializeObject<SettingsModel>(settings);
        }
        public static void Set()
        {
            var settings = new SettingsModel
            {
                Language = AppSettings.Language,
                Wallpaper = AppSettings.Wallpaper,
                Autorun = AppSettings.Autorun,
                ShowSeconds = AppSettings.ShowSeconds,
            };
            File.WriteAllText(_path, JsonConvert.SerializeObject(settings));
        }
    }
}
