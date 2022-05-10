using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Helpers;
using Elsa.Modules.Scheduling.Activities;
using Elsa.Modules.Scheduling.Services;
using Elsa.Persistence.Services;
using Microsoft.Extensions.Hosting;

namespace Elsa.Modules.Scheduling.HostedServices;

/// <summary>
/// Loads all timer-specific workflow bookmarks from the database and create scheduled jobs for them. 
/// </summary>
public class ScheduleWorkflows : BackgroundService
{
    private readonly IWorkflowBookmarkStore _bookmarkStore;
    private readonly IWorkflowBookmarkScheduler _workflowBookmarkScheduler;

    public ScheduleWorkflows(IWorkflowBookmarkStore bookmarkStore, IWorkflowBookmarkScheduler workflowBookmarkScheduler)
    {
        _bookmarkStore = bookmarkStore;
        _workflowBookmarkScheduler = workflowBookmarkScheduler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ScheduleBookmarksAsync(stoppingToken);
    }

    private async Task ScheduleBookmarksAsync(CancellationToken cancellationToken)
    {
        var workflowBookmarks = (await _bookmarkStore.FindManyAsync(ActivityTypeNameHelper.GenerateTypeName<Delay>(), cancellationToken: cancellationToken)).ToImmutableList();
        var groupedBookmarks = workflowBookmarks.GroupBy(x => x.WorkflowInstanceId);

        foreach (var bookmarksGroup in groupedBookmarks)
        {
            var workflowInstanceId = bookmarksGroup.Key;
            await _workflowBookmarkScheduler.ScheduleBookmarksAsync(workflowInstanceId, bookmarksGroup, cancellationToken);
        }
    }
}