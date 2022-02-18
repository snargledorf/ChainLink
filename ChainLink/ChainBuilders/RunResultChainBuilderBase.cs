using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class RunResultChainBuilderBase<T, TChainLink> : ChainBuilderBase<TChainLink>, IRunResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        protected RunResultChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null)
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

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Action<T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunWithInputChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new DelegateWithInputRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>>(del, this));
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
        protected RunResultChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null)
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

        new public IRunResultChainBuilder<TResult, TResult, DelegateRunChainLink<TResult>> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IRunResultChainBuilder<TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IRunResultChainBuilder<TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IRunResultChainBuilder<TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder<TResult>(del, this));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunWithInputChainBuilder<TResult>(del, this));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, T> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, Task<T>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, Task<T>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, T> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithResult<T>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return AddChildChainBuilder(new DelegateWithInputRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>>(del, this));
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
