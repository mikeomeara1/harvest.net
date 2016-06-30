using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class InvoiceCategoryFacts : FactBase, IDisposable
    {
        InvoiceItemCategory _todelete = null;
        #region Standard Api
        [Fact]
        public void ListInvoiceCategories_Returns()
        {
            var list = Api.ListInvoiceCategories();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public void InvoiceCategory_ReturnsInvoiceCategory()
        {
            var invoiceCategory = Api.InvoiceCategory(GetTestId(TestId.InvoiceCategoryId));

            Assert.NotNull(invoiceCategory);
            Assert.Equal("TEST CATEGORY DO NOT DELETE", invoiceCategory.Name);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public void DeleteInvoiceCategory_ReturnsTrue()
        {
            var InvoiceCategory = Api.CreateInvoiceCategory("Test Delete Invoice Category");

            var result = Api.DeleteInvoiceCategory(InvoiceCategory.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public void CreateInvoiceCategory_ReturnsANewInvoiceCategory()
        {
            _todelete = Api.CreateInvoiceCategory("Test Create Invoice Category");

            Assert.Equal("Test Create Invoice Category", _todelete.Name);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public void UpdateInvoiceCategory_UpdatingUnitsOnlyChangesUnits()
        {
            _todelete = Api.CreateInvoiceCategory("Test Update Invoice Category", useAsExpense: true, useAsService: true);

            Assert.Equal(true, _todelete.UseAsExpense);

            var updated = Api.UpdateInvoiceCategory(_todelete.Id, useAsExpense: false);

            // stuff changed
            Assert.NotEqual(_todelete.UseAsExpense, updated.UseAsExpense);
            Assert.Equal(false, updated.UseAsExpense);

            // stuff didn't change
            Assert.Equal(_todelete.Name, updated.Name);
            Assert.Equal(_todelete.UseAsService, updated.UseAsService);
        }

        #endregion

        #region Async Api
        [Fact]
        public async Task ListInvoiceCategoriesAsync_Returns()
        {
            var list = await Api.ListInvoiceCategoriesAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public async Task InvoiceCategoryAsync_ReturnsInvoiceCategory()
        {
            var InvoiceCategory = await Api.InvoiceCategoryAsync(GetTestId(TestId.InvoiceCategoryId));

            Assert.NotNull(InvoiceCategory);
            Assert.Equal("TEST CATEGORY DO NOT DELETE", InvoiceCategory.Name);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public async Task DeleteInvoiceCategoryAsync_ReturnsTrue()
        {
            var InvoiceCategory = await Api.CreateInvoiceCategoryAsync("Test Delete Invoice Category");

            var result = await Api.DeleteInvoiceCategoryAsync(InvoiceCategory.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public async Task CreateInvoiceCategoryAsync_ReturnsANewInvoiceCategory()
        {
            _todelete = await Api.CreateInvoiceCategoryAsync("Test Create Invoice Category");

            Assert.Equal("Test Create Invoice Category", _todelete.Name);
        }

        [Fact(Skip = "GET for a single item does not work. https://github.com/harvesthq/api/issues/90")]
        public async Task UpdateInvoiceCategoryAsync_UpdatingUnitsOnlyChangesUnits()
        {
            _todelete = await Api.CreateInvoiceCategoryAsync("Test Update Invoice Category", useAsExpense: true, useAsService: true);

            Assert.Equal(true, _todelete.UseAsExpense);

            var updated = await Api.UpdateInvoiceCategoryAsync(_todelete.Id, useAsExpense: false);

            // stuff changed
            Assert.NotEqual(_todelete.UseAsExpense, updated.UseAsExpense);
            Assert.Equal(false, updated.UseAsExpense);

            // stuff didn't change
            Assert.Equal(_todelete.Name, updated.Name);
            Assert.Equal(_todelete.UseAsService, updated.UseAsService);
        }
        #endregion
        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteInvoiceCategory(_todelete.Id);
        }
    }
}
