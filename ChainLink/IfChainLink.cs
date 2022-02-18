using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public sealed class IfChainLink : IRunChainLink
    {
        public IfChainLink(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            Condition = condition;
        }

        public Func<IChainLinkRunContext, CancellationToken, Task<bool>> Condition { get; }

        public Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public sealed class IfChainLink<T> : IRunChainLink<T>, IResultChainLink<T>
    {
        public IfChainLink(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition)
        {
            Condition = condition;
        }

        public Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> Condition { get; }

        public T Result { get; private set; }

        public Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            Result = input;
            return Task.CompletedTask;
        }
    }
}