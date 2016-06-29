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
        private const string ClientsResource = "clients";
        private const string ToggleAction = "toggle";

        // https://github.com/harvesthq/api/blob/master/Sections/Clients.md
        private IRestRequest ListClientsRequest(DateTime? updatedSince = null)
        {
            var request = Request(ClientsResource);

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all clients for the authenticated account. Makes a GET request to the Clients resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the client updated-at property</param>
        public IList<Client> ListClients(DateTime? updatedSince = null)
        {
            return Execute<List<Client>>(this.ListClientsRequest(updatedSince));
        }

        /// <summary>
        /// List all clients for the authenticated account. Makes a GET request to the Clients resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the client updated-at property</param>
        /// <param name="cancellationToken"></param>
        public async Task<IList<Client>> ListClientsAsync(DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Client>>(this.ListClientsRequest(updatedSince), cancellationToken);
        }

        /// <summary>
        /// Retrieve a client on the authenticated account. Makes a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The Id of the client to retrieve</param>
        public Client Client(long clientId)
        {
            return Execute<Client>(Request($"{ClientsResource}/{clientId}"));
        }

        /// <summary>
        /// Retrieve a client on the authenticated account. Makes a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The Id of the client to retrieve</param>
        /// <param name="cancellationToken"></param>
        public async Task<Client> ClientAsync(long clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Client>(Request($"{ClientsResource}/{clientId}"), cancellationToken);
        }

        private ClientOptions CreateClientOptions(string name, Currency? currency = null, bool active = true,
            string details = null, long? highriseId = null)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return new ClientOptions()
            {
                Name = name,
                Active = active,
                Currency = currency,
                Details = details,
                HighriseId = highriseId
            };
        }

        /// <summary>
        /// Creates a new client under the authenticated account. Makes both a POST and a GET request to the Clients resource.
        /// </summary>
        /// <param name="name">The name of the client</param>
        /// <param name="currency">The currency for the client</param>
        /// <param name="active">The status of the client</param>
        /// <param name="details">The details (address, phone, etc.) of the client</param>
        /// <param name="highriseId">The related Highrise ID of the client</param>
        public Client CreateClient(string name, Currency? currency = null, bool active = true, string details = null, long? highriseId = null)
        {
            return CreateClient(this.CreateClientOptions(name, currency, active, details, highriseId));
        }

        /// <summary>
        /// Creates a new client under the authenticated account. Makes both a POST and a GET request to the Clients resource.
        /// </summary>
        /// <param name="name">The name of the client</param>
        /// <param name="currency">The currency for the client</param>
        /// <param name="active">The status of the client</param>
        /// <param name="details">The details (address, phone, etc.) of the client</param>
        /// <param name="highriseId">The related Highrise ID of the client</param>
        /// <param name="cancellationToken"></param>
        public async Task<Client> CreateClientAsync(string name, Currency? currency = null, bool active = true, string details = null, long? highriseId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateClientAsync(this.CreateClientOptions(name, currency, active, details, highriseId), cancellationToken);
        }

        private IRestRequest CreateClientRequest(ClientOptions options)
        {
            var request = Request(ClientsResource, RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new client under the authenticated account. Makes a POST and a GET request to the Clients resource.
        /// </summary>
        /// <param name="options">The options for the new client to be created</param>
        public Client CreateClient(ClientOptions options)
        {
            return Execute<Client>(CreateClientRequest(options));
        }

        /// <summary>
        /// Creates a new client under the authenticated account. Makes a POST and a GET request to the Clients resource.
        /// </summary>
        /// <param name="options">The options for the new client to be created</param>
        /// <param name="cancellationToken"></param>
        public async Task<Client> CreateClientAsync(ClientOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Client>(CreateClientRequest(options), cancellationToken);
        }

        private IRestRequest DeleteClientRequest(long clientId)
        {
            return Request($"{ClientsResource}/{clientId}", RestSharp.Method.DELETE);
        }

        /// <summary>
        /// Delete a client from the authenticated account. Makes a DELETE request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to delete</param>
        public bool DeleteClient(long clientId)
        {
            var result = Execute(DeleteClientRequest(clientId));
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a client from the authenticated account. Makes a DELETE request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to delete</param>
        /// <param name="cancellationToken"></param>
        public async Task<bool> DeleteClientAsync(long clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(DeleteClientRequest(clientId), cancellationToken);
            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        private IRestRequest ToggleClientRequest(long clientId)
        {
            return Request($"{ClientsResource}/{clientId}/{ToggleAction}", RestSharp.Method.POST);
        }

        /// <summary>
        /// Toggle the Active status of a client on the authenticated account. Makes a POST request to the Clients/Toggle resource and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to toggle</param>
        public Client ToggleClient(long clientId)
        {
            return Execute<Client>(ToggleClientRequest(clientId));
        }

        /// <summary>
        /// Toggle the Active status of a client on the authenticated account. Makes a POST request to the Clients/Toggle resource and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to toggle</param>
        /// <param name="cancellationToken"></param>
        public async Task<Client> ToggleClientAsync(long clientId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Client>(ToggleClientRequest(clientId), cancellationToken);
        }

    /// <summary>
        /// Update a client on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="currency">The updated currency</param>
        /// <param name="details">The updated details</param>
        /// <param name="highriseId">The updated Highrise ID</param>
        /// <param name="active">The updated state</param>
        public Client UpdateClient(long clientId, string name = null, Currency? currency = null, bool? active = null, string details = null, long? highriseId = null)
        {
            return UpdateClient(clientId, CreateClientOptions(name, currency, active ?? false, details, highriseId));
        }

        /// <summary>
        /// Update a client on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID of the client to update</param>
        /// <param name="name">The updated name</param>
        /// <param name="currency">The updated currency</param>
        /// <param name="details">The updated details</param>
        /// <param name="highriseId">The updated Highrise ID</param>
        /// <param name="active">The updated state</param>
        public async Task<Client> UpdateClientAsync(long clientId, string name = null, Currency? currency = null, bool? active = null, string details = null, long? highriseId = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateClientAsync(clientId, CreateClientOptions(name, currency, active ?? false, details, highriseId), cancellationToken);
        }

        private IRestRequest UpdateClientRequest(long clientId, ClientOptions options)
        {
            var request = Request($"{ClientsResource}/{clientId}", RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Updates a client on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID for the client to update</param>
        /// <param name="options">The options to be updated</param>
        public Client UpdateClient(long clientId, ClientOptions options)
        {
            return Execute<Client>(UpdateClientRequest(clientId, options));
        }

        /// <summary>
        /// Updates a client on the authenticated account. Makes a PUT and a GET request to the Clients resource.
        /// </summary>
        /// <param name="clientId">The ID for the client to update</param>
        /// <param name="options">The options to be updated</param>
        /// <param name="cancellationToken"></param>
        public async Task<Client> UpdateClientAsync(long clientId, ClientOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Client>(UpdateClientRequest(clientId, options), cancellationToken);
        }
    }
}
