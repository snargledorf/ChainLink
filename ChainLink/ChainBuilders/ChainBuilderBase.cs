using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class ChainBuilderBase<TChainLink> : ChainBuilderBase, IChainLinkRunnerFactory
    {
        protected ChainBuilderBase(object[] chainLinkArgs, ChainBuilderBase previous = null)
            : this(ReflectionUtils.CreateObject<TChainLink>(chainLinkArgs), previous)
        {
        }

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

        public IRunChainBuilder<TNewChainLink> Run<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new RunChainBuilder<TNewChainLink>(args, this));
        }

        public IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>
        {
            return AddChildChainBuilder(new RunResultChainBuilder<T, TChainLink>(args, this));
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
            return AddChildChainBuilder(new ResultChainBuilder<T, TChainLink>(args, this));
        }

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IChainLinkRunnerFactory
        {
            Children.Add(child);
            return child;
        }
    }
}
