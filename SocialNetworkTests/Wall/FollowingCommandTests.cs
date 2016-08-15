using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI.Commands.Following;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests.Wall
{
    [TestFixture]
    public class FollowingCommandTests
    {
        [Test]
        public void Should_RegisterTheFactOfFollowingInTheRepository()
        {
            // Arrange
            var follower = "follower";
            var userToFollow = "userToFollow";
            var followersRepository = new Mock<IFollowerRepository>();
            var command = new FollowingCommand(followersRepository.Object, follower, userToFollow);

            // Act
            command.Execute();

            // Arrange
            followersRepository.Verify( repository => repository.Follow(follower, userToFollow), Times.Once);
        }
    }
}
