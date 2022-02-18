using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public abstract class InputRunResultChainBuilderBase<T, TResult, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunResultChainBuilder<T, TResult, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<TResult>
    {
        protected InputRunResultChainBuilderBase(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            var wrapper = new RunChainLinkWrapperArgs(args);
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>>(new[] { wrapper }, this));
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

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, TResult>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateWithInputRunChainBuilder<T, TResult>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, TNewResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, Task<TNewResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateWithInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>>(del, this));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
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

    public abstract class InputRunResultChainBuilderBase<T, TInput, TResult, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunResultChainBuilder<T, TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        protected InputRunResultChainBuilderBase(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            var wrapper = new RunChainLinkWrapperArgs(args);
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>>(new[] { wrapper }, this));
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

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T, TResult>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Action<TResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateWithInputRunChainBuilder<T, TResult>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, TNewResult> del)
        {
            return RunWithResult((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, Task<TNewResult>> del)
        {
            return RunWithResult((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del)
        {
            return RunWithResult((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del)
        {
            return RunWithResult((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateWithInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>>(del, this));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
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
