using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
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
}