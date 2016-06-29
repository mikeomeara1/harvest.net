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
        // https://github.com/harvesthq/api/blob/master/Sections/Accounts.md
        private const string AccountResource = "account";
        private const string WhoAmIAction = "who_am_i";

        private IRestRequest WhoAmIRequest()
        {
            return Request($"{AccountResource}/{WhoAmIAction}");
        }

        /// <summary>
        /// List user and account information for the authenticated account. Makes a GET request to the Account/Who_Am_I resource.
        /// </summary>
        public Account WhoAmI()
        {
            return Execute<Account>(WhoAmIRequest());
        }

        /// <summary>
        /// List user and account information for the authenticated account. Makes a GET request to the Account/Who_Am_I resource.
        /// </summary>
        public async Task<Account> WhoAmIAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteAsync<Account>(WhoAmIRequest(), cancellationToken);
        }
    }
}
