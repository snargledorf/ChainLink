using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class ResultChainBuilderBase<T, TChainLink> : ChainBuilderBase<TChainLink>, IResultChainBuilder<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        protected ResultChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null) 
            : base(chainLink, previous)
        {
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new RunChainBuilder<T, TNewChainLink>(ReflectionUtils.CreateObject<TNewChainLink>(args), this));
        }

        public IRunChainBuilder<T, IRunChainLink<T>> RunWithResult(IRunChainLink<T> chainLink)
        {
            return AddChildChainBuilder(new RunChainBuilder<T, IRunChainLink<T>>(chainLink, this));
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
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
}
