﻿using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class TimeAndExpenseFacts : FactBase
    {
        const string TEST_HOUR_NOTES = "Test DO NOT DELETE";
        const string TEST_EXPENSE_NOTES = "DO NOT DELETE";

        #region Standard Api
        [Fact]
        public void ListUserEntires_ReturnsEntries()
        {
            var list = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_HOUR_NOTES, list.First().Notes);
        }

        [Fact]
        public void ListUserEntires_FiltersCorrectly()
        {
            var notBillable = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: false);
            var billable = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: true);
            var notBilled = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: false);
            var billed = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: true);
            var notClosed = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: false);
            var closed = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: true);
            var rightProject = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), projectId: GetTestId(TestId.ProjectId));
            var wrongProject = Api.ListUserEntries(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), projectId: GetTestId(TestId.ProjectId2));

            Assert.Equal(TEST_HOUR_NOTES, notBillable.First().Notes);
            Assert.Equal(0, billable.Count());
            Assert.Equal(TEST_HOUR_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_HOUR_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
            Assert.Equal(TEST_HOUR_NOTES, rightProject.First().Notes);
            Assert.Equal(0, wrongProject.Count());
        }

        [Fact]
        public void ListProjectEntires_ReturnsEntries()
        {
            var list = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_HOUR_NOTES, list.First().Notes);
        }

        [Fact]
        public void ListProjectEntires_FiltersCorrectly()
        {
            var notBillable = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: false);
            var billable = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: true);
            var notBilled = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: false);
            var billed = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: true);
            var notClosed = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: false);
            var closed = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: true);
            var rightUser = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), userId: GetTestId(TestId.UserId));

            // need a paid account to test wrong user (returns error if userid is not a valid user id)
            //var wrongUser = Api.ListProjectEntries(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), userId: GetTestId(TestId.UserId2));

            Assert.Equal(TEST_HOUR_NOTES, notBillable.First().Notes);
            Assert.Equal(0, billable.Count());
            Assert.Equal(TEST_HOUR_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_HOUR_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
            Assert.Equal(TEST_HOUR_NOTES, rightUser.First().Notes);
            //Assert.Equal(0, wrongUser.Count());
        }

        [Fact]
        public void ListUserExpenses_ReturnsExpenses()
        {
            var list = Api.ListUserExpenses(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_EXPENSE_NOTES, list.First().Notes);
        }

        [Fact]
        public void ListUserExpenses_FiltersCorrectly()
        {
            var notClosed = Api.ListUserExpenses(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: false);
            var closed = Api.ListUserExpenses(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: true);

            Assert.Equal(TEST_EXPENSE_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
        }

        [Fact]
        public void ListProjectExpenses_ReturnsExpenses()
        {
            var list = Api.ListProjectExpenses(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_EXPENSE_NOTES, list.First().Notes);
        }

        [Fact]
        public void ListProjectExpenses_FiltersCorrectly()
        {
            var notBilled = Api.ListProjectExpenses(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isBilled: false);
            var billed = Api.ListProjectExpenses(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isBilled: true);
            var notClosed = Api.ListProjectExpenses(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: false);
            var closed = Api.ListProjectExpenses(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: true);

            Assert.Equal(TEST_EXPENSE_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_EXPENSE_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
        }
        #endregion

        #region Async Api
        [Fact]
        public async Task ListUserEntiresAsync_ReturnsEntries()
        {
            var list = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_HOUR_NOTES, list.First().Notes);
        }

        [Fact]
        public async Task ListUserEntiresAsync_FiltersCorrectly()
        {
            var notBillable = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: false);
            var billable = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: true);
            var notBilled = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: false);
            var billed = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: true);
            var notClosed = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: false);
            var closed = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: true);
            var rightProject = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), projectId: GetTestId(TestId.ProjectId));
            var wrongProject = await Api.ListUserEntriesAsync(GetTestId(TestId.UserId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), projectId: GetTestId(TestId.ProjectId2));

            Assert.Equal(TEST_HOUR_NOTES, notBillable.First().Notes);
            Assert.Equal(0, billable.Count());
            Assert.Equal(TEST_HOUR_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_HOUR_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
            Assert.Equal(TEST_HOUR_NOTES, rightProject.First().Notes);
            Assert.Equal(0, wrongProject.Count());
        }

        [Fact]
        public async Task ListProjectEntiresAsync_ReturnsEntries()
        {
            var list = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_HOUR_NOTES, list.First().Notes);
        }

        [Fact]
        public async Task ListProjectEntiresAsync_FiltersCorrectly()
        {
            var notBillable = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: false);
            var billable = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBillable: true);
            var notBilled = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: false);
            var billed = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isBilled: true);
            var notClosed = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: false);
            var closed = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), isClosed: true);
            var rightUser = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), userId: GetTestId(TestId.UserId));

            // need a paid account to test wrong user (returns error if userid is not a valid user id)
            //var wrongUser = await Api.ListProjectEntriesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 12, 19), new DateTime(2014, 12, 20), userId: GetTestId(TestId.UserId2));

            Assert.Equal(TEST_HOUR_NOTES, notBillable.First().Notes);
            Assert.Equal(0, billable.Count());
            Assert.Equal(TEST_HOUR_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_HOUR_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
            Assert.Equal(TEST_HOUR_NOTES, rightUser.First().Notes);
            //Assert.Equal(0, wrongUser.Count());
        }

        [Fact]
        public async Task ListUserExpensesAsync_ReturnsExpenses()
        {
            var list = await Api.ListUserExpensesAsync(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_EXPENSE_NOTES, list.First().Notes);
        }

        [Fact]
        public async Task ListUserExpensesAsync_FiltersCorrectly()
        {
            var notClosed = await Api.ListUserExpensesAsync(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: false);
            var closed = await Api.ListUserExpensesAsync(GetTestId(TestId.UserId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: true);

            Assert.Equal(TEST_EXPENSE_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
        }

        [Fact]
        public async Task ListProjectExpensesAsync_ReturnsExpenses()
        {
            var list = await Api.ListProjectExpensesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7));

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
            Assert.Equal(TEST_EXPENSE_NOTES, list.First().Notes);
        }

        [Fact]
        public async Task ListProjectExpensesAsync_FiltersCorrectly()
        {
            var notBilled = await Api.ListProjectExpensesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isBilled: false);
            var billed = await Api.ListProjectExpensesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isBilled: true);
            var notClosed = await Api.ListProjectExpensesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: false);
            var closed = await Api.ListProjectExpensesAsync(GetTestId(TestId.ProjectId), new DateTime(2014, 11, 6), new DateTime(2014, 11, 7), isClosed: true);

            Assert.Equal(TEST_EXPENSE_NOTES, notBilled.First().Notes);
            Assert.Equal(0, billed.Count());
            Assert.Equal(TEST_EXPENSE_NOTES, notClosed.First().Notes);
            Assert.Equal(0, closed.Count());
        }
        #endregion

    }
}
