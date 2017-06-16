using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Fulfillment
{
    public class FedExClientPolicy : Policy
    {

        public FedExClientPolicy()
        {
            // Fedex Keys
            this.Key = string.Empty;
            this.Password = string.Empty;
            this.AccountNumber = string.Empty;
            this.MeterNumber = string.Empty;
            this.IntegratorId = string.Empty;
            this.ClientProductId = string.Empty;
            this.ClientProductVersion = string.Empty;

            // ship from address
            this.StreetLine = string.Empty;
            this.City = string.Empty;
            this.StateOrProvinceCode = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryCode = string.Empty;

            this.Length = string.Empty;
            this.Width = string.Empty;
            this.Height = string.Empty;
            this.Weight = string.Empty;

        }

        public string Key { get; set; }
        public string Password { get; set; }
        public string AccountNumber { get; set; }
        public string MeterNumber { get; set; }
        public string IntegratorId { get; set; }
        public string ClientProductId { get; set; }
        public string ClientProductVersion { get; set; }

        // ship from address
        public string StreetLine { get; set; }
        public string City { get; set; }
        public string StateOrProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }

        // Cart item dimension and weight if not set in catalog
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }

    }
}
