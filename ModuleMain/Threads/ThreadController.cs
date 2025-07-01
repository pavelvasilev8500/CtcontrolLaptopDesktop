using ClassesLibrary.SystemInfo;
using ModuleMain.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModuleMain.Threads
{
    public static class ThreadController
    {
        public static ObservableCollection<InfoModel> Info { get; private set; } = new ObservableCollection<InfoModel>()
        {
            new InfoModel()
        };
        private static Thread _update;
        public static System.Windows.Forms.Timer Timer { get;} = new System.Windows.Forms.Timer();

        public static void StartTimer()
        {
            Timer.Enabled = true;
            Timer.Interval = 1000;
            Timer.Start();
        }
        public static void UpdateData(CancellationTokenSource cts)
        {
            _update = new Thread(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    var date = SystemInfo.GetDate();
                    var workTime = SystemInfo.GetPcWorkTime();
                    var temperature = SystemInfo.GetTemperature();
                    Info[0] = new InfoModel()
                    {
                        Date = date.Item1,
                        Time = date.Item2,
                        Day = date.Item4,
                        Worktimeday = workTime.Item1,
                        Worktimehour = workTime.Item2,
                        Worktimeminut = workTime.Item3,
                        Batary = SystemInfo.GetNotebookBatary(),
                        Cputemperature = temperature.Item1,
                        Gputemperature = temperature.Item2,
                    };
                    Thread.Sleep(1000);
                }
            });
        }

        public static void StartUpdate()
        {
            _update.Name = "Update";
            _update.Start();
        }
    }
}
