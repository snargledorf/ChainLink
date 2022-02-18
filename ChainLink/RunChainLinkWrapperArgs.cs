namespace ChainLink.ChainBuilders
{
    public sealed class RunChainLinkWrapperArgs
    {
        public static object[] CreateArgsArray(object[] args)
        {
            return new[] { new RunChainLinkWrapperArgs(args) };
        }

        public RunChainLinkWrapperArgs(object[] args)
        {
            Args = args;
        }

        public object[] Args { get; }
    }
}