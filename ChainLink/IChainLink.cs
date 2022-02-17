using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IRunChainLink
    {
        Task RunAsync(IChainLinkRunContext context, CancellationToken cancellationToken);
    }

    public interface IRunChainLink<T>
    {
        Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken);
    }

    public interface IResultChainLink<T>
    {
        T Result { get; }
    }
}