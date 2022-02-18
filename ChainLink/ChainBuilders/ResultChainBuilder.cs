using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class ResultChainBuilder<T, TChainLink> : ResultChainBuilderBase<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        public ResultChainBuilder(TChainLink chainLink, ChainBuilderBase previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ResultChainLinkRunner<T>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
