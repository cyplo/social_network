using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI.Repositories
{
    public class FollowerRepository : IFollowerRepository
    {
        // user => list of users this user is following
        private readonly Dictionary<string, List<string>> _following = new Dictionary<string, List<string>>();

        public void Follow(string userThatFollows, string userToFollow)
        {
            // TODO: Add CuttingEdge.Conditions
            if (string.IsNullOrWhiteSpace(userThatFollows))
            {
                throw new ArgumentNullException("userThatFollows");
            }
            if (string.IsNullOrWhiteSpace(userThatFollows))
            {
                throw new ArgumentNullException("userToFollow");
            }

            var userlistToChange = new List<string>();
            if (_following.ContainsKey(userThatFollows))
            {
                userlistToChange = _following[userThatFollows];
            }
            userlistToChange.Add(userToFollow);
            _following[userThatFollows] = userlistToChange;
        }

        public IEnumerable<string> GetUsernamesTheUserIsFollowing(string userThatFollows)
        {
            if (_following.ContainsKey(userThatFollows))
            {
                return _following[userThatFollows];
            }
            return new List<string>();
        }
    }
}
