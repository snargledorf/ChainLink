using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputRunResultChainBuilder<T, TInput, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<TInput>
    {
        public InputRunResultChainBuilder(TChainLink chainLink, InputChainBuilderBase<T> previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TChainLink>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputRunResultChainBuilder<T, TInput, TResult, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public InputRunResultChainBuilder(TChainLink chainLink, InputChainBuilderBase<T> previous = null)
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(
                ChainLink,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
