using Harvest.Net.Models;
using System;
using System.Linq;
using Xunit;
using System.Threading.Tasks;


namespace Harvest.Net.Tests
{
    public class AccountFacts : FactBase
    {
        #region Standard Api
        [Fact]
        public void WhoAmI_ReturnsAccountDetails()
        {
            var account = Api.WhoAmI();

            Assert.NotNull(account.User);
            Assert.NotNull(account.Company);
            Assert.NotNull(account.Company.Modules);
            Assert.NotNull(account.User.ProjectManager);

            Assert.Equal(this.Username, account.User.Email);
            Assert.Equal(this.Subdomain + ".harvestapp.com", account.Company.FullDomain);
            Assert.Equal(true, account.User.Admin);
        }
        #endregion

        #region Async Api
        [Fact]
        public async Task WhoAmI_Async_ReturnsAccountDetails()
        {
            var account = await Api.WhoAmIAsync();

            Assert.NotNull(account.User);
            Assert.NotNull(account.Company);
            Assert.NotNull(account.Company.Modules);
            Assert.NotNull(account.User.ProjectManager);

            Assert.Equal(this.Username, account.User.Email);
            Assert.Equal(this.Subdomain + ".harvestapp.com", account.Company.FullDomain);
            Assert.Equal(true, account.User.Admin);
        }
        #endregion
    }
}
