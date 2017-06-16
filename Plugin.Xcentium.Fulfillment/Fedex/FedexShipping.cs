using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using RateAvailableServiceWebServiceClient.RateServiceWebReference;
using Serilog.Core;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Fulfillment;
using CommerceServer.Core.Catalog;

namespace Plugin.Xcentium.Fulfillment.Fedex
{
    public static class FedexShipping
    {
        public static FedExClientPolicy FedExClientPolicy = new FedExClientPolicy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cart"></param>
        /// <param name="getSellableItemPipeline"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static decimal GetCartShippingRate(string name, Cart cart, IGetSellableItemPipeline getSellableItemPipeline, CommercePipelineExecutionContext context)
        {
 
            var input = new FedexReqestInput();
            FedExClientPolicy = context.GetPolicy<FedExClientPolicy>();
            if (cart != null && cart.Lines.Any<CartLineComponent>() && cart.HasComponent<PhysicalFulfillmentComponent>())
            {
                var component = cart.GetComponent<PhysicalFulfillmentComponent>();

                var shippingParty = component?.ShippingParty;

                input.AddressLine1 = shippingParty.Address1;
                input.AddressLine2 = shippingParty.Address2;
                input.City = shippingParty.City;
                input.CountryCode = shippingParty.CountryCode;
                input.StateCode = shippingParty.StateCode;
                input.ZipPostalCode = shippingParty.ZipPostalCode;

                input.PriceValue = cart.Totals.GrandTotal.Amount;

                decimal height = 0.0M;
                decimal width = 0.0M;
                decimal length = 0.0m;
                decimal weight = 0.0m;

                foreach (var cartLineComponent in cart.Lines)
                {

                    // get specific weight value
                    var productArgument = ProductArgument.FromItemId(cartLineComponent.ItemId);
                    if (!productArgument.IsValid()) continue;
                    var sellableItem = getSellableItemPipeline.Run(productArgument, context).Result;
                    var product = context.CommerceContext.Objects.OfType<Product>().FirstOrDefault<Product>((Product p) => p.ProductId.Equals(sellableItem.FriendlyId, StringComparison.OrdinalIgnoreCase));
                    decimal val = 0m;
                    if (product != null)
                    {
                        if (product.HasProperty("Weight") && product["Weight"].ToString().Trim() != "")
                            val = GetFirstDecimalFromString(product["Weight"].ToString());
                        else val = GetFirstDecimalFromString(FedExClientPolicy.Weight);

                        if (val > 0) weight += val;

                        val = product.HasProperty("Height") && product["height"].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product["Height"].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Height);

                        if (val > 0) height += val;

                        val = product.HasProperty("Width") && product["Width"].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product["Width"].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Width);

                        if (val > 0 && val > width) width = val;

                        val = product.HasProperty("Length") && product["Length"].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product["Length"].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Length);

                        if (val > 0 && val > length) length = val;

                    }

                }

                input.Height = Math.Round(height, 0).ToString(CultureInfo.CurrentCulture);
                input.Width = Math.Round(width, 0).ToString(CultureInfo.CurrentCulture);
                input.Length = Math.Round(length, 0).ToString(CultureInfo.CurrentCulture);
                input.Weight = weight;

            }

            var rates = new List<KeyValuePair<string,decimal>>();

            if (input.CountryCode.ToLower() == "us")
            {
                rates = FedxUs.GetShippingRates(input, context);
            }
            else
            {
                rates = FedExInternational.GetShippingRates(input, context);
            }
             

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
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal GetFirstDecimalFromString(string str)
        {
            if (string.IsNullOrEmpty(str)) return 0.00M;
            var decList = Regex.Split(str, @"[^0-9\.]+").Where(c => c != "." && c.Trim() != "").ToList();
            var decimalVal = decList.Any() ? decList.FirstOrDefault() : string.Empty;

            if (string.IsNullOrEmpty(decimalVal)) return 0.00M;
            decimal decimalResult = 0;
            decimal.TryParse(decimalVal, out decimalResult);
            return decimalResult;
        }
    }
}
