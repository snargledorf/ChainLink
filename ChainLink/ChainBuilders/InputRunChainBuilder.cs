using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class InputRunChainBuilder<T, TChainLink> : InputChainBuilder<T>, IInputRunChainBuilder<T, TChainLink>
        where TChainLink : IRunChainLink
    {
        internal InputRunChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner(ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }

    internal class InputRunChainBuilder<T, TInput, TChainLink> : InputChainBuilder<T>, IInputRunChainBuilder<T, TInput, TChainLink>
        where TChainLink : IRunChainLink<TInput>
    {
        public InputRunChainBuilder(object[] chainLinkArgs, IInputChainBuilder<T> previous = null)
            : base(chainLinkArgs, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunChainLinkRunner<TInput>(ReflectionUtils.CreateObject<TChainLink>(ChainLinkArgs), Children.Select(c => c.CreateChainLinkRunner()));
        }
    }
}
