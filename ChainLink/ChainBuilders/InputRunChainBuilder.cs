using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputRunChainBuilder<T, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink
    {
        public InputRunChainBuilder(object[] chainLinkArgs, InputChainBuilderBase<T> previous = null) 
            : base(chainLinkArgs, previous)
        {
        }

        public InputRunChainBuilder(TChainLink chainLink, InputChainBuilderBase<T> previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputRunChainBuilder<T, TInput, TChainLink> : InputChainBuilderBase<T, TChainLink>, IInputRunChainBuilder<T, TInput, TChainLink>
        where TChainLink : IRunChainLink<TInput>
    {
        public InputRunChainBuilder(object[] chainLinkArgs, InputChainBuilderBase<T> previous = null) 
            : base(chainLinkArgs, previous)
        {
        }

        public InputRunChainBuilder(TChainLink chainLink, InputChainBuilderBase<T> previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<TInput>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
