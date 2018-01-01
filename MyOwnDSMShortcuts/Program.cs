﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

static class Win32
{

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string dllToLoad);

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);

}

namespace BeatificaBytes.Synology.Mods
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var hash = args.SingleOrDefault(arg => arg.StartsWith("hash:"));
            if (!string.IsNullOrEmpty(hash))
            {
                var path = hash.Replace("hash:", "");
                if (path == ".") path = AppDomain.CurrentDomain.BaseDirectory;
                Helper.ComputeMD5Hash(path);
            }
            else
            {
                Debugger.Launch();

                string open = null;
                var edit = args.SingleOrDefault(arg => arg.StartsWith("edit:"));
                if (!string.IsNullOrEmpty(edit))
                {
                    open = edit.Replace("edit:", "");
                }
                else if (Properties.Settings.Default.UpgradeRequired)
                {
                    Properties.Settings.Default.Upgrade();
                    Properties.Settings.Default.UpgradeRequired = false;
                    Properties.Settings.Default.Save();
                }
                                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(open));
            }
        }
    }
}
