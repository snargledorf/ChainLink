using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public abstract class InputChainRunResultChainBuilderBase<T, TResult, TChainLink> : InputChainChainBuilderBase<T, TChainLink>, IInputResultChainBuilder<T, TResult>
        where TChainLink : IRunChainLink, IResultChainLink<TResult>
    {
        protected InputChainRunResultChainBuilderBase(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        new public IInputResultChainBuilder<T, TResult> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>>(RunChainLinkWrapperArgs.CreateArgsArray(args), this));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TResult, TNewChainLink>(args, this));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, TResult, TNewResult, TNewChainLink>(args, this));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunChainBuilder<T, TResult>(del, this));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Action<TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputChainBuilder<T, TResult>(del, this));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, TNewResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, Task<TNewResult>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputResultChainBuilder<T, TResult, TNewResult, DelegateRunWithInputResultChainLink<TResult, TNewResult>>(del, this));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T, TResult>(condition, this));
        }
    }

    public abstract class InputChainRunWithInputResultChainBuilderBase<T, TInput, TResult, TChainLink>
        : InputChainChainBuilderBase<T, TChainLink>, IInputResultChainBuilder<T, TResult>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        protected InputChainRunWithInputResultChainBuilderBase(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        new public IInputResultChainBuilder<T, TResult> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, TResult, TResult, RunChainLinkPassInputWrapper<TResult, TNewChainLink>>(RunChainLinkWrapperArgs.CreateArgsArray(args), this));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TResult, TNewChainLink>(args, this));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, TResult, TNewResult, TNewChainLink>(args, this));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunChainBuilder<T, TResult>(del, this));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Action<TResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, Task> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, Task> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputChainBuilder<T, TResult>(del, this));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, TNewResult> del)
        {
            return RunWithInput((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, Task<TNewResult>> del)
        {
            return RunWithInput((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del)
        {
            return RunWithInput((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del)
        {
            return RunWithInput((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputResultChainBuilder<T, TResult, TNewResult, DelegateRunWithInputResultChainLink<TResult, TNewResult>>(del, this));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public new IInputResultChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T, TResult>(condition, this));
        }
    }
}
