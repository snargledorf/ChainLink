using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateWithInputRunChainBuilder<T, TInput> : InputRunResultChainBuilderBase<T, TInput, TInput, DelegateWithInputRunChainLink<TInput>>
    {
        public InputDelegateWithInputRunChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new DelegateWithInputRunChainLink<TInput>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TInput, DelegateWithInputRunChainLink<TInput>>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputDelegateWithInputRunResultChainBuilder<T, TInput, TResult, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TResult, DelegateWithInputRunResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public InputDelegateWithInputRunResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, InputChainBuilderBase<T> previous = null)
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
