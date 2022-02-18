using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{

    internal class DelegateRunResultChainBuilder<T> : RunResultChainBuilderBase<T, DelegateRunResultChainLink<T>>
    {
        public DelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<T>> del, ChainBuilderBase previous = null)
            : base(new DelegateRunResultChainLink<T>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, DelegateRunResultChainLink<T>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateWithInputRunResultChainBuilder<TInput, TResult, TChainLink> : RunResultChainBuilderBase<TInput, TResult, DelegateWithInputRunResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public DelegateWithInputRunResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, ChainBuilderBase previous = null)
            : base(new DelegateWithInputRunResultChainLink<TInput, TResult>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, DelegateWithInputRunResultChainLink<TInput, TResult>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
