using Harvest.Net.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace Harvest.Net
{
    public partial class HarvestRestClient
    {
        // https://github.com/harvesthq/api/blob/master/Sections/Invoice%20Messages.md
        private const string InvoicesResource = "invoices";
        private const string MessagesAction = "messages";

        /// <summary>
        /// Retrieve a list of messages for an invoice on the authenticated account. Makes a GET request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to retrieve messages for</param>
        public IList<InvoiceMessage> ListInvoiceMessages(long invoiceId)
        {
            return Execute<List<InvoiceMessage>>(Request($"{InvoicesResource}/{invoiceId}/messages"));
        }

        /// <summary>
        /// Retrieve a list of messages for an invoice on the authenticated account. Makes a GET request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to retrieve messages for</param>
        public async Task<IList<InvoiceMessage>> ListInvoiceMessagesAsync(long invoiceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<InvoiceMessage>>(Request($"{InvoicesResource}/{invoiceId}/messages"), cancellationToken);
        }

      
        /// <summary>
        /// Retrieve a single message for an invoice on the authenticated account. Makes a GET request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice the message is on</param>
        /// <param name="messageId">The ID of the message to retrieve</param>
        public InvoiceMessage InvoiceMessage(long invoiceId, long messageId)
        {
            return Execute<InvoiceMessage>(Request($"{InvoicesResource}/{invoiceId}/messages/{messageId}"));
        }

        /// <summary>
        /// Retrieve a single message for an invoice on the authenticated account. Makes a GET request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice the message is on</param>
        /// <param name="messageId">The ID of the message to retrieve</param>
        public async Task<InvoiceMessage> InvoiceMessageAsync(long invoiceId, long messageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<InvoiceMessage>(Request($"{InvoicesResource}/{invoiceId}/messages/{messageId}"), cancellationToken);
        }

        /// <summary>
        /// Send an existing invoice to a list of recipients. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to send</param>
        /// <param name="recipients">The email addresses of the recipients</param>
        /// <param name="body">The body of the message</param>
        /// <param name="sendMeACopy">Whether to send a copy of the invoice to the authenticated user</param>
        /// <param name="attachPdf">Whether to attach a pdf copy of the invoice to the email(s)</param>
        public InvoiceMessage SendInvoice(long invoiceId, string recipients, string body = null, bool sendMeACopy = true, bool attachPdf = true, bool includeLink = false)
        {
            return SendInvoice(invoiceId, new InvoiceMessageOptions()
            {
                Recipients = recipients,
                Body = body,
                SendMeACopy = sendMeACopy,
                AttachPdf = attachPdf,
                IncludePayPalLink = includeLink
            });
        }

        /// <summary>
        /// Send an existing invoice to a list of recipients. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to send</param>
        /// <param name="recipients">The email addresses of the recipients</param>
        /// <param name="body">The body of the message</param>
        /// <param name="sendMeACopy">Whether to send a copy of the invoice to the authenticated user</param>
        /// <param name="attachPdf">Whether to attach a pdf copy of the invoice to the email(s)</param>
        public async Task<InvoiceMessage> SendInvoiceAsync(long invoiceId, string recipients, string body = null, bool sendMeACopy = true, bool attachPdf = true, bool includeLink = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SendInvoiceAsync(invoiceId, new InvoiceMessageOptions()
            {
                Recipients = recipients,
                Body = body,
                SendMeACopy = sendMeACopy,
                AttachPdf = attachPdf,
                IncludePayPalLink = includeLink
            }, cancellationToken);
        }

        public IRestRequest SendInvoiceRequest(long invoiceId, InvoiceMessageOptions options)
        {
            var request = Request($"{InvoicesResource}/{invoiceId}/messages", RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Send an existing invoice to a list of recipients. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to send</param>
        /// <param name="options">The options of the message to send</param>
        public InvoiceMessage SendInvoice(long invoiceId, InvoiceMessageOptions options)
        {
           

            return Execute<InvoiceMessage>(SendInvoiceRequest(invoiceId, options));
        }

        /// <summary>
        /// Send an existing invoice to a list of recipients. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to send</param>
        /// <param name="options">The options of the message to send</param>
        public async Task<InvoiceMessage> SendInvoiceAsync(long invoiceId, InvoiceMessageOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<InvoiceMessage>(SendInvoiceRequest(invoiceId, options), cancellationToken);
        }


        /// <summary>
        /// Delete an existing invoice message from the authenticated account. Makes a DELETE request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice containing the message</param>
        /// <param name="messageId">The ID of the message to delete</param>
        public bool DeleteInvoiceMessage(long invoiceId, long messageId)
        {
            var result =
                Execute(Request($"{InvoicesResource}/{invoiceId}/messages/{messageId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an existing invoice message from the authenticated account. Makes a DELETE request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice containing the message</param>
        /// <param name="messageId">The ID of the message to delete</param>
        public async Task<bool> DeleteInvoiceMessageAsync(long invoiceId, long messageId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result =
                await
                    ExecuteAsync(Request($"{InvoicesResource}/{invoiceId}/messages/{messageId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private const string MarkAsClosed = "mark_as_closed";
        private const string MarkAsSent = "mark_as_sent";
        private const string MarkAsDraft = "mark_as_draft";
        private const string ReOpen = "re_open";

        private string InvoiceMessageAction(InvoiceMessageAction action)
        {
            var memberInfo = action.GetType().GetMember(action.ToString())
                                            .FirstOrDefault();

            return ((DescriptionAttribute)memberInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description;
        }

        public bool MarkInvoice(long invoiceId, string body, InvoiceMessageAction action)
        {
            return _createInvoiceMessageAction(invoiceId, body, InvoiceMessageAction(action));
        }

        public async Task<bool> MarkInvoiceAsync(long invoiceId, string body, InvoiceMessageAction action, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _createInvoiceMessageActionAsync(invoiceId, body, InvoiceMessageAction(action), cancellationToken);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as closed (written-off). Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to close</param>
        /// <param name="body">The message body</param>
        public bool MarkInvoiceClosed(long invoiceId, string body)
        {
            return MarkInvoice(invoiceId, body, Models.InvoiceMessageAction.MarkAsClosed);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as closed (written-off). Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to close</param>
        /// <param name="body">The message body</param>
        public async Task<bool> MarkInvoiceClosedAsync(long invoiceId, string body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MarkInvoiceAsync(invoiceId, body, Models.InvoiceMessageAction.MarkAsClosed, cancellationToken);
        }


        /// <summary>
        /// Mark an existing invoice from the authenticated account as sent. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to mark as sent</param>
        /// <param name="body">The message body</param>
        public bool MarkInvoiceSent(long invoiceId, string body)
        {
            return MarkInvoice(invoiceId, body, Models.InvoiceMessageAction.MarkAsSent);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as sent. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to mark as sent</param>
        /// <param name="body">The message body</param>
        public async Task<bool> MarkInvoiceSentAsync(long invoiceId, string body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MarkInvoiceAsync(invoiceId, body, Models.InvoiceMessageAction.MarkAsSent, cancellationToken);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as draft. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to mark as draft</param>
        public bool MarkInvoiceDraft(long invoiceId)
        {
            return MarkInvoice(invoiceId, null, Models.InvoiceMessageAction.MarkAsDraft);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as draft. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to mark as draft</param>
        public async Task<bool> MarkInvoiceDraftAsync(long invoiceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MarkInvoiceAsync(invoiceId, null, Models.InvoiceMessageAction.MarkAsDraft, cancellationToken);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as open. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to open</param>
        /// <param name="body">The message body</param>
        public bool ReopenInvoice(long invoiceId, string body)
        {
            return MarkInvoice(invoiceId, body, Models.InvoiceMessageAction.ReOpen);
        }

        /// <summary>
        /// Mark an existing invoice from the authenticated account as open. Makes a POST request to the Invoices/Messages resource.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice to open</param>
        /// <param name="body">The message body</param>
        public async Task<bool> ReopenInvoiceAsync(long invoiceId, string body, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await MarkInvoiceAsync(invoiceId, body, Models.InvoiceMessageAction.ReOpen, cancellationToken);
        }

        private IRestRequest CreateInvoiceMessageActionRequest(long invoiceId, string body, string action)
        {
            var request = Request($"{InvoicesResource}/{invoiceId}/messages/{action}", RestSharp.Method.POST);

            var options = new InvoiceMessageOptions()
            {
                Body = body
            };

            request.AddBody(options);

            return request;
        }

        private bool _createInvoiceMessageAction(long invoiceId, string body, string action)
        {
            var result = Execute(CreateInvoiceMessageActionRequest(invoiceId, body, action));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private async Task<bool> _createInvoiceMessageActionAsync(long invoiceId, string body, string action, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(CreateInvoiceMessageActionRequest(invoiceId, body, action), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
