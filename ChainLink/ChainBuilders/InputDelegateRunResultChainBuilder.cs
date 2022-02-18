using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunResultChainBuilder<T, TResult> : InputRunResultChainBuilderBase<T, TResult, DelegateRunResultChainLink<TResult>>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public InputDelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del, InputChainBuilderBase<T> previous = null)
            : base(new DelegateRunResultChainLink<TResult>(del), previous)
        {
            this.del = del;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TResult, DelegateRunResultChainLink<TResult>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
