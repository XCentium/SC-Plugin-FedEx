namespace Plugin.Xcentium.Shipping.Fedex.Fedex
{
    /// <summary>
    /// 
    /// </summary>
    public class FedexReqestInput
    {

        /// <summary>
        /// 
        /// </summary>
        public decimal Weight { get; set; }

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
        public decimal PriceValue { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string StateCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ZipPostalCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressLine2 { get; set; }

    }
}
