using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public sealed class DelegateRunChainLink : IRunChainLink
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainLink(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            this.del = del;
        }

        public Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            return del(context, cancellationToken);
        }
    }

    public sealed class DelegateRunChainLink<T> : IRunChainLink<T>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainLink(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            this.del = del;
        }

        public Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            return del(input, context, cancellationToken);
        }
    }

    public sealed class DelegateRunResultChainLink<T> : IRunChainLink, IResultChainLink<T>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<T>> del;

        public DelegateRunResultChainLink(Func<IChainLinkRunContext, CancellationToken, Task<T>> del)
        {
            this.del = del;
        }

        public T Result { get; private set; }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            Result = await del(context, cancellationToken);
        }
    }

    public sealed class DelegateRunResultChainLink<T, TResult> : IRunChainLink<T>, IResultChainLink<TResult>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public DelegateRunResultChainLink(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
        {
            this.del = del;
        }

        public TResult Result { get; private set; }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            Result = await del(input, context, cancellationToken);
        }
    }
}