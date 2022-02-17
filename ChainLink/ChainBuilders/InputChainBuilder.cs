using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class InputChainBuilder<T> : IInputChainBuilder<T>
    {
        protected InputChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous = null)
        {
            ChainLinkArgs = chainLinkArgs;
            Previous = previous;
        }

        public IInputChainBuilder<T> Root => Previous?.Root ?? this;

        public IInputChainBuilder<T> Previous { get; }

        protected object[] ChainLinkArgs { get; }

        protected List<IInputChainBuilder<T>> Children { get; } = new List<IInputChainBuilder<T>>();

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

        public abstract IChainLinkRunner CreateChainLinkRunner();

        protected TChainBuilder AddChildChainBuilder<TChainBuilder>(TChainBuilder child)
            where TChainBuilder : IInputChainBuilder<T>
        {
            Children.Add(child);
            return child;
        }

        public IChain<T> Build()
        {
            if (Root != this)
                return Root.Build();

            // I am root!

            return new Chain<T>((IRunChainLinkRunner<T>)CreateChainLinkRunner());
        }
    }
}
