using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    internal class ElseChainLinkRunner : IRunChainLinkRunner
    {
        private readonly IEnumerable<IRunChainLinkRunner> childLinkRunners;

        public ElseChainLinkRunner(IRunChainLinkRunner[] childLinkRunners)
        {
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            foreach (var child in childLinkRunners)
                await child.RunAsync(context, cancellationToken);
        }
    }

    internal class ElseWithInputChainLinkRunner<T> : IRunWithInputChainLinkRunner<T>
    {
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public ElseWithInputChainLinkRunner(IChainLinkRunner[] childLinkRunners)
        {
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            foreach (var child in childLinkRunners)
            {
                switch (child)
                {
                    case IRunWithInputChainLinkRunner<T> inputRunner:
                        await inputRunner.RunAsync(input, context, cancellationToken);
                        break;
                    case IRunChainLinkRunner runner:
                        await runner.RunAsync(context, cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException($"Not a valid Chain Link Runner: {child.GetType()}");
                }
            }
        }
    }
}
