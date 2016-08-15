using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI.Commands.Posting
{
    public class PostingCommand : ICommand
    {
        public IFollowerRepository FollowerRepository { get; }
        public ITimelineRepository TimelineRepository { get; }
        public string Username { get; }
        public string Argument { get; }

        public PostingCommand(ITimelineRepository timelineRepository, string username, string message)
        {
            Username = username;
            Argument = message;
            TimelineRepository = timelineRepository;
        }

        public string Execute()
        {
            TimelineRepository.Post(Username, Argument);
            return null;
        }
    }
}
