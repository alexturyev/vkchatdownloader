using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace VKChatDownloader
{
    public class Tools
    {

        public static Dictionary<string, string> GetParams(string uri)
        {
            var matches = Regex.Matches(uri, @"[\?&](([^&=]+)=([^&=#]*))", RegexOptions.Compiled);
            return matches.Cast<Match>().ToDictionary(
                m => Uri.UnescapeDataString(m.Groups[2].Value),
                m => Uri.UnescapeDataString(m.Groups[3].Value)
            );
        }

        public static int GetLineCount(string filePath)
        {
            int lineCount = 0;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 1024 * 1024))
            {
                byte[] buffer = new byte[1024 * 1024];
                int bytesRead;

                do
                {
                    bytesRead = fs.Read(buffer, 0, buffer.Length);
                    for (int i = 0; i < bytesRead; i++)
                        if (buffer[i] == '\n')
                            lineCount++;
                }
                while (bytesRead > 0);
            }
            return lineCount;
        }
    }
}
