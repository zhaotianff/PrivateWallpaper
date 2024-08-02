using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PrivateWallpaper.PInvoke
{
    public class User32
    {
        public static readonly uint SPI_SETDESKWALLPAPER = 0x0014;
        public static readonly uint SPIF_UPDATEINIFILE = 0x0001;

        public static readonly uint EVENT_OBJECT_LOCATIONCHANGE = 0x800B;
        public static readonly uint WINEVENT_OUTOFCONTEXT = 0x0000; // Events are ASYNC
        public static readonly uint WINEVENT_SKIPOWNPROCESS = 0x0002; // Don't call back for events on installer's process

        public static readonly uint SW_SHOWMAXIMIZED = 3;

        public delegate void Wineventproc(IntPtr hWinEventHook, uint eventId, IntPtr hwnd, int idObject, int idChild, uint idEventThread, uint dwmsEventTime);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam,string lpImagePath, uint fWinIni);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, Wineventproc pfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("User32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("User32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("User32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref System.Drawing.Rectangle lpRect);
    }

    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
        public System.Drawing.Rectangle rcDevice;
    };
}
