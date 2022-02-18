using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class InputResultChainBuilderBase<T, TResult, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputResultChainBuilder<T, TResult, TChainLink>
        where TChainLink : IResultChainLink<TResult>
    {
        protected InputResultChainBuilderBase(TChainLink chainLink, InputChainBuilderBase<T> previous)
            : base(chainLink, previous)
        {
        }

        public IInputRunChainBuilder<T, TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TResult, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, TNewChainLink> RunWithResult<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TResult, TNewResult, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
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

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T, TResult>(condition, this));
        }
    }
}
