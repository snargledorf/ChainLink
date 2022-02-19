using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class IfChainBuilder : ChainBuilderBase, IChainLinkRunnerFactory, IIfChainBuilder
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        
        private ElseChainBuilder elseBuilder;

        public IfChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition, ChainBuilderBase previous = null)
            : base(previous)
        {
            this.condition = condition;
        }

        public IChainBuilder Else => elseBuilder ?? (elseBuilder = new ElseChainBuilder(this));

        public IChainLinkRunner CreateChainLinkRunner()
        {
            var elseChainLinkRunner = elseBuilder?.CreateChainLinkRunner() as IRunChainLinkRunner;
            return new IfChainLinkRunner(condition, elseChainLinkRunner, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class IfChainBuilder<T> : RunResultChainBuilderBase<T, T, DummyChainLink<T>>, IIfChainBuilder<T>
    {
        private readonly Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        
        private ElseChainBuilder<T> elseBuilder;

        public IfChainBuilder(Func<T, IChainLinkRunContext, CancellationToken, Task<bool>> condition, ChainBuilderBase previous = null)
            : base(Array.Empty<object>(), previous)
        {
            this.condition = condition;
        }

        public IResultChainBuilder<T> Else => elseBuilder ?? (elseBuilder = new ElseChainBuilder<T>(this));

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            var elseChainLinkRunner = elseBuilder?.CreateChainLinkRunner() as IRunWithInputChainLinkRunner<T>;
            return new IfChainLinkRunner<T>(condition, elseChainLinkRunner, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}