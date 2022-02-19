using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IChainLink
    {
        // NoOp
    }

    public interface IRunChainLink : IChainLink
    {
        Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken);
    }

    public interface IRunChainLink<T> : IChainLink
    {
        Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken);
    }

    public interface IResultChainLink<T> : IChainLink
    {
        T Result { get; }
    }
}