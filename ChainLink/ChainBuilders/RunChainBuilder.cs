using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class RunChainBuilder<TChainLink> : ChainBuilderBase<TChainLink>, IRunChainBuilder<TChainLink>
        where TChainLink : IRunChainLink
    {
        public RunChainBuilder(object[] chainLinkArgs, ChainBuilderBase previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public RunChainBuilder(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class RunChainBuilder<T, TChainLink> : ChainBuilderBase<TChainLink>, IRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink<T>
    {
        public RunChainBuilder(object[] chainLinkArgs, ChainBuilderBase previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public RunChainBuilder(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
