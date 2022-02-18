using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputRunChainBuilder<T, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunChainBuilder<T, TChainLink>
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

    internal class InputRunChainBuilder<T, TInput, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunChainBuilder<T, TInput, TChainLink>
        where TChainLink : IRunChainLink<TInput>
    {
        public InputRunChainBuilder(object[] args, InputChainBuilderBase<T> previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<TInput>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
