/*******************************************************************************
 * Copyright 2009-2017 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Marketplace Web Service Products
 * API Version: 2011-10-01
 * Library Version: 2017-03-22
 * Generated: Wed Mar 22 23:24:36 UTC 2017
 */

using MarketplaceWebServiceProducts.Model;
using System;
using System.Collections.Generic;
using Amazon;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using BigElectron.Common;

namespace MarketplaceWebServiceProducts {

    /// <summary>
    /// Runnable sample code to demonstrate usage of the C# client.
    ///
    /// To use, import the client source as a console application,
    /// and mark this class as the startup object. Then, replace
    /// parameters below with sensible values and run.
    /// </summary>
    public class MarketplaceWebServiceProductsSample {

        public static void Main(string[] args)
        {
			var chain = new CredentialProfileStoreChain();
			AWSCredentials awsCredentials;
			chain.TryGetAWSCredentials("RedCarpet MWS", out awsCredentials);
			var creds = awsCredentials.GetCredentials();


			// Developer AWS access key
			string accessKey = creds.AccessKey;

			// Developer AWS secret key
			string secretKey = creds.SecretKey;

			// The client application name
			string appName = "CSharpSampleCode";

			// The client application version
			string appVersion = "1.0";


			// The endpoint for region service and version (see developer guide)
			// ex: https://mws.amazonservices.com
			string serviceURL = "https://mws.amazonservices.com";

			// Create a configuration object
			MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            IMarketplaceWebServiceProducts client = new MarketplaceWebServiceProductsClient(appName, appVersion, accessKey, secretKey, config);

            MarketplaceWebServiceProductsSample sample = new MarketplaceWebServiceProductsSample(client);

            // Uncomment the operation you'd like to test here
            // TODO: Modify the request created in the Invoke method to be valid

            try 
            {
                IMWSResponse response = null;
                //response = sample.InvokeGetCompetitivePricingForASIN();
                // response = sample.InvokeGetCompetitivePricingForSKU();
                // response = sample.InvokeGetLowestOfferListingsForASIN();
                // response = sample.InvokeGetLowestOfferListingsForSKU();
                 response = sample.InvokeGetLowestPricedOffersForASIN();
                // response = sample.InvokeGetLowestPricedOffersForSKU();
                // response = sample.InvokeGetMatchingProduct();
                // response = sample.InvokeGetMatchingProductForId();
                // response = sample.InvokeGetMyFeesEstimate();
                // response = sample.InvokeGetMyPriceForASIN();
                // response = sample.InvokeGetMyPriceForSKU();
                // response = sample.InvokeGetProductCategoriesForASIN();
                // response = sample.InvokeGetProductCategoriesForSKU();
                // response = sample.InvokeGetServiceStatus();
                // response = sample.InvokeListMatchingProducts();
                Console.WriteLine("Response:");
                ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
                // We recommend logging the request id and timestamp of every call.
                Console.WriteLine("RequestId: " + rhmd.RequestId);
                Console.WriteLine("Timestamp: " + rhmd.Timestamp);
				Util.GenerateXmlFile<GetLowestPricedOffersForASINResponse>((GetLowestPricedOffersForASINResponse)response);
                string responseXml = response.ToXML();
                Console.WriteLine(responseXml);
            }
            catch (MarketplaceWebServiceProductsException ex)
            {
                // Exception properties are important for diagnostics.
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
                Console.WriteLine("Service Exception:");
                if(rhmd != null)
                {
                    Console.WriteLine("RequestId: " + rhmd.RequestId);
                    Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                }
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("StatusCode: " + ex.StatusCode);
                Console.WriteLine("ErrorCode: " + ex.ErrorCode);
                Console.WriteLine("ErrorType: " + ex.ErrorType);
                throw ex;
            }
        }

        private readonly IMarketplaceWebServiceProducts client;
		string sellerId = "ARA1ZW7ZHL5MQ";
		string marketplaceId = "ATVPDKIKX0DER";
		string mwsAuthToken = "amzn.mws.c2b0d4ad-e73e-b729-d3a1-b0998fcd6a9f";
		public MarketplaceWebServiceProductsSample(IMarketplaceWebServiceProducts client)
        {
            this.client = client;
        }

        public GetCompetitivePricingForASINResponse InvokeGetCompetitivePricingForASIN()
        {
            // Create a request.
            GetCompetitivePricingForASINRequest request = new GetCompetitivePricingForASINRequest();
            
            request.SellerId = sellerId;
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
			//ASINIdentifier asin = new ASINIdentifier(marketplaceId, "B00005JC8K");
			asinList.ASIN.Add("B01LJUR6TC");

			return this.client.GetCompetitivePricingForASIN(request);
        }

        public GetCompetitivePricingForSKUResponse InvokeGetCompetitivePricingForSKU()
        {
            // Create a request.
            GetCompetitivePricingForSKURequest request = new GetCompetitivePricingForSKURequest();
            
            request.SellerId = sellerId;
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            return this.client.GetCompetitivePricingForSKU(request);
        }

