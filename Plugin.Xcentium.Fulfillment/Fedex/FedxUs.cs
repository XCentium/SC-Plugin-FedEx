using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using RateAvailableServiceWebServiceClient.RateServiceWebReference;
using Sitecore.Commerce.Core;

namespace Plugin.Xcentium.Fulfillment.Fedex
{
    public static class FedxUs
    {

        public static FedExClientPolicy FedExClientPolicy = new FedExClientPolicy();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        internal static List<KeyValuePair<string, decimal>> GetShippingRates(FedexReqestInput input, CommercePipelineExecutionContext context)
        {
            FedExClientPolicy = context.GetPolicy<FedExClientPolicy>();

            var request = CreateRateRequest(input);

            var service = new RateService();

            try
            {
                // Call the web service passing in a RateRequest and returning a RateReply
                var reply = service.getRates(request);

                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS ||
                    reply.HighestSeverity == NotificationSeverityType.NOTE ||
                    reply.HighestSeverity == NotificationSeverityType.WARNING)
                {
                    return ShowRateReply(reply);

                }

            }
            catch (SoapException e)
            {
                Console.WriteLine(e.Detail.InnerText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<KeyValuePair<string, decimal>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static RateRequest CreateRateRequest(FedexReqestInput input)
        {
            // Build the RateRequest
            RateRequest request = new RateRequest
            {
                WebAuthenticationDetail = new WebAuthenticationDetail
                {
                    UserCredential = new WebAuthenticationCredential
                    {
                        Key = FedExClientPolicy.Key,
                        Password = FedExClientPolicy.Password
                    }
                }
            };

            request.WebAuthenticationDetail.ParentCredential = new WebAuthenticationCredential
            {
                Key = FedExClientPolicy.Key,
                Password = FedExClientPolicy.Password
            };


            request.ClientDetail = new ClientDetail
            {
                AccountNumber = FedExClientPolicy.AccountNumber,
                MeterNumber = FedExClientPolicy.MeterNumber
            };

            request.TransactionDetail = new TransactionDetail
            {
                CustomerTransactionId = "***Rate Available Services Request using VC#***"
            };
            // This is a reference field for the customer.  Any value can be used and will be provided in the response.
            request.Version = new VersionId();
            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;
            SetShipmentDetails(input, request);
            //
            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="request"></param>
        private static void SetShipmentDetails(FedexReqestInput input, RateRequest request)
        {
            request.RequestedShipment = new RequestedShipment
            {
                ShipTimestamp = DateTime.Now,
                ShipTimestampSpecified = true,
                DropoffType = DropoffType.REGULAR_PICKUP,
                DropoffTypeSpecified = true,
                PackagingType = PackagingType.YOUR_PACKAGING,
                PackagingTypeSpecified = true
            };
            // Shipping date and time
            //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            //
            SetOrigin(request);
            //
            SetDestination(input, request);
            //
            SetPackageLineItems(input, request);
            //
            request.RequestedShipment.PackageCount = "1";
            //set to true to request COD shipment
            var isCodShipment = false;
            if (isCodShipment)
            {
                SetCod(request);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private static void SetOrigin(RateRequest request)
        {
            request.RequestedShipment.Shipper = new RateAvailableServiceWebServiceClient.RateServiceWebReference.Party
            {
                Address = new Address
                {
                    StreetLines = new string[1] { FedExClientPolicy.StreetLine },
                    City = FedExClientPolicy.City,
                    StateOrProvinceCode = FedExClientPolicy.StateOrProvinceCode,
                    PostalCode = FedExClientPolicy.PostalCode,
                    CountryCode = FedExClientPolicy.CountryCode
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="request"></param>
        private static void SetDestination(FedexReqestInput input, RateRequest request)
        {
            request.RequestedShipment.Recipient = new RateAvailableServiceWebServiceClient.RateServiceWebReference.Party
            {
                Address = new Address
                {
                    StreetLines = new string[2] { input.AddressLine1, input.AddressLine2 },
                    City = input.City,
                    StateOrProvinceCode = input.StateCode,
                    PostalCode = input.ZipPostalCode,
                    CountryCode = input.CountryCode
                }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="request"></param>
        private static void SetPackageLineItems(FedexReqestInput input, RateRequest request)
        {
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
            request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem
            {
                SequenceNumber = "1",
                GroupPackageCount = "1",
                Weight = new Weight
                {
                    Units = WeightUnits.LB,
                    UnitsSpecified = true,
                    Value = input.Weight,
                    ValueSpecified = true
                }
            };
            // package weight
            // package dimensions
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions
            {
                Length = input.Length,
                Width = input.Width,
                Height = input.Height,
                Units = LinearUnits.IN,
                UnitsSpecified = true
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        private static void SetCod(RateRequest request)
        {
            // To get all COD rates, set both COD details at both package and shipment level
            // Set COD at Package level for Ground Services
            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested =
                new PackageSpecialServicesRequested { SpecialServiceTypes = new PackageSpecialServiceType[1] };
            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.SpecialServiceTypes[0] = PackageSpecialServiceType.COD;
            //
            request.RequestedShipment.RequestedPackageLineItems[0].SpecialServicesRequested.CodDetail = new CodDetail
            {
                CollectionType = CodCollectionType.GUARANTEED_FUNDS,
                CodCollectionAmount = new RateAvailableServiceWebServiceClient.RateServiceWebReference.Money
                {
                    Amount = 250,
                    AmountSpecified = true,
                    Currency = "USD"
                }
            };
            // Set COD at Shipment level for Express Services
            request.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested
            {
                SpecialServiceTypes = new ShipmentSpecialServiceType[1]
            };
            // Special service requested
            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.COD;
            //
            request.RequestedShipment.SpecialServicesRequested.CodDetail = new CodDetail
            {
                CodCollectionAmount = new RateAvailableServiceWebServiceClient.RateServiceWebReference.Money
                {
                    Amount = 150,
                    AmountSpecified = true,
                    Currency = "USD"
                },
                CollectionType = CodCollectionType.GUARANTEED_FUNDS
            };
            // ANY, CASH, GUARANTEED_FUNDS
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reply"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, decimal>> ShowRateReply(RateReply reply)
        {
            var rates = new List<KeyValuePair<string, decimal>>();

            foreach (var rateReplyDetail in reply.RateReplyDetails)
            {
                if (rateReplyDetail.ServiceTypeSpecified)

                    if (rateReplyDetail.PackagingTypeSpecified)

                        foreach (var shipmentDetail in rateReplyDetail.RatedShipmentDetails)
                        {
                            rates.Add(
                                new KeyValuePair<string, decimal>(
                                    rateReplyDetail.ServiceType.ToString().ToLower().Replace("_", " "),
                                    ShowPackageRateDetails(shipmentDetail.RatedPackages)));
                        }
            }
            return rates;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ratedPackages"></param>
        /// <returns></returns>
        private static decimal ShowPackageRateDetails(RatedPackageDetail[] ratedPackages)
        {
            if (ratedPackages == null) return 0m;
            foreach (var ratedPackage in ratedPackages)
            {
                if (ratedPackage.PackageRateDetail != null)
                {
                    return ratedPackage.PackageRateDetail.NetCharge.Amount;
                }
            }

            return 0m;

        }

    }
}
