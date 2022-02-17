using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IChain
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }

    public interface IChain<T>
    {
        Task RunAsync(T input, CancellationToken cancellationToken = default);
    }
}
