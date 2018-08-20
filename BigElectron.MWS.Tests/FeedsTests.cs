using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BigElectron.MWS.Feeds;
using BigElectron.MWS.Feeds.Feed;
using BigElectron.MWS.Feeds.Model;
using BigElectron.Common;

namespace BigElectron.MWS.Common.Tests
{
	[TestClass]
	public class FeedsTests
	{

		ImmutableCredentials creds;
		// The client application name
		string appName = "CSharpSampleCode";

		// The client application version
		string appVersion = "1.0";
		string serviceURL = "https://mws.amazonservices.com";

		string sellerId = "A2FYRWBB6FC905"; // me
											//string sellerId = "ARA1ZW7ZHL5MQ"; // RC
		string mWSAuthToken = "amzn.mws.10b0d30f-3c9c-fa00-c792-e9142f66a94c"; // me
																			   //string mWSAuthToken = "amzn.mws.c2b0d4ad-e73e-b729-d3a1-b0998fcd6a9f"; // RC
		public FeedsTests()
		{
			var chain = new CredentialProfileStoreChain();
			AWSCredentials awsCredentials;
			chain.TryGetAWSCredentials("DD MWS", out awsCredentials);
			creds = awsCredentials.GetCredentials();
		}

		[TestMethod]
		public void SubmitFeedTestDD()
		{
			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			SubmitFeedRequest submitFeedRequest = new SubmitFeedRequest();
			submitFeedRequest.MWSAuthToken = mWSAuthToken;
			submitFeedRequest.Merchant = sellerId;
			submitFeedRequest.FeedType = "_POST_PRODUCT_PRICING_DATA_";
			AmazonEnvelope priceFeed = PriceFeedBuilder.Build();
			priceFeed.Message.Add(PriceFeedBuilder.BuildMessage());
			priceFeed.Message.First().MessageID = "1";
			priceFeed.Message.First().Price.StandardPrice.Value = 18.00m;
			priceFeed.Message.First().Price.SKU = "8E-5FMM-A9HN"; // Caves of Steel
																  //priceFeed.Message.Add(new Message() { MessageID = "123" });
			priceFeed.Header.MerchantIdentifier = sellerId;
			var stream = Util.GenerateStreamFromXml<AmazonEnvelope>(priceFeed);
			submitFeedRequest.FeedContent = stream;
			submitFeedRequest.ContentMD5 = Util.CalculateContentMD5(stream);
			SubmitFeedResponse submitFeedResponse = service.SubmitFeed(submitFeedRequest);

			//Util.GenerateFromXml<AmazonEnvelope>(priceFeed);

			Console.WriteLine(submitFeedResponse.SubmitFeedResult.FeedSubmissionInfo.FeedSubmissionId);
		}


		[TestMethod]
		public void SubmitFeedTestRedCarpet()
		{
			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			SubmitFeedRequest submitFeedRequest = new SubmitFeedRequest();
			submitFeedRequest.MWSAuthToken = mWSAuthToken;
			submitFeedRequest.Merchant = sellerId;
			submitFeedRequest.FeedType = "_POST_PRODUCT_PRICING_DATA_";
			AmazonEnvelope priceFeed = PriceFeedBuilder.Build();
			Message msg = PriceFeedBuilder.BuildMessage();
			msg.MessageID = "1";
			msg.Price.StandardPrice.Value = 154.40m;
			msg.Price.SKU = "HEWD9P29A";
			priceFeed.Message.Add(msg);

			Message msg2 = PriceFeedBuilder.BuildMessage();
			msg2.MessageID = "2";
			msg2.Price.StandardPrice.Value = 62.05m;
			msg2.Price.SKU = "HEW35S";
			priceFeed.Message.Add(msg2);

			priceFeed.Header.MerchantIdentifier = sellerId;
			var stream = Util.GenerateStreamFromXml<AmazonEnvelope>(priceFeed);

			Util.GenerateXmlFile<AmazonEnvelope>(priceFeed);

			submitFeedRequest.FeedContent = stream;
			submitFeedRequest.ContentMD5 = Util.CalculateContentMD5(stream);
			SubmitFeedResponse submitFeedResponse = service.SubmitFeed(submitFeedRequest);


			Console.WriteLine(submitFeedResponse.SubmitFeedResult.FeedSubmissionInfo.FeedSubmissionId);
		}

		[TestMethod]
		public void GetFeedSubmissionListTest()
		{
			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);


			GetFeedSubmissionListRequest req = new GetFeedSubmissionListRequest();
			req.MWSAuthToken = mWSAuthToken;
			req.Merchant = sellerId;
			var response = service.GetFeedSubmissionList(req);


			foreach (var item in response.GetFeedSubmissionListResult.FeedSubmissionInfo)
			{
				Console.WriteLine(item.FeedSubmissionId);
			}
		}


		[TestMethod]
		public void GetFeedSubmissionResultTest()
		{
			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			GetFeedSubmissionResultRequest req = new GetFeedSubmissionResultRequest();
			req.MWSAuthToken = mWSAuthToken;
			req.Merchant = sellerId;
			req.FeedSubmissionId = "50014017726";


			//50003017583
			//50002017580
			var response = service.GetFeedSubmissionResultAmazonEnvelope(req);

			Console.WriteLine(response.Message.First().ProcessingReport.ProcessingSummary.MessagesSuccessful);
			Console.WriteLine(response.Message.First().ProcessingReport.ProcessingSummary.MessagesWithError);


			if (response.Message.First().ProcessingReport.Result != null)
			{
				Console.WriteLine(response.Message.First().ProcessingReport.Result.ResultCode);
				Console.WriteLine(response.Message.First().ProcessingReport.Result.ResultMessageCode);
				Console.WriteLine(response.Message.First().ProcessingReport.Result.ResultDescription);
			}
		}



		[TestMethod]
		public void RequestReportTest()
		{

			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			string reportType = "_GET_FLAT_FILE_ORDERS_DATA_";

			RequestReportRequest request = new RequestReportRequest();
			request.ReportType = reportType;
			request.Merchant = sellerId;
			request.MWSAuthToken = mWSAuthToken; // Optional
												 //@TODO: set additional request parameters here
			RequestReportResponse response = service.RequestReport(request);

			Assert.IsTrue(response.RequestReportResult.ReportRequestInfo.ReportType == reportType);
			Console.WriteLine("GeneratedReportId: " + response.RequestReportResult.ReportRequestInfo.GeneratedReportId);
		}




		[TestMethod]
		public void GetReportCountTest()
		{

			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			GetReportCountRequest request = new GetReportCountRequest();
			request.Merchant = sellerId;
			request.MWSAuthToken = mWSAuthToken; // Optional
												 //@TODO: set additional request parameters here
			GetReportCountResponse response = service.GetReportCount(request);

			Assert.IsTrue(response.GetReportCountResult.Count > 0);
			Console.WriteLine("Count: " + response.GetReportCountResult.Count);
		}



		[TestMethod] [Ignore]
		public void GetReportTest()
		{

			MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceURL;

			MarketplaceWebService.MarketplaceWebService service =
				new MarketplaceWebServiceClient(
					creds.AccessKey,
					creds.SecretKey,
					appName,
					appVersion,
					config);

			GetReportRequest request = new GetReportRequest();
			request.Merchant = sellerId;
			request.MWSAuthToken = mWSAuthToken;  // Optional
									   //@TODO: set additional request parameters here
			request.ReportId = "123"; 
			GetReportResponse response = service.GetReport(request);
		}
		
	}
	
}
