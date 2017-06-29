using System.Linq;
using System.Threading.Tasks;
using Plugin.Xcentium.Shipping.Fedex.Fedex;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Catalog.Cs;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Pipelines;

namespace Plugin.Xcentium.Shipping.Fedex.Pipelines.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateCartFulfillmentChargeBlock : PipelineBlock<Cart, Cart, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly GetProductCommand _getProductCommand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="getProductCommand"></param>
        public UpdateCartFulfillmentChargeBlock(GetProductCommand getProductCommand)
        {
            _getProductCommand = getProductCommand;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<Cart> Run(Cart arg, CommercePipelineExecutionContext context)
        {
            var adjustments = arg.Adjustments;

            if (adjustments == null || !adjustments.Any()) return await Task.FromResult(arg);


            var fulfillmentComponent = arg.GetComponent<PhysicalFulfillmentComponent>();

            var postageSelection = fulfillmentComponent.FulfillmentMethod.Name;

            var postalPrice = FedexShipping.GetCartShippingRate(postageSelection, arg, _getProductCommand, context);


            var currency = context.CommerceContext.CurrentCurrency();

            var money = new Money(currency, postalPrice);
            arg.Adjustments[0].Adjustment = money;
            


            return await Task.FromResult(arg);
        }
    }
}
