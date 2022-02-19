using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputIfChainBuilder<T> : InputChainBuilderBase<T>, IChainLinkRunnerFactory, IInputIfChainBuilder<T>
    {
        private readonly Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        private InputElseChainBuilder<T> elseBuilder;

        public InputIfChainBuilder(Func<IChainLinkRunContext, CancellationToken, Task<bool>> condition, InputChainBuilderBase<T> previous = null)
            : base(previous)
        {
            this.condition = condition;
        }

        public IInputChainBuilder<T> Else => elseBuilder ?? (elseBuilder = new InputElseChainBuilder<T>(this));

        public IChainLinkRunner CreateChainLinkRunner()
        {
            IRunChainLinkRunner elseChainLinkRunner = elseBuilder?.CreateChainLinkRunner() as IRunChainLinkRunner;
            return new IfChainLinkRunner(condition, elseChainLinkRunner, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputIfChainBuilder<T, TInputResult> : InputChainRunWithInputResultChainBuilderBase<T, TInputResult, TInputResult, DummyChainLink<TInputResult>>, IInputIfChainBuilder<T, TInputResult>
    {
        private readonly Func<TInputResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition;
        private InputElseWithInputChainBuilder<T, TInputResult> elseBuilder;

        public InputIfChainBuilder(Func<TInputResult, IChainLinkRunContext, CancellationToken, Task<bool>> condition, InputChainBuilderBase<T> previous = null)
            : base(Array.Empty<object>(), previous)
        {
            this.condition = condition;
        }

        public IInputResultChainBuilder<T, TInputResult> Else => elseBuilder ?? (elseBuilder = new InputElseWithInputChainBuilder<T, TInputResult>(this));

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            var elseChainLinkRunner = elseBuilder?.CreateChainLinkRunner() as IRunWithInputChainLinkRunner<TInputResult>;
            return new IfChainLinkRunner<TInputResult>(condition, elseChainLinkRunner, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}