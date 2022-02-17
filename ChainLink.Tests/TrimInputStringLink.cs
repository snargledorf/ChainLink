using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.Tests
{
    internal class TrimInputStringLink : IRunChainLink<string>, IResultChainLink<string?>
    {
        public string? Result { get; private set; }

        public Task RunAsync(string input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            Result = input.Trim();
            return Task.CompletedTask;
        }
    }
}