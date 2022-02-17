using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IChainLinkRunner
    {

    }

    public interface IRunChainLinkRunner : IChainLinkRunner
    {
        Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default);
    }

    public interface IRunChainLinkRunner<T> : IChainLinkRunner
    {
        Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken = default);
    }

    internal class RunChainLinkRunner : IRunChainLinkRunner
    {
        private readonly IRunChainLink chainLink;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunChainLinkRunner(IRunChainLink chainLink, IEnumerable<IChainLinkRunner> childLinkRunners)
        {
            this.chainLink = chainLink;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            await chainLink.RunAsync(context, cancellationToken);
            foreach (var child in childLinkRunners)
                await ((IRunChainLinkRunner)child).RunAsync(context, cancellationToken);
        }
    }

    internal class RunChainLinkRunner<T> : IRunChainLinkRunner<T>
    {
        private readonly IRunChainLink<T> chainLink;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunChainLinkRunner(IRunChainLink<T> chainLink, IEnumerable<IChainLinkRunner> childLinkRunners)
        {
            this.chainLink = chainLink;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            await chainLink.RunAsync(input, context, cancellationToken);
            foreach (var child in childLinkRunners)
                await ((IRunChainLinkRunner)child).RunAsync(context, cancellationToken);
        }
    }

    internal class ResultChainLinkRunner<T> : IRunChainLinkRunner
    {
        private readonly IResultChainLink<T> chainLink;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public ResultChainLinkRunner(IResultChainLink<T> chainLink, IEnumerable<IChainLinkRunner> childLinkRunners)
        {
            this.chainLink = chainLink;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                if (child is IRunChainLinkRunner<T> inputRunner)
                {
                    await inputRunner.RunAsync(result, context, cancellationToken);
                }
                else if (child is IRunChainLinkRunner runner)
                {
                    await runner.RunAsync(context, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException($"Not a Chain Link Runner: {child.GetType()}");
                }
            }
        }
    }

    internal class RunResultChainLinkRunner<TResult, TChainLink> : IRunChainLinkRunner
        where TChainLink : IRunChainLink, IResultChainLink<TResult>
    {
        private readonly TChainLink chainLink;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunResultChainLinkRunner(TChainLink chainLink, IEnumerable<IChainLinkRunner> childLinkRunners)
        {
            this.chainLink = chainLink;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            await chainLink.RunAsync(context, cancellationToken);
            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                if (child is IRunChainLinkRunner<TResult> inputRunner)
                {
                    await inputRunner.RunAsync(result, context, cancellationToken);
                }
                else if (child is IRunChainLinkRunner runner)
                {
                    await runner.RunAsync(context, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException($"Not a Chain Link Runner: {child.GetType()}");
                }
            }
        }
    }

    internal class RunResultChainLinkRunner<TInput, TResult, TChainLink> : IRunChainLinkRunner<TInput>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly TChainLink chainLink;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunResultChainLinkRunner(TChainLink chainLink, IEnumerable<IChainLinkRunner> childLinkRunners)
        {
            this.chainLink = chainLink;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(TInput input, IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            await chainLink.RunAsync(input, context, cancellationToken);
            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                if (child is IRunChainLinkRunner<TResult> inputRunner)
                {
                    await inputRunner.RunAsync(result, context, cancellationToken);
                }
                else if (child is IRunChainLinkRunner runner)
                {
                    await runner.RunAsync(context, cancellationToken);
                }
                else
                {
                    throw new InvalidOperationException($"Not a Chain Link Runner: {child.GetType()}");
                }
            }
        }
    }
}
