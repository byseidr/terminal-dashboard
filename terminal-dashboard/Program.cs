using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace terminal_dashboard
{
    class Program
    {
        private static Dictionary<string, int> zPos = new Dictionary<string, int>(){
            {"bottom", 1},
            {"top", 0},
            {"topmost", -1},
            {"notopmost", -2}
        };

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
            result.Add(profile.Length > 1 && !String.IsNullOrEmpty(profile[1]) ? Int32.Parse(profile[1]) : 200);
            // Window y value
            result.Add(profile.Length > 2 && !String.IsNullOrEmpty(profile[2]) ? Int32.Parse(profile[2]) : 100);
            // Window width
            result.Add(profile.Length > 3 && !String.IsNullOrEmpty(profile[3]) ? Int32.Parse(profile[3]) : 1000);
            // Window height
            result.Add(profile.Length > 4 && !String.IsNullOrEmpty(profile[4]) ? Int32.Parse(profile[4]) : 600);
            // Window z reference
            // Defaults to bottommost position if not explicited
            result.Add(profile.Length > 5 && !String.IsNullOrEmpty(profile[5]) ? zPos[profile[5]] : zPos["notopmost"]);
            // Terminal executable and option (if any)
            // Defaults to Windows Terminal
            if (profile.Length > 6 && !String.IsNullOrEmpty(profile[6]))
            {
                result.Add(profile[6]);
                result.Add(profile.Length > 7 && !String.IsNullOrEmpty(profile[7]) ? profile[7] + " " : "");
            }
            else
            {
                result.Add("wt.exe");
                result.Add("-p" + " ");
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
                    p.StartInfo.FileName = (string)profile[6];
                    p.StartInfo.Arguments = (string)profile[7] + "\"" + (string)profile[0] + "\"";
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
