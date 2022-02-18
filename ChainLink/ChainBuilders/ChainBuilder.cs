using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class ChainBuilder : ChainBuilderBase
    {
        public IRunChainLinkRunner[] Build() => Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunChainLinkRunner>().ToArray();
    }
}
