using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
            var ignoreCount = 0;
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
                    form.LogThis("Downloading " + a + "/" + convos.Conversations.Count + " (" + name + ") " + "[" + convo.type + "]");
                    form.SetStatus("Качаю " + a + " из " + convos.Conversations.Count + " (" + name + ") ");
                    Download(convos.Conversations[a], dir);
                }
                else
                {
                    form.LogThis("Ignoring " + convo.type + " chat (" + convo.id + ")");
                }
                form.LogThis("");
                stop = true;
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
                var filePath = Path.Combine(dirPath, a.Filename);
                if (!File.Exists(filePath))
                {
                    form.LogThis("Downloading " + a.Filename);
                    WebClient myWebClient = new WebClient();
                    myWebClient.DownloadFile(a.Link, filePath);
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
    }
}
