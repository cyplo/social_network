using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkCLI.Commands.Following;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands.Wall
{
    public class WallCommandFactory : ICommandFactory
    {
        public string GetCommandVerb()
        {
            return "wall";
        }

        public ICommand GetCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository, string username,
            string argument = null)
        {
            return new WallCommand(followerRepository, timelineRepository, username);
        }
    }
}
