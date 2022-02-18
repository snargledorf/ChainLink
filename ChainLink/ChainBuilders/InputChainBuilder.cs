using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal sealed class InputChainBuilder<T> : InputChainBuilderBase<T>, IRootInputChainBuilder<T>
    {
        public IRunWithInputChainLinkRunner<T>[] Build() => Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunWithInputChainLinkRunner<T>>().ToArray();

        #region Run Implementation

        #region Run ChainLink

        new public IInputResultChainBuilder<T, T> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, T, T, RunChainLinkPassInputWrapper<T, TChainLink>>(RunChainLinkWrapperArgs.CreateArgsArray(args), this));
        }

        public IInputResultChainBuilder<T, T> RunWithInput<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, T, TChainLink>(args, this));
        }

        public IInputResultChainBuilder<T, TResult> RunWithInput<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputChainRunWithInputResultChainBuilder<T, T, TResult, TChainLink>(args, this));
        }

        #endregion

        #region Run Delegate

        new public IInputResultChainBuilder<T, T> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        new public IInputResultChainBuilder<T, T> Run(Func<Task> del)
        {
            return Run((IChainLinkRunContext _, CancellationToken __) => del());
        }

        new public IInputResultChainBuilder<T, T> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        new public IInputResultChainBuilder<T, T> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunChainBuilder<T, T>(del, this));
        }

        public IInputResultChainBuilder<T, T> Run(Action<T> del)
        {
            return Run((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, T> Run(Func<T, Task> del)
        {
            return Run((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, T> Run(Func<T, IChainLinkRunContext, Task> del)
        {
            return Run((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, T> Run(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputChainBuilder<T, T>(del, this));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, TResult> del)
        {
            return Run((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, Task<TResult>> del)
        {
            return Run((input, _, __) => del(input));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return Run((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return Run((input, context, _) => del(input, context));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return Run((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new InputChainDelegateRunWithInputResultChainBuilder<T, T, TResult, DelegateRunWithInputResultChainLink<T, TResult>>(del, this));
        }

        #endregion

        #endregion

        #region If Implementation

        public IInputResultChainBuilder<T, T> If(Func<T, bool> condition)
        {
            return If((input, _, cancel) => Task.Run(() => condition(input), cancel));
        }

        public IInputResultChainBuilder<T, T> If(Func<T, Task<bool>> condition)
        {
            return If((input, _, __) => condition(input));
        }

        public IInputResultChainBuilder<T, T> If(Func<T, IChainLinkRunContext, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context), cancel));
        }

        public IInputResultChainBuilder<T, T> If(Func<T, IChainLinkRunContext, Task<bool>> condition)
        {
            return If((input, context, _) => condition(input, context));
        }

        public IInputResultChainBuilder<T, T> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((input, context, cancel) => Task.Run(() => condition(input, context, cancel), cancel));
        }

        public IInputResultChainBuilder<T, T> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T, T>(condition));
        }

        public new IInputResultChainBuilder<T, T> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public new IInputResultChainBuilder<T, T> If(Func<Task<bool>> condition)
        {
            return If((_, __, ___) => condition());
        }

        public new IInputResultChainBuilder<T, T> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public new IInputResultChainBuilder<T, T> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((_, context, __) => condition(context));
        }

        public new IInputResultChainBuilder<T, T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((_, context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public new IInputResultChainBuilder<T, T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return If((_, context, cancel) => condition(context, cancel));
        }

        #endregion
    }
}
