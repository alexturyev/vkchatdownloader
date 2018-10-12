namespace VKChatDownloader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.stopButton = new System.Windows.Forms.Button();
            this.downloadButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.dirButton = new System.Windows.Forms.Button();
            this.dirTextbox = new System.Windows.Forms.TextBox();
            this.logBox = new System.Windows.Forms.TextBox();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.downloadAttachCB = new System.Windows.Forms.CheckBox();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(12, 47);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(369, 404);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.downloadAttachCB);
            this.mainPanel.Controls.Add(this.stopButton);
            this.mainPanel.Controls.Add(this.downloadButton);
            this.mainPanel.Controls.Add(this.statusLabel);
            this.mainPanel.Controls.Add(this.dirButton);
            this.mainPanel.Controls.Add(this.dirTextbox);
            this.mainPanel.Controls.Add(this.logBox);
            this.mainPanel.Controls.Add(this.welcomeLabel);
            this.mainPanel.Location = new System.Drawing.Point(12, 13);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(369, 438);
            this.mainPanel.TabIndex = 2;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(198, 112);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(168, 23);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "STOP";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // downloadButton
            // 
            this.downloadButton.Enabled = false;
            this.downloadButton.Location = new System.Drawing.Point(3, 112);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(158, 23);
            this.downloadButton.TabIndex = 5;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.Location = new System.Drawing.Point(-3, 34);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(63, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Загрузка...";
            // 
            // dirButton
            // 
            this.dirButton.Location = new System.Drawing.Point(297, 62);
            this.dirButton.Name = "dirButton";
            this.dirButton.Size = new System.Drawing.Size(69, 20);
            this.dirButton.TabIndex = 3;
            this.dirButton.Text = "DIR";
            this.dirButton.UseVisualStyleBackColor = true;
            this.dirButton.Click += new System.EventHandler(this.dirButton_Click);
            // 
            // dirTextbox
            // 
            this.dirTextbox.Location = new System.Drawing.Point(0, 63);
            this.dirTextbox.Name = "dirTextbox";
            this.dirTextbox.Size = new System.Drawing.Size(289, 20);
            this.dirTextbox.TabIndex = 2;
            this.dirTextbox.TextChanged += new System.EventHandler(this.dirTextbox_TextChanged);
            this.dirTextbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dirTextbox_KeyUp);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(0, 141);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(369, 306);
            this.logBox.TabIndex = 1;
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.welcomeLabel.Location = new System.Drawing.Point(76, 0);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(106, 24);
            this.welcomeLabel.TabIndex = 0;
            this.welcomeLabel.Text = "Загрузка...";
            this.welcomeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // downloadAttachCB
            // 
            this.downloadAttachCB.AutoSize = true;
            this.downloadAttachCB.Checked = true;
            this.downloadAttachCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.downloadAttachCB.Location = new System.Drawing.Point(242, 89);
            this.downloadAttachCB.Name = "downloadAttachCB";
            this.downloadAttachCB.Size = new System.Drawing.Size(124, 17);
            this.downloadAttachCB.TabIndex = 7;
            this.downloadAttachCB.Text = "Качать Доки/Фото";
            this.downloadAttachCB.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 472);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "VKChatDownloader";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button dirButton;
        private System.Windows.Forms.TextBox dirTextbox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox downloadAttachCB;
    }
}

