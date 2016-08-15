using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands.Posting;
using SocialNetworkCLI.Commands.Reading;

namespace SocialNetworkTests
{
    [TestFixture]
    public class ReadingCommandFactoryTests
    {
        [Test]
        public void Should_HaveNoVerbAssigned()
        {
            // Act
            var commandFactory = new ReadingCommandFactory();

            // Assert
            Assert.IsNull(commandFactory.GetCommandVerb());
        }

        [Test]
        public void Should_PassOnTheTimelineRepository()
        {
            // Arrange
            var commandFactory = new ReadingCommandFactory();
            var timelineRepositoryMock = new Mock<ITimelineRepository>();

            // Act
            var command = commandFactory.GetCommand(timelineRepositoryMock.Object, null);

            // Assert
            Assert.AreEqual(timelineRepositoryMock.Object, command.TimelineRepository);
        }

        [Test]
        public void Should_PassOnTheUsername()
        {
            // Arrange
            var commandFactory = new ReadingCommandFactory();
            var username = "some username";

            // Act
            var command = commandFactory.GetCommand(null, username);

            // Assert
            Assert.AreEqual(username, command.Username);
        }
    }
}
