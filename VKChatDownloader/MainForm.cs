using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VKChatDownloader
{
    public partial class MainForm : Form
    {
        delegate void SetTextCallback(string text);

        private UserGlobalStore store;
        private ConversationResponse conversations;
        private MainDownloader downloader;

        public MainForm()
        {
            InitializeComponent();
            store = new UserGlobalStore();
            conversations = new ConversationResponse();
            downloader = new MainDownloader();
        }

        public void LogThis(string msg)
        {
            if (this.logBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(LogThis);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.logBox.AppendText(msg + "\n");
            }
        }

        public bool IsDownloadAttachmentsChecked()
        {
            return downloadAttachCB.Checked;
        }

        public void SetStatus(string msg)
        {
            if (this.statusLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetStatus);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.statusLabel.Text = msg;
            }
        }

        public void SetDownloadCountLabel(string msg)
        {
            if (this.downloadCountLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetDownloadCountLabel);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.downloadCountLabel.Text = msg;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Show();
            mainPanel.Hide();
            LogThis("Loading...");
            webBrowser1.Url = new Uri("https://oauth.vk.com/authorize?client_id=6712753&response_type=token&scope=friends,messages&display=mobile&redirect_uri=https://oauth.vk.com/blank.html&v=5.85");
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if(e.Url.AbsoluteUri.StartsWith("https://oauth.vk.com/blank.html#"))
            {
                try
                {
                    var uri = e.Url.AbsoluteUri;
                    uri = uri.Replace("#", "?");
                    var args = Tools.GetParams(uri);
                    store.userID = Int32.Parse(args["user_id"]);
                    store.accountToken = args["access_token"];

                    webBrowser1.Hide();
                    mainPanel.Show();
                    var username = VKTools.GetUsername(store.accountToken);
                    LogThis("User Profile Loaded");
                    welcomeLabel.Text = "Привет, " + username;

                    conversations = VKTools.GetAllConversations(store.accountToken);
                    LogThis("Total Conversations:" + conversations.Total);
                    SetStatus("У вас " + conversations.Total + " чатов. Укажите папку куда их скачать и нажмите Download.");
                    /*for (var a = 0; a < convos.Conversations.Count; a++)
                    {
                        LogThis("   " + convos.Conversations[a].type + " (" + convos.Conversations[a].id + ") " + convos.Conversations[a].title);
                    }*/
                } catch(Exception ex)
                {
                    LogThis(ex.Message);
                    LogThis(ex.StackTrace);
                }
                
            }
        }


        private void dirButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                dirTextbox.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void dirTextbox_TextChanged(object sender, EventArgs e)
        {
            downloadButton.Enabled = dirTextbox.Text.Length > 0 && conversations.Conversations.Count > 0;
        }

        private void dirTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            downloadButton.Enabled = dirTextbox.Text.Length > 0 && conversations.Conversations.Count > 0;
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(dirTextbox.Text))
            {
                // prepare UI
                MarkUIBusy(true);

                // set downloader information
                downloader.SetConvos(conversations);
                downloader.SetDir(dirTextbox.Text);
                downloader.SetToken(store.accountToken);

                // download asynchronously
                await Task.Run(() => downloader.DownloadAll(this));
                LogThis("DONE!");
                MarkUIBusy(false);
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", dirTextbox.Text);
                LogThis(dirTextbox.Text + " is not a valid file or directory.");
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            LogThis("Stopping all downloads!");
            SetStatus("Закачака чатов остановлена! Нажмите Download чтобы начать с начала.");
            downloader.Stop();
            MarkUIBusy(false);
        }

        private void MarkUIBusy(bool isBusy = true)
        {
            downloadButton.Enabled = !isBusy;
            stopButton.Enabled = isBusy;
            dirButton.Enabled = !isBusy;
            dirTextbox.Enabled = !isBusy;
        }
    }
}
