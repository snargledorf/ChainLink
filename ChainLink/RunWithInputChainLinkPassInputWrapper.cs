using System.Threading;
using System.Threading.Tasks;

namespace ChainLink.ChainBuilders
{
    internal class RunWithInputChainLinkPassInputWrapper<T, TChainLink> : IRunChainLink<T>, IResultChainLink<T>
        where TChainLink : IRunChainLink<T>
    {
        private readonly object[] args;

        public RunWithInputChainLinkPassInputWrapper(RunChainLinkWrapperArgs args)
        {
            this.args = args.Args;
        }

        public T Result { get; private set; }

        public async Task RunAsync(T input, IChainLinkRunContext context, CancellationToken cancellationToken)
        {
            IRunChainLink<T> chainLink = (IRunChainLink<T>)ReflectionUtils.CreateObject(typeof(TChainLink), args);

            await chainLink.RunAsync(input, context, cancellationToken).ConfigureAwait(false);
            Result = input;
        }
    }
}
