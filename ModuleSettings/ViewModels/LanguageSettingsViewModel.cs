using ClassesLibrary.Classes;
using ModuleSettings.Settings;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using ResourcesLibrary.Resources.Languages.Classes;
using System.Collections.Generic;
using System.Globalization;

namespace ModuleSettings.ViewModels
{
    public class LanguageSettingsViewModel : BindableBase, IRegionMemberLifetime
    {
        private readonly IRegionManager _regionManager;
        IEventAggregator _ea;
        public List<CultureInfo> LanguagesCulture { get; set; } = Languages.All_Languages;
        private CultureInfo selectedItem;
        public CultureInfo SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                foreach(var l in Languages.All_Languages)
                {
                    if (selectedItem == l)
                        Languages.Language = l;
                }
            }
        }
        public bool KeepAlive => false;

        public DelegateCommand<string> NavigateCommand { get; set; }
        public LanguageSettingsViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _ea = ea;
            NavigateCommand = new DelegateCommand<string>(Navigate);
            SelectedItem = AppSettings.Language;
            Languages.LanguageChanged += Languages_LanguageChanged;
        }
        private void Languages_LanguageChanged()
        {
            AppSettings.Language = SelectedItem;
            _ea.GetEvent<SendLanguageEvent>().Publish(Languages.Language);
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
    }
}
