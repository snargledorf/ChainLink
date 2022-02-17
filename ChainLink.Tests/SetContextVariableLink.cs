using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.Tests
{
    internal class SetContextVariableLink : IRunChainLink
    {
        public const string ContextVariableName = "MyContextVariable";

        private readonly string arg;

        public SetContextVariableLink(string arg)
        {
            this.arg = arg;
        }

        public Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            context[ContextVariableName] = arg;
            return Task.CompletedTask;
        }
    }
}