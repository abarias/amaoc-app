﻿using Chevron.ITC.AMAOC.Backend.Helpers;
using System;

#if BACKEND
using Microsoft.Azure.Mobile.Server;
#elif MOBILE
//using MvvmHelpers;
#endif

namespace Chevron.ITC.AMAOC.DataObjects
{
    public interface IBaseDataObject
    {
        string Id { get; set; }
    }

#if BACKEND
    public class BaseDataObject : EntityData
    {
        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string RemoteId { get; set; }
    }
#else
    public class BaseDataObject : ObservableObject, IBaseDataObject
    {
        public BaseDataObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string RemoteId { get; set; }

        [Newtonsoft.Json.JsonProperty("id")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }
    }
#endif
}
