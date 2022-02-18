using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IInputChainBuilder<T>
    {
        IInputRunChainBuilder<T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;
        IInputRunChainBuilder<T, IRunChainLink> Run(IRunChainLink chainLink);
        IInputRunResultChainBuilder<T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<TResult>;
        IInputRunResultChainBuilder<T, TResult, TChainLink> Run<TResult, TChainLink>(TChainLink chainLink)
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
        IInputResultChainBuilder<T, TResult, IResultChainLink<TResult>> GetResult<TResult>(IResultChainLink<TResult> chainLink);

        IInputRunChainBuilder<T, IfChainLink> If(Func<bool> condition);
        IInputRunChainBuilder<T, IfChainLink> If(Func<Task<bool>> condition);
        IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, bool> condition);
        IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, Task<bool>> condition);
        IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IInputRunChainBuilder<T, IfChainLink> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IRootInputChainBuilder<T> : IInputChainBuilder<T>
    {
        new IInputRunChainBuilder<T, T, TChainLink> Run<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>;
        IInputRunChainBuilder<T, T, TChainLink> Run<TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink<T>;
        new IInputRunResultChainBuilder<T, T, TResult, TChainLink> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;
        new IInputRunResultChainBuilder<T, T, TResult, TChainLink> Run<TResult, TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;

        new IInputRunResultChainBuilder<T, T, T, DelegateRunChainLink<T>> Run(Action del);
        new IInputRunResultChainBuilder<T, T, T, DelegateRunChainLink<T>> Run(Func<Task> del);
        new IInputRunResultChainBuilder<T, T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, Task> del);
        new IInputRunResultChainBuilder<T, T, T, DelegateRunChainLink<T>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, T, T, DelegateWithInputRunChainLink<T>> Run(Action<T> del);
        IInputRunResultChainBuilder<T, T, T, DelegateWithInputRunChainLink<T>> Run(Func<T, Task> del);
        IInputRunResultChainBuilder<T, T, T, DelegateWithInputRunChainLink<T>> Run(Func<T, IChainLinkRunContext, Task> del);
        IInputRunResultChainBuilder<T, T, T, DelegateWithInputRunChainLink<T>> Run(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, Task<TResult>> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IInputRunResultChainBuilder<T, T, TResult, DelegateWithInputRunResultChainLink<T, TResult>> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<bool> condition);
        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<Task<bool>> condition);
        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, bool> condition);
        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, bool> condition);
        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, Task<bool>> condition);
        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, bool> condition);
        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IInputRunResultChainBuilder<T, T, T, IfChainLink<T>> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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

        new IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Action del);
        new IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<Task> del);
        new IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, Task> del);
        new IInputRunResultChainBuilder<T, TResult, TResult, DelegateRunChainLink<TResult>> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Action<TResult> del);
        IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, Task> del);
        IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, Task> del);
        IInputRunResultChainBuilder<T, TResult, TResult, DelegateWithInputRunChainLink<TResult>> RunWithResult(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del);

        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, Task<TNewResult>> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del);
        IInputRunResultChainBuilder<T, TResult, TNewResult, DelegateWithInputRunResultChainLink<TResult, TNewResult>> RunWithResult<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del);

        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<bool> condition);
        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<Task<bool>> condition);
        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, bool> condition);
        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, bool> condition);
        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, Task<bool>> condition);
        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, bool> condition);
        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition);
        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition);
        IInputRunResultChainBuilder<T, TResult, TResult, IfChainLink<TResult>> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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
