using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class DelegateRunChainBuilder : ChainBuilderBase, IChainLinkRunnerFactory, IRunChainBuilder<DelegateRunChainLink>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public DelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(new DelegateRunChainLink(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class DelegateRunChainBuilder<T> : RunResultChainBuilderBase<T, T, DelegateRunChainLink<T>>
    {
        public DelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(new DelegateRunChainLink<T>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, T, DelegateRunChainLink<T>>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
