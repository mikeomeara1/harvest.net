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
        // https://github.com/harvesthq/api/blob/master/Sections/User%20Assignment.md
        private const string UserAssignmentsAction = "user_assignments";
                
        private IRestRequest ListUserAssignmentsRequest(long projectId, DateTime? updatedSince = null)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}");

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all user assignments on a project. Makes a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve assignemnts</param>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public IList<UserAssignment> ListUserAssignments(long projectId, DateTime? updatedSince = null)
        {
            return Execute<List<UserAssignment>>(ListUserAssignmentsRequest(projectId, updatedSince));
        }

        /// <summary>
        /// List all user assignments on a project. Makes a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve assignemnts</param>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public async Task<IList<UserAssignment>> ListUserAssignmentsAsync(long projectId, DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<UserAssignment>>(ListUserAssignmentsRequest(projectId, updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve an user assignment on a project. Makes a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve an assignemnt</param>
        /// <param name="userAssignmentId">The Id of the assignment to retrieve</param>
        public UserAssignment UserAssignment(long projectId, long userAssignmentId)
        {
            return Execute<UserAssignment>(Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}/" + userAssignmentId));
        }

        /// <summary>
        /// Retrieve an user assignment on a project. Makes a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project for which to retrieve an assignemnt</param>
        /// <param name="userAssignmentId">The Id of the assignment to retrieve</param>
        public async Task<UserAssignment> UserAssignmentAsync(long projectId, long userAssignmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<UserAssignment>(Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}/" + userAssignmentId), cancellationToken);
        }

        private IRestRequest CreateUserAssignmentRequest(long projectId, long userId)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}", RestSharp.Method.POST);

            request.AddBody(new UserAssignmentCreateOptions()
            {
                Id = userId
            });

            return request;
        }

        /// <summary>
        /// Assigns an user to a project. Makes both a POST and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project to which to add the user</param>
        /// <param name="userId">The Id of the user to add</param>
        public UserAssignment CreateUserAssignment(long projectId, long userId)
        {
            return Execute<UserAssignment>(CreateUserAssignmentRequest(projectId, userId));
        }

        /// <summary>
        /// Assigns an user to a project. Makes both a POST and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project to which to add the user</param>
        /// <param name="userId">The Id of the user to add</param>
        public async Task<UserAssignment> CreateUserAssignmentAsync(long projectId, long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<UserAssignment>(CreateUserAssignmentRequest(projectId, userId), cancellationToken);
        }

        /// <summary>
        /// Remove an user from a project. Makes a DELETE request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project from which to remove the user</param>
        /// <param name="userAssignmentId">The Id of the user assignment to remove</param>
        public bool DeleteUserAssignment(long projectId, long userAssignmentId)
        {
            var result = Execute(Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}/{userAssignmentId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Remove an user from a project. Makes a DELETE request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The Id of the project from which to remove the user</param>
        /// <param name="userAssignmentId">The Id of the user assignment to remove</param>
        public async Task<bool> DeleteUserAssignmentAsync(long projectId, long userAssignmentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}/{userAssignmentId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update an user assignment on a project. Makes a PUT and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="userAssignmentId">The ID of the user assignment to update</param>
        /// <param name="userId">The ID of the user assigned</param>
        /// <param name="deactivated">Whether the user assignment is inactive</param>
        /// <param name="hourlyRate">The hourly rate</param>
        /// <param name="budget">The budget</param>
        /// <param name="isProjectManager">Whether this user is a project manager</param>
        public UserAssignment UpdateUserAssignment(long projectId, long userAssignmentId, long userId, bool? deactivated = null, decimal? hourlyRate = null, decimal? budget = null, bool? isProjectManager = null)
        {
            return UpdateUserAssignment(projectId, userAssignmentId, new UserAssignmentOptions()
            {
                UserId = userId,
                ProjectId = projectId,
                Deactivated = deactivated,
                HourlyRate = hourlyRate,
                Budget = budget,
                IsProjectManager = isProjectManager
            });
        }

        /// <summary>
        /// Update an user assignment on a project. Makes a PUT and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="userAssignmentId">The ID of the user assignment to update</param>
        /// <param name="userId">The ID of the user assigned</param>
        /// <param name="deactivated">Whether the user assignment is inactive</param>
        /// <param name="hourlyRate">The hourly rate</param>
        /// <param name="budget">The budget</param>
        /// <param name="isProjectManager">Whether this user is a project manager</param>
        public async Task<UserAssignment> UpdateUserAssignmentAsync(long projectId, long userAssignmentId, long userId, bool? deactivated = null, decimal? hourlyRate = null, decimal? budget = null, bool? isProjectManager = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateUserAssignmentAsync(projectId, userAssignmentId, new UserAssignmentOptions()
            {
                UserId = userId,
                ProjectId = projectId,
                Deactivated = deactivated,
                HourlyRate = hourlyRate,
                Budget = budget,
                IsProjectManager = isProjectManager
            }, cancellationToken);
        }

        private IRestRequest UpdateUserAssignmentRequest(long projectId, long userAssignmentId, UserAssignmentOptions options)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{UserAssignmentsAction}/{userAssignmentId}",
                RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates an user assignment on a project. Makes a PUT and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="userAssignmentId">The ID of the user assignment to update</param>
        /// <param name="options">The options to be updated</param>
        public UserAssignment UpdateUserAssignment(long projectId, long userAssignmentId, UserAssignmentOptions options)
        {
           return Execute<UserAssignment>(UpdateUserAssignmentRequest(projectId, userAssignmentId, options));
        }

        /// <summary>
        /// Updates an user assignment on a project. Makes a PUT and a GET request to the Projects/User_Assignments resource.
        /// </summary>
        /// <param name="projectId">The ID of the project to update</param>
        /// <param name="userAssignmentId">The ID of the user assignment to update</param>
        /// <param name="options">The options to be updated</param>
        /// <param name="cancellationToken"></param>
        public async Task<UserAssignment> UpdateUserAssignmentAsync(long projectId, long userAssignmentId, UserAssignmentOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<UserAssignment>(UpdateUserAssignmentRequest(projectId, userAssignmentId, options), cancellationToken);
        }
    }
}
