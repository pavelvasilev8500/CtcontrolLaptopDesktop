using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Threading;

namespace ResourcesLibrary.Resources.Languages.Classes
{
    public static class Languages
    {
        public delegate void EventHandler();
        public static event EventHandler LanguageChanged;
        public static List<CultureInfo> All_Languages { get; } = new List<CultureInfo>()
        {
            new CultureInfo("en-US"),
            new CultureInfo("ru-RU")
        };
        //public static List<CultureInfo> All_Languages
        //{
        //    get =>  m_Languages;
        //}

        private static ResourceDictionary _russian = Application.LoadComponent(new Uri("/ResourcesLibrary;component/Resources/Languages/lang.ru-RU.xaml", UriKind.Relative)) as ResourceDictionary;
        private static ResourceDictionary _english = Application.LoadComponent(new Uri("/ResourcesLibrary;component/Resources/Languages/lang.xaml", UriKind.Relative)) as ResourceDictionary;

        public static CultureInfo Language
        {
            get => Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (value == Thread.CurrentThread.CurrentUICulture) return;
                Thread.CurrentThread.CurrentUICulture = value;
                switch (value.Name)
                {
                    case "ru-RU":
                        Application.Current.Resources.MergedDictionaries.Add(_russian);
                        Application.Current.Resources.MergedDictionaries.Remove(_english);
                        break;
                    default:
                        Application.Current.Resources.MergedDictionaries.Add(_english);
                        Application.Current.Resources.MergedDictionaries.Remove(_russian);
                        break;
                }
                LanguageChanged?.Invoke();
            }
        }
    }
}
