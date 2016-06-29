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
        // https://github.com/harvesthq/api/blob/master/Sections/Client%20Contacts.md
        private const string ContactsResource = "contacts";

        private IRestRequest ListContactsRequest(DateTime? updatedSince = null)
        {
            var request = Request(ContactsResource);

            if (updatedSince != null)
                request.AddParameter(UpdatedSinceParameter, updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all contacts for the authenticated account. Makes a GET request to the Contacts resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the contact updated-at property</param>
        public IList<Contact> ListContacts(DateTime? updatedSince = null)
        {
            return Execute<List<Contact>>(ListContactsRequest(updatedSince));
        }

        /// <summary>
        /// List all contacts for the authenticated account. Makes a GET request to the Contacts resource.
        /// </summary>
        /// <param name="updatedSince">An optional filter on the contact updated-at property</param>
        /// <param name="cancellationToken"></param>
        public async Task<IList<Contact>> ListContactsAsync(DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Contact>>(ListContactsRequest(updatedSince), cancellationToken);
        }

        private IRestRequest ListClientContactRequest(long clientId, DateTime? updatedSince = null)
        {
            var request = Request($"{ClientsResource}/{clientId}/{ContactsResource}");

            if (updatedSince != null)
                request.AddParameter("updated_since", updatedSince.Value.ToString("yyyy-MM-dd HH:mm"));

            return request;
        }

        /// <summary>
        /// List all contacts for a client for the authenticated account. Makes a GET request to the Clients/Contacts resource.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="updatedSince">An optional filter on the contact updated-at property</param>
        public IList<Contact> ListClientContacts(long clientId, DateTime? updatedSince = null)
        {
            return Execute<List<Contact>>(ListClientContactRequest(clientId, updatedSince));
        }

        /// <summary>
        /// List all contacts for a client for the authenticated account. Makes a GET request to the Clients/Contacts resource.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="updatedSince">An optional filter on the contact updated-at property</param>
        public async Task<IList<Contact>> ListClientContactsAsync(long clientId, DateTime? updatedSince = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<List<Contact>>(ListClientContactRequest(clientId, updatedSince), cancellationToken);
        }

        private IRestRequest ContactRequest(long contactId)
        {
            return Request($"{ContactsResource}/{contactId}");
        }

        /// <summary>
        /// Retrieve a contact on the authenticated account. Makes a GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The Id of the contact to retrieve</param>
        public Contact Contact(long contactId)
        {
            return Execute<Contact>(ContactRequest(contactId));
        }

        /// <summary>
        /// Retrieve a contact on the authenticated account. Makes a GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The Id of the contact to retrieve</param>
        public async Task<Contact> ContactAsync(long contactId)
        {
            return await ExecuteAsync<Contact>(ContactRequest(contactId));
        }

        private ContactOptions ContactOptions(long clientId, string firstName, string lastName, string title = null,
            string email = null, string phoneOffice = null, string phoneMobile = null, string fax = null)
        {
            if (firstName == null)
                throw new ArgumentNullException(nameof(firstName));

            if (lastName == null)
                throw new ArgumentNullException(nameof(lastName));

            return new ContactOptions()
            {
                ClientId = clientId,
                FirstName = firstName,
                LastName = lastName,
                Title = title,
                Email = email,
                PhoneOffice = phoneOffice,
                PhoneMobile = phoneMobile,
                Fax = fax
            };
        }

        /// <summary>
        /// Create a new contact for a client on the authenticated account. Makes both a POST and a GET request to the Contacts resource.
        /// </summary>
        /// <param name="clientId">The ID of the client this contact is for</param>
        /// <param name="firstName">The first name of the contact</param>
        /// <param name="lastName">The last name of the contact</param>
        /// <param name="title">The contact's title</param>
        /// <param name="email">The contact's email</param>
        /// <param name="phoneOffice">The contact's office phone number</param>
        /// <param name="phoneMobile">The contact's mobile phone number</param>
        /// <param name="fax">The contact's fax number</param>
        public Contact CreateContact(long clientId, string firstName, string lastName, string title = null, string email = null, string phoneOffice = null, string phoneMobile = null, string fax = null)
        {
            return CreateContact(ContactOptions(clientId, firstName, lastName, title, email, phoneOffice, phoneMobile, fax));
        }

        /// <summary>
        /// Create a new contact for a client on the authenticated account. Makes both a POST and a GET request to the Contacts resource.
        /// </summary>
        /// <param name="clientId">The ID of the client this contact is for</param>
        /// <param name="firstName">The first name of the contact</param>
        /// <param name="lastName">The last name of the contact</param>
        /// <param name="title">The contact's title</param>
        /// <param name="email">The contact's email</param>
        /// <param name="phoneOffice">The contact's office phone number</param>
        /// <param name="phoneMobile">The contact's mobile phone number</param>
        /// <param name="fax">The contact's fax number</param>
        public async Task<Contact> CreateContactAsync(long clientId, string firstName, string lastName, string title = null, string email = null, string phoneOffice = null, string phoneMobile = null, string fax = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await CreateContactAsync(ContactOptions(clientId, firstName, lastName, title, email, phoneOffice, phoneMobile, fax), cancellationToken);
        }


        private IRestRequest CreateContactRequest(ContactOptions options)
        {
            var request = Request($"{ContactsResource}", RestSharp.Method.POST);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Creates a new contact under the authenticated account. Makes a POST and a GET request to the Contacts resource.
        /// </summary>
        /// <param name="options">The options for the new contact to be created</param>
        public Contact CreateContact(ContactOptions options)
        {
            return Execute<Contact>(CreateContactRequest(options));
        }

        /// <summary>
        /// Creates a new contact under the authenticated account. Makes a POST and a GET request to the Contacts resource.
        /// </summary>
        /// <param name="options">The options for the new contact to be created</param>
        public async Task<Contact> CreateContactAsync(ContactOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Contact>(CreateContactRequest(options), cancellationToken);
        }

        private IRestRequest DeleteContractRequest(long contactId)
        {
            return Request($"{ContactsResource}/{contactId}", RestSharp.Method.DELETE);
        }

        /// <summary>
        /// Delete a contact from the authenticated account. Makes a DELETE request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to delete</param>
        public bool DeleteContact(long contactId)
        {
            var result = Execute(DeleteContractRequest(contactId));

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a contact from the authenticated account. Makes a DELETE request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to delete</param>
        public async Task<bool> DeleteContactAsync(long contactId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteAsync(DeleteContractRequest(contactId), cancellationToken);

            return result.StatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Update an existing contact on the authenticated account. Makes both a PUT and GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update</param>
        /// <param name="clientId">The ID of the client this contact is for</param>
        /// <param name="firstName">The first name of the contact</param>
        /// <param name="lastName">The last name of the contact</param>
        /// <param name="title">The contact's title</param>
        /// <param name="email">The contact's email</param>
        /// <param name="phoneOffice">The contact's office phone number</param>
        /// <param name="phoneMobile">The contact's mobile phone number</param>
        /// <param name="fax">The contact's fax number</param>
        public Contact UpdateContact(long contactId, long? clientId = null, string firstName = null, string lastName = null, string title = null, string email = null, string phoneOffice = null, string phoneMobile = null, string fax = null)
        {
            return UpdateContact(contactId, ContactOptions(clientId ?? 0, firstName, lastName, title, email, phoneOffice, phoneMobile, fax));
        }

        /// <summary>
        /// Update an existing contact on the authenticated account. Makes both a PUT and GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update</param>
        /// <param name="clientId">The ID of the client this contact is for</param>
        /// <param name="firstName">The first name of the contact</param>
        /// <param name="lastName">The last name of the contact</param>
        /// <param name="title">The contact's title</param>
        /// <param name="email">The contact's email</param>
        /// <param name="phoneOffice">The contact's office phone number</param>
        /// <param name="phoneMobile">The contact's mobile phone number</param>
        /// <param name="fax">The contact's fax number</param>
        public async Task<Contact> UpdateContactAsync(long contactId, long? clientId = null, string firstName = null, string lastName = null, string title = null, string email = null, string phoneOffice = null, string phoneMobile = null, string fax = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateContactAsync(contactId, ContactOptions(clientId ?? 0, firstName, lastName, title, email, phoneOffice, phoneMobile, fax), cancellationToken);
        }

        private IRestRequest UpdateContactRequest(long contactId, ContactOptions options)
        {
            var request = Request($"{ContactsResource}/{contactId}", RestSharp.Method.PUT);

            request.AddBody(options);

            return request;
        }

        /// <summary>
        /// Update an existing contact on the authenticated account. Makes both a PUT and GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update</param>
        /// <param name="options">The update options for the contact</param>
        public Contact UpdateContact(long contactId, ContactOptions options)
        {
            return Execute<Contact>(UpdateContactRequest(contactId, options));
        }

        /// <summary>
        /// Update an existing contact on the authenticated account. Makes both a PUT and GET request to the Contacts resource.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update</param>
        /// <param name="options">The update options for the contact</param>
        public async Task<Contact> UpdateContactAsync(long contactId, ContactOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Contact>(UpdateContactRequest(contactId, options), cancellationToken);
        }
    }
}
