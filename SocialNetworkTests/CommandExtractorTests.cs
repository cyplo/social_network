using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests
{
    [TestFixture]
    public class CommandExtractorTests
    {
        [Test]
        public void Should_ExtractDefaultCommand_GivenEmptyCommandFactoryList()
        {
            // Arrange
            var defaultCommandFactoryMock = new Mock<ICommandFactory>();
            var defaultCommandMock = new Mock<ICommand>();
            defaultCommandFactoryMock.Setup(
                commandFactory =>
                    commandFactory.GetCommand(null, It.IsAny<ITimelineRepository>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(defaultCommandMock.Object);
            var extractor = new CommandExtractor(null, null, new List<ICommandFactory>(), defaultCommandFactoryMock.Object);

            // Act
            var command = extractor.Extract("some line");

            // Assert
            Assert.NotNull(command);
            Assert.AreSame(defaultCommandMock.Object, command);
        }

        [Test]
        public void Should_PassCorrectUsername_GivenEmptyCommandFactoryList()
        {
            // Arrange
            var defaultCommandFactoryMock = new Mock<ICommandFactory>();
            var defaultCommand = new Mock<ICommand>();
            defaultCommandFactoryMock.Setup(
                commandFactory =>
                    commandFactory.GetCommand(null, It.IsAny<ITimelineRepository>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(defaultCommand.Object);
            var extractor = new CommandExtractor(null, null, new List<ICommandFactory>(), defaultCommandFactoryMock.Object);

            var username = "some user";
            var line = username;

            // Act
            extractor.Extract(line);

            // Assert
            defaultCommandFactoryMock.Verify( commandFactory => commandFactory.GetCommand(null, It.IsAny<ITimelineRepository>(), username, It.IsAny<string>()) );
        }

        [Test]
        public void Should_ExtractCommand_ForTheMatchingVerb()
        {
            // Arrange
            var factory = new SomeCommandFactory();
            var factories = new List<ICommandFactory>() { factory };
            var extractor = new CommandExtractor(null, null, factories, null);
            var line = "Some user " + factory.GetCommandVerb() + " some data";

            // Act
            var command = extractor.Extract(line);

            // Assert
            Assert.IsNotNull(command);
            Assert.IsInstanceOf<SomeCommand>(command);
        }

        [Test]
        public void Should_PassTheUsernameToTheExtractedCommand()
        {
            // Arrange
            var factory = new SomeCommandFactory();
            var factories = new List<ICommandFactory>() { factory };
            var extractor = new CommandExtractor(null, null, factories, null);
            var username = "Some user";
            var line = username + " " + factory.GetCommandVerb() + " some data";

            // Act
            var command = extractor.Extract(line);

            // Assert
            var someCommand = command as SomeCommand;
            Assert.AreEqual(username, someCommand.Username);
        }

        [Test]
        public void Should_PassTheCommandArgumentToTheExtractedCommand()
        {
            // Arrange
            var factory = new SomeCommandFactory();
            var factories = new List<ICommandFactory>() { factory };
            var extractor = new CommandExtractor(null, null, factories, null);
            var username = "Some user";
            var commandArgument = "some data";
            var line = username + " " + factory.GetCommandVerb() + " " + commandArgument;

            // Act
            var command = extractor.Extract(line);

            // Assert
            var someCommand = command as SomeCommand;
            Assert.AreEqual(commandArgument, someCommand.Argument);
        }

        [Test]
        public void Should_PassTimelineRepositoryTheExtractedCommand()
        {
            // Arrange
            var factory = new SomeCommandFactory();
            var factories = new List<ICommandFactory>() { factory };
            var timelineRepositoryMock = new Mock<ITimelineRepository>();
            var extractor = new CommandExtractor(null, timelineRepositoryMock.Object, factories, null);
            var username = "Some user";
            var commandArgument = "some data";
            var line = username + " " + factory.GetCommandVerb() + " " + commandArgument;

            // Act
            var command = extractor.Extract(line);

            // Assert
            var someCommand = command as SomeCommand;
            Assert.AreEqual(timelineRepositoryMock.Object, someCommand.TimelineRepository);
        }

        [Test]
        public void Should_PassFollowerRepositoryTheExtractedCommand()
        {
            // Arrange
            var factory = new SomeCommandFactory();
            var factories = new List<ICommandFactory>() { factory };
            var followerRepositoryMock = new Mock<IFollowerRepository>();
            var extractor = new CommandExtractor(followerRepositoryMock.Object, null, factories, null);
            var username = "Some user";
            var commandArgument = "some data";
            var line = username + " " + factory.GetCommandVerb() + " " + commandArgument;

            // Act
            var command = extractor.Extract(line);

            // Assert
            var someCommand = command as SomeCommand;
            Assert.AreEqual(followerRepositoryMock.Object, someCommand.FollowerRepository);
        }

        private class SomeCommandFactory : ICommandFactory
        {
            public string GetCommandVerb()
            {
                return "COMMAND";
            }

            public ICommand GetCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository, string username, string argument = null)
            {
                return new SomeCommand(followerRepository, timelineRepository, username, argument);
            }
        }

        private class SomeCommand : ICommand
        {
            public IFollowerRepository FollowerRepository { get; }
            public ITimelineRepository TimelineRepository { get; set; }
            public string Username { get; private set; }
            public string Argument { get; private set; }

            public SomeCommand(IFollowerRepository followerRepository, ITimelineRepository timelineRepository, string username, string argument)
            {
                FollowerRepository = followerRepository;
                TimelineRepository = timelineRepository;
                Username = username;
                Argument = argument;
            }

            public string Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}
