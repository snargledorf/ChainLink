using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class DelegateRunWithInputChainBuilder<T> : RunResultChainBuilderBase<T, T, DelegateRunWithInputChainLink<T>>
    {
        public DelegateRunWithInputChainBuilder(Func<T, IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputResultChainLinkRunner<T, T, DelegateRunWithInputChainLink<T>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
