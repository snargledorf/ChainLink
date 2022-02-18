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

        IChainBuilder If(Func<bool> condition);
        IChainBuilder If(Func<Task<bool>> condition);
        IChainBuilder If(Func<IChainLinkRunContext, bool> condition);
        IChainBuilder If(Func<IChainLinkRunContext, Task<bool>> condition);
        IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        IChainBuilder If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);
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

        new IResultChainBuilder<T> If(Func<bool> condition);
        new IResultChainBuilder<T> If(Func<Task<bool>> condition);
        new IResultChainBuilder<T> If(Func<IChainLinkRunContext, bool> condition);
        new IResultChainBuilder<T> If(Func<IChainLinkRunContext, Task<bool>> condition);
        new IResultChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, bool> condition);
        new IResultChainBuilder<T> If(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition);

        IResultChainBuilder<T> If(Func<T, bool> condition);
        IResultChainBuilder<T> If(Func<T, Task<bool>> condition);
        IResultChainBuilder<T> If(Func<T, IChainLinkRunContext, bool> condition);
        IResultChainBuilder<T> If(Func<T, IChainLinkRunContext, Task<bool>> condition);
        IResultChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, bool> condition);
        IResultChainBuilder<T> If(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition);
    }

    //public interface IRunResultChainBuilder<T, TChainLink>
    //    : IChainBuilder, IRunChainBuilder<TChainLink>, IResultChainBuilder<T, TChainLink>
    //    where TChainLink : IRunResultChainLink<T>
    //{
    //}

    //public interface IRunResultChainBuilder<TInput, TResult, TChainLink>
    //    : IChainBuilder, IRunChainBuilder<TInput, TChainLink>, IResultChainBuilder<TResult, TChainLink>
    //    where TChainLink : IRunResultChainLink<TInput, TResult>
    //{
    //}
}
