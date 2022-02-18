using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    public sealed class RunChainLinkPassInputWrapper<T, TChainLink> : IRunChainLink<T>, IResultChainLink<T>
    {
        private readonly object[] args;

        public RunChainLinkPassInputWrapper(RunChainLinkWrapperArgs args)
        {
            this.args = args.Args;
        }

        public T Result { get; private set; }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            IRunChainLink chainLink = (IRunChainLink)ReflectionUtils.CreateObject(typeof(TChainLink), args);

            await chainLink.RunAsync(context, cancellationToken).ConfigureAwait(false);
            Result = input;
        }
    }
}