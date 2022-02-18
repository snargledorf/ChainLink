using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IChainBuilder
    {
        IRunChainBuilder<TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;
        IRunChainBuilder<TChainLink> Run<TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink;
        IRunResultChainBuilder<T, TChainLink> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>;
        IResultChainBuilder<T, TChainLink> Run<T, TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink, IResultChainLink<T>;

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

        IResultChainBuilder<T, TChainLink> GetResult<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>;

        IChainBuilder If(Func<bool> condition);
        IChainBuilder If(Func<Task<bool>> condition);
        IChainBuilder If(Func<IChainLinkRunContext, bool> condition);
        IChainBuilder If(Func<IChainLinkRunContext, Task<bool>> condition);
        IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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
        IRunChainBuilder<T, IRunChainLink<T>> RunWithResult(IRunChainLink<T> chainLink);

        IRunResultChainBuilder<T, TResult, TNewChainLink> RunWithResult<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>;

        new IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Action del);
        new IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<Task> del);
        new IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, Task> del);
        new IRunResultChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Action<T> del);
        IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, Task> del);
        IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, Task> del);
        IRunResultChainBuilder<T, T, DelegateWithInputRunChainLink<T>> RunWithResult(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, Task<TResult>> del);
        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IRunResultChainBuilder<T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> RunWithResult<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, bool> condition);
        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, Task<bool>> condition);
        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, bool> condition);
        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IRunResultChainBuilder<T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IRunResultChainBuilder<T, TChainLink>
        : IChainBuilder, IRunChainBuilder<TChainLink>, IResultChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
    }

    public interface IRunResultChainBuilder<TInput, TResult, TChainLink>
        : IChainBuilder, IRunChainBuilder<TInput, TChainLink>, IResultChainBuilder<TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
    }
}
