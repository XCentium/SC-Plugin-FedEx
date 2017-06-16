using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Fulfillment;

namespace Plugin.Xcentium.Fulfillment
{
    public class XcentiumPhysicalFulfillmentPolicy : Policy
    {

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
