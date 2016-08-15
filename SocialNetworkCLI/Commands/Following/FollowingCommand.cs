using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands.Following
{
    public class FollowingCommand : ICommand
    {
        public IFollowerRepository FollowerRepository { get; }
        public ITimelineRepository TimelineRepository { get; }
        public string Username { get; }
        public string Argument { get; }

        public FollowingCommand(IFollowerRepository followerRepository, string username, string argument)
        {
            FollowerRepository = followerRepository;
            TimelineRepository = null;
            Username = username;
            Argument = argument;
        }

        public string Execute()
        {
            FollowerRepository.Follow(Username, Argument);
            return null;
        }
    }
}
