using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public interface IStartChainBuilder
    {
        IRunChainBuilder<TChainLink> StartWith<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink;
        IRunChainBuilder<TChainLink> StartWith<TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink;
        IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(params object[] args)
            where TChainLink : IResultChainLink<T>;
        IResultChainBuilder<T, TChainLink> StartWith<T, TChainLink>(TChainLink chainLink)
            where TChainLink : IResultChainLink<T>;
        IRunChainBuilder<DelegateRunChainLink> StartWith(Action del);

        IRunChainBuilder<DelegateRunChainLink> StartWith(Func<Task> del);

        IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, Task> del);

        IRunChainBuilder<DelegateRunChainLink> StartWith(Func<IChainLinkRunContext, CancellationToken, Task> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<T> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<Task<T>> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, T> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, Task<T>> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, T> del);

        IRunResultChainBuilder<T, DelegateRunResultChainLink<T>> StartWith<T>(Func<IChainLinkRunContext, CancellationToken, Task<T>> del);
    }
    public interface IStartChainBuilder<T> : IStartChainBuilder
    {
        IInputRunChainBuilder<T, T, TChainLink> StartWithInputInto<TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>;
        IInputRunChainBuilder<T, T, TChainLink> StartWithInputInto<TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink<T>;
        IInputRunResultChainBuilder<T, T, TResult, TChainLink> StartWithInputInto<TResult, TChainLink>(params object[] args)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;
        IInputRunResultChainBuilder<T, T, TResult, TChainLink> StartWithInputInto<TResult, TChainLink>(TChainLink chainLink)
            where TChainLink : IRunChainLink<T>, IResultChainLink<TResult>;
        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, TResult> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, Task<TResult>> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, TResult> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, Task<TResult>> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, CancellationToken, TResult> del);

        IInputRunResultChainBuilder<T, T, TResult, DelegateRunResultChainLink<T, TResult>> StartWithInputInto<TResult>(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del);
    }
}