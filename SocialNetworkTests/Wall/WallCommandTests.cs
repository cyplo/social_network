using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Commands;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests.Wall
{
    [TestFixture]
    public class WallCommandTests
    {
        [Test]
        public void Should_OutputPastOwnMessages()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var username = "username";
            var message = "message";
            timelineRepository.Post(username, message);
            var command = new WallCommand(followerRepository, timelineRepository, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsTrue(result.Contains(message));
        }

        [Test]
        public void ShouldNot_OutputPastMessagesOfTheOnesUserDoesNotFollow()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var ownUsername = "username";
            var usernameOfTheOneToFollow = "some other person";
            var message = "message";
            timelineRepository.Post(usernameOfTheOneToFollow, message);
            var command = new WallCommand(followerRepository, timelineRepository, ownUsername);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNull(result);
        }
        
        [Test]
        public void Should_OutputPastMessagesOfTheOnesUserDoesFollow()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var ownUsername = "username";
            var usernameOfTheOneToFollow = "some other person";
            var message = "message";
            timelineRepository.Post(usernameOfTheOneToFollow, message);
            followerRepository.Follow(ownUsername, usernameOfTheOneToFollow);
            var command = new WallCommand(followerRepository, timelineRepository, ownUsername);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsTrue(result.Contains(message));
        }

        [Test]
        public void Should_PrependMessagesWithUsername()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var ownUsername = "username";
            var usernameOfTheOneToFollow = "some other person";
            var message = "message";
            followerRepository.Follow(ownUsername, usernameOfTheOneToFollow);
            timelineRepository.Post(usernameOfTheOneToFollow, message);
            var command = new WallCommand(followerRepository, timelineRepository, ownUsername);

            // Act
            var result = command.Execute();

            // Assert
            var lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.True(lines[0].StartsWith(usernameOfTheOneToFollow + " - "));
        }

        [Test]
        public void Should_OutputMessagesPostedAfterFollow()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var ownUsername = "username";
            var usernameOfTheOneToFollow = "some other person";
            var message = "message";
            followerRepository.Follow(ownUsername, usernameOfTheOneToFollow);
            timelineRepository.Post(usernameOfTheOneToFollow, message);
            var command = new WallCommand(followerRepository, timelineRepository, ownUsername);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsTrue(result.Contains(message));
        }

        [Test]
        public void Should_OutputMessagesFromDifferentUsersInTheRightOrder()
        {
            // Arrange
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var usernameOfTheOneToFollow = "some other person";
            var messageText1 = "Some message 1";
            var messageText2 = "Some message 2";
            var messageText3 = "Some message 3";
            var timestamp = DateTime.Now;

            var message1 = new Message(messageText1, timestamp - TimeSpan.FromDays(5));
            var message2 = new Message(messageText2, timestamp - TimeSpan.FromDays(4));
            var message3 = new Message(messageText3, timestamp - TimeSpan.FromDays(10));
            var messagesOfTheOneToFollow = new List<Message>() { message1, message2, message3 };
            timelineRepositoryMock.Setup(repository => repository.Read(usernameOfTheOneToFollow)).Returns(messagesOfTheOneToFollow);
            
            var ownUsername = "this is me, myself";
            var ownMessageText = "this is me speaking";
            var ownMessage = new Message(ownMessageText, timestamp - TimeSpan.FromDays(7));
            var ownMessages = new List<Message>() { ownMessage };
            timelineRepositoryMock.Setup(repository => repository.Read(ownUsername)).Returns(ownMessages);

            var followerRepository = new FollowerRepository();
            followerRepository.Follow(ownUsername, usernameOfTheOneToFollow);
            var command = new WallCommand(followerRepository, timelineRepositoryMock.Object, ownUsername);

            // Act
            var result = command.Execute();

            // Assert
            var lines = result.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            Assert.True(lines[0].Contains(messageText2));
            Assert.True(lines[1].Contains(messageText1));
            Assert.True(lines[2].Contains(ownMessageText));
            Assert.True(lines[3].Contains(messageText3));
        }

        [Test]
        public void Should_ReturnNoOutputOnFreshSystem()
        {
            // Arrange
            var timelineRepository = new TimelineRepository();
            var followerRepository = new FollowerRepository();
            var username = "username";
            var command = new WallCommand(followerRepository, timelineRepository, username);

            // Act
            var result = command.Execute();

            // Assert
            Assert.IsNull(result);
        }

    }
}
