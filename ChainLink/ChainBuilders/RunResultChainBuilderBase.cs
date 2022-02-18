using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class RunResultChainBuilderBase<T, TChainLink> : ChainBuilderBase<TChainLink>, IRunResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        protected RunResultChainBuilderBase(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        new public IRunResultChainBuilder<T, T, RunChainLinkPassInputWrapper<T, TNewChainLink>> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            var wrapper = new RunChainLinkWrapperArgs(args);
            return AddChildChainBuilder(new RunResultChainBuilder<T, T, RunChainLinkPassInputWrapper<T, TNewChainLink>>(new[] { wrapper }, this));
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithInput<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new RunChainBuilder<T, TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithInput<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TResult, TNewChainLink>(args, this));
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

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Action<T> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithInput(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunWithInputChainBuilder<T>(del, this));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, Task<TResult>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
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
        protected RunResultChainBuilderBase(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        new public IRunResultChainBuilder<TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            var wrapper = new RunChainLinkWrapperArgs(args);
            return AddChildChainBuilder(new RunResultChainBuilder<TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>>(new[] { wrapper }, this));
        }

        public IRunChainBuilder<TResult, TNewChainLink> RunWithInput<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new RunChainBuilder<TResult, TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<TResult, T, TNewChainLink> RunWithInput<T, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<TResult, T, TNewChainLink>(args, this));
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

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithInput(Action<TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithInput(Func<TResult, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithInput(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithInput(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunWithInputChainBuilder<TResult>(del, this));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, T> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, Task<T>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, IChainLinkRunContext, T> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, IChainLinkRunContext, Task<T>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, IChainLinkRunContext, CancellationToken, T> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IRunResultChainBuilder<TResult, T, DelegateWithInputRunResultChainLink<TResult, T>> RunWithInput<T>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<T>> del)
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
