using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputDelegateRunChainBuilder<T> : InputChainBuilderBase<T>, IChainLinkRunnerFactory, IInputRunChainBuilder<T, DelegateRunChainLink>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task> del;

        public InputDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            this.del = del;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(new DelegateRunChainLink(del), Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputDelegateRunChainBuilder<T, TInput> : InputRunResultChainBuilderBase<T, TInput, TInput, DelegateRunChainLink<TInput>>
    {
        public InputDelegateRunChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task> del, InputChainBuilderBase<T> previous = null)
            : base(new DelegateRunChainLink<TInput>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TInput, DelegateRunChainLink<TInput>>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
