using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal abstract class DummyChainLink : IChainLink
    {
    }

    internal abstract class DummyChainLink<T> : IRunChainLink<T>, IResultChainLink<T>
    {
        public abstract T Result { get; }

        public abstract Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken);
    }
}