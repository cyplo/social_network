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
        private TextReader _inputReader;

        public MainLoop(TextReader inputReader, TextWriter outputWriter)
        {
            _inputReader = inputReader;
        }

        public void Loop()
        {
            var lastLine = string.Empty;
            do
            {
                lastLine = _inputReader.ReadLine();
                if(!string.IsNullOrWhiteSpace(lastLine))
                {
                    lastLine = lastLine.Trim().ToLower();   
                }
            } while (lastLine != "exit" && lastLine != "quit");
        }
    }
}
