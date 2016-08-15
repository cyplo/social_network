using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SocialNetworkCLI;

namespace SocialNetworkTests
{
    [TestFixture]
    public class MainLoopCommandExecutionTests
    {
        [Test]
        public void Should_ExecuteCommandGivenByTheExtractor()
        {
            // Arrange
            var inputBuilder = new StringBuilder();
            inputBuilder.AppendLine("some command");
            inputBuilder.AppendLine("exit");

            var inputBytes = Encoding.UTF8.GetBytes(inputBuilder.ToString());
            var inputStream = new MemoryStream(inputBytes);
            var rawInputReader = new StreamReader(inputStream);
            var outputWriter = new StringWriter();
            var commandMock = new Mock<ICommand>();
            var commandExtractor = new AlwaysSomeCommandExtractor(commandMock.Object);

            var looper = new MainLoop(rawInputReader, outputWriter, commandExtractor);

            // Act
            looper.Loop();

            // Assert 
            commandMock.Verify( command => command.Execute(), Times.Once );
        }

        private class AlwaysSomeCommandExtractor : ICommandExtractor
        {
            private readonly ICommand _commandToReturn;

            public AlwaysSomeCommandExtractor(ICommand commandToReturn)
            {
                _commandToReturn = commandToReturn;
            }

            public ICommand Extract(string line)
            {
                return _commandToReturn;
            }
        }
    }
}
