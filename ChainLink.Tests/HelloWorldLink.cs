namespace ChainLink.Tests
{
    internal class HelloWorldLink : IResultChainLink<string>
    {
        public string Result { get; } = "Hello World";
    }
}