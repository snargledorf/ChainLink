using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    internal class Chain : IChain
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
}