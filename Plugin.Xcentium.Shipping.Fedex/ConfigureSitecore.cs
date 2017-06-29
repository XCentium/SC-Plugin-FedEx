using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Plugin.Xcentium.Shipping.Fedex.Pipelines.Blocks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Plugin.Carts;
using Sitecore.Commerce.Plugin.Fulfillment;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;
using Sitecore.Framework.Pipelines;


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

            Action<SitecorePipelinesConfigBuilder> actionDelegate = c => c
                    // Add the plugin code block so that it runs when the site is bootstrapped
                    .ConfigurePipeline<IBootstrapPipeline>(d => { d.Add<ChangeFulfillmentOptionsBlock>(); })
                    // This is how you can add the code block to run after a known code block has a modified the input. Same applies to Before
                    .ConfigurePipeline<ICalculateCartLinesPipeline>(
                        d =>
                        {
                            d.Add<UpdateCartLinesFulfillmentChargeBlock>().After<CalculateCartLinesFulfillmentBlock>();
                        })
                    .ConfigurePipeline<ICalculateCartPipeline>(
                        d => { d.Add<UpdateCartFulfillmentChargeBlock>().After<CalculateCartFulfillmentBlock>(); });
            services.Sitecore().Pipelines(actionDelegate);

        }
    }
}
