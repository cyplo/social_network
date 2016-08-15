using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public class Message
    {
        public string Text { get; }
        public DateTime Timestamp { get; }

        public Message(string text, DateTime timestamp)
        {
            Text = text;
            Timestamp = timestamp;
        }
    }
}
