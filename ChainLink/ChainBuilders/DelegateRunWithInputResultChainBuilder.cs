using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{

    internal class DelegateRunResultChainBuilder<T> : RunResultChainBuilderBase<T, DelegateRunResultChainLink<T>>
    {
        public DelegateRunResultChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<T>> del, ChainBuilderBase previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, DelegateRunResultChainLink<T>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateRunWithInputResultChainBuilder<TInput, TResult, TChainLink> : RunResultChainBuilderBase<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public DelegateRunWithInputResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, ChainBuilderBase previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputResultChainLinkRunner<TInput, TResult, DelegateRunWithInputResultChainLink<TInput, TResult>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
