using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VKChatDownloader
{
    public class VKTools
    {
        private static string AccountToken = "";

        private static Dictionary<int, Profile> profiles = new Dictionary<int, Profile>();

        public static string GetUsername(string accountToken)
        {
            string url = BuildURL("account.getProfileInfo", new Dictionary<string, string>(), accountToken);
            var result = Get(url);
            var dynoResult = ReadResponse(result);
            return dynoResult.response.first_name + " " + dynoResult.response.last_name;
        }

        public static MessagesResponse GetMessages(string accountToken, int chatID, int offset = 0)
        {
            var paramDic = new Dictionary<string, string>();
            paramDic.Add("rev", "1");
            paramDic.Add("offset", "" + offset);
            paramDic.Add("count", "200");
            paramDic.Add("peer_id", "" + chatID);
            string url = BuildURL("messages.getHistory", paramDic, accountToken);
            var result = Get(url);
            var dynoResult = ReadResponse(result);
            List<Message> messages = new List<Message>();
            var response = new MessagesResponse();
            response.Total = (int)dynoResult.response.count;
            for (var a = 0; a < dynoResult.response.items.Count; a++) {
                var message = new Message();
                var msg = dynoResult.response.items[a];
                message.Date = DateTimeOffset.FromUnixTimeSeconds((long)msg.date);
                message.Text = msg.text;
                message.FromID = msg.from_id;
                // action
                if(msg.action != null && msg.action.type != null)
                {
                    var actionText = "";
                    if (msg.action.type == "chat_invite_user")
                    {
                        actionText = GetProfile("", (int)msg.action.member_id).name;
                    } else
                    {
                        actionText = msg.action.text;
                    }
                    message.Text += " [" + msg.action.type + " - " + actionText + "]";
                    message.IsAction = true;
                }

                for(var att = 0; att < msg.attachments.Count; att++)
                {
                    var type = (string)msg.attachments[att].type;
                    Attachment attachment = new Attachment(type);
                    if(type == "doc")
                    {
                        attachment.Filename = (string)msg.attachments[att].doc.title;
                        attachment.Link = (string)msg.attachments[att].doc.url;
                        attachment.AccessKey = (string)msg.attachments[att].doc.access_key;
                    } else if(type == "photo")
                    {
                        var sizeURL = "";
                        var maxWidth = 0;
                        for(var s = 0; s < msg.attachments[att].photo.sizes.Count; s++ )
                        {
                            if(msg.attachments[att].photo.sizes[s].width > maxWidth)
                            {
                                maxWidth = (int)msg.attachments[att].photo.sizes[s].width;
                                sizeURL = (string)msg.attachments[att].photo.sizes[s].url;
                            }
                        }
                        attachment.Link = sizeURL;
                        attachment.Filename = "";
                        attachment.AccessKey = (string)msg.attachments[att].photo.access_key;
                    }
                    if(!string.IsNullOrEmpty(attachment.Link))
                    {
                        if (string.IsNullOrEmpty(attachment.Filename))
                        {
                            string file = attachment.Link.Substring(attachment.Link.LastIndexOf("/") + 1);
                            if (file.Contains("."))
                            {
                                attachment.Filename = file;
                            }
                            else
                            {
                                attachment.Filename = string.Format(@"{0}.txt", DateTime.Now.Ticks);
                            }
                        }
                        message.Attachments.Add(attachment);
                    }
                    
                }
                messages.Add(message);
            }
            response.Messages = messages;
            return response;
        }

        public static Profile GetProfile(string accountToken, int id)
        {
            if(profiles.ContainsKey(id))
            {
                return profiles[id];
            } else
            {
                var paramDic = new Dictionary<string, string>();
                paramDic.Add("user_ids", "" + id);
                string url = BuildURL("users.get", paramDic, accountToken);
                var result = Get(url);
                var dynoResult = ReadResponse(result);
                string name = "";
                if(dynoResult.response.Count > 0)
                {
                    name = dynoResult.response[0].first_name + " " + dynoResult.response[0].last_name;
                }
                var profile = new Profile(name);
                profiles.Add(id, profile);
                return profile;
            }
        }

        public static ConversationResponse GetAllConversations(string accountToken)
        {
            Console.WriteLine("Getting all conversations");
            ConversationResponse response = GetConversations(accountToken);
            while(response.Total > response.Conversations.Count)
            {
                var respTemp = GetConversations(accountToken, response.Conversations.Count);
                for(int a = 0; a < respTemp.Conversations.Count; a++)
                {
                    response.Conversations.Add(respTemp.Conversations[a]);
                }
            }
            return response;
        }

        public static ConversationResponse GetConversations(string accountToken, int offset = 0)
        {
            Console.WriteLine("Getting conversations starting with " + offset);
            ConversationResponse response = new ConversationResponse();

            var paramDic = new Dictionary<string, string>();
            paramDic.Add("offset", "" + offset);
            paramDic.Add("count", "" + 200);
            paramDic.Add("extended", "" + 1);
            string url = BuildURL("messages.getConversations", paramDic, accountToken);
            var result = Get(url);
            var dynoResult = ReadResponse(result);
            response.Total = dynoResult.response.count;
            for (int a = 0; a < dynoResult.response.items.Count; a++) {
                Conversation convo = new Conversation();
                convo.id = dynoResult.response.items[a].conversation.peer.id;
                convo.type = dynoResult.response.items[a].conversation.peer.type;
                if(convo.type.Equals("chat") && dynoResult.response.items[a].conversation.chat_settings != null)
                {
                    convo.title = dynoResult.response.items[a].conversation.chat_settings.title;
                }
                response.Conversations.Add(convo);                
            }
            return response;
        }

        private static async Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string BuildURL(string method, Dictionary<string, string> args, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                AccountToken = token;
            } else
            {
                token = AccountToken;
            }
            Task.Delay(340).Wait();
            List<string> listArgs = new List<string>();
            listArgs.Add("v=5.85");
            listArgs.Add("access_token=" + token);
            foreach (var pair in args)
            {
                listArgs.Add(pair.Key + "=" + pair.Value);
            }
            return "https://api.vk.com/method/" + method + "?" + string.Join("&",listArgs);
        }

        private static dynamic ReadResponse(string result)
        {
            dynamic stuff = JObject.Parse(result);
            return stuff;
        }
    }
}
