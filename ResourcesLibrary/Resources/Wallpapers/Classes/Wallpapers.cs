using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ResourcesLibrary.Resources.Wallpapers.Classes
{
    public static class Wallpapers
    {
        private static string _wallpaper;
        public static Dictionary<string, ResourceDictionary> WallpaperDictionaries { get; } = new Dictionary<string, ResourceDictionary>()
        {
            {"BigSurDay" , Application.LoadComponent(new Uri("/ResourcesLibrary;component/Resources/Wallpapers/Dictionaries/BigSurDay.xaml", UriKind.Relative)) as ResourceDictionary},
            {"BigSurNight", Application.LoadComponent(new Uri("/ResourcesLibrary;component/Resources/Wallpapers/Dictionaries/BigSurNight.xaml", UriKind.Relative)) as ResourceDictionary }
        };

        public static string Wallpaper
        {
            get =>_wallpaper;
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == _wallpaper) return;
                _wallpaper = value;
                Application.Current.Resources.MergedDictionaries.Remove(
                    Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                        d => d.Keys.Cast<object>().Any(k => k.ToString().Contains("WallPaper"))));
                WallpaperDictionaries.TryGetValue(value, out var wallpaper);
                Application.Current.Resources.MergedDictionaries.Add(wallpaper);
            }
        }
    }
}
