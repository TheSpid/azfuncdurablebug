using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FunctionApp1;
[DurableTask(nameof(SayHelloTyped))]
public class SayHelloTyped : TaskActivity<string, string>
{
    readonly ILogger? logger;

    public SayHelloTyped(ILoggerFactory? loggerFactory)
    {
        this.logger = loggerFactory?.CreateLogger<SayHelloTyped>();
    }

    public override Task<string> RunAsync(TaskActivityContext context, string cityName)
    {
        this.logger?.LogInformation("Saying hello to {name}", cityName);
        return Task.FromResult($"Hello, {cityName}!");
    }
}
