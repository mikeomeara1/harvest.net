using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RestSharp;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/Tasks.md
        private const string TasksResource = "tasks";
        private const string ActiveAction = "activate";

        private IRestRequest ListTasksRequest(DateTime? updatedSince = null)
        {
            var request = Request(TasksResource);

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all tasks for the authenticated account. Makes a GET request to the Tasks resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the task updated-at property</param>
        public IList<Task> ListTasks(DateTime? updatedSince = null)
        {
            return Execute<List<Task>>(ListTasksRequest(updatedSince));
        }

        /// <summary>
        /// List all tasks for the authenticated account. Makes a GET request to the Tasks resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the task updated-at property</param>
        public async System.Threading.Tasks.Task<IList<Task>> ListTasksAsync(DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Task>>(ListTasksRequest(updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve a task on the authenticated account. Makes a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The Id of the task to retrieve</param>
        public Task Task(long taskId)
        {
            return Execute<Task>(Request($"{TasksResource}/{taskId}"));
        }

        /// <summary>
        /// Retrieve a task on the authenticated account. Makes a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The Id of the task to retrieve</param>
        public async System.Threading.Tasks.Task<Task> TaskAsync(long taskId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Task>(Request($"{TasksResource}/{taskId}"), cancellationToken);
        }

        private TaskOptions CreateTaskOptions(string name, bool billableByDefault = false, bool isDefault = false,
            decimal? defaultHourlyRate = null)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new TaskOptions()
            {
                Name = name,
                BillableByDefault = billableByDefault,
                IsDefault = isDefault,
                DefaultHourlyRate = defaultHourlyRate
            };
        }

        /// <summary>
        /// Creates a new task under the authenticated account. Makes both a POST and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="billableByDefault">Whether the task should be billable when added to a project</param>
        /// <param name="isDefault">Whether the task should be added to new projects</param>
        /// <param name="defaultHourlyRate">The default hourly rate</param>
        public Task CreateTask(string name, bool billableByDefault = false, bool isDefault = false, decimal? defaultHourlyRate = null)
        {
            return CreateTask(CreateTaskOptions(name, billableByDefault, isDefault, defaultHourlyRate));
        }

        /// <summary>
        /// Creates a new task under the authenticated account. Makes both a POST and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="billableByDefault">Whether the task should be billable when added to a project</param>
        /// <param name="isDefault">Whether the task should be added to new projects</param>
        /// <param name="defaultHourlyRate">The default hourly rate</param>
        public async System.Threading.Tasks.Task<Task> CreateTaskAsync(string name, bool billableByDefault = false, bool isDefault = false, decimal? defaultHourlyRate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateTaskAsync(CreateTaskOptions(name, billableByDefault, isDefault, defaultHourlyRate), cancellationToken);
        }

        private IRestRequest CreateTaskRequest(TaskOptions options)
        {
            var request = Request("tasks", RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new task under the authenticated account. Makes a POST and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="options">The options for the new task to be created</param>
        public Task CreateTask(TaskOptions options)
        {
            return Execute<Task>(CreateTaskRequest(options));
        }

        /// <summary>
        /// Creates a new task under the authenticated account. Makes a POST and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="options">The options for the new task to be created</param>
        public async System.Threading.Tasks.Task<Task> CreateTaskAsync(TaskOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Task>(CreateTaskRequest(options), cancellationToken);
        }

        /// <summary>
        /// Delete a task from the authenticated account. Makes a DELETE request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete</param>
        public bool DeleteTask(long taskId)
        {
            var result = Execute(Request($"{TasksResource}/{taskId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a task from the authenticated account. Makes a DELETE request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID of the task to delete</param>
        public async System.Threading.Tasks.Task<bool> DeleteTaskAsync(long taskId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{TasksResource}/{taskId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Activate a task on the authenticated account. Makes a POST request to the Tasks/Activate resource and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID of the task to activate</param>
        public Task ActivateTask(long taskId)
        {
            return Execute<Task>(Request($"{TasksResource}/{taskId}/{ActiveAction}", RestSharp.Method.POST));
        }

        /// <summary>
        /// Activate a task on the authenticated account. Makes a POST request to the Tasks/Activate resource and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID of the task to activate</param>
        public async System.Threading.Tasks.Task<Task> ActivateTaskAsync(long taskId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Task>(Request($"{TasksResource}/{taskId}/{ActiveAction}", RestSharp.Method.POST), cancellationToken);
        }

        /// <summary>
        /// Update a task on the authenticated account. Makes a PUT and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="billableByDefault">Whether the task should be billable when added to a project</param>
        /// <param name="isDefault">Whether the task should be added to new projects</param>
        /// <param name="defaultHourlyRate">The default hourly rate</param>
        public Task UpdateTask(long taskId, string name = null, bool? billableByDefault = null, bool? isDefault = null, decimal? defaultHourlyRate = null)
        {
            return UpdateTask(taskId, new TaskOptions()
            {
                Name = name,
                BillableByDefault = billableByDefault,
                IsDefault = isDefault,
                DefaultHourlyRate = defaultHourlyRate
            });
        }

        /// <summary>
        /// Update a task on the authenticated account. Makes a PUT and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="name">The name of the task</param>
        /// <param name="billableByDefault">Whether the task should be billable when added to a project</param>
        /// <param name="isDefault">Whether the task should be added to new projects</param>
        /// <param name="defaultHourlyRate">The default hourly rate</param>
        public async System.Threading.Tasks.Task<Task> UpdateTaskAsync(long taskId, string name = null, bool? billableByDefault = null, bool? isDefault = null, decimal? defaultHourlyRate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateTaskAsync(taskId, new TaskOptions()
            {
                Name = name,
                BillableByDefault = billableByDefault,
                IsDefault = isDefault,
                DefaultHourlyRate = defaultHourlyRate
            }, cancellationToken);
        }

        private IRestRequest UpdateTaskRequest(long taskId, TaskOptions options)
        {
            var request = Request("tasks/" + taskId, RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates a task on the authenticated account. Makes a PUT and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID for the task to update</param>
        /// <param name="options">The options to be updated</param>
        public Task UpdateTask(long taskId, TaskOptions options)
        {
            return Execute<Task>(UpdateTaskRequest(taskId, options));
        }

        /// <summary>
        /// Updates a task on the authenticated account. Makes a PUT and a GET request to the Tasks resource.
        /// </summary>
        /// <param name="taskId">The ID for the task to update</param>
        /// <param name="options">The options to be updated</param>
        public async System.Threading.Tasks.Task<Task> UpdateTaskAsync(long taskId, TaskOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Task>(UpdateTaskRequest(taskId, options), cancellationToken);
        }
    }
}
