using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public class InputIfChainBuilder<T> : InputChainBuilderBase<T>, IChainLinkRunnerFactory, IInputChainBuilder<T>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition;

        public InputIfChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            this.condition = condition;
        }

        public IChainLinkRunner CreateChainLinkRunner()
        {
            return new IfChainLinkRunner(condition, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    public class InputIfChainBuilder<T, TInputResult> : InputChainRunWithInputResultChainBuilderBase<T, TInputResult, TInputResult, IfChainLink<TInputResult>>
    {
        private readonly Func<TInputResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition;

        public InputIfChainBuilder(Func<TInputResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition, InputChainBuilderBase<T> previous = null)
            : base(Array.Empty<object>(), previous)
        {
            this.condition = condition;
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new IfChainLinkRunner<TInputResult>(condition, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}