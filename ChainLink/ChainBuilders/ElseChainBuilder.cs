using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class ElseChainBuilder : ChainBuilderBase, IChainLinkRunnerFactory
    {
        public ElseChainBuilder(ChainBuilderBase previous)
            : base(previous)
        {
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new ElseChainLinkRunner(Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunChainLinkRunner>().ToArray());
        }
    }

    internal class ElseChainBuilder<T> : RunResultChainBuilderBase<T, T, DummyChainLink<T>>
    {
        public ElseChainBuilder(ChainBuilderBase previous)
            : base(Array.Empty<object>(), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ElseWithInputChainLinkRunner<T>(Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}