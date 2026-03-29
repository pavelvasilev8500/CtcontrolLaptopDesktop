using ClassesLibrary.DI.Interfaces;
using System.Windows.Forms;

namespace ClassesLibrary.DI.Classes
{
    internal class CheckLaptop : ICheckLaptop
    {
        public bool IsLaptop()
        {
            if (SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery || SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.Unknown)
                return false;
            else return true;
        }
    }
}
