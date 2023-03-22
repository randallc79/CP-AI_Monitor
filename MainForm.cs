using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Runtime.InteropServices;

namespace CP_AI_Monitor
{
    public partial class MainForm : Form
    {
        //private static readonly string LogFilePath = $@"C:\Program Files\CodeProject\AI\logs\log-{DateTime.Today:yyyy-MM-dd}.txt";
        private static string LogFilePath = string.Empty;
        private const string ServiceName = "CodeProject.AI Server";
        private const string ErrorString = "Exception";
        private const int ServiceRestartDelay = 30 * 1000; // 30 seconds in milliseconds

        private int errorsDetected = 0;
        private int autoRestartsPerformed = 0;
        private FileSystemWatcher? watcher;

        private int lastProcessedLine = -1;
        private static readonly string LastProcessedLineFile = "LastProcessedLine.txt";

        public MainForm()
        {
            InitializeComponent();
        }

        private bool ValidateLogFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Log file path is not defined.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string directoryPath = Path.GetDirectoryName(path) ?? string.Empty;
            string fileName = Path.GetFileName(path) ?? string.Empty;

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
            LoadConfig();

            if (!ValidateLogFilePath(LogFilePath))
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

            // Update the last processed log line
            LoadLastProcessedLine();

            // Update the service status
            UpdateServiceStatus();

            // Update the log file status
            UpdateLogFileDisplay();

            // Hide the form
            this.Hide();
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(LogFilePath) ?? string.Empty;
            watcher.Filter = Path.GetFileName(LogFilePath) ?? string.Empty;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            UpdateStatusDisplay("Monitoring log file for errors...");
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

        private async void OnChanged(object sender, FileSystemEventArgs e)
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

                        UpdateStatusDisplay($"Error '{ErrorString}' detected. Restarting service...");
                        await RestartService(ServiceName, ServiceRestartDelay);

                        autoRestartsPerformed++;
                        lblAutoRestarts.Text = $"Automatic restarts: {autoRestartsPerformed}";

                        lastProcessedLine = i;
                        SaveLastProcessedLine();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatusDisplay($"Error while reading log file: {ex.Message}");
            }
        }

        private async Task RestartService(string serviceName, int delay)
        {
            try
            {
                using ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    UpdateServiceStatusDisplay("Stopping service...");
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    UpdateServiceStatusDisplay("Service stopped.");
                    UpdateServiceStatus();
                }

                await Task.Delay(delay);
                UpdateServiceStatusDisplay("Starting service...");
                UpdateServiceStatus();

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
                UpdateServiceStatusDisplay("Service running.");
                UpdateServiceStatus();
            }
            catch (Exception ex)
            {
                UpdateStatusDisplay($"Error while restarting service: {ex.Message}");
                UpdateServiceStatus();
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
                UpdateStatusDisplay($"Error while retrieving service status: {ex.Message}");
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

        private async void btnRestartService_Click(object sender, EventArgs e)
        {
            await RestartService(ServiceName, ServiceRestartDelay);
            UpdateServiceStatus();
        }

        private void UpdateLogFileDisplay()
        {
            textBoxLogFile.Text = LogFilePath;
            bool logFileExists = File.Exists(LogFilePath);
            lblLogFileExists.Text = $"Log File Exists: {logFileExists}";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void UpdateServiceStatusDisplay(string status)
        {
            textBoxServiceStatus.Text = status;
            textBoxServiceStatus.Refresh();
        }

        private void UpdateStatusDisplay(string message)
        {
            textBoxStatus.AppendText($"{DateTime.Now}: {message}\r\n");
        }

        private void SaveLastProcessedLine()
        {
            try
            {
                File.WriteAllText(LastProcessedLineFile, lastProcessedLine.ToString());
            }
            catch (Exception ex)
            {
                UpdateStatusDisplay($"Error while saving last processed line: {ex.Message}");
            }
        }

        private void LoadLastProcessedLine()
        {
            try
            {
                if (File.Exists(LastProcessedLineFile))
                {
                    string line = File.ReadAllText(LastProcessedLineFile);
                    lastProcessedLine = int.Parse(line);
                }
                else
                {
                    if (File.Exists(LogFilePath))
                    {
                        string[] lines = File.ReadAllLines(LogFilePath);
                        lastProcessedLine = lines.Length - 1;
                    }
                    else
                    {
                        lastProcessedLine = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatusDisplay($"Error while loading last processed line: {ex.Message}");
            }
        }

        private void LoadConfig()
        {
            try
            {
                if (File.Exists("config.cfg"))
                {
                    string[] configLines = File.ReadAllLines("config.cfg");
                    foreach (string line in configLines)
                    {
                        if (line.StartsWith("LogFile="))
                        {
                            LogFilePath = line.Substring("LogFile=".Length);
                            LogFilePath = string.Format(LogFilePath, DateTime.Today);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("config.cfg file not found. Please ensure it exists in the application directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while loading config file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveConfig()
        {
            try
            {
                string content = $"LogFile={LogFilePath}";
                File.WriteAllText("config.cfg", content);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while saving config file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditLogFilePath()
        {
            string newPath = textBoxLogFile.Text;

            if (!ValidateLogFilePath(newPath))
            {
                MessageBox.Show("The new log file path is invalid. Please provide a valid path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LogFilePath = newPath;
            SaveConfig();
            UpdateLogFileDisplay();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EditLogFilePath();
        }
    }
}