using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class DelegateRunWithInputChainBuilder<T> : RunResultChainBuilderBase<T, T, DelegateWithInputRunChainLink<T>>
    {
        public DelegateRunWithInputChainBuilder(Func<T, IChainLinkRunContext, CancellationToken, Task> del, ChainBuilderBase previous = null)
            : base(new DelegateWithInputRunChainLink<T>(del), previous)
        {
        }

        public override IChainLinkRunner CreateChainLinkRunner()
        {
            return new RunResultChainLinkRunner<T, T, DelegateWithInputRunChainLink<T>>(ChainLink, Children.Select(c => c.CreateChainLinkRunner()).ToArray());
        }
    }
}
