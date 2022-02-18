using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{

    internal class DelegateRunResultChainBuilder<T> : RunResultChainBuilderBase<T, DelegateRunResultChainLink<T>>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<T>> del;

        public DelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<T>> del, ChainBuilderBase previous = null)
            : base(new DelegateRunResultChainLink<T>(del), previous)
        {
            this.del = del;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, DelegateRunResultChainLink<T>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateRunResultChainBuilder<TInput, TResult, TChainLink> : RunResultChainBuilderBase<TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public DelegateRunResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, ChainBuilderBase previous = null)
            : base(new DelegateRunResultChainLink<TInput, TResult>(del), previous)
        {
            this.del = del;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
