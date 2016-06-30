using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class InvoicePaymentFacts : FactBase, IDisposable
    {
        Payment _todelete = null;

        #region Standard Api
        [Fact]
        public void ListPayments_Returns()
        {
            var list = Api.ListPayments(GetTestId(TestId.InvoiceId));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void Payment_ReturnsPayment()
        {
            var Payment = Api.Payment(GetTestId(TestId.InvoiceId), GetTestId(TestId.PaymentId));

            Assert.NotNull(Payment);
            Assert.Equal("TEST PAYMENT DO NOT DELETE", Payment.Notes);
        }

        [Fact]
        public void DeletePayment_ReturnsTrue()
        {
            var Payment = Api.CreatePayment(GetTestId(TestId.InvoiceId), 1, DateTime.Now, "Test Delete Payment");

            var result = Api.DeletePayment(Payment.InvoiceId, Payment.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public void CreatePayment_ReturnsANewPayment()
        {
            _todelete = Api.CreatePayment(GetTestId(TestId.InvoiceId), 1, DateTime.Now, "Test Create Payment");

            Assert.Equal("Test Create Payment", _todelete.Notes);
        }

        #endregion

        #region Async Api
        [Fact]
        public async Task ListPaymentsAsync_Returns()
        {
            var list = await Api.ListPaymentsAsync(GetTestId(TestId.InvoiceId));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task PaymentAsync_ReturnsPayment()
        {
            var Payment = await Api.PaymentAsync(GetTestId(TestId.InvoiceId), GetTestId(TestId.PaymentId));

            Assert.NotNull(Payment);
            Assert.Equal("TEST PAYMENT DO NOT DELETE", Payment.Notes);
        }

        [Fact]
        public async Task DeletePaymentAsync_ReturnsTrue()
        {
            var Payment = await Api.CreatePaymentAsync(GetTestId(TestId.InvoiceId), 1, DateTime.Now, "Test Delete Payment");

            var result = await Api.DeletePaymentAsync(Payment.InvoiceId, Payment.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task CreatePaymentAsync_ReturnsANewPayment()
        {
            _todelete = await Api.CreatePaymentAsync(GetTestId(TestId.InvoiceId), 1, DateTime.Now, "Test Create Payment");

            Assert.Equal("Test Create Payment", _todelete.Notes);
        }
        #endregion

        public void Dispose()
        {
            if (_todelete != null)
                Api.DeletePayment(_todelete.InvoiceId, _todelete.Id);
        }
    }
}
