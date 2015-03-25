using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NKCraddock.AmazonItemLookup.Client;

namespace BargainHunter.Helpers
{
    public class AmazonHelper
    {
        private static readonly String AwsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        private static readonly String AwsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");
        private static readonly String AwsAssociateTag = Environment.GetEnvironmentVariable("AWS_ASSOCIATE_TAG");

        public double? GetPriceByAsin(String asin)
        {
            return GetClient().ItemLookupByAsin(asin).OfferPrice;
        }

        private static AwsProductApiClient GetClient()
        {

            var client = new AwsProductApiClient(new ProductApiConnectionInfo
            {
                AWSAccessKey = AwsAccessKey,
                AWSSecretKey = AwsSecretKey,
                AWSAssociateTag = AwsAssociateTag,
                AWSServerUri = "webservices.amazon.co.uk"
            });

            return client;
        }
    }
}