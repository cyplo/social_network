using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests
{
    [TestFixture]
    public class FollowerRepositoryTests
    {
        [Test]
        public void ShouldNot_AllowFollowingEmptyUsers()
        {
            // Arrange
            var repository = new FollowerRepository();

            // Assert
            Assert.Throws<ArgumentNullException>( // Act
                () => repository.Follow(null, null));
        }

        [Test]
        public void Should_RegisterTheFollower()
        {
            // Arrange
            var repository = new FollowerRepository();

            var follower = "username 1";
            var toFollow = "username 2";
            repository.Follow(follower, toFollow);

            // Act
            var usernamesList = repository.GetUsernamesTheUserIsFollowing(follower);

            // Assert
            Assert.IsNotNull(usernamesList);
            Assert.AreEqual(1, usernamesList.Count());
            Assert.AreEqual(toFollow, usernamesList.First());
        }

        [Test]
        public void Should_ReturnAllUsersTheUserIsFollowing()
        {
            // Arrange
            var repository = new FollowerRepository();

            var follower =  "username 1";
            var toFollow1 = "username 2";
            var toFollow2 = "username 4";
            var toFollow3 = "username 8";
            repository.Follow(follower, toFollow1);
            repository.Follow(follower, toFollow2);
            repository.Follow(follower, toFollow3);

            // Act
            var usernamesList = repository.GetUsernamesTheUserIsFollowing(follower);

            // Assert
            Assert.IsNotNull(usernamesList);
            Assert.AreEqual(3, usernamesList.Count());
        }

        [Test]
        public void Should_NotAllowLeaksBetweenUsers()
        {
            // Arrange
            var repository = new FollowerRepository();

            var follower1 = "follower 1";
            var follower2 = "follower 2";
            var toFollow = "username 2";
            repository.Follow(follower1, toFollow);

            // Act
            var usernamesList = repository.GetUsernamesTheUserIsFollowing(follower2);

            // Assert
            Assert.IsNotNull(usernamesList);
            Assert.AreEqual(0, usernamesList.Count());
        }
    }
}
