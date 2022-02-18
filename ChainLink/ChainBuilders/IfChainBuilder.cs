using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class IfChainBuilder : ChainBuilderBase, IChainLinkRunnerFactory, IRunChainBuilder<IfChainLink>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition;

        public IfChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.condition = condition;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new IfChainLinkRunner(condition, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class IfChainBuilder<T> : RunResultChainBuilderBase<T, T, IfChainLink<T>>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition;

        public IfChainBuilder(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition, ChainBuilderBase previous = null)
            : base(Array.Empty<object>(), previous)
        {
            this.condition = condition;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new IfChainLinkRunner<T>(condition, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}