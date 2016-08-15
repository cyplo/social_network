using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Commands
{
    public class MessageFormatter
    {
        private readonly Message _messageToFormat;

        public MessageFormatter(Message messageToFormat)
        {
            _messageToFormat = messageToFormat;
        }

        public string Format()
        {
            return _messageToFormat.Text + " (" + FormatTime(_messageToFormat.Timestamp) + ")";
        }
        
        // TODO: look into using libraries that might do the time => text transformation for us (Humanizer maybe ?)
        private string FormatTime(DateTime timestamp)
        {
            var messageAge = DateTime.Now - timestamp;
            var ageInMinutes = messageAge.TotalMinutes;
            if (ageInMinutes < 1)
            {
                return "just now";
            }

            if (ageInMinutes >= 1 && ageInMinutes < 2)
            {
                return "1 minute ago";
            }

            return Math.Floor(ageInMinutes) + " minutes ago";
        }
    }
}
