using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace FunctionApp1;

public class Function
{
    [Function("Function_HttpStart")]
    public async Task<HttpResponseData> HttpStart(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
        [DurableClient] DurableTaskClient client,
        FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("Function_HttpStart");

        // Function input comes from the request content.
        //var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(nameof(RunOrchestrator));
        var instanceId = await client.ScheduleNewHelloCitiesTypedInstanceAsync();

        logger.LogInformation("Created new orchestration with instance ID = {instanceId}", instanceId);

        return client.CreateCheckStatusResponse(req, instanceId);
    }

    [Function(nameof(RunOrchestrator))]
    public async Task<List<string>> RunOrchestrator([OrchestrationTrigger] TaskOrchestrationContext context, string input)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(Function));
        logger.LogInformation("Saying hello.");
        var outputs = new List<string>();

        // Replace name and input with values relevant for your Durable Functions Activity
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Tokyo"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "Seattle"));
        outputs.Add(await context.CallActivityAsync<string>(nameof(SayHello), "London"));

        // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        return outputs;
    }

    [Function(nameof(SayHello))]
    public string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
    {
        ILogger logger = executionContext.GetLogger("SayHello");
        logger.LogInformation("Saying hello to {name}.", name);
        return $"Hello {name}!";
    }
}
