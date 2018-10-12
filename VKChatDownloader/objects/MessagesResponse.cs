using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class MessagesResponse
    {
        public int Total;
        public List<Message> Messages;

        public MessagesResponse()
        {
            Total = 0;
            Messages = new List<Message>();
        }
    }
}
