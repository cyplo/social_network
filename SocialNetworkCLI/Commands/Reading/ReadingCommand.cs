using System;
using System.Linq;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands.Reading
{
    public class ReadingCommand : ICommand
    {
        public IFollowerRepository FollowerRepository { get; }
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
                              select new MessageFormatter(message).Format();
            var resultText = string.Join(Environment.NewLine, resultTexts);
            return resultText;
        }
    }
}
