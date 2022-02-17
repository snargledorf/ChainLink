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
        protected ChainBuilder(Type chainLinkType, object[] chainLinkArgs, IChainBuilder previous = null)
        {
            ChainLinkType = chainLinkType;
            ChainLinkArgs = chainLinkArgs;
            Previous = previous;
        }

        public IChainBuilder Root => Previous?.Root ?? this;

        public IChainBuilder Previous { get; }

        public Type ChainLinkType { get; }

        public object[] ChainLinkArgs { get; }

        protected List<IChainBuilder> Children { get; } = new List<IChainBuilder>();

        public static IRunChainBuilder<TChainLink> StartWith<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            return new RunChainBuilder<TChainLink>(args);
        }

        public static IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>
        {
            return new ResultChainBuilder<T, TChainLink>(args);
        }

        public static IRunChainBuilder<DelegateRunChainLink> StartWith(Action del)
        {
            return StartWith((_, c) => Task.Run(del, c));
        }

        public static IRunChainBuilder<DelegateRunChainLink> StartWith(Func<Task> del)
        {
            return StartWith((_, __) => del());
        }

        public static IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, Task> del)
        {
            return StartWith((context, _) => del(context));
        }

        public static IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return new DelegateRunChainBuilder(del);
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<T> del)
        {
            return StartWith((_, cancel) => Task.Run(() => del(), cancel));
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<Task<T>> del)
        {
            return StartWith((_, __) => del());
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, T> del)
        {
            return StartWith((context, cancel) => Task.Run(() => del(context), cancel));
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, Task<T>> del)
        {
            return StartWith((context, _) => del(context));
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, T> del)
        {
            return StartWith((context, cancel) => Task.Run(() => del(context, cancel), cancel));
        }

        public static IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return new DelegateRunResultChainBuilder<T>(del);
        }

        public IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            var child = new RunChainBuilder<TChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>
        {
            var child = new RunResultChainBuilder<T, TChainLink>(args, this);

            Children.Add(child);

            return child;
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

        public IChain Build()
        {
            if (Root != this)
                return Root.Build();

            // I am root!

            return new Chain(CreateChainLinkRunner());
        }

        public abstract IChainLinkRunner CreateChainLinkRunner();

        protected TChainLink CreateChainLink<TChainLink>()
        {
            if (ChainLinkArgs.Length == 0)
                return Activator.CreateInstance<TChainLink>();

            ConstructorInfo constructorInfo = ChainLinkType.GetConstructor(ChainLinkArgs.Select(arg => arg.GetType()).ToArray());
            return (TChainLink)constructorInfo.Invoke(ChainLinkArgs);
        }

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IChainBuilder
        {
            Children.Add(child);
            return child;
        }
    }
}
