using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Shipping.Fedex
{

    /// <summary>
    /// 
    /// </summary>
    public class FedExClientPolicy : Policy
    {
        /// <summary>
        /// 
        /// </summary>
        public FedExClientPolicy()
        {
            // Fedex Keys
            Key = string.Empty;
            Password = string.Empty;
            AccountNumber = string.Empty;
            MeterNumber = string.Empty;
            IntegratorId = string.Empty;
            ClientProductId = string.Empty;
            ClientProductVersion = string.Empty;

            // ship from address
            StreetLine = string.Empty;
            City = string.Empty;
            StateOrProvinceCode = string.Empty;
            PostalCode = string.Empty;
            CountryCode = string.Empty;

            Length = string.Empty;
            Width = string.Empty;
            Height = string.Empty;
            Weight = string.Empty;

            CompensationMultipier = 1m;

        }

        /// <summary>
        /// Fedex API Key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Fedex API Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Fedex API Account number
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Fedex API Meter number
        /// </summary>
        public string MeterNumber { get; set; }

        /// <summary>
        /// Fedex API Integrator ID
        /// </summary>
        public string IntegratorId { get; set; }

        /// <summary>
        /// Fedex API Client product ID
        /// </summary>
        public string ClientProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientProductVersion { get; set; }

        // ship from address
        /// <summary>
        /// Ship from address line 1
        /// </summary>
        public string StreetLine { get; set; }

        /// <summary>
        /// Ship from city
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Ship from state or province
        /// </summary>
        public string StateOrProvinceCode { get; set; }

        /// <summary>
        /// Ship from postal or zip code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Ship from country code
        /// </summary>
        public string CountryCode { get; set; }

        
        /// <summary>
        /// Default value for Cart item lemgth if not set in catalog
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// Default value for Cart item width if not set in catalog
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Default value for Cart item height if not set in catalog
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Default value for Cart item weight if not set in catalog
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// This multiplies the cart total with an inconvenience factor incase postage needs compensation paid out.
        /// </summary>
        public decimal CompensationMultipier { get; set; }

    }


}
