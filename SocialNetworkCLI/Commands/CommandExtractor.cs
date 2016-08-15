using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public class CommandExtractor : ICommandExtractor
    {
        private readonly ITimelineRepository _timelineRepository;
        private readonly IList<ICommandFactory> _availableCommandFactories;
        private readonly ICommandFactory _defaultCommandFactory;

        public CommandExtractor(ITimelineRepository timelineRepository, IList<ICommandFactory> availableCommandFactories, ICommandFactory defaultCommandFactory){
            _timelineRepository = timelineRepository;
            _availableCommandFactories = availableCommandFactories;
            _defaultCommandFactory = defaultCommandFactory;
        }

        public ICommand Extract(string line)
        {
            var matchingFactories = from commandFactory in _availableCommandFactories
                where line.Contains(commandFactory.GetCommandVerb())
                select commandFactory;

            var factory = _defaultCommandFactory;
            var lineParts = new[] { line };
            if (matchingFactories.Any())
            {
                factory = matchingFactories.First();
                lineParts = line.Split(new[] { factory.GetCommandVerb() }, StringSplitOptions.RemoveEmptyEntries);
            }

            var username = lineParts[0].Trim();
            var argument = string.Empty;
            if (lineParts.Length > 1)
            {
                argument = lineParts[1].Trim();
            }

            return factory.GetCommand(_timelineRepository, username, argument);
        }
    }
}
