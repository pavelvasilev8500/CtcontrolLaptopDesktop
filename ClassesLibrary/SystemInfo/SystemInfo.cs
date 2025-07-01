using System;
using System.Globalization;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;

namespace ClassesLibrary.SystemInfo
{
    public static class SystemInfo
    {
        private static string _cTemperature = "N/A";
        private static string _gTemperature = "N/A";

        public static Computer computer = new Computer()
        {
            CPUEnabled = true,
            GPUEnabled = true,
        };

        public static (string, string, string, string) GetDate()
        {
            DateTime now = DateTime.Now;
            CultureInfo culture = CultureInfo.CurrentUICulture;
            return (
                string.Format(now.ToString("dd ") + $"{now.ToString("Y", culture)}"),
                now.ToString("HH:mm"),
                now.ToString("ss"),
                char.ToUpper(now.ToString("dddd", culture)[0]) + now.ToString("dddd", culture).Substring(1)
                );
        }

        public static (string, string, string, string) GetPcWorkTime()
        {
            int systemUptime = Environment.TickCount;
            var ts = TimeSpan.FromMilliseconds(systemUptime);
            return (
                string.Format($"{ts.Days}"),
                string.Format($"{ts.Hours}"),
                string.Format($"{ts.Minutes}"),
                string.Format($"{ts.Seconds}")
                );
        }
        public static string GetNotebookBatary()
        {
            return (SystemInformation.PowerStatus.BatteryLifePercent * 100).ToString() + "%";
        }
        public static float GetNotebookBataryFloat()
        {
            return (SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }
        public static (string, string) GetTemperature()
        {
            try
            {
                foreach (var hardware in computer.Hardware)
                {
                    hardware.Update();
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            if (hardware.HardwareType == HardwareType.CPU)
                                _cTemperature = sensor.Value.ToString().Substring(0, 2) + " ℃ " + $"({hardware.Name})";
                            if (hardware.HardwareType.ToString().ToLower().Contains("gpu"))
                                _gTemperature = sensor.Value.ToString() + " ℃ " + $"({hardware.Name})";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return (_cTemperature, _gTemperature);
        }
    }
}
