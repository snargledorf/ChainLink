using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunChainBuilder<T> : InputChainBuilderBase<T, DelegateRunChainLink>, IInputRunChainBuilder<T, DelegateRunChainLink>
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

    internal class InputDelegateRunChainBuilder<T, TInput> : InputRunResultChainBuilderBase<T, TInput, TInput, DelegateRunChainLink<TInput>>
    {
        public InputDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new[] { del }, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TInput, DelegateRunChainLink<TInput>>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
