using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class RunResultChainBuilder<T, TChainLink> : RunResultChainBuilderBase<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        public RunResultChainBuilder(TChainLink chainLink, ChainBuilderBase previous = null)
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, TChainLink>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class RunResultChainBuilder<TInput, TResult, TChainLink> : RunResultChainBuilderBase<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public RunResultChainBuilder(TChainLink chainLink, ChainBuilderBase previous = null) 
            : base(chainLink, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
