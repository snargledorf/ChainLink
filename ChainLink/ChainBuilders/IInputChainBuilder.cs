using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IInputChainBuilder<T>
    {
        IInputChainBuilder<T> Root { get; }

        IInputChainBuilder<T> Previous { get; }

        IChain<T> Build();

        IChainLinkRunner CreateChainLinkRunner();

        IInputRunChainBuilder<T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;
        IInputRunResultChainBuilder<T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<TResult>;

        IInputRunChainBuilder<T, DelegateRunChainLink> Run(Action del);
        IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<Task> del);
        IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<IChainLinkRunContext, Task> del);
        IInputRunChainBuilder<T, DelegateRunChainLink> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<TResult> del);
        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<Task<TResult>> del);
        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, TResult> del);
        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, Task<TResult>> del);
        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, TResult> del);
        IInputRunResultChainBuilder<T, TResult, DelegateRunResultChainLink<TResult>> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        IInputResultChainBuilder<T, TResult, TChainLink> GetResult<TResult, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<TResult>;
    }

    public interface IInputRunChainBuilder<T, TChainLink> : IInputChainBuilder<T>
        where TChainLink : IRunChainLink
    {
    }

    public interface IInputRunChainBuilder<T, TInput, TChainLink> : IInputChainBuilder<T>
        where TChainLink : IRunChainLink<TInput>
    {
    }

    public interface IInputResultChainBuilder<T, TResult, TChainLink> : IInputChainBuilder<T>
        where TChainLink : IResultChainLink<TResult>
    {
        IInputRunChainBuilder<T, TResult, TNewChainLink> RunWithResult<TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>;

        IInputRunResultChainBuilder<T, TResult, TNewResult, TNewChainLink> RunWithResult<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>;

        IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Action<TResult> del);
        IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del);
        IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del);
        IInputRunChainBuilder<T, TResult, DelegateRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, Task<TNewResult>> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del);
    }

    public interface IInputRunResultChainBuilder<T, TResult, TChainLink>
        : IInputChainBuilder<T>, IInputRunChainBuilder<T, TChainLink>, IInputResultChainBuilder<T, TResult, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<TResult>
    {
    }

    public interface IInputRunResultChainBuilder<T, TInput, TResult, TChainLink>
        : IInputChainBuilder<T>, IInputRunChainBuilder<T, TInput, TChainLink>, IInputResultChainBuilder<T, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
    }
}
