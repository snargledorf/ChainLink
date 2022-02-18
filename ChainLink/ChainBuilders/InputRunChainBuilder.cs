using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputRunChainBuilder<T, TChainLink> : InputChainChainBuilderBase<T, TChainLink>
        where TChainLink : IRunChainLink
    {
        public InputRunChainBuilder(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputRunChainBuilder<T, TInput, TChainLink> 
        : InputChainRunWithInputResultChainBuilderBase<T, TInput, TInput, RunWithInputChainLinkPassInputWrapper<TInput, TChainLink>>
        where TChainLink : IRunChainLink<TInput>
    {
        public InputRunChainBuilder(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputChainLinkRunner<TInput>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
