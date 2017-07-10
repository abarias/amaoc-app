using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Models
{
    public class User
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string CAI { get; set; }

    }

    public class AccountResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }

    [Flags]
    public enum Access
    {
        None = 0,
        Admin = 1,
        Write = 1 << 1,
        Read = 1 << 2,
    }
}
