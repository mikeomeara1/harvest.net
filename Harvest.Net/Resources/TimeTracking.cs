using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using Timer = Harvest.Net.Models.Timer;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/Time%20Tracking.md
        private const string DailyResource = "daily";
        private const string ShowAction = "show";
        private const string TimerAction = "timer";
        private const string DeleteAction = "delete";
        private const string UpdateAction = "update";

        private IRestRequest DailyRequest(short? dayOfTheYear = null, short? year = null, long? ofUser = null)
        {
            if (year == null && dayOfTheYear != null)
                throw new ArgumentNullException(nameof(year), "You must provide both dayOfTheYear and year or neither.");
            if (year != null && dayOfTheYear == null)
                throw new ArgumentNullException(nameof(dayOfTheYear), "You must provide both dayOfYear and year or neither.");

            var request =
                Request(DailyResource + (dayOfTheYear != null ? "/" + dayOfTheYear + "/" + year : string.Empty));

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// List all time entries logged by the current user for a given day of the year. Makes a GET request to the Daily resource.
        /// </summary>
        /// <param name="dayOfTheYear">The day of the year to list (1-366). If null, lists today.</param>
        /// <param name="year">The year to list. If null, lists today.</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Daily Daily(short? dayOfTheYear = null, short? year = null, long? ofUser = null)
        {
            return Execute<Daily>(DailyRequest(dayOfTheYear, year, ofUser));
        }

        /// <summary>
        /// List all time entries logged by the current user for a given day of the year. Makes a GET request to the Daily resource.
        /// </summary>
        /// <param name="dayOfTheYear">The day of the year to list (1-366). If null, lists today.</param>
        /// <param name="year">The year to list. If null, lists today.</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Daily> DailyAsync(short? dayOfTheYear = null, short? year = null, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Daily>(DailyRequest(dayOfTheYear, year, ofUser), cancellationToken);
        }

        private IRestRequest DailyRequestShowRequest(long dayEntryId, long? ofUser = null)
        {
            var request = Request($"{DailyResource}/{ShowAction}/{dayEntryId}");

            if (ofUser != null)
                request.AddParameter("of_user", ofUser.Value);

            return request;
        }

        /// <summary>
        /// Retrieve a single day entry by ID. Makes a GET request to the Daily/Show resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to retrieve</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Timer Daily(long dayEntryId, long? ofUser = null)
        {
            return Execute<Timer>(DailyRequestShowRequest(dayEntryId, ofUser));
        }

        /// <summary>
        /// Retrieve a single day entry by ID. Makes a GET request to the Daily/Show resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to retrieve</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Timer> DailyAsync(long dayEntryId, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Timer>(DailyRequestShowRequest(dayEntryId, ofUser), cancellationToken);
        }

        private IRestRequest ToggleDailyRequest(long dayEntryId, long? ofUser = null)
        {
            var request = Request($"{DailyResource}/{TimerAction}/{dayEntryId}");

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// Toggles a single day entry timer by ID. Makes a GET request to the Daily/Timer resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to retrieve</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Timer ToggleDaily(long dayEntryId, long? ofUser = null)
        {
            return Execute<Timer>(ToggleDailyRequest(dayEntryId, ofUser));
        }

        /// <summary>
        /// Toggles a single day entry timer by ID. Makes a GET request to the Daily/Timer resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to retrieve</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Timer> ToggleDailyAsync(long dayEntryId, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Timer>(ToggleDailyRequest(dayEntryId, ofUser), cancellationToken);
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="hours">The hours on the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Timer CreateDaily(DateTime spentAt, long projectId, long taskId, decimal hours, string notes = null,
            long? ofUser = null)
        {
            return CreateDaily(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                Hours = hours.ToString("f2")
            }, ofUser);
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="hours">The hours on the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Timer> CreateDailyAsync(DateTime spentAt, long projectId, long taskId, decimal hours,
            string notes = null, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateDailyAsync(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                Hours = hours.ToString("f2")
            }, ofUser, cancellationToken);
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="startedAt">The start timestamp of the entry</param>
        /// <param name="endedAt">The end timestamp of the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Timer CreateDaily(DateTime spentAt, long projectId, long taskId, TimeSpan startedAt, TimeSpan endedAt,
            string notes = null, long? ofUser = null)
        {
            return CreateDaily(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                StartedAt = new DateTime(2000, 1, 1).Add(startedAt).ToString("h:mmtt").ToLower(),
                EndedAt = new DateTime(2000, 1, 1).Add(endedAt).ToString("h:mmtt").ToLower()
            }, ofUser);
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="startedAt">The start timestamp of the entry</param>
        /// <param name="endedAt">The end timestamp of the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Timer> CreateDailyAsync(DateTime spentAt, long projectId, long taskId, TimeSpan startedAt,
            TimeSpan endedAt, string notes = null, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateDailyAsync(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                StartedAt = new DateTime(2000, 1, 1).Add(startedAt).ToString("h:mmtt").ToLower(),
                EndedAt = new DateTime(2000, 1, 1).Add(endedAt).ToString("h:mmtt").ToLower()
            }, ofUser, cancellationToken);
        }

        private IRestRequest CreateDailyRequest(DailyOptions options, long? ofUser)
        {
            var request = Request(ofUser != null ? $"{DailyResource}/add?{OfUserParameter}={ofUser}" : "daily/add",
                RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="options">The options for the time entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        internal Timer CreateDaily(DailyOptions options, long? ofUser)
        {
            return Execute<Timer>(CreateDailyRequest(options, ofUser));
        }

        /// <summary>
        /// Create a new time entry for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="options">The options for the time entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        internal async Task<Timer> CreateDailyAsync(DailyOptions options, long? ofUser, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Timer>(CreateDailyRequest(options, ofUser), cancellationToken);
        }

        /// <summary>
        /// Starts a new timer for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public Timer StartTimer(DateTime spentAt, long projectId, long taskId, string notes = null, long? ofUser = null)
        {
            return CreateDaily(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                Hours = " "
            }, ofUser);
        }

        /// <summary>
        /// Starts a new timer for the logged in user. Makes a POST request to the Daily/Add resource.
        /// </summary>
        /// <param name="spentAt">The date of the entry</param>
        /// <param name="projectId">The project ID for the entry</param>
        /// <param name="taskId">The task ID for the entry</param>
        /// <param name="notes">The notes on the entry</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<Timer> StartTimerAsync(DateTime spentAt, long projectId, long taskId, string notes = null,
            long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateDailyAsync(new DailyOptions()
            {
                Notes = notes,
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                Hours = " "
            }, ofUser, cancellationToken);
        }

        private IRestRequest DeleteDailyRequest(long dayEntryId, long? ofUser = null)
        {
            var request = Request($"{DailyResource}/{DeleteAction}/{dayEntryId}", RestSharp.Method.DELETE);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// Delete a day entry for the logged in user. Makes a DELETE request tot he Daily/Delete resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to delete</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public bool DeleteDaily(long dayEntryId, long? ofUser = null)
        {
            var result = Execute(DeleteDailyRequest(dayEntryId, ofUser));
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a day entry for the logged in user. Makes a DELETE request tot he Daily/Delete resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the day entry to delete</param>
        /// <param name="ofUser">The user the day entry belongs to</param>
        public async Task<bool> DeleteDailyAsync(long dayEntryId, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(DeleteDailyRequest(dayEntryId, ofUser), cancellationToken);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private DailyOptions CreateDailyOptions(long dayEntryId, DateTime? spentAt = null, long? projectId = null,
            long? taskId = null, decimal? hours = null, TimeSpan? startedAt = null, TimeSpan? endedAt = null,
            string notes = null, long? ofUser = null)
        {
            var options = new DailyOptions()
            {
                SpentAt = spentAt,
                ProjectId = projectId,
                TaskId = taskId,
                Notes = notes
            };

            if (hours != null)
                options.Hours = hours.Value.ToString("f2");

            if (startedAt != null)
                options.StartedAt = new DateTime(2000, 1, 1).Add(startedAt.Value).ToString("h:mmtt").ToLower();

            if (endedAt != null)
                options.EndedAt = new DateTime(2000, 1, 1).Add(endedAt.Value).ToString("h:mmtt").ToLower();

            return options;
        }

        /// <summary>
        /// Update a time entry for the logged in user. Makes a POST request to the Daily/Update resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the time entry to update</param>
        /// <param name="spentAt">The new date of the entry</param>
        /// <param name="projectId">The new project ID of the entry</param>
        /// <param name="taskId">The new task ID of the entry</param>
        /// <param name="hours">The new hours for the entry</param>
        /// <param name="startedAt">The new start timestamp for the entry</param>
        /// <param name="endedAt">The new end timestamp for the entry</param>
        /// <param name="notes">The new notes for the entry</param>
        /// <param name="ofUser">The user the entry belongs to</param>
        public Timer UpdateDaily(long dayEntryId, DateTime? spentAt = null, long? projectId = null, long? taskId = null,
            decimal? hours = null, TimeSpan? startedAt = null, TimeSpan? endedAt = null, string notes = null,
            long? ofUser = null)
        {
            var options = CreateDailyOptions(dayEntryId, spentAt, projectId, taskId, hours, startedAt, endedAt, notes,
                ofUser);

            return UpdateDaily(dayEntryId, options, ofUser);
        }

        /// <summary>
        /// Update a time entry for the logged in user. Makes a POST request to the Daily/Update resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the time entry to update</param>
        /// <param name="spentAt">The new date of the entry</param>
        /// <param name="projectId">The new project ID of the entry</param>
        /// <param name="taskId">The new task ID of the entry</param>
        /// <param name="hours">The new hours for the entry</param>
        /// <param name="startedAt">The new start timestamp for the entry</param>
        /// <param name="endedAt">The new end timestamp for the entry</param>
        /// <param name="notes">The new notes for the entry</param>
        /// <param name="ofUser">The user the entry belongs to</param>
        public async Task<Timer> UpdateDailyAsync(long dayEntryId, DateTime? spentAt = null, long? projectId = null, long? taskId = null,
            decimal? hours = null, TimeSpan? startedAt = null, TimeSpan? endedAt = null, string notes = null,
            long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var options = CreateDailyOptions(dayEntryId, spentAt, projectId, taskId, hours, startedAt, endedAt, notes,
                ofUser);

            return await UpdateDailyAsync(dayEntryId, options, ofUser, cancellationToken);
        }

        private IRestRequest UpdateDailyRequest(long dayEntryId, DailyOptions options, long? ofUser)
        {
            var request = Request($"{DailyResource}/{UpdateAction}/{dayEntryId}", RestSharp.Method.POST);

            if (ofUser != null)
                request.AddParameter("of_user", ofUser.Value);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Update a time entry for the logged in user. Makes a POST request to the Daily/Update resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the entry to update</param>
        /// <param name="options">The options to update</param>
        /// <param name="ofUser">The user the entry belongs to</param>
        internal Timer UpdateDaily(long dayEntryId, DailyOptions options, long? ofUser)
        {
            return Execute<Timer>(UpdateDailyRequest(dayEntryId, options, ofUser));
        }

        /// <summary>
        /// Update a time entry for the logged in user. Makes a POST request to the Daily/Update resource.
        /// </summary>
        /// <param name="dayEntryId">The ID of the entry to update</param>
        /// <param name="options">The options to update</param>
        /// <param name="ofUser">The user the entry belongs to</param>
        internal async Task<Timer> UpdateDailyAsync(long dayEntryId, DailyOptions options, long? ofUser, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Timer>(UpdateDailyRequest(dayEntryId, options, ofUser), cancellationToken);
        }
    }
}
