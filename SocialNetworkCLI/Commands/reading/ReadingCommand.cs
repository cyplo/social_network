using System;
using System.Linq;

namespace SocialNetworkCLI.Commands.Reading
{
    public class ReadingCommand : ICommand
    {
        public ITimelineRepository TimelineRepository { get; }
        public string Username { get; }
        public string Argument { get; }

        public ReadingCommand(ITimelineRepository timelineRepository, string username)
        {
            TimelineRepository = timelineRepository;
            Username = username;
            Argument = null;
        }


        public string Execute()
        {
            var allMessages = TimelineRepository.Read(Username).OrderByDescending( message => message.Timestamp );
            if (!allMessages.Any())
            {
                return null;
            }

            var resultTexts = from message in allMessages
                              select message.Text + " (" + FormatTime(message.Timestamp) + ")";
            var resultText = string.Join(Environment.NewLine, resultTexts);
            return resultText;
        }

        // TODO: probably worth extracting the time formatter part to a separate class
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
