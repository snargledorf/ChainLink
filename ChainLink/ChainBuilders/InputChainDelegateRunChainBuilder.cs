using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunChainBuilder<T> : InputChainChainBuilderBase<T, DelegateRunChainLink>
    {
        public InputDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputChainDelegateRunChainBuilder<T, TInput> : InputChainRunWithInputResultChainBuilderBase<T, TInput, TInput, DelegateRunChainLink<TInput>>
    {
        public InputChainDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputResultChainLinkRunner<TInput, TInput, DelegateRunChainLink<TInput>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
