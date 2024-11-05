using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrivateWallpaper.AdminTask
{
    internal class Program
    {
        /// <summary>
        /// main(must x64)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length != 2)
                return;

            var path = args[0];
            var mode = args[1];

            if(mode.ToUpper() == "INSTALL")
            {
                Install(path);
            }
            else if(mode.ToUpper() == "USERINIT")
            {
                RunUserInit();
            }
            else
            {
                Uninstall();
            }
        }

        private static void Install(string path)
        {
            if (System.IO.File.Exists(path) == false)
                return;

            WriteToShell(path);
        }

        private static void Uninstall()
        {
            WriteToShell("explorer.exe");
        }

        private static void RunUserInit()
        {
            try
            {
                var winLogonKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);

                if (winLogonKey != null)
                {
                    var shellPath = winLogonKey.GetValue("Shell");
                    winLogonKey.SetValue("Shell", "explorer.exe", Microsoft.Win32.RegistryValueKind.String);
                    var userInitPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\userinit.exe";
                    Thread.Sleep(100);
                    System.Diagnostics.Process.Start(userInitPath);
                    Thread.Sleep(100);
                    winLogonKey.SetValue("Shell", shellPath, Microsoft.Win32.RegistryValueKind.String);
                    winLogonKey.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteToShell(string path)
        {
            try
            {
                var winLogonKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);

                if (winLogonKey != null)
                {
                    winLogonKey.SetValue("Shell", path + " selector", Microsoft.Win32.RegistryValueKind.String);
                    winLogonKey.Dispose();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
