namespace ChainLink.ChainBuilders
{
    public interface IChainLinkRunnerFactory
    {
        IChainLinkRunner CreateChainLinkRunner();
    }
}