        public GetLowestOfferListingsForASINResponse InvokeGetLowestOfferListingsForASIN()
        {
            // Create a request.
            GetLowestOfferListingsForASINRequest request = new GetLowestOfferListingsForASINRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            string itemCondition = "new";
            request.ItemCondition = itemCondition;
            bool excludeMe = true;
            request.ExcludeMe = excludeMe;
            return this.client.GetLowestOfferListingsForASIN(request);
        }

        public GetLowestOfferListingsForSKUResponse InvokeGetLowestOfferListingsForSKU()
        {
            // Create a request.
            GetLowestOfferListingsForSKURequest request = new GetLowestOfferListingsForSKURequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            string itemCondition = "new";
            request.ItemCondition = itemCondition;
            bool excludeMe = true;
            request.ExcludeMe = excludeMe;
            return this.client.GetLowestOfferListingsForSKU(request);
        }

        public GetLowestPricedOffersForASINResponse InvokeGetLowestPricedOffersForASIN()
        {
            // Create a request.
            GetLowestPricedOffersForASINRequest request = new GetLowestPricedOffersForASINRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string asin = "B0038G25VK";
            request.ASIN = asin;
            string itemCondition = "new";
            request.ItemCondition = itemCondition;
            return this.client.GetLowestPricedOffersForASIN(request);
        }

        public GetLowestPricedOffersForSKUResponse InvokeGetLowestPricedOffersForSKU()
        {
            // Create a request.
            GetLowestPricedOffersForSKURequest request = new GetLowestPricedOffersForSKURequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string sellerSKU = "example";
            request.SellerSKU = sellerSKU;
            string itemCondition = "new";
            request.ItemCondition = itemCondition;
            return this.client.GetLowestPricedOffersForSKU(request);
        }

        public GetMatchingProductResponse InvokeGetMatchingProduct()
        {
            // Create a request.
            GetMatchingProductRequest request = new GetMatchingProductRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            return this.client.GetMatchingProduct(request);
        }

        public GetMatchingProductForIdResponse InvokeGetMatchingProductForId()
        {
            // Create a request.
            GetMatchingProductForIdRequest request = new GetMatchingProductForIdRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string idType = "ASIN";
            request.IdType = idType;
            IdListType idList = new IdListType();
			idList.Id = new List<string>();
			idList.Id.Add("B0038G25VK");
            request.IdList = idList;
            return this.client.GetMatchingProductForId(request);
        }

		public GetMyFeesEstimateResponse InvokeGetMyFeesEstimate()
		{
			// Create a request.
			GetMyFeesEstimateRequest request = new GetMyFeesEstimateRequest();
			FeesEstimateRequestList list = new FeesEstimateRequestList();
			MoneyType price = new MoneyType() { Amount = 100m, CurrencyCode = "USD" };
			MoneyType ship = new MoneyType() { Amount = 10m, CurrencyCode = "USD" };
			PriceToEstimateFees priceToEstimateFees = new PriceToEstimateFees() { ListingPrice = price, Shipping = ship };
			list.FeesEstimateRequest.Add(new FeesEstimateRequest()
			{ Identifier = "123", IdType = "ASIN", IdValue = "B01LWB4SVH",
				IsAmazonFulfilled = true, MarketplaceId = marketplaceId,
				PriceToEstimateFees = priceToEstimateFees
				});
			request.FeesEstimateRequestList = list;

			request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
  
            return this.client.GetMyFeesEstimate(request);
        }

        public GetMyPriceForASINResponse InvokeGetMyPriceForASIN()
        {
            // Create a request.
            GetMyPriceForASINRequest request = new GetMyPriceForASINRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            return this.client.GetMyPriceForASIN(request);
        }

        public GetMyPriceForSKUResponse InvokeGetMyPriceForSKU()
        {
            // Create a request.
            GetMyPriceForSKURequest request = new GetMyPriceForSKURequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            return this.client.GetMyPriceForSKU(request);
        }

        public GetProductCategoriesForASINResponse InvokeGetProductCategoriesForASIN()
        {
            // Create a request.
            GetProductCategoriesForASINRequest request = new GetProductCategoriesForASINRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string asin = "example";
            request.ASIN = asin;
            return this.client.GetProductCategoriesForASIN(request);
        }

        public GetProductCategoriesForSKUResponse InvokeGetProductCategoriesForSKU()
        {
            // Create a request.
            GetProductCategoriesForSKURequest request = new GetProductCategoriesForSKURequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string sellerSKU = "example";
            request.SellerSKU = sellerSKU;
            return this.client.GetProductCategoriesForSKU(request);
        }

        public GetServiceStatusResponse InvokeGetServiceStatus()
        {
            // Create a request.
            GetServiceStatusRequest request = new GetServiceStatusRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            return this.client.GetServiceStatus(request);
        }

        public ListMatchingProductsResponse InvokeListMatchingProducts()
        {
            // Create a request.
            ListMatchingProductsRequest request = new ListMatchingProductsRequest();
            
            request.SellerId = sellerId;
            
            request.MWSAuthToken = mwsAuthToken;
            
            request.MarketplaceId = marketplaceId;
            string query = "example";
            request.Query = query;
            string queryContextId = "example";
            request.QueryContextId = queryContextId;
            return this.client.ListMatchingProducts(request);
        }


    }
}
