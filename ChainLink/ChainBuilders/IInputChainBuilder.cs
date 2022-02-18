using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IInputChainBuilder<T> : IDelegateRunInputChainBuilder<T>, IIfInputChainBuilder<T>, IGetResultInputChainBuilder<T>
    {
        IInputRunChainBuilder<T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;
        IInputRunResultChainBuilder<T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<TResult>;
    }

    public interface IRootInputChainBuilder<T> : IDelegateRunInputChainBuilder<T>, IIfInputChainBuilder<T>, IGetResultInputChainBuilder<T>
    {
        // TODO Figure out a way to not have to use 'new' override
        IInputRunChainBuilder<T, T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>;
        IInputRunResultChainBuilder<T, T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;

        IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Action<T> del);
        IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, Task> del);
        IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, IChainLinkRunContext, Task> del);
        IInputRunChainBuilder<T, T, DelegateRunChainLink<T>> Run(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, Task<TResult>> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        IRootInputChainBuilder<T> If(Func<T, bool> condition);
        IRootInputChainBuilder<T> If(Func<T, Task<bool>> condition);
        IRootInputChainBuilder<T> If(Func<T, IChainLinkRunContext, bool> condition);
        IRootInputChainBuilder<T> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IRootInputChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IRootInputChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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

        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, bool> condition);
        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, Task<bool>> condition);
        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, IChainLinkRunContext, bool> condition);
        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition);
        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition);
        IInputResultChainBuilder<T, TResult, TChainLink> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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

    public interface IGetResultInputChainBuilder<T>
    {
        IInputResultChainBuilder<T, TResult, TChainLink> GetResult<TResult, TChainLink>(params object[] args)
              where TChainLink : IResultChainLink<TResult>;
    }

    public interface IDelegateRunInputChainBuilder<T>
    {
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
    }

    public interface IIfInputChainBuilder<T>
    {
        IInputChainBuilder<T> If(Func<bool> condition);
        IInputChainBuilder<T> If(Func<Task<bool>> condition);
        IInputChainBuilder<T> If(Func<IChainLinkRunContext, bool> condition);
        IInputChainBuilder<T> If(Func<IChainLinkRunContext, Task<bool>> condition);
        IInputChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IInputChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }
}
