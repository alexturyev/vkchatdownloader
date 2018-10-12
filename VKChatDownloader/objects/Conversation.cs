using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class Conversation
    {
        public string type;
        public string title;
        public int id;
        public List<int> active_ids;
    }
}
