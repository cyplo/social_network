using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands.Reading;

namespace SocialNetworkTests
{
    [TestFixture]
    public class ReadingCommandTest
    {
        [Test]
        public void Should_ReturnEmptyResponse_GivenNoMessagesForTheUser()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var response = command.Execute();

            // Assert
            Assert.IsNull(response);
        }

        [Test]
        public void Should_AskTimelineRepositoryForInformationForTheRightUser()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            command.Execute();

            // Assert
            timelineRepositoryMock.Verify( repository => repository.Read(username), Times.Once );
        }

        [Test]
        public void Should_ReturnTheSoleMessage_GivenOneMessageInRepository()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText = "Some message";
            var timestamp = DateTime.Now;
            var message = new Message(messageText, timestamp);
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(new List<Message>() {message});

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue( result.StartsWith(messageText) );
        }

        [Test]
        public void Should_MarkTheMessageJustNow_GivenMessagePostedLessThanMinuteAgo()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText = "Some message";
            var timestamp = DateTime.Now;
            var message = new Message(messageText, timestamp);
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(new List<Message>() { message });

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.EndsWith("(just now)"));
        }

        [Test]
        public void Should_MarkTheMessageWithAgeInMinutes_GivenMessagePostedAMinuteAgo()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText = "Some message";
            var timestamp = DateTime.Now - TimeSpan.FromSeconds(61);
            var message = new Message(messageText, timestamp);
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(new List<Message>() { message });

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.EndsWith("(1 minute ago)"));
        }

        [Test]
        public void Should_MarkTheMessageWithAgeInMinutes_GivenMessagePosted3MinutesAgo()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText = "Some message";
            var timestamp = DateTime.Now - TimeSpan.FromSeconds(3 * 60 + 1);
            var message = new Message(messageText, timestamp);
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(new List<Message>() { message });

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.EndsWith("(3 minutes ago)"));
        }

        [Test]
        public void Should_ReturnNumberOfLinesEqualToNumberOfMessages()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText1 = "Some message 1";
            var messageText2 = "Some message 2";
            var messageText3 = "Some message 3";
            var timestamp = DateTime.Now;

            var message1 = new Message(messageText1, timestamp);
            var message2 = new Message(messageText2, timestamp);
            var message3 = new Message(messageText3, timestamp);
            var messages = new List<Message>() {message1, message2, message3};
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(messages);

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            var lines = result.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            Assert.AreEqual(3, lines.Length);
        }

        [Test]
        public void Should_ReturnAllMessages()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText1 = "Some message 1";
            var messageText2 = "Some message 2";
            var messageText3 = "Some message 3";
            var timestamp = DateTime.Now;

            var message1 = new Message(messageText1, timestamp);
            var message2 = new Message(messageText2, timestamp);
            var message3 = new Message(messageText3, timestamp);
            var messages = new List<Message>() {message1, message2, message3};
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(messages);

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.True(result.Contains(messageText1));
            Assert.True(result.Contains(messageText2));
            Assert.True(result.Contains(messageText3));
        }

        [Test]
        public void Should_ReturnMessagesInReverseChronologicalOrder()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var username = "Alice";
            var messageText1 = "Some message 1 - this should be first";
            var messageText2 = "Some message 2 - this should be last";
            var messageText3 = "Some message 3 - this should be the middle one";
            var timestamp = DateTime.Now - TimeSpan.FromHours(1);

            var message1 = new Message(messageText1, timestamp + TimeSpan.FromMinutes(1) ); // later
            var message2 = new Message(messageText2, timestamp - TimeSpan.FromMinutes(1) ); // earlier
            var message3 = new Message(messageText3, timestamp );
            var messages = new List<Message>() { message1, message2, message3 };
            timelineRepositoryMock.Setup(repository => repository.Read(username))
                .Returns(messages);

            var command = new ReadingCommand(timelineRepositoryMock.Object, username);

            // Act
            var result = command.Execute();

            // Assert
            var lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.True(lines[0].StartsWith(messageText1));
            Assert.True(lines[1].StartsWith(messageText3));
            Assert.True(lines[2].StartsWith(messageText2));
        }
    }
}
