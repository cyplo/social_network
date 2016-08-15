using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkTests
{
    [TestFixture]
    [Timeout(5 * 1000)]
    public class MainLoopTests
    {
        [Test]
        public void ShouldNot_ConsumeAnythingFromTheInput_IfNotStarted()
        {
            // Arrange
            var inputReader = new Mock<TextReader>();
            var outputWriter = new Mock<TextWriter>();
            var looper = new MainLoop(inputReader.Object, outputWriter.Object, null);
            
            // Assert
            inputReader.Verify( reader => reader.Read(), Times.Never );
        }

        [Test]
        public void ShouldNot_WriteAnythingToTheOutput_IfNotStarted()
        {
            // Arrange
            var inputReader = new Mock<TextReader>();
            var outputWriter = new Mock<TextWriter>();
            var looper = new MainLoop(inputReader.Object, outputWriter.Object, null);

            // Assert
            outputWriter.Verify(writer => writer.Write(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Should_Exit_AfterConsumingSingleExitLine()
        {
            // Arrange
            var rawInputReader = new StringReader("exit" + Environment.NewLine);
            var outputWriter = new Mock<TextWriter>();
            var extractor = new AlwaysNullCommandExtractor();
            var looper = new MainLoop(rawInputReader, outputWriter.Object, extractor);

            // Act
            looper.Loop();

            // Assert 
            // We just want no timeout triggered here
        }

        [Test]
        public void Should_Exit_AfterConsumingSingleQuitLine()
        {
            // Arrange
            var rawInputReader = new StringReader("quit" + Environment.NewLine);
            var outputWriter = new Mock<TextWriter>();
            var extractor = new AlwaysNullCommandExtractor();
            var looper = new MainLoop(rawInputReader, outputWriter.Object, extractor);

            // Act
            looper.Loop();

            // Assert 
            // We just want no timeout triggered here
        }

        [Test]
        public void Should_Exit_AfterConsumingSingleExitLineCasedStrangely()
        {
            // Arrange
            var rawInputReader = new StringReader("eXiT" + Environment.NewLine);
            var outputWriter = new Mock<TextWriter>();
            var extractor = new AlwaysNullCommandExtractor();
            var looper = new MainLoop(rawInputReader, outputWriter.Object, extractor);

            // Act
            looper.Loop();

            // Assert 
            // We just want no timeout triggered here
        }

        [Test]
        public void Should_ConsumeAllInput_BeforeExiting()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("a");
            inputBuilder.AppendLine("b");
            inputBuilder.AppendLine("c");
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriter = new Mock<TextWriter>();
            var extractor = new AlwaysNullCommandExtractor();
            var looper = new MainLoop(rawInputReader, outputWriter.Object, extractor);

            // Act
            looper.Loop();

            // Assert 
            Assert.GreaterOrEqual(inputStream.Position, inputBytes.Length);
        }

        [Test]
        public void Should_ConsumeAllEmptyLines_BeforeExiting()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine();
            inputBuilder.AppendLine();
            inputBuilder.AppendLine();
            inputBuilder.AppendLine();
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriter = new Mock<TextWriter>();
            var extractor = new AlwaysNullCommandExtractor();
            var looper = new MainLoop(rawInputReader, outputWriter.Object, extractor);

            // Act
            looper.Loop();

            // Assert 
            Assert.GreaterOrEqual(inputStream.Position, inputBytes.Length);
        }

        [Test]
        public void Should_WriteCommandOutputToOutputWriter()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("some command");
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriterMock = new Mock<TextWriter>();
            var someMessage = "some message"; 
            var extractor = new AlwaysMessageCommandExtractor(someMessage);
            var looper = new MainLoop(rawInputReader, outputWriterMock.Object, extractor);

            // Act
            looper.Loop();

            // Assert
            outputWriterMock.Verify( writer => writer.Write( someMessage + Environment.NewLine ), Times.Once );
        }

        [Test]
        public void ShouldNot_WriteNullCommandOutputToOutputWriter()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("some command");
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriterMock = new Mock<TextWriter>();
            string someMessage = null;
            var extractor = new AlwaysMessageCommandExtractor(someMessage);
            var looper = new MainLoop(rawInputReader, outputWriterMock.Object, extractor);

            // Act
            looper.Loop();

            // Assert
            outputWriterMock.Verify(writer => writer.Write(someMessage + Environment.NewLine), Times.Never);
        }

        [Test]
        public void ShouldNot_ChangeTheCaseOfTheLinePassed()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            var line = "SOME Command";
            inputBuilder.AppendLine(line);
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriterMock = new Mock<TextWriter>();
            var extractorMock = new Mock<ICommandExtractor>();
            extractorMock.Setup(extractor => extractor.Extract(It.IsAny<string>())).Returns(new NullCommand());
            var looper = new MainLoop(rawInputReader, outputWriterMock.Object, extractorMock.Object);

            // Act
            looper.Loop();

            // Assert
            extractorMock.Verify( extractor => extractor.Extract(line), Times.Once);
        }

        private class AlwaysNullCommandExtractor : ICommandExtractor
        {
            public ICommand Extract(string line)
            {
                return new NullCommand();
            }
        }

        private class AlwaysMessageCommandExtractor : ICommandExtractor
        {
            private readonly string _message;

            public AlwaysMessageCommandExtractor(string message)
            {
                _message = message;
            }

            public ICommand Extract(string line)
            {
                return new AlwaysMessageCommand(_message);
            }
        }

        private class NullCommand : ICommand
        {
            public IFollowerRepository FollowerRepository { get; }
            public ITimelineRepository TimelineRepository { get; }
            public string Username { get; }
            public string Argument { get; }

            public string Execute()
            {
                return null;
            }
        }

        private class AlwaysMessageCommand : ICommand
        {
            public IFollowerRepository FollowerRepository { get; }
            public ITimelineRepository TimelineRepository { get; }
            public string Username { get; }
            public string Argument { get; }

            private readonly string _message;

            public AlwaysMessageCommand(string message)
            {
                _message = message;
            }

            public string Execute()
            {
                return _message;
            }
        }



    }
}
