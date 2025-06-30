using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSettings.Settings
{
    public static class AppSettings
    {
        public static CultureInfo Language { get; set; } = CultureInfo.GetCultureInfo("en-US");
        public static string Wallpaper { get; set; } = "BigSurDay";
        public static bool Autorun { get; set; } = false;
        public static bool ShowSeconds { get; set; } = true;
    }
}
