using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public class MainLoop
    {
        private readonly TextReader _inputReader;
        private readonly ICommandExtractor _commandExtractor;
        private readonly TextWriter _outputWriter;

        public MainLoop(TextReader inputReader, TextWriter outputWriter, ICommandExtractor commandExtractor)
        {
            _inputReader = inputReader;
            _outputWriter = outputWriter;
            _commandExtractor = commandExtractor;
        }

        public void Loop()
        {
            var line = string.Empty;
            do
            {
                line = _inputReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                line = line.Trim();
                if (line.ToLower() == "exit" || line.ToLower() == "quit")
                {
                    break;
                }

                var command = _commandExtractor.Extract(line);
                var result = command.Execute();
                if (string.IsNullOrWhiteSpace(result))
                {
                    continue;
                }

                _outputWriter.Write(result + Environment.NewLine);
                _outputWriter.Flush();

            } while (true);
        }
    }
}
