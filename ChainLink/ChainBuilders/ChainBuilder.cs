using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public abstract class ChainBuilder : IChainBuilder
    {
        protected ChainBuilder(object[] chainLinkArgs, ChainBuilder previous = null)
        {
            ChainLinkArgs = chainLinkArgs;
            Previous = previous;
        }

        private ChainBuilder Root => Previous?.Root ?? this;

        private ChainBuilder Previous { get; }

        protected object[] ChainLinkArgs { get; }

        protected List<IChainBuilder> Children { get; } = new List<IChainBuilder>();

        public IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            return AddChildChainBuilder(new RunChainBuilder<TChainLink>(args, this));
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

        public abstract IChainLinkRunner CreateChainLinkRunner();

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IChainBuilder
        {
            Children.Add(child);
            return child;
        }
    }
}
