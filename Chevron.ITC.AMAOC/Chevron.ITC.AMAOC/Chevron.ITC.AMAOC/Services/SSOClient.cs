using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.Services
{
    public class SSOClient : ISSOClient
    {
        private readonly StoreManager storeManager;

        public SSOClient(StoreManager storeManager)
        {
            if (storeManager == null)
            {
                throw new ArgumentNullException(nameof(storeManager));
            }

            this.storeManager = storeManager;
        }

        public SSOClient()
        {
            storeManager = DependencyService.Get<IStoreManager>() as StoreManager;

            if (storeManager == null)
            {
                throw new InvalidOperationException($"The {typeof(SSOClient).FullName} requires a {typeof(StoreManager).FullName}.");
            }
        }

        public async Task<AccountResponse> LoginAsync(string accessToken)
        {
            MobileServiceUser user = await storeManager.LoginAsync(accessToken);

            return AccountFromMobileServiceUser(user);
        }

        public async Task LogoutAsync()
        {
            await storeManager.LogoutAsync();
        }

        private AccountResponse AccountFromMobileServiceUser(MobileServiceUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IDictionary<string, string> claims = JwtUtility.GetClaims(user.MobileServiceAuthenticationToken);

            var account = new AccountResponse();
            account.Success = true;
            account.User = new User
            {
                UserId = claims[JwtClaimNames.Subject]
                //FullName = claims[JwtClaimNames.FullName],
                //CAI = claims[JwtClaimNames.CAI]
            };

            return account;
        }
    }
}
