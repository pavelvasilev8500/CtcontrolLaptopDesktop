using ModuleSettings.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ResourcesLibrary.Resources.Wallpapers.Classes;
using System;
using System.Linq;
using System.Windows;

namespace ModuleSettings.ViewModels
{
    class WallpaperSettingsViewModel : BindableBase, IRegionMemberLifetime, IConfirmNavigationRequest
    {
        private readonly IRegionManager _regionManager;
        private int SelectedItem { get; set; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand NextCommand { get; set; }
        public DelegateCommand ApplyCommand { get; set; }
        public DelegateCommand PreviousCommand { get; set; }
        public bool KeepAlive => false;
        public WallpaperSettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            for (int i = 0; i < Wallpapers.WallpaperDictionaries.Keys.Count; i++)
                if (Wallpapers.WallpaperDictionaries.Keys.ToList()[i] == AppSettings.Wallpaper)
                    SelectedItem = i;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            NextCommand = new DelegateCommand(Next);
            ApplyCommand = new DelegateCommand(Apply);
            PreviousCommand = new DelegateCommand(Previous);
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
        private void Next()
        {
            SelectedItem++;
            if (SelectedItem >= Wallpapers.WallpaperDictionaries.Keys.Count)
                SelectedItem = 0;
            Wallpapers.Wallpaper = Wallpapers.WallpaperDictionaries.Keys.ToList()[SelectedItem];
        }
        private void Apply()
        {
            AppSettings.Wallpaper = Wallpapers.Wallpaper;
        }
        private void Previous()
        {
            if (SelectedItem <= 0)
                SelectedItem = Wallpapers.WallpaperDictionaries.Keys.Count;
            SelectedItem--;
            Wallpapers.Wallpaper = Wallpapers.WallpaperDictionaries.Keys.ToList()[SelectedItem];
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            Apply();
            continuationCallback(true);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
