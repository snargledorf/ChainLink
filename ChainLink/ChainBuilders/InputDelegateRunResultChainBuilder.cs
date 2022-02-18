using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunResultChainBuilder<T, TResult> : InputRunResultChainBuilderBase<T, TResult, DelegateRunResultChainLink<TResult>>
    {
        public InputDelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del, InputChainBuilderBase<T> previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TResult, DelegateRunResultChainLink<TResult>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
