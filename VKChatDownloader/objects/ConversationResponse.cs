using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class ConversationResponse
    {
        public int Total;
        public List<Conversation> Conversations;

        public ConversationResponse()
        {
            Total = 0;
            Conversations = new List<Conversation>();
        }
    }
}
