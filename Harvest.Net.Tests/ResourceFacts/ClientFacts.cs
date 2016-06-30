using Harvest.Net.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Harvest.Net.Tests
{
    public class ClientFacts : FactBase, IDisposable
    {
        Client _todelete = null;

        #region Standard Api
        [Fact]
        public void ListClients_Returns()
        {
            var list = Api.ListClients();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public void Client_ReturnsClient()
        {
            var client = Api.Client(GetTestId(TestId.ClientId));

            Assert.NotNull(client);
            Assert.Equal("Harvest.Net", client.Name);
        }

        [Fact(Skip = "Does not work on free account")]
        public void DeleteClient_ReturnsTrue()
        {
            var client = Api.CreateClient("Test Delete Client");

            var result = Api.DeleteClient(client.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Does not work on free account")]
        public void CreateClient_ReturnsANewClient()
        {
            _todelete = Api.CreateClient("Test Create Client");
            
            Assert.Equal("Test Create Client", _todelete.Name);
        }

        [Fact(Skip = "Does not work on free account")]
        public void ToggleClient_TogglesTheClientStatus()
        {
            _todelete = Api.CreateClient("Test Toggle Client");

            Assert.Equal(true, _todelete.Active);

            var toggled = Api.ToggleClient(_todelete.Id);

            Assert.Equal(false, toggled.Active);
        }

        [Fact(Skip = "Does not work on free account")]
        public void UpdateClient_UpdatesOnlyChangedValues()
        {
            _todelete = Api.CreateClient("Test Update Client");

            var updated = Api.UpdateClient(_todelete.Id, "Updated Client", details: "details");
            
            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Client", updated.Name);
            Assert.NotEqual(_todelete.Details, updated.Details);
            Assert.Equal("details", updated.Details);

            // stuff didn't change
            Assert.Equal(_todelete.Active, updated.Active);
            Assert.Equal(_todelete.Currency, updated.Currency);
        }
        #endregion

        #region Async Api
        [Fact]
        public async Task ListClientsAsync_Returns()
        {
            var list =  await Api.ListClientsAsync();

            Assert.True(list != null, "Result list is null.");
            Assert.NotEqual(0, list.First().Id);
        }

        [Fact]
        public async Task ClientAsync_ReturnsClient()
        {
            var client = await Api.ClientAsync(GetTestId(TestId.ClientId));

            Assert.NotNull(client);
            Assert.Equal("Harvest.Net", client.Name);
        }

        [Fact(Skip = "Does not work on free account")]
        public async Task DeleteClientAsync_ReturnsTrue()
        {
            var client = await Api.CreateClientAsync("Test Delete Client");

            var result = await Api.DeleteClientAsync(client.Id);

            Assert.Equal(true, result);
        }

        [Fact(Skip = "Does not work on free account")]
        public async Task CreateClientAsync_ReturnsANewClient()
        {
            _todelete = await Api.CreateClientAsync("Test Create Client");

            Assert.Equal("Test Create Client", _todelete.Name);
        }

        [Fact(Skip = "Does not work on free account")]
        public async Task ToggleClientAsync_TogglesTheClientStatus()
        {
            _todelete = await Api.CreateClientAsync("Test Toggle Client");

            Assert.Equal(true, _todelete.Active);

            var toggled = await Api.ToggleClientAsync(_todelete.Id);

            Assert.Equal(false, toggled.Active);
        }

        [Fact(Skip = "Does not work on free account")]
        public async Task UpdateClientAsync_UpdatesOnlyChangedValues()
        {
            _todelete = await Api.CreateClientAsync("Test Update Client");

            var updated = await Api.UpdateClientAsync(_todelete.Id, "Updated Client", details: "details");

            // stuff changed
            Assert.NotEqual(_todelete.Name, updated.Name);
            Assert.Equal("Updated Client", updated.Name);
            Assert.NotEqual(_todelete.Details, updated.Details);
            Assert.Equal("details", updated.Details);

            // stuff didn't change
            Assert.Equal(_todelete.Active, updated.Active);
            Assert.Equal(_todelete.Currency, updated.Currency);
        }
        #endregion

        public void Dispose()
        {
            if (_todelete != null)
                Api.DeleteClient(_todelete.Id);
        }
    }
}
