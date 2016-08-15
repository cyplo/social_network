using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Repositories
{
    public class TimelineRepository : ITimelineRepository
    {
        // user => message list
        private readonly Dictionary<string, List<Message>> _timelines = new Dictionary<string, List<Message>>();

        public void Post(string username, string message)
        {
            // TODO: evaluate using CuttingEdge for the project
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Cannot post with empty username");
            }

            var timelineToChange = new List<Message>();
            if (_timelines.ContainsKey(username))
            {
                timelineToChange = _timelines[username];
            }
            timelineToChange.Add(new Message(message, DateTime.Now));
            _timelines[username] = timelineToChange;
        }

        public IEnumerable<Message> Read(string username)
        {
            if (_timelines.ContainsKey(username))
            {
                return _timelines[username];
            }
            return new List<Message>();
        }
    }
}
