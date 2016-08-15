using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using SocialNetworkCLI.Commands.Posting;
using SocialNetworkCLI.Commands.Reading;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI
{
	class MainClass
	{
		public static int Main (string[] args)
		{
            var availableCommandFactories = new List<ICommandFactory>() {new PostingCommandFactory()};
            var defaultCommandFactory = new ReadingCommandFactory();
		    ICommandExtractor commandExtractor = new CommandExtractor(new TimelineRepository(), availableCommandFactories, defaultCommandFactory);
			var looper = new MainLoop(Console.In, Console.Out, commandExtractor);
            looper.Loop();

            Console.Out.Flush();
            return 0;
		}
	}

    
}
