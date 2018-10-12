using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class Attachment
    {
        public string Type;
        public string Link;
        public string Filename;
        public string AccessKey;
        public int Year;

        public Attachment(string type)
        {
            this.Type = type;
        }
    }
}
