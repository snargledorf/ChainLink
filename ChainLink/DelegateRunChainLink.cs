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

    public sealed class DelegateRunChainLink<T> : IRunChainLink<T>, IResultChainLink<T>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainLink(Func<IChainLinkRunContext, CancellationToken, Task> del)
        {
            this.del = del;
        }

        public T Result { get; private set; }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            await del(context, cancellationToken);
            Result = input;
        }
    }
}