using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputRunResultChainBuilder<T, TInput, TChainLink> : InputChainBuilder<T>, IInputRunResultChainBuilder<T, TInput, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<TInput>
    {
        internal InputRunResultChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous)
            : base(chainLinkArgs, previous)
        {
        }

        public IInputRunChainBuilder<T, TInput, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TInput>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TInput, TNewChainLink>(args, this));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TInput, TResult, TNewChainLink>(args, this));
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TChainLink>(
                ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs),
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }

        public IInputRunChainBuilder<T, TInput, DelegateRunChainLink<TInput>> RunWithResult(Action<TInput> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunChainBuilder<T, TInput, DelegateRunChainLink<TInput>> RunWithResult(Func<TInput, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunChainBuilder<T, TInput, DelegateRunChainLink<TInput>> RunWithResult(Func<TInput, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunChainBuilder<T, TInput, DelegateRunChainLink<TInput>> RunWithResult(Func<TInput, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, TInput>(del, this));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, Task<TResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, IChainLinkRunContext, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>> RunWithResult<TResult>(Func<TInput, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateRunResultChainBuilder<T, TInput, TResult, DelegateRunResultChainLink<TInput, TResult>>(del, this));
        }
    }

    internal class InputRunResultChainBuilder<T, TInput, TResult, TChainLink> : InputChainBuilder<T>, IInputRunResultChainBuilder<T, TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        internal InputRunResultChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public IInputRunChainBuilder<T, TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TResult, TNewChainLink>(args, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, TNewChainLink> RunWithResult<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TResult, TNewResult, TNewChainLink>(args, this));
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(
                ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs),
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }

        public IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, TResult>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, TNewResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, Task<TNewResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>>(del, this));
        }
    }
}
