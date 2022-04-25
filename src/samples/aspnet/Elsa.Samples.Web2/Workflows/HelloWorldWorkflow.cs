using Elsa.Modules.Activities.Console;
using Elsa.Services;

namespace Elsa.Samples.Web2.Workflows;

public class HelloWorldWorkflow : IWorkflow
{
    public void Build(IWorkflowDefinitionBuilder workflow)
    {
        workflow.WithRoot(new WriteLine("Hello World!"));
    }
}