using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class MainDownloader
    {
        private MainForm form;
        private 
        ConversationResponse convos = new ConversationResponse();
        string dir;
        private string token;
        private bool stop = false;
        const int MAX_DOWNLOADS = 25;

        public MainDownloader()
        {

        }

        public void SetToken(string token)
        {
            this.token = token;
        }

        public void SetDir(string dir)
        {
            this.dir = dir;
        }
        public void SetConvos(ConversationResponse convos)
        {
            this.convos = convos;
        }
        public void DownloadAll(MainForm form)
        {
            stop = false;
            Console.WriteLine("Downloading All");
            this.form = form;
            for(var a = 0; a < convos.Conversations.Count && !stop; a++)
            {
                var convo = convos.Conversations[a];
                var id = convo.id;
                string name = convo.title;
                if (string.IsNullOrEmpty(name)) {
                    name = "" + id;
                }
                if (convo.type == "chat" || convo.type == "user")
                {
                    form.LogThis("Downloading " + (a+1) + "/" + convos.Conversations.Count + " (" + name + ") " + "[" + convo.type + "]");
                    form.SetStatus("Качаю " + (a + 1) + " из " + convos.Conversations.Count + " (" + name + ") ");
                    Download(convos.Conversations[a], dir);
                }
                else
                {
                    form.LogThis("Ignoring " + convo.type + " chat (" + convo.id + ")");
                }
                if(downloadCounter > MAX_DOWNLOADS / 2)
                {
                    form.LogThis("Too many downloads... waiting for most of them to finish");
                    while(downloadCounter > MAX_DOWNLOADS / 2 && !stop)
                    {
                        Task.Delay(100);
                    }
                }
                form.LogThis("");
            }
        }

        public void Stop()
        {
            stop = true;
        }

        public void Download(Conversation convo, string dirPath)
        {
            var convoName = "";
            if (convo.type == "user")
            {
                var profile = VKTools.GetProfile(token, convo.id);
                convoName = profile.name;
            } else if (!string.IsNullOrEmpty(convo.title))
            {
                convoName = convo.title;
            }
            if(string.IsNullOrEmpty(convoName))
            {
                convoName = "" + convo.id;
            } else
            {
                convoName += " - " + convo.id;
            }
            convoName = CleanFileName(convoName);
            var convoFilename = convoName + ".txt";

            form.LogThis("  Filename: " + convoFilename);

            var filePath = Path.Combine(dir, convoFilename);
            int lineCount = 0;
            if(!File.Exists(filePath))
            {
                form.LogThis("Creating new file");
                File.Create(filePath).Close();
            } else
            {
                lineCount = Tools.GetLineCount(filePath);
                form.LogThis("File already has " + lineCount + " lines");
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
            {
                int lastMessageCount = 0;
                do
                {
                    var response = VKTools.GetMessages(token, convo.id, lineCount);
                    lastMessageCount = response.Messages.Count;
                    form.LogThis("   " + lineCount + " / " + response.Total + " messages");
                    List<Attachment> attachments = new List<Attachment>();
                    for (var m = 0; m < response.Messages.Count; m++)
                    {
                        attachments.AddRange(response.Messages[m].Attachments);
                        file.WriteLine(response.Messages[m].GenerateTextline());
                    }
                    if(form.IsDownloadAttachmentsChecked())
                    {
                        for (var a = 0; a < attachments.Count && !stop; a++)
                        {
                            DownloadAttachment(attachments[a], convoName);
                            while (downloadCounter > MAX_DOWNLOADS && !stop)
                            {
                                Task.Delay(100);
                            }
                        }
                    }
                    
                    lineCount += response.Messages.Count;
                } while (lastMessageCount == 200 && !stop);
            }
        }

        private void DownloadAttachment(Attachment a, string dirName)
        {
            if (!string.IsNullOrEmpty(a.Link) && !string.IsNullOrEmpty(a.Filename))
            {
                var dirPath = Path.Combine(dir, dirName);
                if(!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                dirPath = Path.Combine(dirPath, "" + a.Year);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                var filePath = Path.Combine(dirPath, a.Filename);
                bool alreadyExists = false;
                if(File.Exists(filePath)) {
                    alreadyExists = true;
                    // might want to check for empty files here
                }
                if (!alreadyExists)
                {
                    form.LogThis("Downloading " + a.Filename);
                    AddDownload(new Uri(a.Link), filePath);
                }
                else
                {
                    form.LogThis(a.Filename + " already exists.");
                }
            }            
        }

        private string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        int downloadCounter = 0;

        public async void AddDownload(Uri uri, string filePath)
        {
            using (WebClient myWebClient = new WebClient())
            {
                IncreaseDownloadCounter();
                try
                {
                    await myWebClient.DownloadFileTaskAsync(uri, filePath);
                    DecreaseDownloadCounter();
                } catch(Exception ex)
                {
                    DecreaseDownloadCounter();
                    form.LogThis(uri.AbsoluteUri);
                    form.LogThis("ERROR:" + ex.Message + " [" + ex.StackTrace + "]");
                }
            }
        }

        private void IncreaseDownloadCounter()
        {
            Interlocked.Increment(ref downloadCounter);
            //downloadCounter++;
            form.SetDownloadCountLabel("Качается - " + downloadCounter + " файлов");
        }

        private void DecreaseDownloadCounter()
        {
            Interlocked.Decrement(ref downloadCounter);
            form.SetDownloadCountLabel("Качается - " + downloadCounter + " файлов");
        }
    }
}
