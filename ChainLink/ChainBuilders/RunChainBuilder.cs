using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class RunChainBuilder<TChainLink> : ChainBuilderBase<TChainLink>
        where TChainLink : IRunChainLink
    {
        public RunChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
