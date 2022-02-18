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
            IChain chain = new Chain(configure => configure.GetResult<string, HelloWorldLink>());

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task ChainLinkArgs()
        {
            IChain chain = new Chain(configure => configure.GetResult<string, ReturnArgumentLink>("Hello World"));

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateMultiLinkChain()
        {
            IChain chain = new Chain(
                configure =>
                {
                    configure
                        .GetResult<string, ReturnArgumentLink>(" Hello World ")
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
                    .Run<SetContextVariableLink>(" Hello World ")
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
                    .GetResult<string, HelloWorldLink>()
                    .GetResult<string, HelloWorldLink>();
            });

            await chain.RunAsync();
        }

        [TestMethod]
        public async Task CreateChainWithMultipleBranches()
        {
            var chain = new Chain(configure =>
            {
                var helloWorldResult = configure.GetResult<string, HelloWorldLink>();

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
                var helloWorldResult = configure.Run(() => " Hello World ");

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
                    .RunWithInput<string?, TrimInputStringLink>()
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
                    .RunWithInput((input) => input.Trim())
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
                    .RunWithInput<string?, TrimInputStringLink>(new TrimInputStringLink())
                    .RunWithResult(input => trimmedString = input)
                    .GetResult(new HelloWorldLink())
                    .Run(new SetContextVariableLink(" Test "));
            });

            await chain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task Conditionals()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain chain = new Chain(configure =>
            {
                configure
                    .If(() => true)
                    .Run(() => " Hello World ")
                    .RunWithResult<string?, TrimInputStringLink>()
                    .If((input) => input == "Hello World")
                    .RunWithResult(input => trimmedString = input);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);

            trimmedString = null;

            IChain<string> inputChain = new Chain<string>(configure =>
            {
                configure
                    .If(() => true)
                    .If((input) => input == " Hello World ")
                    .RunWithResult<string?, TrimInputStringLink>()
                    .If((input) => input == "Hello World")
                    .If(() => true)
                    .RunWithResult(input => trimmedString = input);
            });

            await inputChain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }
    }
}