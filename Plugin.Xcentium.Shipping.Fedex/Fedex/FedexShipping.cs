using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Fulfillment;

namespace Plugin.Xcentium.Shipping.Fedex.Fedex
{

    /// <summary>
    /// 
    /// </summary>
    public static class FedexShipping
    {

        /// <summary>
        /// 
        /// </summary>
        public static FedExClientPolicy FedExClientPolicy = new FedExClientPolicy();

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cart"></param>
        /// <param name="getSellableItemPipeline"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static decimal GetCartShippingRate(string name, Cart cart, IGetSellableItemPipeline getSellableItemPipeline,  CommercePipelineExecutionContext context)
        {
 
             var rates = GetCartShippingRates(cart, context.GetPolicy<FedExClientPolicy>(), getSellableItemPipeline, context.CommerceContext);

            if (rates == null || !rates.Any()) return 0m;
            try
            {
                return rates.FirstOrDefault(x => x.Key == name.ToLower()).Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0m;
        }

        /// <summary>
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="fedExClientPolicy"></param>
        /// <param name="getSellableItemPipeline"></param>
        /// <param name="commerceContext"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<string, decimal>> GetCartShippingRates(Cart cart,
              FedExClientPolicy fedExClientPolicy, IGetSellableItemPipeline getSellableItemPipeline, CommerceContext commerceContext)
        {
            var input = new FedexReqestInput();
            FedExClientPolicy = fedExClientPolicy;
            if (cart != null && cart.Lines.Any<CartLineComponent>() && cart.HasComponent<PhysicalFulfillmentComponent>())
            {
                var component = cart.GetComponent<PhysicalFulfillmentComponent>();

                var shippingParty = component?.ShippingParty;

                input.AddressLine1 = shippingParty?.Address1;
                input.AddressLine2 = shippingParty?.Address2;
                input.City = shippingParty?.City;
                input.CountryCode = shippingParty?.CountryCode;
                input.StateCode = shippingParty?.StateCode;
                input.ZipPostalCode = shippingParty?.ZipPostalCode;

                input.PriceValue = cart.Totals.SubTotal.Amount;

                var height = 0.0M;
                var width = 0.0M;
                var length = 0.0m;
                var weight = 0.0m;

                foreach (var cartLineComponent in cart.Lines)
                {

                    // get specific weight value
                    var productArgument = ProductArgument.FromItemId(cartLineComponent.ItemId);
                    if (!productArgument.IsValid()) continue;

                    var sellableItem = getSellableItemPipeline.Run(productArgument, commerceContext.GetPipelineContextOptions()).Result;

                    if (sellableItem != null && sellableItem.HasComponent<ItemSpecificationsComponent>())
                    {
                        var itemSpec = sellableItem.GetComponent<ItemSpecificationsComponent>();

                        if (itemSpec.Weight > 0) weight += itemSpec.Weight;

                        if (itemSpec.Height > 0) height += itemSpec.Height;

                        if (itemSpec.Width > 0 && itemSpec.Width > width) width = itemSpec.Width;

                        if (itemSpec.Length > 0 && itemSpec.Length > length) length = itemSpec.Length;
                    }

                }

                input.Height = Math.Ceiling(height).ToString(CultureInfo.CurrentCulture);
                input.Width = Math.Ceiling(width).ToString(CultureInfo.CurrentCulture);
                input.Length = Math.Ceiling(length).ToString(CultureInfo.CurrentCulture);
                input.Weight = Math.Ceiling(weight);

            }

            var rates = new List<KeyValuePair<string, decimal>>();

            if (input.CountryCode.ToLower() == "us")
            {
                rates = FedxUs.GetShippingRates(input, FedExClientPolicy);
            }
            else
            {
                rates = FedExInternational.GetShippingRates(input, FedExClientPolicy);
            }


            return rates;
        }


    }
}
