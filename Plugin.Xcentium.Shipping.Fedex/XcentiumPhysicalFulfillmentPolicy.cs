using System;
using System.Collections.Generic;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Fulfillment;

namespace Plugin.Xcentium.Shipping.Fedex
{
    /// <summary>
    /// 
    /// </summary>
    public class XcentiumPhysicalFulfillmentPolicy : Policy
    {
        /// <summary>
        /// 
        /// </summary>
        public Decimal MaxShippingWeight { get; set; }

        /// <summary>Gets or sets the measurement units.</summary>
        public string MeasurementUnits { get; set; }

        /// <summary>Gets or sets the weight units.</summary>
        public string WeightUnits { get; set; }

        /// <summary>
        /// Gets or sets the default shipping fee for multiple currency types.
        /// </summary>
        public IList<Money> DefaultCartFulfillmentFees { get; set; }

        /// <summary>
        /// Default FulfillmentFee when no currency specific fee is provided
        /// </summary>
        public Money DefaultCartFulfillmentFee { get; set; }

        /// <summary>
        /// Gets or sets the default shipping fee for multiple currency types.
        /// </summary>
        public IList<Money> DefaultItemFulfillmentFees { get; set; }

        /// <summary>
        /// Default FulfillmentFee when no currency specific fee is provided
        /// </summary>
        public Money DefaultItemFulfillmentFee { get; set; }

        /// <summary>Gets or sets the fulfillment fees.</summary>
        /// <value>The fulfillment fees.</value>
        public IList<FulfillmentFee> FulfillmentFees { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public XcentiumPhysicalFulfillmentPolicy()
        {
            this.DefaultCartFulfillmentFee = new Money("USD", new Decimal(5));
            this.DefaultItemFulfillmentFee = new Money("USD", new Decimal(2));
            this.DefaultCartFulfillmentFees = (IList<Money>)new List<Money>();
            this.DefaultItemFulfillmentFees = (IList<Money>)new List<Money>();
            this.FulfillmentFees = (IList<FulfillmentFee>)new List<FulfillmentFee>();

        }
    }
}
