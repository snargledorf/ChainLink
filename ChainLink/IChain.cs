using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChainLink
{
    public interface IChain
    {
        Task RunAsync(CancellationToken cancelationToken = default);
    }
}
