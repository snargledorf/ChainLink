using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
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
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
        {
            this.chainLinkDescription = chainLinkDescription;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            IRunChainLink chainLink = (IRunChainLink)ReflectionUtils.CreateObject(chainLinkDescription.Type, chainLinkDescription.Args);

            await chainLink.RunAsync(context, cancellationToken);
            foreach (var child in childLinkRunners)
                await ((IRunChainLinkRunner)child).RunAsync(context, cancellationToken);
        }
    }

    internal class RunChainLinkRunner<T> : IRunChainLinkRunner<T>
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
        {
            this.chainLinkDescription = chainLinkDescription;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            IRunChainLink<T> chainLink = (IRunChainLink<T>)ReflectionUtils.CreateObject(chainLinkDescription.Type, chainLinkDescription.Args);

            await chainLink.RunAsync(input, context, cancellationToken);
            foreach (var child in childLinkRunners)
            {
                switch (child)
                {
                    case IRunChainLinkRunner<T> inputRunner:
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

    internal class ResultChainLinkRunner<T> : IRunChainLinkRunner
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public ResultChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
        {
            this.chainLinkDescription = chainLinkDescription;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            IResultChainLink<T> chainLink = (IResultChainLink<T>)ReflectionUtils.CreateObject(chainLinkDescription.Type, chainLinkDescription.Args);

            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                switch (child)
                {
                    case IRunChainLinkRunner<T> inputRunner:
                        await inputRunner.RunAsync(result, context, cancellationToken);
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

    internal class RunResultChainLinkRunner<TResult, TChainLink> : IRunChainLinkRunner
        where TChainLink : IRunChainLink, IResultChainLink<TResult>
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunResultChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
        {
            this.chainLinkDescription = chainLinkDescription;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            TChainLink chainLink = (TChainLink)ReflectionUtils.CreateObject(chainLinkDescription.Type, chainLinkDescription.Args);

            await chainLink.RunAsync(context, cancellationToken);
            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                switch (child)
                {
                    case IRunChainLinkRunner<TResult> inputRunner:
                        await inputRunner.RunAsync(result, context, cancellationToken);
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

    internal class RunResultChainLinkRunner<TInput, TResult, TChainLink> : IRunChainLinkRunner<TInput>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunResultChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
        {
            this.chainLinkDescription = chainLinkDescription;
            this.childLinkRunners = childLinkRunners;
        }

        public async Task RunAsync(TInput input, IChainLinkRunContext context, CancellationToken cancellationToken = default)
        {
            TChainLink chainLink = (TChainLink)ReflectionUtils.CreateObject(chainLinkDescription.Type, chainLinkDescription.Args);

            await chainLink.RunAsync(input, context, cancellationToken);
            var result = chainLink.Result;
            foreach (var child in childLinkRunners)
            {
                switch (child)
                {
                    case IRunChainLinkRunner<TResult> inputRunner:
                        await inputRunner.RunAsync(result, context, cancellationToken);
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
