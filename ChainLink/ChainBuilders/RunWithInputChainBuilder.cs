using System;
using System.Linq;

namespace ChainLink.ChainBuilders
{
    internal class RunWithInputChainBuilder<T, TChainLink> : RunWithInputResultChainBuilder<T, T, RunWithInputChainLinkPassInputWrapper<T, TChainLink>>
        where TChainLink : IRunChainLink<T>
    {
        public RunWithInputChainBuilder(object[] args, ChainBuilderBase previous = null)
            : base(RunChainLinkWrapperArgs.CreateArgsArray(args), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunWithInputChainLinkRunner<T>(ChainLinkDescription, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
