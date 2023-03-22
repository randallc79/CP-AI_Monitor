using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Runtime.InteropServices;

namespace CP_AI_Monitor
{
    public partial class MainForm : Form
    {
        private static readonly string LogFilePath = $@"C:\Program Files\CodeProject\AI\logs\log-{DateTime.Today:yyyy-MM-dd}.log";
        private const string ServiceName = "CodeProject.AI Server";
        private const string ErrorString = "Exception";
        private const int ServiceRestartDelay = 30 * 1000; // 30 seconds in milliseconds

        //private const string LogFilePath = $@"C:\Program Files\CodeProject\AI\logs\log-{DateTime.Today:yyyy-MM-dd}.log";
        //private const string ServiceName = "CodeProject.AI Server";
        //private const string ErrorString = "YOUR_ERROR_STRING";
        //private const int ServiceRestartDelay = 30 * 1000; // 30 seconds in milliseconds

        private int errorsDetected = 0;
        private int autoRestartsPerformed = 0;
        private FileSystemWatcher? watcher;

        private int lastProcessedLine = -1;

        public MainForm()
        {
            InitializeComponent();
        }

        private bool ValidateLogFilePath()
        {
            if (string.IsNullOrWhiteSpace(LogFilePath))
            {
                MessageBox.Show("Log file path is not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string directoryPath = Path.GetDirectoryName(LogFilePath) ?? string.Empty;
            string fileName = Path.GetFileName(LogFilePath) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(directoryPath) || string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Log file path is not valid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show($"Directory '{directoryPath}' does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!ValidateLogFilePath())
            {
                return;
            }

            string directoryPath = Path.GetDirectoryName(LogFilePath) ?? string.Empty;
            string fileName = Path.GetFileName(LogFilePath) ?? string.Empty;

            // Initialize and configure the FileSystemWatcher
            watcher = new FileSystemWatcher
            {
                Path = directoryPath,
                Filter = fileName,
                NotifyFilter = NotifyFilters.LastWrite
            };

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            // Update the service status
            UpdateServiceStatus();

            // Hide the form
            this.Hide();
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(LogFilePath) ?? string.Empty;
            watcher.Filter = Path.GetFileName(LogFilePath) ?? string.Empty;
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
        
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            try
            {
                string[] lines = File.ReadAllLines(LogFilePath);
                for (int i = lines.Length - 1; i > lastProcessedLine; i--)
                {
                    if (lines[i].Contains(ErrorString))
                    {
                        errorsDetected++;
                        lblErrorsDetected.Text = $"Errors detected: {errorsDetected}";

                        Console.WriteLine($"Error '{ErrorString}' detected. Restarting service...");
                        RestartService(ServiceName, ServiceRestartDelay);

                        autoRestartsPerformed++;
                        lblAutoRestarts.Text = $"Automatic restarts: {autoRestartsPerformed}";

                        lastProcessedLine = i;
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
                using ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }

                Thread.Sleep(delay);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while restarting service: {ex.Message}");
            }
        }
        
        private void UpdateServiceStatus()
        {
            try
            {
                using ServiceController service = new ServiceController(ServiceName);
                lblServiceStatus.Text = $"Service status: {service.Status}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while retrieving service status: {ex.Message}");
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

        private void btnRestartService_Click(object sender, EventArgs e)
        {
            RestartService(ServiceName, ServiceRestartDelay);
            UpdateServiceStatus();
        }
    }
}