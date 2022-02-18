using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{

    internal class RunWithInputResultChainBuilder<TInput, TResult, TChainLink>
        : RunResultChainBuilderBase<TInput, TResult, TChainLink>
        where TChainLink : IRunChainLink<TInput>, IResultChainLink<TResult>
    {
        public RunWithInputResultChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(args, previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputResultChainLinkRunner<TInput, TResult, TChainLink>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
