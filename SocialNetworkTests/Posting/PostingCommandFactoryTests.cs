using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands.Posting;

namespace SocialNetworkTests
{
    [TestFixture]
    public class PostingCommandFactoryTests
    {
        [Test]
        public void Should_HaveTheCorrectVerbAssigned()
        {
            // Act
            var commandFactory = new PostingCommandFactory();
            
            // Assert
            Assert.AreEqual("->", commandFactory.GetCommandVerb());
        }

        [Test]
        public void Should_PassOnTheTimelineRepository()
        {
            // Arrange
            var commandFactory = new PostingCommandFactory();
            var timelineRepositoryMock = new Mock<ITimelineRepository>();

            // Act
            var command = commandFactory.GetCommand(null, timelineRepositoryMock.Object, null);

            // Assert
            Assert.AreEqual(timelineRepositoryMock.Object, command.TimelineRepository);
        }

        [Test]
        public void Should_PassOnTheUsername()
        {
            // Arrange
            var commandFactory = new PostingCommandFactory();
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
            var commandFactory = new PostingCommandFactory();
            var commandArgument = "some argument";

            // Act
            var command = commandFactory.GetCommand(null, null, null, commandArgument);

            // Assert
            Assert.AreEqual(commandArgument, command.Argument);
        }
    }
}
