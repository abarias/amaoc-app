using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Chevron.ITC.AMAOC.Droid
{
    public class DataRefreshServiceBinder : Binder
    {
        DataRefreshService service;

        public DataRefreshServiceBinder(DataRefreshService service)
        {
            this.service = service;
        }

        public DataRefreshService GetDemoService()
        {
            return service;
        }
    }
}