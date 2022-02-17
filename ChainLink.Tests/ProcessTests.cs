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
            IChain chain = new Chain(configure => configure.StartWith<string, HelloWorldLink>());

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task ChainLinkArgs()
        {
            IChain chain = new Chain(configure => configure.StartWith<string, ReturnArgumentLink>("Hello World"));

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateMultiLinkChain()
        {
            IChain chain = new Chain(
                configure =>
                {
                    configure
                        .StartWith<string, ReturnArgumentLink>(" Hello World ")
                        .RunWithResult<string?, TrimInputStringLink>();
                });

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithContextVariables()
        {
            IChain chain = new Chain(configure =>
            {
                configure
                    .StartWith<SetContextVariableLink>(" Hello World ")
                    .Run<string?, TrimContextVariableStringLink>();
            });

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWhereResultIsIgnored()
        {
            IChain chain = new Chain(configure =>
            {
                configure
                    .StartWith<string, HelloWorldLink>()
                    .GetResult<string, HelloWorldLink>();
            });

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithMultipleBranches()
        {
            var chain = new Chain(configure =>
            {
                var helloWorldResult = configure.StartWith<string, HelloWorldLink>();

                helloWorldResult.RunWithResult<TrimInputStringLink>();
                helloWorldResult.GetResult<string, HelloWorldLink>();
            });
            
            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithDelegates()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;
            string? helloWorld = null;

            IChain chain = new Chain(configure =>
            {
                var helloWorldResult = configure.StartWith(() => " Hello World ");

                helloWorldResult
                    .RunWithResult<string?, TrimInputStringLink>()
                    .RunWithResult(input => trimmedString = input);

                helloWorldResult
                    .GetResult<string, HelloWorldLink>()
                    .RunWithResult(input => helloWorld = input);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);
            Assert.AreEqual(Expected, helloWorld);
        }

        [TestMethod]
        public async Task RunChainWithArgs()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain<string> chain = new Chain<string>(configure =>
            {
                configure
                    .StartWithInputInto<string?, TrimInputStringLink>()
                    .RunWithResult(input => trimmedString = input);
            });

            await chain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task RunChainWithArgsIntoDelegate()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain<string> chain = new Chain<string>(configure =>
            {
                configure
                    .StartWithInputInto((input) => input.Trim())
                    .RunWithResult(input => trimmedString = input);
            });

            await chain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task InstantiateChainLinks()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain<string> chain = new Chain<string>(configure =>
            {
                configure
                    .StartWithInputInto<string?, TrimInputStringLink>(new TrimInputStringLink())
                    .RunWithResult(input => trimmedString = input);
            });

            await chain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }
    }
}