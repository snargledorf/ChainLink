using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public sealed class DelegateWithInputRunChainLink<T> : IRunChainLink<T>, IResultChainLink<T>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateWithInputRunChainLink(Func<T, IChainLinkRunContext, CancellationToken, Task> del)
        {
            this.del = del;
        }

        public T Result { get; private set; }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            await del(input, context, cancellationToken);
            Result = input;
        }
    }

    public sealed class DelegateWithInputRunResultChainLink<T, TResult> : IRunChainLink<T>, IResultChainLink<TResult>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del;

        public DelegateWithInputRunResultChainLink(Func<T, IChainLinkRunContext, CancellationToken, Task<TResult>> del)
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