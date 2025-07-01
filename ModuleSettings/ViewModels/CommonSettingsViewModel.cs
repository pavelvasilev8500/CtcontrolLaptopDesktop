using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ModuleSettings.Settings;
using ClassesLibrary.SystemControls;

namespace ModuleSettings.ViewModels
{
    public class CommonSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }

        private bool _autostartSwitcher;
        private bool _secondSwitcher;
        public bool AutostartSwitcher
        {
            get => _autostartSwitcher;
            set
            {
                SetProperty(ref _autostartSwitcher, value);
                AppSettings.Autorun = value;
                Autorun.SetAutorunValue(value);
            }
        }

        public bool SecondSwitcher
        {
            get => _secondSwitcher;
            set
            {
                SetProperty(ref _secondSwitcher, value);
                AppSettings.ShowSeconds = value;
            }
        }

        public CommonSettingsViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            AutostartSwitcher = AppSettings.Autorun;
            SecondSwitcher = AppSettings.ShowSeconds;
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
        public bool KeepAlive => false;
    }
}
