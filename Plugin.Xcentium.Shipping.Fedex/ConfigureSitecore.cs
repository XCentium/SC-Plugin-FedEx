using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Xcentium.Shipping.Fedex.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Plugin.Xcentium.Shipping.Fedex
{
    /// <summary>Defines the fulfillment ConfigureSitecore class.</summary>
    /// <seealso cref="T:Sitecore.Framework.Configuration.IConfigureSitecore" />
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>The configure services.</summary>
        /// <param name="services">The services.</param>

        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<IBootstrapPipeline>(d =>
                {
                    d.Add<ChangeFulfillmentOptionsBlock>();
                })
                .ConfigurePipeline<ICalculateCartLinesPipeline>(d =>
                {
                    d.Add<UpdateCartLinesFulfillmentChargeBlock>().After<CalculateCartLinesFulfillmentBlock>();
                })
                .ConfigurePipeline<ICalculateCartPipeline>(d =>
                {
                    d.Add<UpdateCartFulfillmentChargeBlock>().After<CalculateCartFulfillmentBlock>();
                })
            );

            services.RegisterAllCommands(assembly);

        }
    }
}
