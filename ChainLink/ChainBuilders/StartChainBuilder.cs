using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class StartChainBuilderBase : IStartChainBuilder
    {
        private IChainLinkRunnerFactory rootBuilder;

        protected IChainLinkRunner BuildRunner()
        {
            return rootBuilder.CreateChainLinkRunner();
        }

        public abstract IRunChainBuilder<TChainLink> StartWith<TChainLink>(params object[] args) where TChainLink : IRunChainLink;
        public abstract IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(params object[] args) where TChainLink : IResultChainLink<T>;

        public IRunChainBuilder<TChainLink> StartWith<TChainLink>(TChainLink chainLink) where TChainLink : IRunChainLink
        {
            return SetRootBuilder(new RunChainBuilder<TChainLink>(chainLink));
        }

        public IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(TChainLink chainLink) where TChainLink : IResultChainLink<T>
        {
            return SetRootBuilder(new ResultChainBuilder<T, TChainLink>(chainLink));
        }

        public IRunChainBuilder<DelegateRunChainLink> StartWith(Action del)
        {
            return StartWith((_, c) => Task.Run(del, c));
        }

        public IRunChainBuilder<DelegateRunChainLink> StartWith(Func<Task> del)
        {
            return StartWith((_, __) => del());
        }

        public IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, Task> del)
        {
            return StartWith((context, _) => del(context));
        }
        public abstract IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, CancellationToken, Task> del);

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<T> del)
        {
            return StartWith((_, cancel) => Task.Run(() => del(), cancel));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<Task<T>> del)
        {
            return StartWith((_, __) => del());
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, T> del)
        {
            return StartWith((context, cancel) => Task.Run(() => del(context), cancel));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, Task<T>> del)
        {
            return StartWith((context, _) => del(context));
        }

        public IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, T> del)
        {
            return StartWith((context, cancel) => Task.Run(() => del(context, cancel), cancel));
        }

        public abstract IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del);

        protected TChainBuilder SetRootBuilder<TChainBuilder>(TChainBuilder builder)
            where TChainBuilder : IChainLinkRunnerFactory
        {
            rootBuilder = builder;
            return builder;
        }
    }

    internal class StartChainBuilder : StartChainBuilderBase
    {
        public IRunChainLinkRunner Build()
        {
            return (IRunChainLinkRunner)BuildRunner();
        }

        public override IRunChainBuilder<TChainLink> StartWith<TChainLink>(params object[] args)
        {
            return SetRootBuilder(new RunChainBuilder<TChainLink>(args));
        }

        public override IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(params object[] args)
        {
            return SetRootBuilder(new ResultChainBuilder<T, TChainLink>(args));
        }

        public override IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            return SetRootBuilder(new DelegateRunChainBuilder(del));
        }

        public override IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            return SetRootBuilder(new DelegateRunResultChainBuilder<T>(del));
        }
    }

    internal sealed class StartChainBuilder<T> : StartChainBuilder, IStartChainBuilder<T>
    {
        public new IRunChainLinkRunner<T> Build()
        {
            return (IRunChainLinkRunner<T>)BuildRunner();
        }

        public IInputRunChainBuilder<T, T, TChainLink> StartWithInputInto<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>
        {
            return SetRootBuilder(new InputRunChainBuilder<T, T, TChainLink>(args));
        }

        public IInputRunChainBuilder<T, T, TChainLink> StartWithInputInto<TChainLink>(TChainLink chainLink) where TChainLink : IRunChainLink<T>
        {
            return SetRootBuilder(new InputRunChainBuilder<T, T, TChainLink>(chainLink));
        }

        public IInputRunResultChainBuilder<T, T, TResult, TChainLink> StartWithInputInto<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return SetRootBuilder(new InputRunResultChainBuilder<T, T, TResult, TChainLink>(args));
        }

        public IInputRunResultChainBuilder<T, T, TResult, TChainLink> StartWithInputInto<TResult, TChainLink>(TChainLink chainLink) where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>
        {
            return SetRootBuilder(new InputRunResultChainBuilder<T, T, TResult, TChainLink>(chainLink));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, TResult> del)
        {
            return StartWithInputInto((input, _, cancel) => Task.Run(() => del(input), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, Task<TResult>> del)
        {
            return StartWithInputInto((input, _, __) => del(input));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, TResult> del)
        {
            return StartWithInputInto((input, context, cancel) => Task.Run(() => del(input, context), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del)
        {
            return StartWithInputInto((input, context, _) => del(input, context));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del)
        {
            return StartWithInputInto((input, context, cancel) => Task.Run(() => del(input, context, cancel), cancel));
        }

        public IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            return SetRootBuilder(new InputDelegateRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>>(del));
        }
    }
}
