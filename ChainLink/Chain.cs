using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    internal sealed class Chain : IChain
    {
        private readonly IRunChainLinkRunner chainLinkRunner;

        public Chain(IChainLinkRunner chainLinkRunner)
        {
            this.chainLinkRunner = (IRunChainLinkRunner)chainLinkRunner;
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            var context = new ChainLinkRunContext();
            return chainLinkRunner.RunAsync(context, cancellationToken);
        }
    }
    internal sealed class Chain<T> : IChain<T>
    {
        private readonly IRunChainLinkRunner<T> chainLinkRunner;

        public Chain(IRunChainLinkRunner<T> chainLinkRunner)
        {
            this.chainLinkRunner = chainLinkRunner;
        }

        public Task RunAsync(T input, CancellationToken cancellationToken = default)
        {
            var context = new ChainLinkRunContext();
            return chainLinkRunner.RunAsync(input, context, cancellationToken);
        }
    }
}