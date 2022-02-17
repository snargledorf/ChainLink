using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IChainProcessor
    {
        void AddChain(IChain chainDescription);
        void RemoveChain(IChain chainDescription);
        void RemoveChain(Guid chainId);

        Task RunAsync(CancellationToken cancellationToken = default);
    }
}