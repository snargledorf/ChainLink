using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
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

        IRunChainBuilder<DelegateRunChainLink> Run(Action del);
        IRunChainBuilder<DelegateRunChainLink> Run(Func<Task> del);
        IRunChainBuilder<DelegateRunChainLink> Run(Func<IChainLinkRunContext, Task> del);
        IRunChainBuilder<DelegateRunChainLink> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<T> del);
        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<Task<T>> del);
        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, T> del);
        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, Task<T>> del);
        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, CancellationToken, T> del);
        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> Run<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del);

        IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>;

        IResultChainBuilder<T, TChainLink> GetResult<T, TChainLink>(params object[] args)
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

        IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Action<T> del);
        IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, Task> del);
        IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, Task> del);
        IRunChainBuilder<T, DelegateRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, Task<TResult>> del);
        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);
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
}
