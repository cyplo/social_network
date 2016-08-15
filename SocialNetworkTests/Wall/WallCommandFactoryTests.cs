using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands.Posting;
using SocialNetworkCLI.Commands.Wall;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests.Wall
{
    [TestFixture]
    public class WallCommandFactoryTests
    {
        [Test]
        public void Should_HaveTheCorrectVerbAssigned()
        {
            // Act
            var commandFactory = new WallCommandFactory();

            // Assert
            Assert.AreEqual("wall", commandFactory.GetCommandVerb());
        }

        [Test]
        public void Should_PassOnTheTimelineRepository()
        {
            // Arrange
            var commandFactory = new WallCommandFactory();
            var timelineRepositoryMock = new Mock<ITimelineRepository>();

            // Act
            var command = commandFactory.GetCommand(null, timelineRepositoryMock.Object, null);

            // Assert
            Assert.AreEqual(timelineRepositoryMock.Object, command.TimelineRepository);
        }

        [Test]
        public void Should_PassOnTheFollowerRepository()
        {
            // Arrange
            var commandFactory = new WallCommandFactory();
            var followerRepositoryMock = new Mock<IFollowerRepository>();

            // Act
            var command = commandFactory.GetCommand(followerRepositoryMock.Object, null, null);

            // Assert
            Assert.AreEqual(followerRepositoryMock.Object, command.FollowerRepository);
        }

        [Test]
        public void Should_PassOnTheUsername()
        {
            // Arrange
            var commandFactory = new WallCommandFactory();
            var username = "some username";

            // Act
            var command = commandFactory.GetCommand(null, null, username);

            // Assert
            Assert.AreEqual(username, command.Username);
        }
    }
}
