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
        // https://github.com/harvesthq/api/blob/master/Sections/Invoice%20Payments.md
        private const string PaymentsResource = "payments";

        /// <summary>
        /// List all invoice payments for and invoice on the authenticated account. Makes a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice for which to list payments</param>
        public List<Payment> ListPayments(long invoiceId)
        {
            return Execute<List<Payment>>(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}"));
        }

        /// <summary>
        /// List all invoice payments for and invoice on the authenticated account. Makes a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice for which to list payments</param>
        public async Task<List<Payment>> ListPaymentsAsync(long invoiceId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Payment>>(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}"), cancellationToken);
        }

        /// <summary>
        /// Retrieve an invoice payment on the authenticated account. Makes a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice on which the payment exists</param>
        /// <param name="paymentId">The Id of the payment to retrieve</param>
        public Payment Payment(long invoiceId, long paymentId)
        {
            return Execute<Payment>(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}/{paymentId}"));
        }

        /// <summary>
        /// Retrieve an invoice payment on the authenticated account. Makes a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice on which the payment exists</param>
        /// <param name="paymentId">The Id of the payment to retrieve</param>
        public async Task<Payment> PaymentAsync(long invoiceId, long paymentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Payment>(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}/{paymentId}"), cancellationToken);
        }

        /// <summary>
        /// Creates a new payment under the authenticated account. Makes both a POST and a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice being paid</param>
        /// <param name="amount">The amount of the payment</param>
        /// <param name="paidAt">The the date of the payment</param>
        /// <param name="notes">Notes on the payment</param>
        public Payment CreatePayment(long invoiceId, decimal amount, DateTime paidAt, string notes = null)
        {
            return CreatePayment(invoiceId, new PaymentOptions()
            {
                Amount = amount,
                PaidAt = paidAt,
                Notes = notes
            });
        }

        /// <summary>
        /// Creates a new payment under the authenticated account. Makes both a POST and a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice being paid</param>
        /// <param name="amount">The amount of the payment</param>
        /// <param name="paidAt">The the date of the payment</param>
        /// <param name="notes">Notes on the payment</param>
        public async Task<Payment> CreatePaymentAsync(long invoiceId, decimal amount, DateTime paidAt, string notes = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreatePaymentAsync(invoiceId, new PaymentOptions()
            {
                Amount = amount,
                PaidAt = paidAt,
                Notes = notes
            }, cancellationToken);
        }

        private IRestRequest CreatePaymentRequest(long invoiceId, PaymentOptions options)
        {
            var request = Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}", RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new payment under the authenticated account. Makes a POST and a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice being paid</param>
        /// <param name="options">The options for the new payment to be created</param>
        public Payment CreatePayment(long invoiceId, PaymentOptions options)
        {
           return Execute<Payment>(CreatePaymentRequest(invoiceId, options));
        }

        /// <summary>
        /// Creates a new payment under the authenticated account. Makes a POST and a GET request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice being paid</param>
        /// <param name="options">The options for the new payment to be created</param>
        public async Task<Payment> CreatePaymentAsync(long invoiceId, PaymentOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Payment>(CreatePaymentRequest(invoiceId, options), cancellationToken);
        }

        /// <summary>
        /// Delete a payment from the authenticated account. Makes a DELETE request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice containing the payment to be deleted</param>
        /// <param name="paymentId">The Id of the payment to delete</param>
        public bool DeletePayment(long invoiceId, long paymentId)
        {
            var result = Execute(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}/{paymentId}", RestSharp.Method.DELETE));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a payment from the authenticated account. Makes a DELETE request to the Invoices/Payments resource.
        /// </summary>
        /// <param name="invoiceId">The Id of the invoice containing the payment to be deleted</param>
        /// <param name="paymentId">The Id of the payment to delete</param>
        public async Task<bool> DeletePaymentAsync(long invoiceId, long paymentId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(Request($"{InvoicesResource}/{invoiceId}/{PaymentsResource}/{paymentId}", RestSharp.Method.DELETE), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
