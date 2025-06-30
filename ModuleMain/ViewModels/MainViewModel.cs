using Prism.Mvvm;
using ClassesLibrary.SystemInfo;
using Prism.Regions;
using System.Threading;
using System;
using Prism.Events;
using ClassesLibrary.Classes;
using System.Windows;
using ModuleSettings.Settings;
using ModuleMain.Models;
using System.Threading.Tasks;
using System.Diagnostics;
using ImTools;
using ModuleMain.Threads;

namespace ModuleMain.ViewModels
{
    public class MainViewModel : BindableBase, IRegionMemberLifetime, IConfirmNavigationRequest
    {
        IEventAggregator _ea;
        private string _date;
        private string _time;
        private string _second;
        private string _day;
        private string _worktimeday;
        private string _worktimehour;
        private string _worktimeminut;
        private string _worktimesecond;
        private string _batary;
        private string _cputemperature;
        private string _gputemperature;
        private Visibility _secondVisibility;
        private Visibility _batteryVisibility;
        private bool IsLaptop { get; set; }
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }
        public string Second
        {
            get { return _second; }
            set { SetProperty(ref _second, value); }
        }
        public string Day
        {
            get { return _day; }
            set { SetProperty(ref _day, value); }
        }
        public string WorkTimeDay
        {
            get { return _worktimeday; }
            set { SetProperty(ref _worktimeday, value); }
        }
        public string WorkTimeHour
        {
            get { return _worktimehour; }
            set { SetProperty(ref _worktimehour, value); }
        }
        public string WorkTimeMinut
        {
            get { return _worktimeminut; }
            set { SetProperty(ref _worktimeminut, value); }
        }
        public string WorkTimeSecond
        {
            get { return _worktimesecond; }
            set { SetProperty(ref _worktimesecond, value); }
        }
        public string Batary
        {
            get { return _batary; }
            set { SetProperty(ref _batary, value); }
        }
        public string CPUtemperature
        {
            get { return _cputemperature; }
            set 
            {
                SetProperty(ref _cputemperature, value); 
            }
        }
        public string GPUtemperature
        {
            get { return _gputemperature; }
            set 
            { 
                SetProperty(ref _gputemperature, value); 
            }
        }
        public Visibility SecondVisibility
        {
            get
            {
                return _secondVisibility;
            }
            set
            {
                SetProperty(ref _secondVisibility, value);
            }
        }
        public Visibility BatteryVisibility
        {
            get
            {
                return _batteryVisibility;
            }
            set
            {
                SetProperty(ref _batteryVisibility, value);
            }
        }
        public bool KeepAlive
        {
            get { return false; }
        }
        public MainViewModel(IEventAggregator ea)
        {
            _ea = ea;
            _ea.GetEvent<SendBoolEvent>().Subscribe(BoolMessageRecived);
            SecondVisibility = AppSettings.ShowSeconds ? Visibility.Visible : Visibility.Hidden;
            ThreadController.Info.CollectionChanged += _info_CollectionChanged;
            ThreadController.Timer.Tick += UpdateSecondsTimer;
        }

        private void _info_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    if (e.NewItems[0] is InfoModel im)
                    {
                        Date = im.Date;
                        Time = im.Time;
                        Day = im.Day;
                        WorkTimeDay = im.Worktimeday;
                        WorkTimeHour = im.Worktimehour;
                        WorkTimeMinut = im.Worktimeminut;
                        Batary = im.Batary;
                        CPUtemperature = im.Cputemperature;
                        GPUtemperature = im.Gputemperature;
                    }
                    break;
            }
        }

        private void BoolMessageRecived(bool islaptop)
        {
            IsLaptop = islaptop;
            if (IsLaptop)
                _batteryVisibility = Visibility.Visible;
            else
                _batteryVisibility = Visibility.Hidden;
        }

        private void UpdateSecondsTimer(object sender, EventArgs e)
        {
            Second = SystemInfo.GetDate().Item3;
            WorkTimeSecond = SystemInfo.GetPcWorkTime().Item4;
        }
        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
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
