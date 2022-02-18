using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class RunResultChainBuilder<T, TChainLink> : RunResultChainBuilderBase<T, TChainLink>
        where TChainLink : IRunChainLink, IResultChainLink<T>
    {
        public RunResultChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, TChainLink>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }

    internal class RunResultChainBuilder<TInput, TResult, TChainLink> : RunResultChainBuilderBase<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public RunResultChainBuilder(object[] args, ChainBuilderBase previous = null) 
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<TInput, TResult, TChainLink>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
