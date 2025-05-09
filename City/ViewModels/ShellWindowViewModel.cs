using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ResourcesLibrary.Resources.Languages.Classes;
using ResourcesLibrary.Resources.Wallpapers.Classes;
using System.Diagnostics;
using Prism.Events;
using ClassesLibrary.Client;
using ClassesLibrary.Classes;
using System.Windows;
using System.Collections.Generic;
using City.MainWindowClasses;
using WarningDialog.Classes;
using ClassesLibrary.SystemInfo;
using ModuleSettings.Settings;
using City.Models;
using Newtonsoft.Json.Serialization;

namespace City.ViewModels
{
    class ShellWindowViewModel : BindableBase
    {
        private IRegion _region;
        private readonly IRegionManager _regionManager;
        IEventAggregator _ea;
        private ObservableCollection<object> _views = new ObservableCollection<object>();
        private List<string> _oldViews = new List<string>();
        private string _newObject;
        private static string ClientId { get; set; }
        private static bool IsLaptop { get; set; }
        public DelegateCommand CloseAppCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; set; }
        public ObservableCollection<object> Views
        {
            get { return _views; }
            set { SetProperty(ref _views, value); }
        }
        private string NewObject
        {
            get { return _newObject; }
            set { _newObject = value; }
        }
        private Visibility backButtonVisible = Visibility.Hidden;
        public Visibility BackButtonVisible
        {
            get { return backButtonVisible; }
            set { SetProperty(ref backButtonVisible, value); }
        }
        private Visibility mainButtonVisible = Visibility.Visible;
        public Visibility MainButtonVisible
        {
            get { return mainButtonVisible; }
            set { SetProperty(ref mainButtonVisible, value); }
        }
        private Visibility markerVisisbiliry = Visibility.Visible;
        public Visibility MarkerVivibiliti
        {
            get { return markerVisisbiliry; }
            set { SetProperty(ref markerVisisbiliry, value); }
        }
        private string commandNavigationParameter;
        public string CommandNavigationParameter
        {
            get { return commandNavigationParameter; }
            set { SetProperty(ref commandNavigationParameter, value); }
        }
        private bool isButtonEnabled = true;
        public bool IsButtonEnabled
        {
            get { return isButtonEnabled; }
            set { SetProperty(ref isButtonEnabled, value); }
        }

        public ShellWindowViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            SystemInfo.computer.Open();
            _ea = ea;
            IsLaptop = LaptopCheck.IsPcLaptop();
            new CheckProgramStart(Process.GetCurrentProcess());
            new RunAsAdministrator();
            new BatteryCheck();
            CloseAppCommand = new DelegateCommand(CloseApp);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            _regionManager = regionManager;
            _regionManager.Regions.CollectionChanged += Regions_CollectionChanged;
        }

        private void Regions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                _region = (IRegion)e.NewItems[0];
                _region.Views.CollectionChanged += Views_CollectionChanged;
            }
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var a = _region.Views;
                var b = a.GetType();
                Views.Add(e.NewItems[0].GetType().Name);
                NewObject = e.NewItems[0].GetType().Name;
                switch(NewObject)
                {
                    case "MainView":
                        OnMainViewsNavigation(e);
                        break;
                    case "ControlView":
                        OnMainViewsNavigation(e);
                        break;
                    case "MobileView":
                        break;
                    case "MainSettingsView":
                        MarkerVivibiliti = Visibility.Hidden;
                        MainButtonVisible = Visibility.Hidden;
                        BackButtonVisible = Visibility.Visible;
                        IsButtonEnabled = false;
                        CommandNavigationParameter = _oldViews[_oldViews.Count - 1];
                        SettingsInitialazation.Set();
                        break;
                    case "LanguageSettingsView":
                        OnSettingNavigation(e);
                        break;
                    case "WallpaperSettingsView":
                        OnSettingNavigation(e);
                        break;
                    case "CommonSettingsView":
                        OnSettingNavigation(e);
                        break;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                Views.Remove(e.OldItems[0].GetType().Name);
            }
        }

        private void OnMainViewsNavigation(NotifyCollectionChangedEventArgs e)
        {
            MarkerVivibiliti = Visibility.Visible;
            IsButtonEnabled = true;
            _oldViews.Clear();
            _oldViews.Add(e.NewItems[0].GetType().Name);
        }

        private void OnSettingNavigation(NotifyCollectionChangedEventArgs e)
        {
            MarkerVivibiliti = Visibility.Hidden;
            MainButtonVisible = Visibility.Hidden;
            BackButtonVisible = Visibility.Visible;
            IsButtonEnabled = false;
            CommandNavigationParameter = "MainSettingsView";
        }

        private void Navigate(string navigatePath)
        {

            if (navigatePath != null)
            {
                if (NewObject != navigatePath)
                    _regionManager.RequestNavigate("ContentRegion", navigatePath);
            }
            if(navigatePath == "MainView")
            {
                MainButtonVisible = Visibility.Visible;
                BackButtonVisible = Visibility.Hidden;
                _ea.GetEvent<SendBoolEvent>().Publish(IsLaptop);
            }
            if(navigatePath == "ControlView")
            {
                MainButtonVisible = Visibility.Visible;
                BackButtonVisible = Visibility.Hidden;
                _ea.GetEvent<SendBoolEvent>().Publish(IsLaptop);
            }
            if(navigatePath == "MobileView")
            {
                MainButtonVisible = Visibility.Visible;
                BackButtonVisible = Visibility.Hidden;
                //_ea.GetEvent<SendIdEvent>().Publish(Id());
            }
        }

        //private static string Id()
        //{
        //    if (Properties.Settings.Default.FirstStart)
        //    {
        //        Properties.Settings.Default.ClientId = GenerateClientId.Id();
        //        ClientId = Properties.Settings.Default.ClientId;
        //        Properties.Settings.Default.FirstStart = false;
        //        Properties.Settings.Default.Save();
        //    }
        //    else
        //    {
        //        ClientId = Properties.Settings.Default.ClientId;
        //    }
        //    return ClientId;
        //}

        private void CloseApp()
        {
            Environment.Exit(0);
        }
    }
}