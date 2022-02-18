using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public abstract class InputChainBuilderBase<T, TChainLink> : InputChainBuilderBase<T>, IChainLinkRunnerFactory
    {
        protected InputChainBuilderBase(object[] args, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            ChainLinkDescription = new ChainLinkDescription(typeof(TChainLink), args);
        }

        public ChainLinkDescription ChainLinkDescription { get; }

        public abstract IChainLinkRunner CreateChainLinkRunner();
    }

    public abstract class InputChainBuilderBase<T> : IInputChainBuilder<T>
    {
        protected InputChainBuilderBase(InputChainBuilderBase<T> previous = null)
        {
            Previous = previous;
        }

        public InputChainBuilderBase<T> Root => Previous?.Root ?? this;

        public InputChainBuilderBase<T> Previous { get; }

        protected List<IChainLinkRunnerFactory> Children { get; } = new List<IChainLinkRunnerFactory>();

        public IInputRunChainBuilder<T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new InputRunChainBuilder<T, TChainLink>(args, this));
        }

        public IInputRunResultChainBuilder<T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputRunResultChainBuilder<T, TResult, TChainLink>(args, this));
        }

        public IInputRunChainBuilder<T, DelegateRunChainLink> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        public IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        public IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new InputDelegateRunChainBuilder<T>(del, this));
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<TResult> del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<Task<TResult>> del)
        {
            return Run((_, __) => del());
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, TResult> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, Task<TResult>> del)
        {
            return Run((context, _) => del(context));
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return AddChildChainBuilder(new InputDelegateRunResultChainBuilder<T, TResult>(del, this));
        }

        public IInputResultChainBuilder<T, TResult, TChainLink> GetResult<TResult, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<TResult>
        {
            return AddChildChainBuilder(new InputResultChainBuilder<T, TResult, TChainLink>(args, this));
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<Task<bool>> condition)
        {
            return If((_, __) => condition());
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((context, _) => condition(context));
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new InputIfChainBuilder<T>(condition, this));
        }

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IChainLinkRunnerFactory
        {
            Children.Add(child);
            return child;
        }
    }
}
