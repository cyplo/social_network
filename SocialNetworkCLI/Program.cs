using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using SocialNetworkCLI.Commands.Following;
using SocialNetworkCLI.Commands.Posting;
using SocialNetworkCLI.Commands.Reading;
using SocialNetworkCLI.Commands.Wall;
using SocialNetworkCLI.Repositories;

namespace SocialNetworkCLI
{
	class MainClass
	{
		public static int Main (string[] args)
		{
            var availableCommandFactories = new List<ICommandFactory>() {new PostingCommandFactory(), new FollowingCommandFactory(), new WallCommandFactory()};
            var defaultCommandFactory = new ReadingCommandFactory();
		    ICommandExtractor commandExtractor = new CommandExtractor(new FollowerRepository(), new TimelineRepository(), availableCommandFactories, defaultCommandFactory);
			var looper = new MainLoop(Console.In, Console.Out, commandExtractor);
            looper.Loop();

            Console.Out.Flush();
            return 0;
		}
	}

    
}
