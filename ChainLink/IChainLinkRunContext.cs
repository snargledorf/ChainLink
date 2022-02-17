namespace ChainLink
{
    public interface IChainLinkRunContext
    {
        object this[string variableName] { get; set; }

        T Get<T>(string variableName);
    }
}