using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/Invoice%20Categories.md
        private const string InvoiceItemResource = "invoice_item_categories";


       
        /// <summary>
        /// List all invoice categories for the authenticated account. Makes a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        public List<InvoiceItemCategory> ListInvoiceCategories()
        {
            return Execute<List<InvoiceItemCategory>>(Request(InvoiceItemResource));
        }

        /// <summary>
        /// List all invoice categories for the authenticated account. Makes a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        public async Task<List<InvoiceItemCategory>> ListInvoiceCategoriesAsync()
        {
            return await ExecuteAsync<List<InvoiceItemCategory>>(Request(InvoiceItemResource));
        }

        /// <summary>
        /// Retrieve an invoice category on the authenticated account. Makes a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="InvoiceCategoryId">The Id of the invoice category to retrieve</param>
        /// <param name="invoiceCategoryId"></param>
        public InvoiceItemCategory InvoiceCategory(long invoiceCategoryId)
        {
            return Execute<InvoiceItemCategory>(Request($"{InvoiceItemResource}/{invoiceCategoryId}"));
        }

        /// <summary>
        /// Retrieve an invoice category on the authenticated account. Makes a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="InvoiceCategoryId">The Id of the invoice category to retrieve</param>
        /// <param name="invoiceCategoryId"></param>
        public async Task<InvoiceItemCategory> InvoiceCategoryAsync(long invoiceCategoryId)
        {
            return await ExecuteAsync<InvoiceItemCategory>(Request($"{InvoiceItemResource}/{invoiceCategoryId}"));
        }

        private InvoiceItemCategoryOptions CreateInvoiceItemCategoryOptions(string name, bool useAsExpense = false,
            bool useAsService = false)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new InvoiceItemCategoryOptions()
            {
                Name = name,
                UseAsExpense = useAsExpense,
                UseAsService = useAsService
            };
        }

        /// <summary>
        /// Creates a new invoice category under the authenticated account. Makes both a POST and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="name">The name of the invoice category</param>
        /// <param name="useAsExpense"></param>
        /// <param name="useAsService"></param>
        public InvoiceItemCategory CreateInvoiceCategory(string name, bool useAsExpense = false, bool useAsService = false)
        {
            return CreateInvoiceCategory(CreateInvoiceItemCategoryOptions(name, useAsExpense, useAsService));
        }

        /// <summary>
        /// Creates a new invoice category under the authenticated account. Makes both a POST and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="name">The name of the invoice category</param>
        /// <param name="useAsExpense"></param>
        /// <param name="useAsService"></param>
        public async Task<InvoiceItemCategory> CreateInvoiceCategoryAsync(string name, bool useAsExpense = false, bool useAsService = false)
        {
            return await CreateInvoiceCategoryAsync(CreateInvoiceItemCategoryOptions(name, useAsExpense, useAsService));
        }

        private IRestRequest CreateInvoiceCategoryRequest(InvoiceItemCategoryOptions options)
        {
            var request = Request(InvoiceItemResource, RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new invoice category under the authenticated account. Makes a POST and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="options">The options for the new invoice category to be created</param>
        public InvoiceItemCategory CreateInvoiceCategory(InvoiceItemCategoryOptions options)
        {
            return Execute<InvoiceItemCategory>(CreateInvoiceCategoryRequest(options));
        }

        /// <summary>
        /// Creates a new invoice category under the authenticated account. Makes a POST and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="options">The options for the new invoice category to be created</param>
        public async Task<InvoiceItemCategory> CreateInvoiceCategoryAsync(InvoiceItemCategoryOptions options)
        {
            return await ExecuteAsync<InvoiceItemCategory>(CreateInvoiceCategoryRequest(options));
        }

     
        /// <summary>
        /// Delete an invoice category from the authenticated account. Makes a DELETE request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID of the invoice category to delete</param>
        public bool DeleteInvoiceCategory(long invoiceCategoryId)
        {
            var result = Execute(Request($"{InvoiceItemResource}/{invoiceCategoryId}", RestSharp.Method.DELETE);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an invoice category from the authenticated account. Makes a DELETE request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID of the invoice category to delete</param>
        public async Task<bool> DeleteInvoiceCategoryAsync(long invoiceCategoryId)
        {
            var result = await ExecuteAsync(Request($"{InvoiceItemResource}/{invoiceCategoryId}", RestSharp.Method.DELETE);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update a invoice category on the authenticated account. Makes a PUT and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID of the invoice category to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="unitName">The updated unit name (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The updated unit price (Unit name and price must be set together)</param>
        public InvoiceItemCategory UpdateInvoiceCategory(long invoiceCategoryId, string name = null, bool? useAsExpense = null, bool? useAsService = null)
        {
            return UpdateInvoiceCategory(invoiceCategoryId, new InvoiceItemCategoryOptions()
            {
                Name = name,
                UseAsExpense = useAsExpense,
                UseAsService = useAsService
            });
        }

        /// <summary>
        /// Update a invoice category on the authenticated account. Makes a PUT and a GET request to the Invoice_Item_Categories resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID of the invoice category to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="unitName">The updated unit name (Unit name and price must be set together)</param>
        /// <param name="unitPrice">The updated unit price (Unit name and price must be set together)</param>
        public async Task<InvoiceItemCategory> UpdateInvoiceCategoryAsync(long invoiceCategoryId, string name = null, bool? useAsExpense = null, bool? useAsService = null)
        {
            return await UpdateInvoiceCategoryAsync(invoiceCategoryId, new InvoiceItemCategoryOptions()
            {
                Name = name,
                UseAsExpense = useAsExpense,
                UseAsService = useAsService
            });
        }

        private IRestRequest UpdateInvoiceCategoryRequest(long invoiceCategoryId, InvoiceItemCategoryOptions options)
        {
            var request = Request($"{InvoiceItemResource}/{invoiceCategoryId}", RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates an Invoice category on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID for the invoice category to update</param>
        /// <param name="options">The options to be updated</param>
        public InvoiceItemCategory UpdateInvoiceCategory(long invoiceCategoryId, InvoiceItemCategoryOptions options)
        {
            return Execute<InvoiceItemCategory>(UpdateInvoiceCategoryRequest(invoiceCategoryId, options));
        }

        /// <summary>
        /// Updates an Invoice category on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="invoiceCategoryId">The ID for the invoice category to update</param>
        /// <param name="options">The options to be updated</param>
        public async Task<InvoiceItemCategory> UpdateInvoiceCategoryAsync(long invoiceCategoryId, InvoiceItemCategoryOptions options)
        {
            return await ExecuteAsync<InvoiceItemCategory>(UpdateInvoiceCategoryRequest(invoiceCategoryId, options));
        }
    }
}
