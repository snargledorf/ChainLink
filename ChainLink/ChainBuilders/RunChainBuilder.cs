using System.Linq;

namespace ChainLink.ChainBuilders
{
    public class RunChainBuilder<TChainLink> : ChainBuilder, IRunChainBuilder<TChainLink>
        where TChainLink : IRunChainLink
    {
        internal RunChainBuilder(object[] chainLinkArgs, ChainBuilder previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    public class RunChainBuilder<T, TChainLink> : ChainBuilder, IRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink<T>
    {
        public RunChainBuilder(object[] chainLinkArgs, ChainBuilder previous)
            : base(chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
