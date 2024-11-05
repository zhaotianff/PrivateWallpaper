using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWallpaper.Util
{
    public class ProcessHelper
    {
        public static string ExecuteAndGetOutput(string exePath, string args)
        {
            Process process = new Process();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = false;
            psi.FileName = exePath;
            psi.Arguments = args;
            psi.RedirectStandardError = true;
            psi.RedirectStandardOutput = true;
            process.StartInfo = psi;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return output;
        }
    }
}
