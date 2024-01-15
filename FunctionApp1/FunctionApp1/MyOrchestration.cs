using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FunctionApp1;
[DurableTask(nameof(MyOrchestration))]
public class MyOrchestration : TaskOrchestrator<string, List<string>>
{
    public async override Task<List<string>> RunAsync(TaskOrchestrationContext context, string input)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(MyOrchestration));
        logger.LogInformation("Saying hello.");
        var outputs = new List<string>();

        // Replace name and input with values relevant for your Durable Functions Activity
        outputs.Add(await context.CallMyActivityAsync("Tokyo"));
        outputs.Add(await context.CallMyActivityAsync("Seattle"));
        outputs.Add(await context.CallMyActivityAsync("London"));

        // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        return outputs;
    }
}
