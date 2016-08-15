using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Commands.Reading
{
    public class ReadingCommandFactory : ICommandFactory
    {
        public string GetCommandVerb()
        {
            return null;
        }

        public ICommand GetCommand(ITimelineRepository timelineRepository, string username, string argument = null)
        {
            return new ReadingCommand(timelineRepository, username);
        }
    }
}
