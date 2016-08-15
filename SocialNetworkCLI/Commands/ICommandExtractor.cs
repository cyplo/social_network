using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkCLI
{
    public interface ICommandExtractor
    {
        ICommand Extract(string line);
    }
}
