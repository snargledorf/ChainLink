using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class RunChainBuilder<TChainLink> : ChainBuilderBase<TChainLink>, IRunChainBuilder<TChainLink>
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

    internal class RunChainBuilder<T, TChainLink> : ChainBuilderBase<TChainLink>, IRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink<T>
    {
        public RunChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
