using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputResultChainBuilder<T, TResult, TChainLink> : InputResultChainBuilderBase<T, TResult, TChainLink>
        where TChainLink : IResultChainLink<TResult>
    {
        public InputResultChainBuilder(object[] args, InputChainBuilderBase<T> previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new GetResultChainLinkRunner<TResult>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
