using Chevron.ITC.AMAOC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chevron.ITC.AMAOC.Models;
using Chevron.ITC.AMAOC.Abstractions;
using Xamarin.Forms;

namespace Chevron.ITC.AMAOC.MockStores
{
    public class SSOClient : ISSOClient
    {
        private readonly IStoreManager storeManager;

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
            return new AccountResponse
            {
                Success = true,
                Token = "ThisIsTestToken",
                User = new User
                {
                    CAI = "BOPD",
                    Email = "ABArias@chevron.com",
                    FullName = "Anthony Bernard C. Arias",
                    UserId = "001"
                }
            };
        }
        

        public async Task LogoutAsync()
        {
            Chevron.ITC.AMAOC.Helpers.Settings.UserId = string.Empty;
            Chevron.ITC.AMAOC.Helpers.Settings.AuthToken = string.Empty;
            Chevron.ITC.AMAOC.Helpers.Settings.Current.CAI = string.Empty;
            Chevron.ITC.AMAOC.Helpers.Settings.Current.FullName = string.Empty;
            Chevron.ITC.AMAOC.Helpers.Settings.Current.Email = string.Empty;            
        }
    }
}
