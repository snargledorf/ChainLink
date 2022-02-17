using System.Linq;

namespace ChainLink.ChainBuilders
{
    public class RunChainBuilder<TChainLink> : ChainBuilder, IRunChainBuilder<TChainLink>
        where TChainLink : IRunChainLink
    {
        internal RunChainBuilder(object[] chainLinkArgs, IChainBuilder previous = null)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    public class RunChainBuilder<T, TChainLink> : ChainBuilder, IRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink<T>
    {
        public RunChainBuilder(object[] chainLinkArgs, IChainBuilder previous)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }
}
