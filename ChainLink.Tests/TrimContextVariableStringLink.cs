using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.Tests
{
    internal class TrimContextVariableStringLink : IRunChainLink, IResultChainLink<string?>
    {
        public string? Result { get; private set; }

        public Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            Result = context.Get<string>(SetContextVariableLink.ContextVariableName).Trim();
            return Task.CompletedTask;
        }
    }
}