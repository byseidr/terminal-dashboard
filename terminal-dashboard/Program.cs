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
            result.Add(Int32.Parse(profile[1]));
            // Window y value
            result.Add(Int32.Parse(profile[2]));
            // Window width
            result.Add(Int32.Parse(profile[3]));
            // Window height
            result.Add(Int32.Parse(profile[4]));

            // Window z reference
            // Defaults to bottommost position if not explicited
            if (profile.Length > 5)
            {
                result.Add(Int32.Parse(profile[5]));
            }
            else
            {
                result.Add(HWND_BOTTOM);
            }

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
