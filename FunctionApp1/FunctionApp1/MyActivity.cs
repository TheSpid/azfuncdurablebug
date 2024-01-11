using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FunctionApp1;
[DurableTask(nameof(MyActivity))]
public class MyActivity : TaskActivity<string, string>
{
    private readonly ILogger logger;

    public MyActivity(ILogger<MyActivity> logger) // activites have access to DI.
    {
        this.logger = logger;
    }

    public async override Task<string> RunAsync(TaskActivityContext context, string name)
    {
        logger.LogInformation("Saying hello to {name}.", name);
        return $"Hello {name}!";
    }
}
