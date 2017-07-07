using Chevron.ITC.AMAOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Interfaces
{
    public interface ISSOClient
    {
        Task<AccountResponse> LoginAsync(string username, string password);

        Task LogoutAsync();
    }
}
