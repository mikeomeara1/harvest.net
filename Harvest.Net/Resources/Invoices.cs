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
        // https://github.com/harvesthq/api/blob/master/Sections/Invoices.md
        private const string PageParameter = "page";
        private const string FromParameter = "from";
        private const string ToParameter = "to";
        private const string StatusParameter = "status";
        private const string ClientParameter = "client";

        private IRestRequest ListInvoicesRequest(int page = 1, DateTime? from = null, DateTime? to = null,
            DateTime? updatedSince = null, InvoiceState? status = null, long? clientId = null)
        {
            var request = Request(InvoicesResource);

            if (page > 1)
                request.AddParameter(PageParameter, page);

            if (from != null)
                request.AddParameter(FromParameter, from.Value.ToString("yyyyMMdd"));
            if (to != null)
                request.AddParameter(ToParameter, to.Value.ToString("yyyyMMdd"));

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            if (status != null)
                request.AddParameter(StatusParameter, status.Value.ToString().ToLower());

            if (clientId != null)
                request.AddParameter(ClientParameter, clientId.Value);

            return request;
        }

        /// <summary>
        /// Retrieve a list of invoices from the authenticated account. Makes a GET request to the Invoices resource.
        /// </summary>
        /// <param name="page">The page to retrieve (1-based)</param>
        /// <param name="from">The earliest date of invoices to retrieve</param>
        /// <param name="to">The latest date of invoices to retrieve</param>
        /// <param name="updatedSince">The earliest update date of invoices to retrieve</param>
        /// <param name="status">The status of invoices to retrieve</param>
        /// <param name="clientId">The client ID of invoices to retrieve</param>
        public IList<Invoice> ListInvoices(int page = 1, DateTime? from = null, DateTime? to = null, DateTime? updatedSince = null, InvoiceState? status = null, long? clientId = null)
        {
            return Execute<List<Invoice>>(ListInvoicesRequest(page, from, to, updatedSince, status, clientId));
        }

        /// <summary>
        /// Retrieve a list of invoices from the authenticated account. Makes a GET request to the Invoices resource.
        /// </summary>
        /// <param name="page">The page to retrieve (1-based)</param>
        /// <param name="from">The earliest date of invoices to retrieve</param>
        /// <param name="to">The latest date of invoices to retrieve</param>
        /// <param name="updatedSince">The earliest update date of invoices to retrieve</param>
        /// <param name="status">The status of invoices to retrieve</param>
        /// <param name="clientId">The client ID of invoices to retrieve</param>
        public async Task<IList<Invoice>> ListInvoicesAsync(int page = 1, DateTime? from = null, DateTime? to = null, DateTime? updatedSince = null, InvoiceState? status = null, long? clientId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Invoice>>(ListInvoicesRequest(page, from, to, updatedSince, status, clientId), cancellationToken);
        }

        /// <summary>
        /// Retrieve an invoice from the authenticated account. Makes a GET request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to retrieve</param>
        public Invoice Invoice(long invoiceId)
        {
            return Execute<Invoice>(Request($"{InvoicesResource}/{invoiceId}"));
        }

        /// <summary>
        /// Retrieve an invoice from the authenticated account. Makes a GET request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to retrieve</param>
        public async Task<Invoice> InvoiceAsync(long invoiceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Invoice>(Request($"{InvoicesResource}/{invoiceId}"), cancellationToken);
        }

        private InvoiceOptions CreateInvoiceOptions(InvoiceKind kind, long clientId, DateTime issuedAt,
            DateTime? dueAt = null, Currency? currency = null, string subject = null, string notes = null,
            string number = null, long[] projectIds = null, List<InvoiceItem> lineItems = null)
        {
            var invoice = new InvoiceOptions()
            {
                Kind = kind,
                ClientId = clientId,
                IssuedAt = issuedAt,
                DueAt = dueAt,
                Currency = currency,
                Subject = subject,
                Notes = notes,
                Number = number,
            };

            if (projectIds != null)
                invoice.ProjectsToInvoice = string.Join(",", projectIds.Select(id => id.ToString()));

            invoice.SetInvoiceItems(lineItems);

            return invoice;
        }

        /// <summary>
        /// Create a new invoice on the authenticated account. Makes both a POST and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="kind">The kind of invoice to create</param>
        /// <param name="clientId">The client the new invoice is for</param>
        /// <param name="issuedAt">The date the invoice should be issued</param>
        /// <param name="dueAt">The date the invoice should be due</param>
        /// <param name="currency">The currency of the invoice</param>
        /// <param name="subject">The invoice subject</param>
        /// <param name="notes">Notes to include on the invoice</param>
        /// <param name="number">The number for the invoice</param>
        /// <param name="projectIds">The IDs of projects to include in the invoice (useless for FreeForm invoices)</param>
        /// <param name="lineItems">A collection of line items for the invoice (only for FreeForm invoices)</param>
        public Invoice CreateInvoice(InvoiceKind kind, long clientId, DateTime issuedAt,
            DateTime? dueAt = null, Currency? currency = null, string subject = null, string notes = null,
            string number = null, long[] projectIds = null, List<InvoiceItem> lineItems = null)
        {
            return
                CreateInvoice(CreateInvoiceOptions(kind, clientId, issuedAt, dueAt, currency, subject, notes, number,
                    projectIds, lineItems));
        }

        /// <summary>
        /// Create a new invoice on the authenticated account. Makes both a POST and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="kind">The kind of invoice to create</param>
        /// <param name="clientId">The client the new invoice is for</param>
        /// <param name="issuedAt">The date the invoice should be issued</param>
        /// <param name="dueAt">The date the invoice should be due</param>
        /// <param name="currency">The currency of the invoice</param>
        /// <param name="subject">The invoice subject</param>
        /// <param name="notes">Notes to include on the invoice</param>
        /// <param name="number">The number for the invoice</param>
        /// <param name="projectIds">The IDs of projects to include in the invoice (useless for FreeForm invoices)</param>
        /// <param name="lineItems">A collection of line items for the invoice (only for FreeForm invoices)</param>
        public async Task<Invoice> CreateInvoiceAsync(InvoiceKind kind, long clientId, DateTime issuedAt,
            DateTime? dueAt = null, Currency? currency = null, string subject = null, string notes = null,
            string number = null, long[] projectIds = null, List<InvoiceItem> lineItems = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await
                CreateInvoiceAsync(CreateInvoiceOptions(kind, clientId, issuedAt, dueAt, currency, subject, notes, number,
                    projectIds, lineItems), cancellationToken);
        }

        private IRestRequest CreateInvoiceRequest(InvoiceOptions options)
        {
            var request = Request("invoices", RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Create a new invoice on the authenticated account. Makes both a POST and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="options">The options for the new invoice to create</param>
        public Invoice CreateInvoice(InvoiceOptions options)
        {
            return Execute<Invoice>(CreateInvoiceRequest(options));
        }

        /// <summary>
        /// Create a new invoice on the authenticated account. Makes both a POST and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="options">The options for the new invoice to create</param>
        public async Task<Invoice> CreateInvoiceAsync(InvoiceOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Invoice>(CreateInvoiceRequest(options), cancellationToken);
        }
        
        /// <summary>
        /// Delete an invoice from the authenticated account. Makes a DELETE request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public bool DeleteInvoice(long invoiceId)
        {
            var result = Execute(Request($"{InvoicesResource}/{invoiceId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an invoice from the authenticated account. Makes a DELETE request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteInvoiceAsync(long invoiceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{InvoicesResource}/{invoiceId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private IRestRequest UpdateInvoiceRequest(long invoiceId, InvoiceOptions options)
        {
            var request = Request("invoices/" + invoiceId, RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Update an existing invoice on the authenticated account. Makes both a PUT and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to update</param>
        /// <param name="options">The fields to be updated</param>
        public Invoice UpdateInvoice(long invoiceId, InvoiceOptions options)
        {
            return Execute<Invoice>(UpdateInvoiceRequest(invoiceId, options));
        }

        /// <summary>
        /// Update an existing invoice on the authenticated account. Makes both a PUT and a GET request to the Invoices resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to update</param>
        /// <param name="options">The fields to be updated</param>
        public async Task<Invoice> UpdateInvoiceAsync(long invoiceId, InvoiceOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Invoice>(UpdateInvoiceRequest(invoiceId, options), cancellationToken);
        }
    }
}
