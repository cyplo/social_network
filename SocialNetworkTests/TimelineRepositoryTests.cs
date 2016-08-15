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
    public class TimelineRepositoryTests
    {
        [Test]
        public void Reading_ShouldReturnEmptyList_OnNewRepository()
        {
            // Arrange
            var repository = new TimelineRepository();
            var username = "some username";

            // Act
            var messages = repository.Read(username);

            // Assert
            Assert.IsNotNull(messages);
            Assert.AreEqual(0, messages.Count());
        }

        [Test]
        public void Should_AcceptEmptyMessage()
        {
            // Arrange
            var repository = new TimelineRepository();
            var username = "some username";
            var message = string.Empty;
            
            // Act
            repository.Post(username, message);
        }

        [Test]
        public void ShouldNot_AcceptEmptyUsername()
        {
            // Arrange
            var repository = new TimelineRepository();
            var message = "some username";
            var username = string.Empty;

            // Act
            Assert.Throws<ArgumentException>( () => repository.Post(username, message) );
        }

        [Test]
        public void Should_BeAbleToRetrieveMessageAfterPosting()
        {
            // Arrange
            var repository = new TimelineRepository();
            var username = "some username";
            var messageText = "message";

            // Act
            repository.Post(username, messageText);

            // Assert
            var allMessages = repository.Read(username);
            Assert.AreEqual(1, allMessages.Count());
            var message = allMessages.First();
            Assert.AreEqual(messageText, message.Text);
        }

        [Test]
        public void Should_AddReasonableTimestampToMessage()
        {
            // Arrange
            var repository = new TimelineRepository();
            var username = "some username";
            var messageText = "message";
            var now = DateTime.Now;
            // Act
            repository.Post(username, messageText);

            // Assert
            var allMessages = repository.Read(username);
            Assert.AreEqual(1, allMessages.Count());
            var message = allMessages.First();
            Assert.GreaterOrEqual(now, message.Timestamp);
            Assert.LessOrEqual(message.Timestamp, now + TimeSpan.FromSeconds(1));
        }

        [Test]
        public void Should_NotAllowMessageLeaksBetweenUsers()
        {
            // Arrange
            var repository = new TimelineRepository();
            var username1 = "some username 1";
            var username2 = "some username 2";
            var messageText = "message";

            // Act
            repository.Post(username1, messageText);

            // Assert
            var allMessages = repository.Read(username2);
            Assert.AreEqual(0, allMessages.Count());
        }

    }
}
