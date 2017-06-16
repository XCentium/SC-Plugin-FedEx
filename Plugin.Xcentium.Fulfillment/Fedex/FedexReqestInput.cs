using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.Xcentium.Fulfillment.Fedex
{
    public class FedexReqestInput
    {
        public decimal Weight { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }

        public decimal PriceValue { get; set; }

        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string ZipPostalCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

    }
}
