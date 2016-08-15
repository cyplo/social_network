using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI
{
    public interface ICommandFactory
    {
        string GetCommandVerb();
        ICommand GetCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository, string username, string argument=null);
    }

    public interface ICommand
    {
        IFollowerRepository FollowerRepository { get; }
        ITimelineRepository TimelineRepository { get; }
        string Username { get; }
        string Argument { get; }
        string Execute();
    }
}
