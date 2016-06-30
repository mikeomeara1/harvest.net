﻿using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class ExpenseCategoryFacts : FactBase, IDisposable
    {
        ExpenseCategory _todelete = null;

        #region Standard Api
        [Fact]
        public void ListExpenseCategories_Returns()
        {
            var list = Api.ListExpenseCategories();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void ExpenseCategory_ReturnsExpenseCategory()
        {
            var expenseCategory = Api.ExpenseCategory(GetTestId(TestId.ExpenseCategoryId)); // Id of base Entertainment expense category

            Assert.NotNull(expenseCategory);
            Assert.Equal("Entertainment", expenseCategory.Name);
        }

        [Fact]
        public void DeleteExpenseCategory_ReturnsTrue()
        {
            var expenseCategory = Api.CreateExpenseCategory("Test Delete Expense Category");

            var result = Api.DeleteExpenseCategory(expenseCategory.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public void CreateExpenseCategory_ReturnsANewExpenseCategory()
        {
            _todelete = Api.CreateExpenseCategory("Test Create Expense Category");

            Assert.Equal("Test Create Expense Category", _todelete.Name);
        }

        [Fact]
        public void UpdateExpenseCategory_UpdatingUnitsOnlyChangesUnits()
        {
            _todelete = Api.CreateExpenseCategory("Test Update Expense Category", unitName: "UNIT", unitPrice: 1);

            Assert.Equal(1, _todelete.UnitPrice);

            var updated = Api.UpdateExpenseCategory(_todelete.Id, unitName: "TEST", unitPrice: _todelete.UnitPrice);

            // stuff changed
            Assert.NotEqual(_todelete.UnitName, updated.UnitName);
            Assert.Equal("TEST", updated.UnitName);

            // stuff didn't change
            Assert.Equal(_todelete.Name, updated.Name);
            Assert.Equal(_todelete.UnitPrice, updated.UnitPrice);
        }

        [Fact(Skip = "This test currently fails because of a bug in the API. https://github.com/harvesthq/api/issues/81")]
        public void UpdateExpenseCategory_UpdatingNameOnlyChangesName()
        {
            _todelete = Api.CreateExpenseCategory("Test Update Expense Category", unitName: "UNIT", unitPrice: 1);

            var updated = Api.UpdateExpenseCategory(_todelete.Id, name: "Updated Expense Category");

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Expense Category", updated.Name);

            // stuff didn't change
            Assert.Equal(_todelete.UnitName, updated.UnitName);
            Assert.Equal(_todelete.UnitPrice, updated.UnitPrice);
        }
        #endregion
        #region Async Api
        [Fact]
        public async Task ListExpenseCategoriesAsync_Returns()
        {
            var list = await Api.ListExpenseCategoriesAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task ExpenseCategoryAsync_ReturnsExpenseCategory()
        {
            var expenseCategory = await Api.ExpenseCategoryAsync(GetTestId(TestId.ExpenseCategoryId)); // Id of base Entertainment expense category

            Assert.NotNull(expenseCategory);
            Assert.Equal("Entertainment", expenseCategory.Name);
        }

        [Fact]
        public async Task DeleteExpenseCategoryAsync_ReturnsTrue()
        {
            var expenseCategory = await Api.CreateExpenseCategoryAsync("Test Delete Expense Category");

            var result = await Api.DeleteExpenseCategoryAsync(expenseCategory.Id);

            Assert.Equal(true, result);
        }

        [Fact]
        public async Task CreateExpenseCategoryAsync_ReturnsANewExpenseCategory()
        {
            _todelete = await Api.CreateExpenseCategoryAsync("Test Create Expense Category");

            Assert.Equal("Test Create Expense Category", _todelete.Name);
        }

        [Fact]
        public async Task UpdateExpenseCategoryAsync_UpdatingUnitsOnlyChangesUnits()
        {
            _todelete = await Api.CreateExpenseCategoryAsync("Test Update Expense Category", unitName: "UNIT", unitPrice: 1);

            Assert.Equal(1, _todelete.UnitPrice);

            var updated = await Api.UpdateExpenseCategoryAsync(_todelete.Id, unitName: "TEST", unitPrice: _todelete.UnitPrice);

            // stuff changed
            Assert.NotEqual(_todelete.UnitName, updated.UnitName);
            Assert.Equal("TEST", updated.UnitName);

            // stuff didn't change
            Assert.Equal(_todelete.Name, updated.Name);
            Assert.Equal(_todelete.UnitPrice, updated.UnitPrice);
        }

        [Fact(Skip = "This test currently fails because of a bug in the API. https://github.com/harvesthq/api/issues/81")]
        public async Task UpdateExpenseCategoryAsync_UpdatingNameOnlyChangesName()
        {
            _todelete = await Api.CreateExpenseCategoryAsync("Test Update Expense Category", unitName: "UNIT", unitPrice: 1);

            var updated = await Api.UpdateExpenseCategoryAsync(_todelete.Id, name: "Updated Expense Category");

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Expense Category", updated.Name);

            // stuff didn't change
            Assert.Equal(_todelete.UnitName, updated.UnitName);
            Assert.Equal(_todelete.UnitPrice, updated.UnitPrice);
        }
        #endregion
        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteExpenseCategory(_todelete.Id);
        }
    }
}
