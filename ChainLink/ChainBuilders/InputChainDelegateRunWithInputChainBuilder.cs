using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputChainDelegateRunWithInputChainBuilder<T, TInput> 
        : InputChainRunWithInputResultChainBuilderBase<T, TInput, TInput, DelegateRunWithInputChainLink<TInput>>
    {
        public InputChainDelegateRunWithInputChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputResultChainLinkRunner<TInput, TInput, DelegateRunWithInputChainLink<TInput>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputChainDelegateRunWithInputResultChainBuilder<T, TInput, TResult, TChainLink> 
        : InputChainRunWithInputResultChainBuilderBase<T, TInput, TResult, DelegateRunWithInputResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public InputChainDelegateRunWithInputResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, InputChainBuilderBase<T> previous = null)
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
