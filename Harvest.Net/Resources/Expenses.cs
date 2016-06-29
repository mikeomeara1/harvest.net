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
        // https://github.com/harvesthq/api/blob/master/Sections/Expense%20Tracking.md
        private const string ExpensesResource = "expenses";
        private const string OfUserParameter = "of_user";

        private IRestRequest ListExpensesRequest(long? ofUser = null)
        {
            var request = Request(ExpensesResource);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// List all expenses for the authenticated account. Makes a GET request to the Expenses resource.
        /// </summary>
        public IList<Expense> ListExpenses(long? ofUser = null)
        {
            return Execute<List<Expense>>(ListExpensesRequest(ofUser));
        }

        /// <summary>
        /// List all expenses for the authenticated account. Makes a GET request to the Expenses resource.
        /// </summary>
        public async Task<IList<Expense>> ListExpensesAsync(long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Expense>>(ListExpensesRequest(ofUser), cancellationToken);
        }

        private IRestRequest ExpenseRequest(long expenseId, long? ofUser = null)
        {
            var request = Request($"{ExpensesResource}/{expenseId}");

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// Retrieve an expense on the authenticated account. Makes a GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The Id of the expense to retrieve</param>
        /// <param name="ofUser"></param>
        public Expense Expense(long expenseId, long? ofUser = null)
        {
            return Execute<Expense>(ExpenseRequest(expenseId, ofUser));
        }

        /// <summary>
        /// Retrieve an expense on the authenticated account. Makes a GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The Id of the expense to retrieve</param>
        /// <param name="ofUser"></param>
        public async Task<Expense> ExpenseAsync(long expenseId, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Expense>(ExpenseRequest(expenseId, ofUser), cancellationToken);
        }

        private ExpenseOptions CreateExpenseOptions(DateTime spentAt, long projectId, long expenseCategoryId,
            decimal? totalCost = null, decimal? units = null, string notes = null, bool isBillable = true)
        {
            if (totalCost != null && units != null)
                throw new ArgumentException("You may only set TotalCost OR Units. Not both.");

            if (totalCost == null && units == null)
                throw new ArgumentException("You must set either TotalCost OR Units.");

            return new ExpenseOptions()
            {
                SpentAt = spentAt,
                ProjectId = projectId,
                ExpenseCategoryId = expenseCategoryId,
                Notes = notes,
                Billable = isBillable,
                TotalCost = totalCost,
                Units = units
            };
        }


        /// <summary>
        /// Create a new expense for on the authenticated account. Makes both a POST and a GET request to the Expense resource.
        /// </summary>
        /// <param name="spentAt">The date of the expense</param>
        /// <param name="projectId">The project to bill</param>
        /// <param name="expenseCategoryId">The category of the expense</param>
        /// <param name="totalCost">The total expense price</param>
        /// <param name="notes">The notes on the expense</param>
        /// <param name="isBillable">Whether the expense is billable</param>
        /// <param name="ofUser"></param>
        /// DateTime spentAt, long projectId, long expenseCategoryId, decimal? totalCost = null, decimal? units = null, string notes = null, bool isBillable = true, long? ofUser = null
        public Expense CreateExpense(DateTime spentAt, long projectId, long expenseCategoryId,
            decimal? totalCost = null, decimal? units = null, string notes = null, bool isBillable = true,
            long? ofUser = null)
        {
            return CreateExpense(CreateExpenseOptions(spentAt, projectId, expenseCategoryId, totalCost, units, notes, isBillable), ofUser);
        }

        /// <summary>
        /// Create a new expense for on the authenticated account. Makes both a POST and a GET request to the Expense resource.
        /// </summary>
        /// <param name="spentAt">The date of the expense</param>
        /// <param name="projectId">The project to bill</param>
        /// <param name="expenseCategoryId">The category of the expense</param>
        /// <param name="totalCost">The total expense price</param>
        /// <param name="notes">The notes on the expense</param>
        /// <param name="isBillable">Whether the expense is billable</param>
        /// <param name="ofUser"></param>
        /// DateTime spentAt, long projectId, long expenseCategoryId, decimal? totalCost = null, decimal? units = null, string notes = null, bool isBillable = true, long? ofUser = null
        public async Task<Expense> CreateExpenseAsync(DateTime spentAt, long projectId, long expenseCategoryId,
            decimal? totalCost = null, decimal? units = null, string notes = null, bool isBillable = true,
            long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateExpenseAsync(CreateExpenseOptions(spentAt, projectId, expenseCategoryId, totalCost, units, notes, isBillable), ofUser, cancellationToken);
        }

        private IRestRequest CreateExpenseRequest(ExpenseOptions options, long? ofUser = null)
        {
            var request = Request(ExpensesResource, RestSharp.Method.POST);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new expense under the authenticated account. Makes a POST and a GET request to the Expenses resource.
        /// </summary>
        /// <param name="options">The options for the new expense to be created</param>
        public Expense CreateExpense(ExpenseOptions options, long? ofUser = null)
        {
            return Execute<Expense>(CreateExpenseRequest(options, ofUser));
        }

        /// <summary>
        /// Creates a new expense under the authenticated account. Makes a POST and a GET request to the Expenses resource.
        /// </summary>
        /// <param name="options">The options for the new expense to be created</param>
        /// <param name="cancellationToken"></param>
        public async Task<Expense> CreateExpenseAsync(ExpenseOptions options, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Expense>(CreateExpenseRequest(options, ofUser), cancellationToken);
        }

        private IRestRequest DeleteExpenseRequest(long expenseId, long? ofUser = null)
        {
            var request = Request($"{ExpensesResource}/{expenseId}", RestSharp.Method.DELETE);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            return request;
        }

        /// <summary>
        /// Delete an expense from the authenticated account. Makes a DELETE request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to delete</param>
        public bool DeleteExpense(long expenseId, long? ofUser = null)
        {
            var result = Execute(DeleteExpenseRequest(expenseId, ofUser));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an expense from the authenticated account. Makes a DELETE request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to delete</param>
        /// <param name="cancellationToken"></param>
        public async Task<bool> DeleteExpenseAsync(long expenseId, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(DeleteExpenseRequest(expenseId, ofUser), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update an existing expense on the authenticated account. Makes both a PUT and GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to update</param>
        /// <param name="spentAt">The new date of the expense</param>
        /// <param name="projectId">The new project ID of the expense</param>
        /// <param name="expenseCategoryId">The new expense category ID of the expense</param>
        /// <param name="totalCost">The new total cost of the expense</param>
        /// <param name="units">The new unit count of the expense</param>
        /// <param name="notes">The new notes of the expense</param>
        /// <param name="isBillable">The new billable status of the expense</param>
        /// <param name="ofUser"></param>
        public Expense UpdateExpense(long expenseId, DateTime? spentAt = null, long? projectId = null,
            long? expenseCategoryId = null, decimal? totalCost = null, decimal? units = null, string notes = null,
            bool? isBillable = null, long? ofUser = null)
        {
            return UpdateExpense(expenseId, new ExpenseOptions()
            {
                SpentAt = spentAt,
                ProjectId = projectId,
                ExpenseCategoryId = expenseCategoryId,
                Notes = notes,
                Billable = isBillable,
                TotalCost = totalCost,
                Units = units
            }, ofUser);
        }


        /// <summary>
        /// Update an existing expense on the authenticated account. Makes both a PUT and GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to update</param>
        /// <param name="spentAt">The new date of the expense</param>
        /// <param name="projectId">The new project ID of the expense</param>
        /// <param name="expenseCategoryId">The new expense category ID of the expense</param>
        /// <param name="totalCost">The new total cost of the expense</param>
        /// <param name="units">The new unit count of the expense</param>
        /// <param name="notes">The new notes of the expense</param>
        /// <param name="isBillable">The new billable status of the expense</param>
        /// <param name="ofUser"></param>
        public async Task<Expense> UpdateExpenseAsync(long expenseId, DateTime? spentAt = null, long? projectId = null,
            long? expenseCategoryId = null, decimal? totalCost = null, decimal? units = null, string notes = null,
            bool? isBillable = null, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateExpenseAsync(expenseId, new ExpenseOptions()
            {
                SpentAt = spentAt,
                ProjectId = projectId,
                ExpenseCategoryId = expenseCategoryId,
                Notes = notes,
                Billable = isBillable,
                TotalCost = totalCost,
                Units = units
            }, ofUser, cancellationToken);
        }

        private IRestRequest UpdateExpenseRequest(long expenseId, ExpenseOptions options, long? ofUser = null)
        {
            var request = Request($"{ExpensesResource}/{expenseId}", RestSharp.Method.PUT);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Update an existing expense on the authenticated account. Makes both a PUT and GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to update</param>
        /// <param name="options">The update options for the expense</param>
        public Expense UpdateExpense(long expenseId, ExpenseOptions options, long? ofUser = null)
        {
            return Execute<Expense>(UpdateExpenseRequest(expenseId, options, ofUser));
        }

        /// <summary>
        /// Update an existing expense on the authenticated account. Makes both a PUT and GET request to the Expenses resource.
        /// </summary>
        /// <param name="expenseId">The ID of the expense to update</param>
        /// <param name="options">The update options for the expense</param>
        /// <param name="cancellationToken"></param>
        public async Task<Expense> UpdateExpenseAsync(long expenseId, ExpenseOptions options, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Expense>(UpdateExpenseRequest(expenseId, options, ofUser), cancellationToken);
        }


        private static readonly Dictionary<string, string> AllowedReceiptFileTypes = new Dictionary<string, string>()
        {
            { "png", "image/png" },
            { "gif", "image/gif" },
            { "pdf", "application/pdf" },
            { "jpg", "image/jpeg" },
            { "jpeg", "image/jpeg" }
        };

        private IRestRequest AttachExpenseReceiptRequest(long expenseId, byte[] bytes, string fileName,
            long? ofUser = null)
        {
            var extension = fileName.Split('.').Last();
            if (!AllowedReceiptFileTypes.ContainsKey(extension))
                throw new ArgumentOutOfRangeException(nameof(fileName), "Receipt Allowed file types: " + string.Join(", ", AllowedReceiptFileTypes.Values.ToArray()));

            var request = Request($"{ExpensesResource}/{expenseId}/receipt", RestSharp.Method.POST);

            if (ofUser != null)
                request.AddParameter(OfUserParameter, ofUser.Value);

            request.AddFile("expense[receipt]", bytes, fileName, AllowedReceiptFileTypes[extension]);

            return request;
        }

        public Expense AttachExpenseReceipt(long expenseId, byte[] bytes, string fileName, long? ofUser = null)
        {
            Execute(AttachExpenseReceiptRequest(expenseId, bytes, fileName, ofUser));

            return Expense(expenseId, ofUser);
        }

        public async Task<Expense> AttachExpenseReceiptAsync(long expenseId, byte[] bytes, string fileName, long? ofUser = null, CancellationToken cancellationToken = default(CancellationToken))
        {
           await ExecuteAsync(AttachExpenseReceiptRequest(expenseId, bytes, fileName, ofUser), cancellationToken);

            return Expense(expenseId, ofUser);
        }
    }
}
