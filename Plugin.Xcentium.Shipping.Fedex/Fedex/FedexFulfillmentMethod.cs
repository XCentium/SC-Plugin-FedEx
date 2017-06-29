using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Shipping.Fedex.Fedex
{
    /// <summary>
    /// 
    /// </summary>
    public class FedexFulfillmentMethod : CommerceEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Price { get; set; }

    }
}
