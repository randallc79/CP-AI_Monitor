using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Runtime.InteropServices;

namespace LogFileMonitor
{
    class Program
    {
        private static readonly string LogFilePath = $@"C:\Program Files\CodeProject\AI\logs\log-{DateTime.Today:yyyy-MM-dd}.log";
        private const string ServiceName = "CodeProject.AI Server";
        private const string ErrorString = "Exception";
        private const int ServiceRestartDelay = 30 * 1000; // 30 seconds in milliseconds

        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(LogFilePath);
            watcher.Filter = Path.GetFileName(LogFilePath);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Monitoring log file for errors...");
            using (watcher)
            {
                // Hide console window to make the application run in the background
                IntPtr hWnd = NativeMethods.GetConsoleWindow();
                NativeMethods.ShowWindow(hWnd, NativeMethods.SW_HIDE);

                // Keep the application running in the background
                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(LogFilePath);
                foreach (string line in lines)
                {
                    if (line.Contains(ErrorString))
{                   {
                        Console.WriteLine($"Error '{ErrorString}' detected. Restarting service...");
                        RestartService(ServiceName, ServiceRestartDelay);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading log file: {ex.Message}");
            }
        }

        private static void RestartService(string serviceName, int delay)
        {
            try
            {
                using (ServiceController service = new ServiceController(serviceName))
                {
                    if (service.Status != ServiceControllerStatus.Stopped)
                    {
                        service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }

                    Thread.Sleep(delay);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while restarting service: {ex.Message}");
            }
        }

        public static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetConsoleWindow();

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;
        }
    }
}
