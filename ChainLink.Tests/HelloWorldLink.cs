namespace ChainLink.Tests
{
    internal class HelloWorldLink : IResultChainLink<string>
    {
        public string Result => "Hello World";
    }
}