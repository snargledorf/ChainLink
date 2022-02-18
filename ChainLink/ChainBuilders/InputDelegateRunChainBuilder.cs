using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunChainBuilder<T> : InputChainBuilderBase<T>, IChainLinkRunnerFactory, IInputRunChainBuilder<T, DelegateRunChainLink>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public InputDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(new DelegateRunChainLink(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputDelegateRunChainBuilder<T, TInput> : InputChainBuilderBase<T>, IChainLinkRunnerFactory, IInputRunChainBuilder<T, TInput, DelegateRunChainLink<TInput>>
    {
        private readonly Func<TInput, IChainLinkRunContext, CancellationToken, Task> del;

        public InputDelegateRunChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<TInput>(new DelegateRunChainLink<TInput>(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

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

    internal class InputDelegateRunResultChainBuilder<T, TInput, TResult, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public InputDelegateRunResultChainBuilder(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del, InputChainBuilderBase<T> previous = null)
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
