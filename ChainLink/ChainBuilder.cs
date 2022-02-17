using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChainLink
{
    public interface IChainBuilder
    {
        Type ChainLinkType { get; }

        object[] ChainLinkArgs { get; }

        IChainBuilder Root { get; }

        IChainBuilder Previous { get; }

        IChain Build();

        IChainLinkRunner CreateChainLinkRunner();

        IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;

        IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>;

        IResultChainBuilder<T, TChainLink> Get<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>;
    }

    public interface IRunChainBuilder<TChainLink> : IChainBuilder
        where TChainLink : IRunChainLink
    {
    }

    public interface IRunChainBuilder<T, TChainLink> : IChainBuilder
        where TChainLink : IRunChainLink<T>
    {
    }

    public interface IResultChainBuilder<T, TChainLink> : IChainBuilder
        where TChainLink : IResultChainLink<T>
    {
        IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>;

        IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>;
    }

    public interface IRunResultChainBuilder<TInput, TResult, TChainLink>
        : IChainBuilder, IRunChainBuilder<TInput, TChainLink>, IResultChainBuilder<TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
    }

    public interface IRunResultChainBuilder<T, TChainLink>
        : IChainBuilder, IRunChainBuilder<TChainLink>, IResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
    }

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

        public IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink
        {
            var child = new RunChainBuilder<TChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public IResultChainBuilder<T, TChainLink> Get<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>
        {
            var child = new ResultChainBuilder<T, TChainLink>(args, this);

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
    }

    public class RunChainBuilder<TChainLink> : ChainBuilder, IRunChainBuilder<TChainLink>
        where TChainLink : IRunChainLink
    {
        internal RunChainBuilder(object[] chainLinkArgs, IChainBuilder previous = null)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    public class RunChainBuilder<T, TChainLink> : ChainBuilder, IRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink<T>
    {
        public RunChainBuilder(object[] chainLinkArgs, IChainBuilder previous)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<T>(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    public class ResultChainBuilder<T, TChainLink> : ChainBuilder, IResultChainBuilder<T, TChainLink>
        where TChainLink : IResultChainLink<T>
    {
        public ResultChainBuilder(object[] chainLinkArgs, IChainBuilder previous = null)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>
        {
            var child = new RunChainBuilder<T, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            var child = new RunResultChainBuilder<T, TResult, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ResultChainLinkRunner<T>(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    public class RunResultChainBuilder<T, TChainLink> : ChainBuilder, IRunResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        internal RunResultChainBuilder(object[] chainLinkArgs, IChainBuilder previous)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public IRunChainBuilder<T, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>
        {
            var child = new RunChainBuilder<T, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args) 
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            var child = new RunResultChainBuilder<T, TResult, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, TChainLink>(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    public class RunResultChainBuilder<TInput, TResult, TChainLink> : ChainBuilder, IRunResultChainBuilder<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        internal RunResultChainBuilder(object[] chainLinkArgs, IChainBuilder previous)
            : base(typeof(TChainLink), chainLinkArgs, previous)
        {
        }

        public IRunChainBuilder<TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args) 
            where TNewChainLink : IRunChainLink<TResult>
        {
            var child = new RunChainBuilder<TResult, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public IRunResultChainBuilder<TResult, T, TNewChainLink> RunWithResult<T, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<T>
        {
            var child = new RunResultChainBuilder<TResult, T, TNewChainLink>(args, this);

            Children.Add(child);

            return child;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(CreateChainLink<TChainLink>(), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }
}
