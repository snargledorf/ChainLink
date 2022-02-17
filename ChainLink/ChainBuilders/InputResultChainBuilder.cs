using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputResultChainBuilder<T, TResult, TChainLink> : InputChainBuilder<T>, IInputResultChainBuilder<T, TResult, TChainLink>
        where TChainLink : IResultChainLink<TResult>
    {
        public InputResultChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous = null)
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
            return new ResultChainLinkRunner<TResult>(ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs), Children.Select(c => c.CreateChainLinkRunner()));
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
