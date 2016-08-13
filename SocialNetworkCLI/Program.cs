using System;

namespace SocialNetworkCLI
{
	class MainClass
	{
		public static int Main (string[] args)
		{
			var looper = new MainLoop(Console.In, Console.Out);
            looper.Loop();
			return 0;
		}
	}
}
