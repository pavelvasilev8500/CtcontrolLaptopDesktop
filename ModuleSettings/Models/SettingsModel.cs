using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleSettings.Models
{
    public class SettingsModel
    {
        public CultureInfo Language { get; set; }
        public string Wallpaper { get; set; }
        public bool Autorun { get; set; }
        public bool ShowSeconds { get; set; }
    }
}
