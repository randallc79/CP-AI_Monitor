namespace CP_AI_Monitor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            notifyIcon1 = new NotifyIcon(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            maximizeToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            fileSystemWatcher1 = new FileSystemWatcher();
            btnRestartService = new Button();
            lblServiceStatus = new Label();
            lblErrorsDetected = new Label();
            lblAutoRestarts = new Label();
            lblLogFile = new Label();
            lblLogFileExists = new Label();
            textBoxLogFile = new TextBox();
            btnExit = new Button();
            textBoxServiceStatus = new TextBox();
            textBoxStatus = new TextBox();
            btnSave = new Button();
            textBoxLogDir = new TextBox();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "CP-AI Monitor";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { maximizeToolStripMenuItem, exitToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(126, 48);
            // 
            // maximizeToolStripMenuItem
            // 
            maximizeToolStripMenuItem.Name = "maximizeToolStripMenuItem";
            maximizeToolStripMenuItem.Size = new Size(125, 22);
            maximizeToolStripMenuItem.Text = "Maximize";
            maximizeToolStripMenuItem.Click += maximizeToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(125, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // fileSystemWatcher1
            // 
            fileSystemWatcher1.EnableRaisingEvents = true;
            fileSystemWatcher1.SynchronizingObject = this;
            fileSystemWatcher1.Changed += fileSystemWatcher1_Changed;
            // 
            // btnRestartService
            // 
            btnRestartService.Location = new Point(12, 415);
            btnRestartService.Name = "btnRestartService";
            btnRestartService.Size = new Size(107, 23);
            btnRestartService.TabIndex = 1;
            btnRestartService.Text = "Restart Service";
            btnRestartService.UseVisualStyleBackColor = true;
            btnRestartService.Click += btnRestartService_Click;
            // 
            // lblServiceStatus
            // 
            lblServiceStatus.AutoSize = true;
            lblServiceStatus.Location = new Point(12, 33);
            lblServiceStatus.Name = "lblServiceStatus";
            lblServiceStatus.Size = new Size(79, 15);
            lblServiceStatus.TabIndex = 2;
            lblServiceStatus.Text = "Service Status";
            // 
            // lblErrorsDetected
            // 
            lblErrorsDetected.AutoSize = true;
            lblErrorsDetected.Location = new Point(12, 9);
            lblErrorsDetected.Name = "lblErrorsDetected";
            lblErrorsDetected.Size = new Size(87, 15);
            lblErrorsDetected.TabIndex = 3;
            lblErrorsDetected.Text = "Errors Detected";
            // 
            // lblAutoRestarts
            // 
            lblAutoRestarts.AutoSize = true;
            lblAutoRestarts.Location = new Point(12, 88);
            lblAutoRestarts.Name = "lblAutoRestarts";
            lblAutoRestarts.RightToLeft = RightToLeft.Yes;
            lblAutoRestarts.Size = new Size(77, 15);
            lblAutoRestarts.TabIndex = 4;
            lblAutoRestarts.Text = "Auto Restarts";
            // 
            // lblLogFile
            // 
            lblLogFile.AutoSize = true;
            lblLogFile.Location = new Point(12, 118);
            lblLogFile.Name = "lblLogFile";
            lblLogFile.Size = new Size(94, 15);
            lblLogFile.TabIndex = 5;
            lblLogFile.Text = "Current Log File:";
            // 
            // lblLogFileExists
            // 
            lblLogFileExists.AutoSize = true;
            lblLogFileExists.Location = new Point(363, 139);
            lblLogFileExists.Name = "lblLogFileExists";
            lblLogFileExists.Size = new Size(83, 15);
            lblLogFileExists.TabIndex = 6;
            lblLogFileExists.Text = "Log File Exists:";
            // 
            // textBoxLogFile
            // 
            textBoxLogFile.Location = new Point(12, 136);
            textBoxLogFile.Name = "textBoxLogFile";
            textBoxLogFile.ReadOnly = true;
            textBoxLogFile.Size = new Size(345, 23);
            textBoxLogFile.TabIndex = 7;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(125, 415);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(75, 23);
            btnExit.TabIndex = 8;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // textBoxServiceStatus
            // 
            textBoxServiceStatus.Location = new Point(12, 51);
            textBoxServiceStatus.Name = "textBoxServiceStatus";
            textBoxServiceStatus.ReadOnly = true;
            textBoxServiceStatus.Size = new Size(345, 23);
            textBoxServiceStatus.TabIndex = 9;
            // 
            // textBoxStatus
            // 
            textBoxStatus.Location = new Point(12, 219);
            textBoxStatus.Multiline = true;
            textBoxStatus.Name = "textBoxStatus";
            textBoxStatus.ReadOnly = true;
            textBoxStatus.ScrollBars = ScrollBars.Vertical;
            textBoxStatus.Size = new Size(776, 190);
            textBoxStatus.TabIndex = 10;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(363, 164);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(38, 23);
            btnSave.TabIndex = 11;
            btnSave.Text = "save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // textBoxLogDir
            // 
            textBoxLogDir.Location = new Point(12, 165);
            textBoxLogDir.Name = "textBoxLogDir";
            textBoxLogDir.Size = new Size(345, 23);
            textBoxLogDir.TabIndex = 12;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBoxLogDir);
            Controls.Add(btnSave);
            Controls.Add(textBoxStatus);
            Controls.Add(textBoxServiceStatus);
            Controls.Add(btnExit);
            Controls.Add(textBoxLogFile);
            Controls.Add(lblLogFileExists);
            Controls.Add(lblLogFile);
            Controls.Add(lblAutoRestarts);
            Controls.Add(lblErrorsDetected);
            Controls.Add(lblServiceStatus);
            Controls.Add(btnRestartService);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "CP-AI Monitor";
            Load += MainForm_Load;
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fileSystemWatcher1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private FileSystemWatcher fileSystemWatcher1;
        private Label lblAutoRestarts;
        private Label lblErrorsDetected;
        private Label lblServiceStatus;
        private Button btnRestartService;
        private TextBox textBoxLogFile;
        private Label lblLogFileExists;
        private Label lblLogFile;
        private Button btnExit;
        private TextBox textBoxServiceStatus;
        private TextBox textBoxStatus;
        private Button btnSave;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem maximizeToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private TextBox textBoxLogDir;
    }
}