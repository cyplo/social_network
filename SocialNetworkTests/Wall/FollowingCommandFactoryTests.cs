using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands.Following;
using SocialNetworkCLI.Commands.Posting;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests.Wall
{
    [TestFixture]
    public class FollowingCommandFactoryTests
    {
        [Test]
        public void Should_HaveTheCorrectVerbAssigned()
        {
            // Act
            var commandFactory = new FollowingCommandFactory();

            // Assert
            Assert.AreEqual("follows", commandFactory.GetCommandVerb());
        }

        [Test]
        public void Should_PassOnTheUsername()
        {
            // Arrange
            var commandFactory = new FollowingCommandFactory();
            var username = "some username";

            // Act
            var command = commandFactory.GetCommand(null, null, username);

            // Assert
            Assert.AreEqual(username, command.Username);
        }

        [Test]
        public void Should_PassOnTheCommandArgument()
        {
            // Arrange
            var commandFactory = new FollowingCommandFactory();
            var commandArgument = "some argument";

            // Act
            var command = commandFactory.GetCommand(null, null, null, commandArgument);

            // Assert
            Assert.AreEqual(commandArgument, command.Argument);
        }

        [Test]
        public void Should_PassOnTheFollowerRepository()
        {
            // Arrange
            var commandFactory = new FollowingCommandFactory();
            var followerRepositoryMock = new Mock<IFollowerRepository>();

            // Act
            var command = commandFactory.GetCommand(followerRepositoryMock.Object, null, null);

            // Assert
            Assert.AreEqual(followerRepositoryMock.Object, command.FollowerRepository);
        }

    }
}
