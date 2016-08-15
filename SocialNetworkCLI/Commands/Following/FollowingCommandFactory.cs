using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands.Following
{
    public class FollowingCommandFactory : ICommandFactory
    {
        public string GetCommandVerb()
        {
            return "follows";
        }

        public ICommand GetCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository, string username, string argument = null)
        {
            return new FollowingCommand(followerRepository, username, argument);
        }
    }
}
