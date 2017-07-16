using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chevron.ITC.AMAOC.Interfaces
{
    public interface IPushNotifications
    {
        Task<bool> RegisterForNotifications();

        bool IsRegistered { get; }

        void OpenSettings();
    }
}
