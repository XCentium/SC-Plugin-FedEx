using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Shipping.Fedex.Policies
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

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MeterNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IntegratorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientProductVersion { get; set; }

        // ship from address
        /// <summary>
        /// 
        /// </summary>
        public string StreetLine { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StateOrProvinceCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }

        // Cart item dimension and weight if not set in catalog
        /// <summary>
        /// 
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Weight { get; set; }

    }
}
