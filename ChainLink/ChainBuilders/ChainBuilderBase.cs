using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class ChainBuilderBase<TChainLink> : ChainBuilderBase, IChainLinkRunnerFactory
        where TChainLink : IChainLink
    {
        protected ChainBuilderBase(object[] args, ChainBuilderBase previous = null)
            : base(previous)
        {
            ChainLinkDescription = new ChainLinkDescription(typeof(TChainLink), args);
        }

        public ChainLinkDescription ChainLinkDescription { get; }

        public abstract IChainLinkRunner CreateChainLinkRunner();
    }

    internal abstract class ChainBuilderBase : IChainBuilder
    {
        protected List<IChainLinkRunnerFactory> Children { get; } = new List<IChainLinkRunnerFactory>();

        protected ChainBuilderBase(ChainBuilderBase previous = null)
        {
            Previous = previous;
        }

        protected ChainBuilderBase Root => Previous?.Root ?? this;

        protected ChainBuilderBase Previous { get; }

        public IChainBuilder Run<TChainLink>(params object[] args) where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new RunChainBuilder<TChainLink>(args, this));
        }

        public IResultChainBuilder<T> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TChainLink>(args, this));
        }

        public IChainBuilder Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IChainBuilder Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        public IChainBuilder Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        public IChainBuilder Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder(del, this));
        }

        public IResultChainBuilder<T> Run<T>(Func<T> del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IResultChainBuilder<T> Run<T>(Func<Task<T>> del)
        {
            return Run((_, __) => del());
        }

        public IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, T> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context), cancel));
        }

        public IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, Task<T>> del)
        {
            return Run((context, _) => del(context));
        }

        public IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, CancellationToken, T> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context, cancel), cancel));
        }

        public IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<T>(del, this));
        }

        public IResultChainBuilder<T> GetResult<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>
        {
            return AddChildChainBuilder(new GetResultChainBuilder<T, TChainLink>(args, this));
        }

        public IIfChainBuilder If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public IIfChainBuilder If(Func<Task<bool>> condition)
        {
            return If((_, __) => condition());
        }

        public IIfChainBuilder If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public IIfChainBuilder If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((context, _) => condition(context));
        }

        public IIfChainBuilder If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public IIfChainBuilder If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            return AddChildChainBuilder(new IfChainBuilder(condition, this));
        }

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IChainLinkRunnerFactory
        {
            Children.Add(child);
            return child;
        }
    }
}
