using System.Windows;
using System.Diagnostics;
using System.Security.Principal;

namespace GpsEmulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (!pricipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                Process p = Process.GetCurrentProcess();
                processInfo.FileName = appStartPath + "\\" + p.ProcessName + ".exe";
                try
                {
                    Process.Start(processInfo);
                }
                catch
                {
                    //Do nothing. Probably the user canceled the UAC window
                }
                Application.Current.Shutdown();
            }
        }
    }
}
