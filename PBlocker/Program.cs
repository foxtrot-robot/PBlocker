using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using System.IO;

namespace ConsoleApp2
{
    class config
    {
        // Config variables

        public static bool HideConsoleWindow = false;
        public static string[] lines = { "\n127.0.0.1 pornhub.com", "127.0.0.1 brazzers.com", "127.0.0.1 xvideos.com", "127.0.0.1 indian-porn.com",
                        "127.0.0.1 fuq.com", "127.0.0.1 youporn.com", "127.0.0.1 xnxx.com", "127.0.0.1 toppornsites.com"};
        public static bool AutoRun = false;
        public static string AutoRunName = "Windows Service";
        public static string PersistencePath = @$"C:\Users\{Environment.UserName}\AppData\Roaming\PBlocker\pb.exe";
        public static bool InstallSelf = true;
        public static bool Hidden = false;
        public static string logfile = @$"C:\Users\{Environment.UserName}\AppData\Roaming\PBlocker\log.txt";
    }
    class Program
    {
        // DLL Imports
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool Window(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            HideConsoleWindow();
            persistence.InstallSelf();
            persistence.AutoRun();
            ModifyHostsFile();
            persistence.CheckEditedHosts();
        }
        private static void HideConsoleWindow()
        {
            if (config.HideConsoleWindow == true)
            {
                IntPtr handle = GetConsoleWindow();
                Window(handle, 0);
            }
        }
        public static void ModifyHostsFile()
        {
            try
            {
                using (StreamWriter file = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts")))
                {
                    //string[] lines = { "\n127.0.0.1 pornhub.com", "127.0.0.1 brazzers.com", "127.0.0.1 xvideos.com", "127.0.0.1 indian-porn.com",
                    //    "127.0.0.1 fuq.com", "127.0.0.1 youporn.com", "127.0.0.1 xnxx.com", "127.0.0.1 toppornsites.com"};

                    foreach (string line in config.lines)
                    {
                        file.WriteLine(line);
                    }

                    // file.WriteLine(someline);  
                }
            }
            catch (Exception e)
            {
                // Writes exception information to file (path: C:\Users\{user}\AppData\Roaming\PBlocker\log.txt)

                string path = Environment.SpecialFolder.CommonApplicationData + "\\PBlocker\\log.txt";
                File.Create(
                    path
                );
                using (StreamWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine($"Exception catched: {e}");
                }
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
