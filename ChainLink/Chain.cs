using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ChainLink.ChainBuilders;

namespace ChainLink
{
    public sealed class Chain : IChain
    {
        private readonly IEnumerable<IRunChainLinkRunner> chainLinkRunners;

        public Chain(Action<IChainBuilder> configure)
        {
            var builder = new ChainBuilder();
            configure(builder);
            chainLinkRunners = builder.Build();
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            foreach (var runner in chainLinkRunners)
            {
                var context = new ChainLinkRunContext();
                await runner.RunAsync(context, cancellationToken).ConfigureAwait(false);
            }
        }
    }
    
    public sealed class Chain<T> : IChain<T>
    {
        private readonly IEnumerable<IRunChainLinkRunner<T>> chainLinkRunners;

        public Chain(Action<IRootInputChainBuilder<T>> configure)
        {
            var builder = new InputChainBuilder<T>();
            configure(builder);
            chainLinkRunners = builder.Build();
        }

        public async Task RunAsync(T input, CancellationToken cancellationToken = default)
        {
            foreach (var runner in chainLinkRunners)
            {
                var context = new ChainLinkRunContext();
                await runner.RunAsync(input, context, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}