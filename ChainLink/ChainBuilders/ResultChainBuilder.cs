using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class ResultChainBuilder<T, TChainLink> : ResultChainBuilderBase<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        public ResultChainBuilder(object[] args, ChainBuilderBase previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ResultChainLinkRunner<T>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
