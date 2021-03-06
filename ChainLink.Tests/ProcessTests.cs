using System;
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
                        .RunWithInput<TrimInputStringLink>();
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
                    .Run<TrimContextVariableStringLink>();
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

                helloWorldResult.RunWithInput<TrimInputStringLink>();
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
                    .RunWithInput<string?, TrimInputStringLink>()
                    .RunWithInput(input => trimmedString = input);

                helloWorldResult
                    .GetResult<string, HelloWorldLink>()
                    .RunWithInput(input => helloWorld = input);
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
                    .RunWithInput(input => trimmedString = input);
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
                    .Run((input) => input.Trim())
                    .RunWithInput(input => trimmedString = input);
            });

            await chain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task ResultsPassedAlong()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain chain = new Chain(configure =>
            {
                configure
                    .Run(() => " Hello World ")
                    .RunWithInput(input => Console.Write(input))
                    .RunWithInput(input => input.Trim())
                    .Run(() => Console.Write("Hello"))
                    .Run<SetContextVariableLink>("Bar")
                    .RunWithInput(input => trimmedString = input);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);

            trimmedString = null;

            IChain<string> inputChain = new Chain<string>(configure =>
            {
                configure
                    .Run(input => Console.Write(input))
                    .RunWithInput(input => input.Trim())
                    .Run(() => Console.Write("Hello"))
                    .RunWithInput(input => input + " ")
                    .Run(() => Console.Write("Hello"))
                    .RunWithInput<string?, TrimInputStringLink>()
                    .Run(() => Console.Write("Hello"))
                    .Run<SetContextVariableLink>("Bar")
                    .RunWithInput(input => trimmedString = input);
            });

            await chain.RunAsync();

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
                    .RunWithInput<string?, TrimInputStringLink>()
                    .RunWithInput(input => trimmedString = input)
                    .GetResult<string, HelloWorldLink>()
                    .Run<SetContextVariableLink>(" Test ");
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
                    .If((IChainLinkRunContext context) => true)
                    .RunWithInput<string?, TrimInputStringLink>()
                    .If((input) => input == "Hello World")
                    .RunWithInput(input => trimmedString = input);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);

            trimmedString = null;

            IChain<string> inputChain = new Chain<string>(configure =>
            {
                configure
                    .If(() => true)
                    .If((input) => input == " Hello World ")
                    .RunWithInput<string?, TrimInputStringLink>()
                    .If((input) => input == "Hello World")
                    .If(() => true)
                    .RunWithInput(input => trimmedString = input);
            });

            await inputChain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task ReassignDelegateAndIfBuilders()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;

            IChain chain = new Chain(configure =>
            {
                IResultChainBuilder<string> helloWorld = configure
                    .Run(() => " Hello World ")
                    .If((string input) => input != null);

                helloWorld = helloWorld.RunWithInput(i => i.Trim());

                helloWorld.RunWithInput(i => trimmedString = i);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);

            trimmedString = null;

            IChain<string> inputChain = new Chain<string>(configure =>
            {
                IInputResultChainBuilder<string, string> helloWorld = configure
                    .If((string input) => input != null);

                helloWorld = helloWorld.RunWithInput(i => i.Trim());

                helloWorld.RunWithInput(i => trimmedString = i);
            });

            await inputChain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
        }

        [TestMethod]
        public async Task Else()
        {
            const string Expected = "Hello World";

            string? trimmedString = null;
            string? otherString = Expected;

            IChain chain = new Chain(configure =>
            {
                IIfChainBuilder<string> ifHelloWorldNull = configure
                    .Run(() => " Hello World ")
                    .If((string input) => input == null);

                ifHelloWorldNull.RunWithInput(i => otherString = i);

                ifHelloWorldNull
                    .Else
                    .RunWithInput(i => i.Trim())
                    .RunWithInput(i => trimmedString = i);
            });

            await chain.RunAsync();

            Assert.AreEqual(Expected, trimmedString);
            Assert.IsNotNull(otherString);

            trimmedString = null;

            IChain<string> inputChain = new Chain<string>(configure =>
            {
                IInputIfChainBuilder<string, string> ifHelloWorldNull = configure
                    .If((string input) => input == null);

                ifHelloWorldNull.RunWithInput(i => otherString = i);

                ifHelloWorldNull
                    .Else
                    .RunWithInput(i => i.Trim())
                    .RunWithInput(i => trimmedString = i);
            });

            await inputChain.RunAsync(" Hello World ");

            Assert.AreEqual(Expected, trimmedString);
            Assert.IsNotNull(otherString);
        }
    }
}