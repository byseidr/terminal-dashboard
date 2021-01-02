using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace terminal_dashboard
{
    class Program
    {
        public static int HWND_BOTTOM = 1;

        // FindWindow
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        // SetWindowPos
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        private static List<object> getProfile(string arg)
        {
            string[] profile = arg.Split(";");
            List<object> result = new List<object>();

            // Terminal profile
            result.Add(profile[0]);
            // Window x value
            result.Add(profile.Length > 1 ? Int32.Parse(profile[1]) : 200);
            // Window y value
            result.Add(profile.Length > 2 ? Int32.Parse(profile[2]) : 100);
            // Window width
            result.Add(profile.Length > 3 ? Int32.Parse(profile[3]) : 1000);
            // Window height
            result.Add(profile.Length > 4 ? Int32.Parse(profile[4]) : 600);

            // Window z reference
            // Defaults to bottommost position if not explicited
            result.Add(profile.Length > 5 ? Int32.Parse(profile[5]) : HWND_BOTTOM);

            return result;
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (string arg in args)
                {
                    List<object> profile = getProfile(arg);
                    Process p = new Process();
                    p.StartInfo.FileName = "wt.exe";
                    p.StartInfo.Arguments = "-p \"" + (string)profile[0] + "\"";
                    p.Start();
                    while (FindWindow(null, (string)profile[0]) == IntPtr.Zero)
                    {
                    }
                    IntPtr hwnd = FindWindow(null, (string)profile[0]);
                    SetWindowPos(hwnd, (int)profile[5], (int)profile[1], (int)profile[2], (int)profile[3], (int)profile[4], 0x00);
                }
            }
        }
    }
}
