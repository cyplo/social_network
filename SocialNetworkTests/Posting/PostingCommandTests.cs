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
    public class PostingCommandTests
    {
        [Test]
        public void Executing_Should_PostToUserTimeline()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();

            var username = "Alice";
            var message = "I love the weather today";
            var command = new PostingCommand(timelineRepositoryMock.Object, username, message);

            // Act
            command.Execute();

            // Assert
            timelineRepositoryMock.Verify( repository => repository.Post(username, message), Times.Once );
        }
    }
}
