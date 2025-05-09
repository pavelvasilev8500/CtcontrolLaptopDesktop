using System.Windows;
using City.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using ModuleMain;
using ModuleSettings;
using ModuleMobile;
using ResourcesLibrary.Resources.Wallpapers.Classes;
using ResourcesLibrary.Resources.Languages.Classes;
using ModuleSettings.Settings;
using City.Properties;
using ClassesLibrary.Client;
using System.Threading;
using ModuleMain.Threads;

namespace City
{
    public partial class App : PrismApplication
    {
        //Settings File
        //c:\Users\City\AppData\Local\[User]\Ctcontrol_Notebook.exe_Url_dpv31hor4zp05mavjepss4elamcs3gax\0.0.0.0\
        CancellationTokenSource cts = new CancellationTokenSource();
        protected override Window CreateShell()
        {
            if (Settings.Default.FirstStart)
            {
                SettingsInitialazation.Set();
                Settings.Default.FirstStart = false;
                Settings.Default.ClientId = GenerateClientId.Id();
                Settings.Default.Save();
            }
            else
            {
                var settings = SettingsInitialazation.Get(); 
                Languages.Language = AppSettings.Language = settings.Language;
                Wallpapers.Wallpaper = AppSettings.Wallpaper = settings.Wallpaper;
                AppSettings.ShowSeconds = settings.ShowSeconds;
                AppSettings.Autorun = settings.Autorun;
            }
            ThreadController.StartTimer();
            ThreadController.UpdateData(cts);
            ThreadController.StartUpdate();
            return Container.Resolve<ShellWindow>();
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleMainModule>();
            moduleCatalog.AddModule<ModuleSettingsModule>();
            moduleCatalog.AddModule<ModuleMobileModule>();
        }
    }
}
