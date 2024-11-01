using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWallpaper.AdminTask
{
    internal class Program
    {
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

        private static void WriteToShell(string path)
        {
            try
            {
                var winLogonKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);

                if (winLogonKey != null)
                {
                    winLogonKey.SetValue("Shell", path, Microsoft.Win32.RegistryValueKind.String);
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
