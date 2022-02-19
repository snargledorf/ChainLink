using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IChainBuilder
    {
        IChainBuilder Run<TChainLink>(params object[] args) where TChainLink : IRunChainLink;
        IResultChainBuilder<T> Run<T, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink, IResultChainLink<T>;

        IChainBuilder Run(Action del);
        IChainBuilder Run(Func<Task> del);
        IChainBuilder Run(Func<IChainLinkRunContext, Task> del);
        IChainBuilder Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IResultChainBuilder<T> Run<T>(Func<T> del);
        IResultChainBuilder<T> Run<T>(Func<Task<T>> del);
        IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, T> del);
        IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, Task<T>> del);
        IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, CancellationToken, T> del);
        IResultChainBuilder<T> Run<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del);

        IResultChainBuilder<T> GetResult<T, TChainLink>(params object[] args) where TChainLink : IResultChainLink<T>;

        IIfChainBuilder If(Func<bool> condition);
        IIfChainBuilder If(Func<Task<bool>> condition);
        IIfChainBuilder If(Func<IChainLinkRunContext, bool> condition);
        IIfChainBuilder If(Func<IChainLinkRunContext, Task<bool>> condition);
        IIfChainBuilder If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IIfChainBuilder If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    //public interface IRunChainBuilder<TChainLink> : IChainBuilder
    //    where TChainLink : IRunChainLink
    //{
    //}

    //public interface IRunChainBuilder<T, TChainLink> : IChainBuilder
    //    where TChainLink : IRunChainLink<T>
    //{
    //}

    public interface IResultChainBuilder<T> : IChainBuilder
    {
        new IResultChainBuilder<T> Run<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink;

        IResultChainBuilder<T> RunWithInput<TNewChainLink>(params object[] args) where TNewChainLink : IRunChainLink<T>;

        IResultChainBuilder<TResult> RunWithInput<TResult, TNewChainLink>(params object[] args)
            where TNewChainLink : IRunChainLink<T>, IResultChainLink<TResult>;

        new IResultChainBuilder<T> Run(Action del);
        new IResultChainBuilder<T> Run(Func<Task> del);
        new IResultChainBuilder<T> Run(Func<IChainLinkRunContext, Task> del);
        new IResultChainBuilder<T> Run(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IResultChainBuilder<T> RunWithInput(Action<T> del);
        IResultChainBuilder<T> RunWithInput(Func<T, Task> del);
        IResultChainBuilder<T> RunWithInput(Func<T, IChainLinkRunContext, Task> del);
        IResultChainBuilder<T> RunWithInput(Func<T, IChainLinkRunContext, CancellationToken, Task> del);

        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, TResult> del);
        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, Task<TResult>> del);
        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, IChainLinkRunContext, TResult> del);
        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);
        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);
        IResultChainBuilder<TResult> RunWithInput<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);

        new IIfChainBuilder<T> If(Func<bool> condition);
        new IIfChainBuilder<T> If(Func<Task<bool>> condition);
        new IIfChainBuilder<T> If(Func<IChainLinkRunContext, bool> condition);
        new IIfChainBuilder<T> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IIfChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IIfChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IIfChainBuilder<T> If(Func<T, bool> condition);
        IIfChainBuilder<T> If(Func<T, Task<bool>> condition);
        IIfChainBuilder<T> If(Func<T, IChainLinkRunContext, bool> condition);
        IIfChainBuilder<T> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IIfChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IIfChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    public interface IIfChainBuilder : IChainBuilder
    {
        IChainBuilder Else { get; }
    }

    public interface IIfChainBuilder<T> : IResultChainBuilder<T>
    {
        IResultChainBuilder<T> Else { get; }
    }
}
