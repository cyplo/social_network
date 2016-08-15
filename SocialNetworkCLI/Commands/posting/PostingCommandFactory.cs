using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Commands.Posting
{
    public class PostingCommandFactory : ICommandFactory
    {
        public string GetCommandVerb()
        {
            return "->";
        }

        public ICommand GetCommand(ITimelineRepository timelineRepository, string username, string argument = null)
        {
            return new PostingCommand(timelineRepository, username, argument);
        }
    }
}
