using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputResultChainBuilder<T, TResult, TChainLink> : InputResultChainBuilderBase<T, TResult, TChainLink>
        where TChainLink : IResultChainLink<TResult>
    {
        public InputResultChainBuilder(TChainLink chainLink, InputChainBuilderBase<T> previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ResultChainLinkRunner<TResult>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
