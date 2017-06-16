HOW TO INSTALL
==============

1. Include in your project

2. Add it as a dependency to your Sitecore Commerce Engine Project.Json file by adding the line below
    "Plugin.Xcentium.Fulfillment": "1.0.2301"

3. Add it as a dependency to your Adventure works or Habitat or Custom Site plugin' project.json file

4. Add the settings below with your Fedex API key to your Sitecore Commerce Engine's environment json files

      {
        "$type": "Plugin.Xcentium.Fulfillment.FedExClientPolicy, Plugin.Xcentium.Fulfillment",
        "Key": "XXXXXXX",
        "Password": "XXXXXXX",
        "AccountNumber": "XXXXXXX",
        "MeterNumber": "XXXXXXX",
        "IntegratorId": "XXXXXXX",
        "ClientProductId": "TEST",
        "ClientProductVersion": "9999",
        "StreetLines": "SHIPPER ADDRESS LINE 1",
        "City": "Austin",
        "StateOrProvinceCode": "TX",
        "PostalCode": "73301",
        "CountryCode": "US",
        "ConnectTimeout": 120000,
        "Weight": "0.2",
        "Width": "2",
        "Length": "2",
        "Height": "2",
        "PolicyId": "FedExClientPolicy",
        "Models": {
          "$type": "System.Collections.Generic.List`1[[Sitecore.Commerce.Core.Model, Sitecore.Commerce.Core]], mscorlib",
          "$values": [
          ]
        }
      },

5. Add the required ship from address and default weight and dimension of a catalog product. The default values will be used when there is no dimension and weight specified in Sitecore for the product item.
