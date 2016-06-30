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
        // https://github.com/harvesthq/api/blob/master/Sections/Expense%20Categories.md
        private const string ExpenseCategoriesResource = "expense_categories";

        private IRestRequest ListExpenseCategoriesRequest(DateTime? updatedSince = null)
        {
            var request = Request(ExpenseCategoriesResource);

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all expense categories for the authenticated account. Makes a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public List<ExpenseCategory> ListExpenseCategories(DateTime? updatedSince = null)
        {
            return Execute<List<ExpenseCategory>>(ListExpenseCategoriesRequest(updatedSince));
        }

        /// <summary>
        /// List all expense categories for the authenticated account. Makes a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the updated-at property</param>
        public async Task<List<ExpenseCategory>> ListExpenseCategoriesAsync(DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<ExpenseCategory>>(ListExpenseCategoriesRequest(updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve an expense category on the authenticated account. Makes a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="expenseCategoryId">The Id of the expense category to retrieve</param>
        public ExpenseCategory ExpenseCategory(long expenseCategoryId)
        {
            return Execute<ExpenseCategory>(Request($"{ExpenseCategoriesResource}/{expenseCategoryId}"));
        }

        /// <summary>
        /// Retrieve an expense category on the authenticated account. Makes a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="expenseCategoryId">The Id of the expense category to retrieve</param>
        /// <param name="cancellationToken"></param>
        public async Task<ExpenseCategory> ExpenseCategoryAsync(long expenseCategoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<ExpenseCategory>(Request($"{ExpenseCategoriesResource}/{expenseCategoryId}"), cancellationToken);
        }

        private ExpenseCategoryOptions ExpenseCategoryOptions(string name, string unitName = null, decimal? unitPrice = null)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new ExpenseCategoryOptions()
            {
                Name = name,
                UnitName = unitName,
                UnitPrice = unitPrice
            };
        }

        /// <summary>
        /// Creates a new expense category under the authenticated account. Makes both a POST and a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="name">The name of the expense category</param>
        /// <param name="unitName">The unit name of the expense category (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The unit price of the expense category (Unit name and price must be set together)</param>
        public ExpenseCategory CreateExpenseCategory(string name, string unitName = null, decimal? unitPrice = null)
        {
            return CreateExpenseCategory(ExpenseCategoryOptions(name, unitName, unitPrice));
        }

        /// <summary>
        /// Creates a new expense category under the authenticated account. Makes both a POST and a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="name">The name of the expense category</param>
        /// <param name="unitName">The unit name of the expense category (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The unit price of the expense category (Unit name and price must be set together)</param>
        public async Task<ExpenseCategory> CreateExpenseCategoryAsync(string name, string unitName = null, decimal? unitPrice = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateExpenseCategoryAsync(ExpenseCategoryOptions(name, unitName, unitPrice), cancellationToken);
        }

        private IRestRequest CreateExpenseCategoryRequest(ExpenseCategoryOptions options)
        {
            var request = Request(ExpenseCategoriesResource, RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new expense category under the authenticated account. Makes a POST and a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="options">The options for the new expense category to be created</param>
        public ExpenseCategory CreateExpenseCategory(ExpenseCategoryOptions options)
        {
          return Execute<ExpenseCategory>(CreateExpenseCategoryRequest(options));
        }

        /// <summary>
        /// Creates a new expense category under the authenticated account. Makes a POST and a GET request to the Expense_Categories resource.
        /// </summary>
        /// <param name="options">The options for the new expense category to be created</param>
        /// <param name="cancellationToken"></param>
        public async Task<ExpenseCategory> CreateExpenseCategoryAsync(ExpenseCategoryOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<ExpenseCategory>(CreateExpenseCategoryRequest(options), cancellationToken);
        }
      
        /// <summary>
        /// Delete an expense category from the authenticated account. Makes a DELETE request to the Expense_Categories resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID of the expense category to delete</param>
        public bool DeleteExpenseCategory(long expenseCategoryId)
        {
            var result = Execute(Request($"{ExpenseCategoriesResource}/{expenseCategoryId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an expense category from the authenticated account. Makes a DELETE request to the Expense_Categories resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID of the expense category to delete</param>
        /// <param name="cancellationToken"></param>
        public async Task<bool> DeleteExpenseCategoryAsync(long expenseCategoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{ExpenseCategoriesResource}/{expenseCategoryId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update a expense category on the authenticated account. Makes a PUT and a GET request to the Expense_Category resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID of the expense category to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="unitName">The updated unit name (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The updated unit price (Unit name and price must be set together)</param>
        public ExpenseCategory UpdateExpenseCategory(long expenseCategoryId, string name = null, string unitName = null,
            decimal? unitPrice = null)
        {
            return UpdateExpenseCategory(expenseCategoryId,
                new ExpenseCategoryOptions() { Name = name, UnitName = unitName, UnitPrice = unitPrice });
        }

        /// <summary>
        /// Update a expense category on the authenticated account. Makes a PUT and a GET request to the Expense_Category resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID of the expense category to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="unitName">The updated unit name (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The updated unit price (Unit name and price must be set together)</param>
        /// <param name="cancellationToken"></param>
        public async Task<ExpenseCategory> UpdateExpenseCategoryAsync(long expenseCategoryId, string name = null, string unitName = null,
            decimal? unitPrice = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateExpenseCategoryAsync(expenseCategoryId,
                new ExpenseCategoryOptions() { Name = name, UnitName = unitName, UnitPrice = unitPrice }, cancellationToken);
        }

        private IRestRequest UpdateExpenseCategoryRequest(long expenseCategoryId, ExpenseCategoryOptions options)
        {
            var request = Request($"{ExpenseCategoriesResource}/{expenseCategoryId}", RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates an expense category on the authenticated account. Makes a PUT and a GET request to the Expense_Category resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID for the expense category to update</param>
        /// <param name="options">The options to be updated</param>
        public ExpenseCategory UpdateExpenseCategory(long expenseCategoryId, ExpenseCategoryOptions options)
        {
            return Execute<ExpenseCategory>(UpdateExpenseCategoryRequest(expenseCategoryId, options));
        }

        /// <summary>
        /// Updates an expense category on the authenticated account. Makes a PUT and a GET request to the Expense_Category resource.
        /// </summary>
        /// <param name="expenseCategoryId">The ID for the expense category to update</param>
        /// <param name="options">The options to be updated</param>
        /// <param name="cancellationToken"></param>
        public async Task<ExpenseCategory> UpdateExpenseCategoryAsync(long expenseCategoryId, ExpenseCategoryOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<ExpenseCategory>(UpdateExpenseCategoryRequest(expenseCategoryId, options), cancellationToken);
        }
    }
}
