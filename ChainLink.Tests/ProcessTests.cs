using System.Threading.Tasks;

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
                .Get<string, HelloWorldLink>()
                .Build();

            await chain.RunAsync();
        }
    }
}