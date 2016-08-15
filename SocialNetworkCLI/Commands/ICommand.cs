using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public interface ICommandFactory
    {
        string GetCommandVerb();
        ICommand GetCommand(ITimelineRepository timelineRepository, string username, string argument=null);
    }

    public interface ICommand
    {
        ITimelineRepository TimelineRepository { get; }
        string Username { get; }
        string Argument { get; }
        string Execute();
    }
}
