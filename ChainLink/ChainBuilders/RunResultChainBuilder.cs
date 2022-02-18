using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class RunResultChainBuilder<T, TChainLink> : RunResultChainBuilderBase<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        public RunResultChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, TChainLink>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
