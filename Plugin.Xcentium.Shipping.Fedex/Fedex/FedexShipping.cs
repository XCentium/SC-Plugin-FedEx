using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog;
using Sitecore.Commerce.Plugin.Catalog.Cs;
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
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cart"></param>
        /// <param name="getProductCommand"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static decimal GetCartShippingRate(string name, Cart cart, GetProductCommand getProductCommand, CommercePipelineExecutionContext context)
        {
 
             var rates = GetCartShippingRates(cart, getProductCommand, context.GetPolicy<FedExClientPolicy>(), context.CommerceContext);

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


        internal static List<KeyValuePair<string, decimal>> GetCartShippingRates(Cart cart,
             GetProductCommand getProductCommand, FedExClientPolicy fedExClientPolicy, CommerceContext commerceContext)
        {
            var input = new FedexReqestInput();
            FedExClientPolicy = fedExClientPolicy;
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

                input.PriceValue = cart.Totals.SubTotal.Amount;

                decimal height = 0.0M;
                decimal width = 0.0M;
                decimal length = 0.0m;
                decimal weight = 0.0m;

                foreach (var cartLineComponent in cart.Lines)
                {

                    // get specific weight value
                    var productArgument = ProductArgument.FromItemId(cartLineComponent.ItemId);
                    if (!productArgument.IsValid()) continue;
    
                    var product = getProductCommand.Process(commerceContext, productArgument.CatalogName, productArgument.ProductId).Result;

                    decimal val = 0m;
                    if (product != null)
                    {
                        if (product.HasProperty(FedExClientPolicy.WeightFieldName) && product[FedExClientPolicy.WeightFieldName].ToString().Trim() != "")
                            val = GetFirstDecimalFromString(product[FedExClientPolicy.WeightFieldName].ToString());
                        else val = GetFirstDecimalFromString(FedExClientPolicy.Weight);

                        if (val > 0) weight += val;

                        val = product.HasProperty(FedExClientPolicy.HeightFieldName) && product[FedExClientPolicy.HeightFieldName].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product[FedExClientPolicy.HeightFieldName].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Height);

                        if (val > 0) height += val;

                        val = product.HasProperty(FedExClientPolicy.WidthFieldName) && product[FedExClientPolicy.WidthFieldName].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product[FedExClientPolicy.WidthFieldName].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Width);

                        if (val > 0 && val > width) width = val;

                        val = product.HasProperty(FedExClientPolicy.LengthFieldName) && product[FedExClientPolicy.LengthFieldName].ToString().Trim() != ""
                            ? GetFirstDecimalFromString(product[FedExClientPolicy.LengthFieldName].ToString())
                            : GetFirstDecimalFromString(FedExClientPolicy.Length);

                        if (val > 0 && val > length) length = val;

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
