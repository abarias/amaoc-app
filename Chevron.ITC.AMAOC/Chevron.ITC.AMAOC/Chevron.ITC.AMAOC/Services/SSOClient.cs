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

        public async Task<AccountResponse> LoginAsync(string idToken, string accessToken)
        {
            MobileServiceUser user = await storeManager.LoginAsync(accessToken);

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return AccountFromMobileServiceUser(idToken);
        }

        public async Task LogoutAsync()
        {
            await storeManager.LogoutAsync();
        }

        private AccountResponse AccountFromMobileServiceUser(string idToken)
        {            
            IDictionary<string, string> claims = JwtUtility.GetClaims(idToken);

            var account = new AccountResponse();
            account.Success = true;
            account.User = new User
            {
                UserId = claims[JwtClaimNames.Subject],
                FullName = claims[JwtClaimNames.FullName],
                CAI = claims[JwtClaimNames.CAI],
                Email = claims[JwtClaimNames.Email][0].ToString()
            };

            return account;
        }
    }
}
