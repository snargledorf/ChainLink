using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class InputRunResultChainBuilder<T, TInput, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<TInput>
    {
        public InputRunResultChainBuilder(object[] args, InputChainBuilderBase<T> previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TChainLink>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class InputRunResultChainBuilder<T, TInput, TResult, TChainLink> : InputRunResultChainBuilderBase<T, TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public InputRunResultChainBuilder(object[] args, InputChainBuilderBase<T> previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(
                ChainLinkDescription,
                Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
