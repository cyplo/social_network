using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public interface ITimelineRepository
    {
        void Post(string username, string message);
        IEnumerable<Message> Read(string username);
    }
}
