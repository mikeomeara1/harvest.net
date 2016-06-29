using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/Task%20Assignment.md
        private const string TaskAssignmentsAction = "task_assignments";

        private IRestRequest ListTaskAssignmentsRequest(long projectId, DateTime? updatedSince = null)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}");

            if (updatedSince != null)
                request.AddParameter("updated_since", updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all task assignments on a project. Makes a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve assignemnts</param>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public IList<TaskAssignment> ListTaskAssignments(long projectId, DateTime? updatedSince = null)
        {
            return Execute<List<TaskAssignment>>(ListTaskAssignmentsRequest(projectId, updatedSince));
        }

        /// <summary>
        /// List all task assignments on a project. Makes a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve assignemnts</param>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public async Task<IList<TaskAssignment>> ListTaskAssignmentsAsync(long projectId, DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<TaskAssignment>>(ListTaskAssignmentsRequest(projectId, updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve a task assignment on a project. Makes a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve an assignemnt</param>
        /// <param name="taskAssignmentId">The Id of the assignment to retrieve</param>
        public TaskAssignment TaskAssignment(long projectId, long taskAssignmentId)
        {
            return Execute<TaskAssignment>(Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}"));
        }

        /// <summary>
        /// Retrieve a task assignment on a project. Makes a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve an assignemnt</param>
        /// <param name="taskAssignmentId">The Id of the assignment to retrieve</param>
        public async Task<TaskAssignment> TaskAssignmentAsync(long projectId, long taskAssignmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<TaskAssignment>(Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}"), cancellationToken);
        }

        private IRestRequest CreateTaskAssignmentRequest(long projectId, long taskId)
        {
            var request = Request("projects/" + projectId + "/task_assignments", RestSharp.Method.POST);

            request.AddBody(new TaskAssignmentCreateOptions()
            {
                Id = taskId
            });

            return request;
        }

        /// <summary>
        /// Assign a task to a project. Makes both a POST and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project to which to add the task</param>
        /// <param name="taskId">The Id of the task to add</param>
        public TaskAssignment CreateTaskAssignment(long projectId, long taskId)
        {
            return Execute<TaskAssignment>(CreateTaskAssignmentRequest(projectId, taskId));
        }

        /// <summary>
        /// Assign a task to a project. Makes both a POST and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project to which to add the task</param>
        /// <param name="taskId">The Id of the task to add</param>
        public async Task<TaskAssignment> CreateTaskAssignmentAsync(long projectId, long taskId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<TaskAssignment>(CreateTaskAssignmentRequest(projectId, taskId), cancellationToken);
        }

        private IRestRequest CreateNewTaskAndAssignRequest(long projectId, string name)
        {
            var request = Request("projects/" + projectId + "/task_assignments/add_with_create_new_task", RestSharp.Method.POST);

            request.AddBody(new TaskOptions()
            {
                Name = name
            });

            return request;
        }

        /// <summary>
        /// Create a new task and assign it to a project. Makes a POST request to the Projects/Task_Assignments/Add_With_Create_New_Task resource, and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of th project to which to add the new task</param>
        /// <param name="name">The name of the new task</param>
        public TaskAssignment CreateNewTaskAndAssign(long projectId, string name)
        {
            return Execute<TaskAssignment>(CreateNewTaskAndAssignRequest(projectId, name));
        }

        /// <summary>
        /// Create a new task and assign it to a project. Makes a POST request to the Projects/Task_Assignments/Add_With_Create_New_Task resource, and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of th project to which to add the new task</param>
        /// <param name="name">The name of the new task</param>
        public async Task<TaskAssignment> CreateNewTaskAndAssignAsync(long projectId, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<TaskAssignment>(CreateNewTaskAndAssignRequest(projectId, name), cancellationToken);
        }

        /// <summary>
        /// Remove a task from a project. Makes a DELETE request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project from which to remove the task</param>
        /// <param name="taskAssignmentId">The Id of the task assignment to remove</param>
        public bool DeleteTaskAssignment(long projectId, long taskAssignmentId)
        {
            var result = Execute(Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}/{taskAssignmentId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Remove a task from a project. Makes a DELETE request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project from which to remove the task</param>
        /// <param name="taskAssignmentId">The Id of the task assignment to remove</param>
        public async Task<bool> DeleteTaskAssignmentAsync(long projectId, long taskAssignmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}/{taskAssignmentId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update a task assignment on a project. Makes a PUT and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="taskAssignmentId">The ID of the task assignment to update</param>
        /// <param name="billable">Whether the task assignment is billable</param>
        /// <param name="deactivated">Whether the task assignment is inactive</param>
        /// <param name="hourlyRate">The hourly rate</param>
        /// <param name="budget">The budget</param>
        public TaskAssignment UpdateTaskAssignment(long projectId, long taskAssignmentId, bool? billable = null, bool? deactivated = null, decimal? hourlyRate = null, decimal? budget = null)
        {
            return UpdateTaskAssignment(projectId, taskAssignmentId, new TaskAssignmentOptions()
            {
                Billable = billable,
                Deactivated = deactivated,
                HourlyRate = hourlyRate,
                Budget = budget,
            });
        }

        /// <summary>
        /// Update a task assignment on a project. Makes a PUT and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="taskAssignmentId">The ID of the task assignment to update</param>
        /// <param name="billable">Whether the task assignment is billable</param>
        /// <param name="deactivated">Whether the task assignment is inactive</param>
        /// <param name="hourlyRate">The hourly rate</param>
        /// <param name="budget">The budget</param>
        public async Task<TaskAssignment> UpdateTaskAssignmentAsync(long projectId, long taskAssignmentId, bool? billable = null, bool? deactivated = null, decimal? hourlyRate = null, decimal? budget = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateTaskAssignmentAsync(projectId, taskAssignmentId, new TaskAssignmentOptions()
            {
                Billable = billable,
                Deactivated = deactivated,
                HourlyRate = hourlyRate,
                Budget = budget,
            }, cancellationToken);
        }

        private IRestRequest UpdateTaskAssignmentRequest(long projectId, long taskAssignmentId,
            TaskAssignmentOptions options)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{TaskAssignmentsAction}/{taskAssignmentId}", RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Update a task assignment on a project. Makes a PUT and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="taskAssignmentId">The ID of the task assignment to update</param>
        /// <param name="options">The options to be updated</param>
        public TaskAssignment UpdateTaskAssignment(long projectId, long taskAssignmentId, TaskAssignmentOptions options)
        {
            return Execute<TaskAssignment>(UpdateTaskAssignmentRequest(projectId, taskAssignmentId, options));
        }

        /// <summary>
        /// Update a task assignment on a project. Makes a PUT and a GET request to the Projects/Task_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="taskAssignmentId">The ID of the task assignment to update</param>
        /// <param name="options">The options to be updated</param>
        public async Task<TaskAssignment> UpdateTaskAssignmentAsync(long projectId, long taskAssignmentId, TaskAssignmentOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<TaskAssignment>(UpdateTaskAssignmentRequest(projectId, taskAssignmentId, options), cancellationToken);
        }
    }
}
