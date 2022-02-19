using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IInputChainBuilder<T>
    {
        IInputChainBuilder<T> Run<TChainLink>(params object[] args) where TChainLink : IRunChainLink;
        IInputResultChainBuilder<T, TResult> Run<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<TResult>;

        IInputChainBuilder<T> Run(Action del);
        IInputChainBuilder<T> Run(Func<Task> del);
        IInputChainBuilder<T> Run(Func<IChainLinkRunContext, Task> del);
        IInputChainBuilder<T> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<Task<TResult>> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<IChainLinkRunContext, TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<IChainLinkRunContext, Task<TResult>> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        IInputResultChainBuilder<T, TResult> GetResult<TResult, TChainLink>(params object[] args) where TChainLink : IResultChainLink<TResult>;

        IInputIfChainBuilder<T> If(Func<bool> condition);
        IInputIfChainBuilder<T> If(Func<Task<bool>> condition);
        IInputIfChainBuilder<T> If(Func<IChainLinkRunContext, bool> condition);
        IInputIfChainBuilder<T> If(Func<IChainLinkRunContext, Task<bool>> condition);
        IInputIfChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IInputIfChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IRootInputChainBuilder<T> : IInputChainBuilder<T>
    {
        new IInputResultChainBuilder<T, T> Run<TChainLink>(params object[] args) where TChainLink : IRunChainLink;

        IInputResultChainBuilder<T, T> RunWithInput<TChainLink>(params object[] args) where TChainLink : IRunChainLink<T>;

        IInputResultChainBuilder<T, TResult> RunWithInput<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;

        new IInputResultChainBuilder<T, T> Run(Action del);
        new IInputResultChainBuilder<T, T> Run(Func<Task> del);
        new IInputResultChainBuilder<T, T> Run(Func<IChainLinkRunContext, Task> del);
        new IInputResultChainBuilder<T, T> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputResultChainBuilder<T, T> Run(Action<T> del);
        IInputResultChainBuilder<T, T> Run(Func<T, Task> del);
        IInputResultChainBuilder<T, T> Run(Func<T, IChainLinkRunContext, Task> del);
        IInputResultChainBuilder<T, T> Run(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, Task<TResult>> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IInputResultChainBuilder<T, TResult> Run<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        new IInputIfChainBuilder<T, T> If(Func<bool> condition);
        new IInputIfChainBuilder<T, T> If(Func<Task<bool>> condition);
        new IInputIfChainBuilder<T, T> If(Func<IChainLinkRunContext, bool> condition);
        new IInputIfChainBuilder<T, T> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IInputIfChainBuilder<T, T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IInputIfChainBuilder<T, T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IInputIfChainBuilder<T, T> If(Func<T, bool> condition);
        IInputIfChainBuilder<T, T> If(Func<T, Task<bool>> condition);
        IInputIfChainBuilder<T, T> If(Func<T, IChainLinkRunContext, bool> condition);
        IInputIfChainBuilder<T, T> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IInputIfChainBuilder<T, T> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IInputIfChainBuilder<T, T> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IInputResultChainBuilder<T, TResult> : IInputChainBuilder<T>
    {
        new IInputResultChainBuilder<T, TResult> Run<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink;

        IInputResultChainBuilder<T, TResult> RunWithInput<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<TResult>;

        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<TResult>, IResultChainLink<TNewResult>;

        new IInputResultChainBuilder<T, TResult> Run(Action del);
        new IInputResultChainBuilder<T, TResult> Run(Func<Task> del);
        new IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, Task> del);
        new IInputResultChainBuilder<T, TResult> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IInputResultChainBuilder<T, TResult> RunWithInput(Action<TResult> del);
        IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, Task> del);
        IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, Task> del);
        IInputResultChainBuilder<T, TResult> RunWithInput(Func<TResult, IChainLinkRunContext, CancellationToken, Task> del);

        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, TNewResult> del);
        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, Task<TNewResult>> del);
        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, TNewResult> del);
        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, Task<TNewResult>> del);
        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, TNewResult> del);
        IInputResultChainBuilder<T, TNewResult> RunWithInput<TNewResult>(Func<TResult, IChainLinkRunContext, CancellationToken, Task<TNewResult>> del);

        new IInputIfChainBuilder<T, TResult> If(Func<bool> condition);
        new IInputIfChainBuilder<T, TResult> If(Func<Task<bool>> condition);
        new IInputIfChainBuilder<T, TResult> If(Func<IChainLinkRunContext, bool> condition);
        new IInputIfChainBuilder<T, TResult> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IInputIfChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IInputIfChainBuilder<T, TResult> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IInputIfChainBuilder<T, TResult> If(Func<TResult, bool> condition);
        IInputIfChainBuilder<T, TResult> If(Func<TResult, Task<bool>> condition);
        IInputIfChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, bool> condition);
        IInputIfChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, Task<bool>> condition);
        IInputIfChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, bool> condition);
        IInputIfChainBuilder<T, TResult> If(Func<TResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IInputIfChainBuilder<T> : IInputChainBuilder<T>
    {
        IInputChainBuilder<T> Else { get; }
    }

    public interface IInputIfChainBuilder<T, TResult> : IInputResultChainBuilder<T, TResult>
    {
        IInputResultChainBuilder<T, TResult> Else { get; }
    }
}
