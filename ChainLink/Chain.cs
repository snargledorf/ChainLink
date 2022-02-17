using System;
using System.Threading;
using System.Threading.Tasks;

using ChainLink.ChainBuilders;

namespace ChainLink
{
    public sealed class Chain : IChain
    {
        private readonly IRunChainLinkRunner chainLinkRunner;

        public Chain(Action<IStartChainBuilder> configure)
        {
            var builder = new StartChainBuilder();
            configure(builder);
            chainLinkRunner = builder.Build();
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            var context = new ChainLinkRunContext();
            return chainLinkRunner.RunAsync(context, cancellationToken);
        }
    }
    
    public sealed class Chain<T> : IChain<T>
    {
        private readonly IRunChainLinkRunner<T> chainLinkRunner;

        public Chain(Action<IStartChainBuilder<T>> configure)
        {
            var builder = new StartChainBuilder<T>();
            configure(builder);
            chainLinkRunner = builder.Build();
        }

        public Task RunAsync(T input, CancellationToken cancellationToken = default)
        {
            var context = new ChainLinkRunContext();
            return chainLinkRunner.RunAsync(input, context, cancellationToken);
        }
    }
}