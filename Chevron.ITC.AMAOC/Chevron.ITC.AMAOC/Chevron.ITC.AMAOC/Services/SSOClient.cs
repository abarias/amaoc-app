using Chevron.ITC.AMAOC.Abstractions;
using Chevron.ITC.AMAOC.Interfaces;
using Chevron.ITC.AMAOC.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var account = new AccountResponse();
            try
            {
                IDictionary<string, object> claims = JwtUtility.GetClaims(idToken);

                var emailArray = (JArray)claims[JwtClaimNames.Email];

                account.Success = true;
                account.User = new User
                {
                    UserId = claims[JwtClaimNames.Subject].ToString(),
                    FullName = claims[JwtClaimNames.FullName].ToString(),
                    CAI = claims[JwtClaimNames.CAI].ToString(),
                    Email = emailArray.ToObject<string[]>()[0].ToString()
                };
            }
            catch (Exception jex)
            {

            }                                   

            return account;
        }
    }
}
