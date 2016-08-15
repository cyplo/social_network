using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using SocialNetworkCLI.Commands.Reading;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands
{
    public class WallCommand : ICommand
    {
        public IFollowerRepository FollowerRepository { get; }
        public ITimelineRepository TimelineRepository { get; }
        public string Username { get; }
        public string Argument { get; }

        public WallCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository,
            string username)
        {
            FollowerRepository = followerRepository;
            TimelineRepository = timelineRepository;
            Username = username;
            Argument = null;
        }

        public string Execute()
        {
            var usersFollowed = FollowerRepository.GetUsernamesTheUserIsFollowing(Username);
            
            // (username, Message)
            var allMessages = new List<Tuple<string, Message>>();
            var ownMessages = TimelineRepository.Read(Username);
            foreach (var message in ownMessages)
            {
                allMessages.Add(new Tuple<string, Message>(Username, message));
            }

            foreach (var user in usersFollowed)
            {
                var userMessages = TimelineRepository.Read(user);
                foreach (var message in userMessages)
                {
                    allMessages.Add(new Tuple<string, Message>(user, message));
                }
            }

            if (!allMessages.Any())
            {
                return null;
            }

            var allMessagesSorted = allMessages.OrderByDescending(userMessage => userMessage.Item2.Timestamp);

            var resultTexts = from userMessage in allMessagesSorted
                              select userMessage.Item1 + " - " + (new MessageFormatter(userMessage.Item2).Format());
            var resultText = string.Join(Environment.NewLine, resultTexts);
            return resultText;
        }
    }
}
