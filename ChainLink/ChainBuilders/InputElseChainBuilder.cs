using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputElseChainBuilder<T> : InputChainBuilderBase<T>, IChainLinkRunnerFactory
    {
        public InputElseChainBuilder(InputChainBuilderBase<T> previous)
            : base(previous)
        {
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new ElseChainLinkRunner(Children.Select(c => c.CreateChainLinkRunner()).Cast<IRunChainLinkRunner>().ToArray());
        }
    }

    internal class InputElseWithInputChainBuilder<T, TInputResult> : InputChainRunWithInputResultChainBuilderBase<T, TInputResult, TInputResult, DummyChainLink<TInputResult>>
    {
        public InputElseWithInputChainBuilder(InputChainBuilderBase<T> previous)
            : base(Array.Empty<object>(), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new ElseWithInputChainLinkRunner<T>(Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}