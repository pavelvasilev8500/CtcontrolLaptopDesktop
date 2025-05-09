using Prism.Mvvm;
using ClassesLibrary.SystemInfo;
using ClassesLibrary.SystemControls;
using Prism.Regions;
using Prism.Commands;
using System;
using System.Threading;
using Prism.Events;
using ClassesLibrary.Classes;
using System.Windows;
using ModuleSettings.Settings;
using ModuleMain.Threads;
using ModuleMain.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Windows.Forms;

namespace ModuleMain.ViewModels
{
    public class ControlViewModel : BindableBase, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        private string _worktimeday;
        private string _worktimehour;
        private string _worktimeminut;
        private string _worktimesecond;
        private string _batary;
        private string _cputemperature;
        private string _gputemperature;
        private readonly IRegionManager _regionManager;
        IEventAggregator _ea;
        private Visibility _batteryVisibility;
        public string WorkTimeDay
        {
            get => _worktimeday;
            set => SetProperty(ref _worktimeday, value);
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
            set { SetProperty(ref _cputemperature, value); }
        }
        public string GPUtemperature
        {
            get { return _gputemperature; }
            set { SetProperty(ref _gputemperature, value); }
        }
        public Visibility BatteryVisibility
        {
            get => _batteryVisibility;
            set =>  SetProperty(ref _batteryVisibility, value);
        }
        private bool IsLaptop { get; set; }
        public bool KeepAlive => false;

        public DelegateCommand<string> NavigateCommand { get; set; }
        public DelegateCommand ShutdonCommand { get; private set; }
        public DelegateCommand RestartCommand { get; private set; }
        public DelegateCommand SleepCommand { get; private set; }
        public ControlViewModel(IRegionManager regionManager, IEventAggregator ea)
        {
            _regionManager = regionManager;
            _ea = ea;
            _ea.GetEvent<SendBoolEvent>().Subscribe(BoolMessageRecived);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            ShutdonCommand = new DelegateCommand(Shutdown);
            RestartCommand = new DelegateCommand(Restart);
            SleepCommand = new DelegateCommand(Sleep);
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
            WorkTimeSecond = SystemInfo.GetPcWorkTime().Item4;
        }
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }
        private void Shutdown()
        {
            SystemControl.halt(false, false);
        }
        private void Restart()
        {
            SystemControl.halt(true, false);
        }
        private void Sleep()
        {
            SystemControl.Sleep(false, false, false);
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        //Put.PutData(statusuri, id, CreateJson.CreateDataJson(new ClassesLibrary.DataModels.StatusDataModel(), id, status));
        #region VMFunctions
        public void OnNavigatedTo(NavigationContext navigationContext){}
        public bool IsNavigationTarget(NavigationContext navigationContext){ return true;}
        public void OnNavigatedFrom(NavigationContext navigationContext){}
        #endregion
    }
}
