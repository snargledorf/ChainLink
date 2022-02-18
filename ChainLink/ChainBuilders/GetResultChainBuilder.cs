using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class GetResultChainBuilder<T, TChainLink> : ResultChainBuilderBase<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        public GetResultChainBuilder(object[] args, ChainBuilderBase previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new GetResultChainLinkRunner<T>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
