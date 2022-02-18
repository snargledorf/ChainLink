```
internal class TrimInputStringLink : IRunChainLink<string>, IResultChainLink<string?>
{
    public string? Result { get; private set; }

    public Task RunAsync(string input, IChainLinkRunContext context, CancellationToken cancellationToken)
    {
        Result = input.Trim();
        return Task.CompletedTask;
    }
}
```
```
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
```
```
const string Expected = "Hello World";

string? trimmedString = null;

IChain<string> chain = new Chain<string>(configure =>
{
    configure
        .RunWithInput((input) => input.Trim())
        .RunWithInput(input => trimmedString = input);
});

await chain.RunAsync(" Hello World ");

Assert.AreEqual(Expected, trimmedString);
```