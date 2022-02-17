namespace ChainLink.Tests
{
    internal class ReturnArgumentLink : IResultChainLink<string>
    {
        public ReturnArgumentLink(string arg)
        {
            Result = arg;
        }

        public string Result { get; }
    }
}