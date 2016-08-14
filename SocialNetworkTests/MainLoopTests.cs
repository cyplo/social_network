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

namespace SocialNetworkTests
{
    [TestFixture]
    [Timeout(1 * 5000)]
    public class MainLoopTests
    {
        [Test]
        public void ShouldNot_ConsumeAnythingFromTheInput_IfNotStarted()
        {
            // Arrange
            var inputReader = new Mock<TextReader>();
            var outputWriter = new Mock<TextWriter>();
            var looper = new MainLoop(inputReader.Object, outputWriter.Object);
            
            // Assert
            inputReader.Verify( reader => reader.Read(), Times.Never );
        }

        [Test]
        public void ShouldNot_WriteAnythingToTheOutput_IfNotStarted()
        {
            // Arrange
            var inputReader = new Mock<TextReader>();
            var outputWriter = new Mock<TextWriter>();
            var looper = new MainLoop(inputReader.Object, outputWriter.Object);

            // Assert
            outputWriter.Verify(writer => writer.Write(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Should_Exit_AfterConsumingSingleExitLine()
        {
            // Arrange
            var rawInputReader = new StringReader("exit" + Environment.NewLine);
            var outputWriter = new Mock<TextWriter>();
            var looper = new MainLoop(rawInputReader, outputWriter.Object);

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
            var looper = new MainLoop(rawInputReader, outputWriter.Object);

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
            var looper = new MainLoop(rawInputReader, outputWriter.Object);

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
            var looper = new MainLoop(rawInputReader, outputWriter.Object);

            // Act
            looper.Loop();

            // Assert 
            Assert.IsTrue(inputStream.Position > 0);
            Assert.AreEqual(inputStream.Length, inputStream.Position);
        }

    }
}
