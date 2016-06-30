using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class PeopleFacts : FactBase, IDisposable
    {
        User _todelete = null;

        #region Standard Api
        [Fact]
        public void ListUsers_Returns()
        {
            var list = Api.ListUsers();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void User_ReturnsUser()
        {
            var User = Api.User(GetTestId(TestId.UserId));

            Assert.NotNull(User);
            Assert.Equal("Joel", User.FirstName);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public void DeleteUser_ReturnsTrue()
        {
            var User = Api.CreateUser("deletetest@example.com", "Test", "Delete User");

            var result = Api.DeleteUser(User.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public void CreateUser_ReturnsANewUser()
        {
            _todelete = Api.CreateUser("createtest@example.com", "Test", "Create User");

            Assert.Equal("Test", _todelete.FirstName);
            Assert.Equal("Create User", _todelete.LastName);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public void ToggleUser_TogglesTheUserStatus()
        {
            _todelete = Api.CreateUser("toggletest@example.com", "Test", "Toggle User");

            Assert.Equal(true, _todelete.IsActive);

            var toggled = Api.ToggleUser(_todelete.Id);

            Assert.Equal(false, toggled.IsActive);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public void UpdateUser_UpdatesOnlyChangedValues()
        {
            _todelete = Api.CreateUser("updatetest@example.com", "Test", "Update User");

            var updated = Api.UpdateUser(_todelete.Id, lastName: "Updated User", department: "department");

            // stuff changed
            Assert.NotEqual(_todelete.LastName, updated.LastName);
            Assert.Equal("Updated User", updated.LastName);
            Assert.NotEqual(_todelete.Department, updated.Department);
            Assert.Equal("department", updated.Department);

            // stuff didn't change
            Assert.Equal(_todelete.IsActive, updated.IsActive);
            Assert.Equal(_todelete.DefaultHourlyRate, updated.DefaultHourlyRate);
            Assert.Equal(_todelete.FirstName, updated.FirstName);
            Assert.Equal(_todelete.Timezone, updated.Timezone);
        }
        #endregion

        #region Async Api
        [Fact]
        public async Task ListUsersAsync_Returns()
        {
            var list = await Api.ListUsersAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task UserAsync_ReturnsUser()
        {
            var User = await Api.UserAsync(GetTestId(TestId.UserId));

            Assert.NotNull(User);
            Assert.Equal("Joel", User.FirstName);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public async Task DeleteUserAsync_ReturnsTrue()
        {
            var User = await Api.CreateUserAsync("deletetest@example.com", "Test", "Delete User");

            var result = await Api.DeleteUserAsync(User.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public async Task CreateUserAsync_ReturnsANewUser()
        {
            _todelete = await Api.CreateUserAsync("createtest@example.com", "Test", "Create User");

            Assert.Equal("Test", _todelete.FirstName);
            Assert.Equal("Create User", _todelete.LastName);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public async Task ToggleUserAsync_TogglesTheUserStatus()
        {
            _todelete = await Api.CreateUserAsync("toggletest@example.com", "Test", "Toggle User");

            Assert.Equal(true, _todelete.IsActive);

            var toggled = await Api.ToggleUserAsync(_todelete.Id);

            Assert.Equal(false, toggled.IsActive);
        }

        [Fact(Skip = "Fails because the account has hit the max users limit")]
        public async Task UpdateUserAsync_UpdatesOnlyChangedValues()
        {
            _todelete = await Api.CreateUserAsync("updatetest@example.com", "Test", "Update User");

            var updated = await Api.UpdateUserAsync(_todelete.Id, lastName: "Updated User", department: "department");

            // stuff changed
            Assert.NotEqual(_todelete.LastName, updated.LastName);
            Assert.Equal("Updated User", updated.LastName);
            Assert.NotEqual(_todelete.Department, updated.Department);
            Assert.Equal("department", updated.Department);

            // stuff didn't change
            Assert.Equal(_todelete.IsActive, updated.IsActive);
            Assert.Equal(_todelete.DefaultHourlyRate, updated.DefaultHourlyRate);
            Assert.Equal(_todelete.FirstName, updated.FirstName);
            Assert.Equal(_todelete.Timezone, updated.Timezone);
        }
        #endregion

        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteUser(_todelete.Id);
        }
    }
}
