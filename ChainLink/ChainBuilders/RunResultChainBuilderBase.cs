using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class RunResultChainBuilderBase<T, TChainLink> : ChainBuilderBase<TChainLink>, IRunResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        public RunResultChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(chainLink, previous)
        {
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new RunChainBuilder<T, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IRunChainBuilder<T, IRunChainLink<T>> RunWithResult(IRunChainLink<T> chainLink)
        {
            return AddChildChainBuilder(new RunChainBuilder<T, IRunChainLink<T>>(chainLink, this));
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TResult, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Action<T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>>(del, this));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new IfChainBuilder<T>(condition, this));
        }
    }

    internal abstract class RunResultChainBuilderBase<TInput, TResult, TChainLink> : ChainBuilderBase<TChainLink>, IRunResultChainBuilder<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public RunResultChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(chainLink, previous)
        {
        }

        public IRunChainBuilder<TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new RunChainBuilder<TResult, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IRunChainBuilder<TResult, IRunChainLink<TResult>> RunWithResult(IRunChainLink<TResult> chainLink)
        {
            return AddChildChainBuilder(new RunChainBuilder<TResult, IRunChainLink<TResult>>(chainLink, this));
        }

        public IRunResultChainBuilder<TResult, T, TNewChainLink> RunWithResult<T, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<TResult, T, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunChainBuilder<TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<TResult>(del, this));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, Task<T>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, Task<T>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<TResult, T, DelegateRunResultChainLink<TResult, T>>(del, this));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new IfChainBuilder<TResult>(condition, this));
        }
    }
}
