using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    internal class IfChainLinkRunner : IRunChainLinkRunner
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        private readonly IRunChainLinkRunner elseChainLinkRunner;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public IfChainLinkRunner(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition, IRunChainLinkRunner elseChainLinkRunner, IChainLinkRunner[] childLinkRunners)
        {
            this.condition = condition;
            this.elseChainLinkRunner = elseChainLinkRunner;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            if (!await condition(context, cancellationToken))
            {
                await elseChainLinkRunner?.RunAsync(context, cancellationToken);
                return;
            }

            foreach (var child in childLinkRunners)
                await ((IRunChainLinkRunner)child).RunAsync(context, cancellationToken);
        }
    }

    internal class IfChainLinkRunner<T> : IRunWithInputChainLinkRunner<T>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        private readonly IRunWithInputChainLinkRunner<T> elseChainLinkRunner;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public IfChainLinkRunner(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition, IRunWithInputChainLinkRunner<T> elseChainLinkRunner, IChainLinkRunner[] childLinkRunners)
        {
            this.condition = condition;
            this.elseChainLinkRunner = elseChainLinkRunner;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            if (!await condition(input, context, cancellationToken))
            {
                await elseChainLinkRunner?.RunAsync(input, context, cancellationToken);
                return;
            }

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
                        throw new InvalidOperationException($"Not a Chain Link Runner: {child.GetType()}");
                }
            }
        }
    }
}
