using System.Threading.Tasks;

using ChainLink.ChainBuilders;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChainLink.Tests
{
    [TestClass]
    public class ProcessTests
    {
        [TestMethod]
        public async Task HelloWorldChain()
        {
            IChain chain = ChainBuilder
                .StartWith<string, HelloWorldLink>()
                .Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task ChainLinkArgs()
        {
            IChain chain = ChainBuilder
                .StartWith<string, ReturnArgumentLink>("Hello World")
                .Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateMultiLinkChain()
        {
            IChain chain = ChainBuilder
                .StartWith<string, ReturnArgumentLink>(" Hello World ")
                .RunWithResult<string?, TrimInputStringLink>()
                .Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithContextVariables()
        {
            IChain chain = ChainBuilder
                .StartWith<SetContextVariableLink>(" Hello World ")
                .Run<string?, TrimContextVariableStringLink>()
                .Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWhereResultIsIgnored()
        {
            IChain chain = ChainBuilder
                .StartWith<string, HelloWorldLink>()
                .GetResult<string, HelloWorldLink>()
                .Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithMultipleBranches()
        {
            var helloWorldResult = ChainBuilder
                .StartWith<string, HelloWorldLink>();

            helloWorldResult.RunWithResult<TrimInputStringLink>();
            helloWorldResult.GetResult<string, HelloWorldLink>();

            IChain chain = helloWorldResult.Build();

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithDelegates()
        {
            const string Expected = "Hello World";

            var helloWorldResult = ChainBuilder
                .StartWith(() => " Hello World ");

            string? trimmedString = null;
            string? helloWorld = null;

            helloWorldResult
                .RunWithResult<string?, TrimInputStringLink>()
                .RunWithResult(input => trimmedString = input);

            helloWorldResult
                .GetResult<string, HelloWorldLink>()
                .RunWithResult(input => helloWorld = input);

            IChain chain = helloWorldResult.Build();

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);
            Assert.AreEqual(Expected, helloWorld);
        }
    }
}