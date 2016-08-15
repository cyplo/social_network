using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Repositories
{
    public interface IFollowerRepository
    {
        void Follow(string userThatFollows, string userToFollow);
        IEnumerable<string> GetUsernamesTheUserIsFollowing(string userThatFollows);
    }
}
