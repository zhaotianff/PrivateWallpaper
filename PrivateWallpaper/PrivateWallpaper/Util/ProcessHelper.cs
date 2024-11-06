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
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = args;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return output;
        }

        public static void Execute(string exePath, string args)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = exePath;
            process.StartInfo.Arguments = args;
            process.Start();
            process.WaitForExit();
            process.Close();
        }
    }
}
