using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Elsa.Modules.Http.Models;
using Elsa.Modules.Http.Services;
using Microsoft.AspNetCore.Http;

namespace Elsa.Modules.Http.Handlers;

public class DefaultHttpEndpointWorkflowFaultHandler : IHttpEndpointWorkflowFaultHandler
{
    public virtual async ValueTask HandleAsync(HttpEndpointFaultedWorkflowContext context)
    {
        var httpContext = context.HttpContext;
        var workflowInstance = context.WorkflowInstance;

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var faultedResponse = JsonSerializer.Serialize(new
        {
            errorMessage = $"Workflow faulted at {workflowInstance.FaultedAt!} with error: {workflowInstance.Fault!.Message}",
            exception = workflowInstance.Fault?.Exception,
            workflow = new
            {
                name = workflowInstance.Name,
                version = workflowInstance.Version,
                instanceId = workflowInstance.Id
            }
        });

        await httpContext.Response.WriteAsync(faultedResponse, context.CancellationToken);
    }
}