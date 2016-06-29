using Harvest.Net;
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
        // All reporting API endpoints are only available to administrator users.
        // https://github.com/harvesthq/api/blob/master/Sections/Time%20and%20Expense%20Reporting.md
        private const string EntriesAction = "entries";
        private const string ProjectIdParameter = "project_id";
        private const string BillableParameter = "billable";
        private const string OnlyBilledParameter = "only_billed";
        private const string OnlyUnbilledParameter = "only_unbilled";
        private const string IsClosedParameter = "is_closed";
        private const string UserIdParameter = "user_id";

        private IRestRequest ListUserEntriesRequest(long userId, DateTime beginTime, DateTime endTime, long? projectId = null,
            bool? isBillable = null, bool? isBilled = null, bool? isClosed = null, DateTime? updatedSince = null)
        {
            var request = Request($"{PeopleResource}/{userId}/{EntriesAction}");

            request.AddParameter(FromParameter, beginTime.ToString("yyyyMMdd"));

            request.AddParameter(ToParameter, endTime.ToString("yyyyMMdd"));

            if (projectId != null)
                request.AddParameter(ProjectIdParameter, projectId.ToString());

            if (isBillable != null)
                request.AddParameter(BillableParameter, isBillable.Value.ToYesNo());

            if (isBilled != null)
            {
                if (isBilled == true)
                    request.AddParameter(OnlyBilledParameter, "yes");
                else
                    request.AddParameter(OnlyUnbilledParameter, "yes");
            }

            if (isClosed != null)
                request.AddParameter(IsClosedParameter, isClosed.Value.ToYesNo());

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all time entries logged by a user for a given timeframe with optional filters. Makes a GET request to the People/Entries resource.
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="projectId">Optional. Gets all project entries for the given timeframe for the user</param>
        /// <param name="isBillable">Optional. If the returned day entries are billable</param>
        /// <param name="isBilled">Optional. If the returned day entries are billed</param>
        /// <param name="isClosed">Optional. If the returned day entries are closed</param>
        public IList<DayEntry> ListUserEntries(long userId, DateTime beginTime, DateTime endTime, long? projectId = null,
            bool? isBillable = null, bool? isBilled = null, bool? isClosed = null, DateTime? updatedSince = null)
        {
            return
                Execute<List<DayEntry>>(ListUserEntriesRequest(userId, beginTime, endTime, projectId, isBillable,
                    isBilled, isClosed, updatedSince));
        }

        /// <summary>
        /// List all time entries logged by a user for a given timeframe with optional filters. Makes a GET request to the People/Entries resource.
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="projectId">Optional. Gets all project entries for the given timeframe for the user</param>
        /// <param name="isBillable">Optional. If the returned day entries are billable</param>
        /// <param name="isBilled">Optional. If the returned day entries are billed</param>
        /// <param name="isClosed">Optional. If the returned day entries are closed</param>
        /// <param name="updatedSince"></param>
        public async Task<IList<DayEntry>> ListUserEntriesAsync(long userId, DateTime beginTime, DateTime endTime,
            long? projectId = null, bool? isBillable = null, bool? isBilled = null, bool? isClosed = null,
            DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                await
                    ExecuteAsync<List<DayEntry>>(ListUserEntriesRequest(userId, beginTime, endTime, projectId,
                        isBillable, isBilled, isClosed, updatedSince), cancellationToken);
        }

        private IRestRequest ListProjectEntriesRequest(long projectId, DateTime beginTime, DateTime endTime,
            long? userId = null, bool? isBillable = null, bool? isBilled = null, bool? isClosed = null,
            DateTime? updatedSince = null)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{EntriesAction}");

            request.AddParameter(FromParameter, beginTime.ToString("yyyyMMdd"));

            request.AddParameter(ToParameter, endTime.ToString("yyyyMMdd"));

            if (userId != null)
                request.AddParameter(UserIdParameter, userId.ToString());

            if (isBillable != null)
                request.AddParameter(BillableParameter, isBillable.Value.ToYesNo());

            if (isBilled != null)
            {
                if (isBilled == true)
                    request.AddParameter(OnlyBilledParameter, "yes");
                else
                    request.AddParameter(OnlyUnbilledParameter, "yes");
            }

            if (isClosed != null)
                request.AddParameter(IsClosedParameter, isClosed.Value.ToYesNo());

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// Get all time entries logged to a project for a given timeframe with optional filters. Makes a GET request to the Projects/Entries resource.
        /// </summary>
        /// <param name="projectId">The projectId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="userId">The optional userId, can be used on your own entries only</param>
        /// <param name="isBillable">Optional. If the returned day entries are billable</param>
        /// <param name="isBilled">Optional. If the returned day entries are billed</param>
        /// <param name="isClosed">Optional. If the returned day entries are closed</param>
        public IList<DayEntry> ListProjectEntries(long projectId, DateTime beginTime, DateTime endTime,
            long? userId = null, bool? isBillable = null, bool? isBilled = null, bool? isClosed = null,
            DateTime? updatedSince = null)
        {
            return
                Execute<List<DayEntry>>(ListProjectEntriesRequest(projectId, beginTime, endTime, userId, isBillable,
                    isBilled, isClosed, updatedSince));
        }

        /// <summary>
        /// Get all time entries logged to a project for a given timeframe with optional filters. Makes a GET request to the Projects/Entries resource.
        /// </summary>
        /// <param name="projectId">The projectId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="userId">The optional userId, can be used on your own entries only</param>
        /// <param name="isBillable">Optional. If the returned day entries are billable</param>
        /// <param name="isBilled">Optional. If the returned day entries are billed</param>
        /// <param name="isClosed">Optional. If the returned day entries are closed</param>
        /// <param name="updatedSince"></param>
        /// <param name="cancellationToken"></param>
        public async Task<IList<DayEntry>> ListProjectEntriesAsync(long projectId, DateTime beginTime, DateTime endTime,
            long? userId = null, bool? isBillable = null, bool? isBilled = null, bool? isClosed = null,
            DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                await
                    ExecuteAsync<List<DayEntry>>(ListProjectEntriesRequest(projectId, beginTime, endTime, userId,
                        isBillable, isBilled, isClosed, updatedSince), cancellationToken);
        }

        private IRestRequest ListUserExpensesRequest(long userId, DateTime beginTime, DateTime endTime, bool? isClosed = null, DateTime? updatedSince = null)
        {
            var request = Request($"{PeopleResource}/{userId}/{ExpensesResource}");

            request.AddParameter(FromParameter, beginTime.ToString("yyyyMMdd"));

            request.AddParameter(ToParameter, endTime.ToString("yyyyMMdd"));

            if (isClosed != null)
                request.AddParameter(IsClosedParameter, isClosed.Value.ToYesNo());

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all expenses logged by a user for a given timeframe with optional filters. Makes a GET request to the People/Expenses resource.
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="isClosed">Optional. If the returned expenses are closed</param>
        /// <param name="updatedSince">Optional. Returns expenses update since</param>
        public IList<Expense> ListUserExpenses(long userId, DateTime beginTime, DateTime endTime, bool? isClosed = null, DateTime? updatedSince = null)
        {
            return Execute<List<Expense>>(ListUserExpensesRequest(userId, beginTime, endTime, isClosed, updatedSince));
        }

        /// <summary>
        /// List all expenses logged by a user for a given timeframe with optional filters. Makes a GET request to the People/Expenses resource.
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="isClosed">Optional. If the returned expenses are closed</param>
        /// <param name="updatedSince">Optional. Returns expenses update since</param>
        public async Task<IList<Expense>> ListUserExpensesAsync(long userId, DateTime beginTime, DateTime endTime,
            bool? isClosed = null, DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                await
                    ExecuteAsync<List<Expense>>(ListUserExpensesRequest(userId, beginTime, endTime, isClosed,
                        updatedSince), cancellationToken);
        }

        private IRestRequest ListProjectExpensesRequest(long projectId, DateTime beginTime, DateTime endTime,
            bool? isClosed = null, bool? isBilled = null, DateTime? updatedSince = null)
        {
            var request = Request($"{ProjectsResource}/{projectId}/{ExpensesResource}");

            request.AddParameter(FromParameter, beginTime.ToString("yyyyMMdd"));

            request.AddParameter(ToParameter, endTime.ToString("yyyyMMdd"));

            if (isClosed != null)
                request.AddParameter(IsClosedParameter, isClosed.Value.ToYesNo());

            if (isBilled != null)
            {
                if (isBilled == true)
                    request.AddParameter(OnlyBilledParameter, "yes");
                else
                    request.AddParameter(OnlyUnbilledParameter, "yes");
            }

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all expense entries logged to a project for a given timeframe with optional filters. Makes a GET request to the Projects/Expenses resource.
        /// </summary>
        /// <param name="projectId">The projectId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="isClosed">Optional. If the returned expenses are closed</param>
        /// <param name="isBilled">Optional. If the returned expenses are billed</param>
        /// <param name="updatedSince">Optional. Returns expenses update since</param>
        public IList<Expense> ListProjectExpenses(long projectId, DateTime beginTime, DateTime endTime,
            bool? isClosed = null, bool? isBilled = null, DateTime? updatedSince = null)
        {
            return
                Execute<List<Expense>>(ListProjectExpensesRequest(projectId, beginTime, endTime, isClosed, isBilled,
                    updatedSince));
        }

        /// <summary>
        /// List all expense entries logged to a project for a given timeframe with optional filters. Makes a GET request to the Projects/Expenses resource.
        /// </summary>
        /// <param name="projectId">The projectId</param>
        /// <param name="beginTime">The start of the timeframe</param>
        /// <param name="endTime">The end of the timeframe</param>
        /// <param name="isClosed">Optional. If the returned expenses are closed</param>
        /// <param name="isBilled">Optional. If the returned expenses are billed</param>
        /// <param name="updatedSince">Optional. Returns expenses update since</param>
        public async Task<IList<Expense>> ListProjectExpensesAsync(long projectId, DateTime beginTime, DateTime endTime,
            bool? isClosed = null, bool? isBilled = null, DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return
                await
                    ExecuteAsync<List<Expense>>(ListProjectExpensesRequest(projectId, beginTime, endTime, isClosed,
                        isBilled, updatedSince), cancellationToken);
        }
    }
}
