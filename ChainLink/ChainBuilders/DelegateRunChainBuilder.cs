using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class DelegateRunChainBuilder : ChainBuilderBase<DelegateRunChainLink>, IRunChainBuilder<DelegateRunChainLink>
    {
        public DelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateRunChainBuilder<T> : RunResultChainBuilderBase<T, T, DelegateRunChainLink<T>>
    {
        public DelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, T, DelegateRunChainLink<T>>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
