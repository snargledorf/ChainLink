using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class ChainBuilderBase<TChainLink> : ChainBuilderBase, IChainLinkRunnerFactory
    {
        protected ChainBuilderBase(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(previous)
        {
            ChainLink = chainLink;
        }

        public TChainLink ChainLink { get; }

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

        public IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new RunChainBuilder<TChainLink>(ReflectionUtils.CreateObject<TChainLink>(args), this));
        }

        public IRunChainBuilder<TChainLink> Run<TChainLink>(TChainLink chainLink) 
            where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new RunChainBuilder<TChainLink>(chainLink, this));
        }

        public IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TChainLink>(ReflectionUtils.CreateObject<TChainLink>(args), this));
        }

        public IResultChainBuilder<T, TChainLink> Run<T, TChainLink>(TChainLink chainLink) 
            where TChainLink : IRunChainLink, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TChainLink>(chainLink, this));
        }

        public IRunChainBuilder<DelegateRunChainLink> Run(Action del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IRunChainBuilder<DelegateRunChainLink> Run(Func<Task> del)
        {
            return Run((_, __) => del());
        }

        public IRunChainBuilder<DelegateRunChainLink> Run(Func<IChainLinkRunContext, Task> del)
        {
            return Run((context, _) => del(context));
        }

        public IRunChainBuilder<DelegateRunChainLink> Run(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return AddChildChainBuilder(new DelegateRunChainBuilder(del, this));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<T> del)
        {
            return Run((_, cancel) => Task.Run(del, cancel));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<Task<T>> del)
        {
            return Run((_, __) => del());
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, T> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context), cancel));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, Task<T>> del)
        {
            return Run((context, _) => del(context));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, CancellationToken, T> del)
        {
            return Run((context, cancel) => Task.Run(() => del(context, cancel), cancel));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return AddChildChainBuilder(new DelegateRunResultChainBuilder<T>(del, this));
        }

        public IResultChainBuilder<T, TChainLink> GetResult<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>
        {
            return AddChildChainBuilder(new ResultChainBuilder<T, TChainLink>(ReflectionUtils.CreateObject<TChainLink>(args), this));
        }

        public IChainBuilder If(Func<bool> condition)
        {
            return If((_, cancel) => Task.Run(() => condition(), cancel));
        }

        public IChainBuilder If(Func<Task<bool>> condition)
        {
            return If((_, __) => condition());
        }

        public IChainBuilder If(Func<IChainLinkRunContext, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context), cancel));
        }

        public IChainBuilder If(Func<IChainLinkRunContext, Task<bool>> condition)
        {
            return If((context, _) => condition(context));
        }

        public IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, bool> condition)
        {
            return If((context, cancel) => Task.Run(() => condition(context, cancel), cancel));
        }

        public IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
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
