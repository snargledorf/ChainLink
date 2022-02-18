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

    public interface IRunWithInputChainLinkRunner<T> : IChainLinkRunner
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

    internal class RunWithInputChainLinkRunner<T> : IRunWithInputChainLinkRunner<T>
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunWithInputChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
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

    internal class GetResultChainLinkRunner<T> : IRunChainLinkRunner
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public GetResultChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
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
                    case IRunWithInputChainLinkRunner<T> inputRunner:
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
                    case IRunWithInputChainLinkRunner<TResult> inputRunner:
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

    internal class RunWithInputResultChainLinkRunner<TInput, TResult, TChainLink> : IRunWithInputChainLinkRunner<TInput>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        private readonly ChainLinkDescription chainLinkDescription;
        private readonly IEnumerable<IChainLinkRunner> childLinkRunners;

        public RunWithInputResultChainLinkRunner(ChainLinkDescription chainLinkDescription, IChainLinkRunner[] childLinkRunners)
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
                    case IRunWithInputChainLinkRunner<TResult> inputRunner:
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
