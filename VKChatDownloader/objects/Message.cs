using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class Message
    {
        public DateTimeOffset Date;
        public int FromID;
        public string Text;
        public bool IsAction;
        public List<Attachment> Attachments;

        public Message()
        {
            Attachments = new List<Attachment>();
        }

        public string GenerateTextline()
        {
            string msg = "[" + Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") + "]";
            msg += " " + VKTools.GetProfile("", FromID).name;
            var MsgText = Text.Replace("\n", "\t");
            msg += ": " + MsgText;
            if(Attachments.Count > 0)
            {
                msg += " [" + Attachments.Count + " attachments: ";
                for(var a = 0; a < Attachments.Count; a++)
                {
                    if(a > 0)
                    {
                        msg += ",";
                    }
                    msg += Attachments[a].Type;
                    if(!string.IsNullOrEmpty(Attachments[a].Filename))
                    {
                        msg += " '" + Attachments[a].Filename + "'";
                    }
                }
                msg += "]";
            }
            return msg;
        }
    }
}
