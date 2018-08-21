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
using BigElectron.MWS.Handlers;
using Moq;
using NLog;

namespace BigElectron.MWS.Common.Tests
{
	[TestClass]
	public class ReportHandlerTests
	{
		ServiceContext serviceContext = new ServiceContext();
		Mock<ILogger> mockLogger;
		ImmutableCredentials creds;
		// The client application name
		string appName = "CSharpSampleCode";

		// The client application version
		string appVersion = "1.0";
		string serviceURL = "https://mws.amazonservices.com";

		string sellerId = "A2FYRWBB6FC905"; // me
		string mWSAuthToken = ""; // me


		ReportHandler reportHandler;


		public ReportHandlerTests()
		{
			var chain = new CredentialProfileStoreChain();
			AWSCredentials awsCredentials;
			chain.TryGetAWSCredentials("DD MWS", out awsCredentials);
			creds = awsCredentials.GetCredentials();

			serviceContext.MwsAuthToken = mWSAuthToken;
			serviceContext.AppVersion = appVersion;
			serviceContext.AccessKey = creds.AccessKey;
			serviceContext.SecretKey = creds.SecretKey;
			serviceContext.SellerId = sellerId;
			serviceContext.ApplicationName = "test";
			serviceContext.MwsServiceUrl = serviceURL;

			mockLogger = new Mock<ILogger>();
			reportHandler = new ReportHandler(serviceContext, mockLogger.Object);
		}

		[TestMethod]
		public void RequestReportTest()
		{

			var response = reportHandler.RequestReport(ReportType._GET_FLAT_FILE_ORDERS_DATA_, DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(1));

			Assert.IsTrue(response.ReportRequestInfo.ReportType == ReportType._GET_FLAT_FILE_ORDERS_DATA_);
			Console.WriteLine("GeneratedReportId: " + response.ReportRequestInfo.GeneratedReportId);
			Console.WriteLine("ReportRequestId: " + response.ReportRequestInfo.ReportRequestId); 
		}


		[TestMethod]
		public void GetReportCountTest()
		{
			GetReportCountResult response = reportHandler.GetReportCount();

			Assert.IsTrue(response.Count > 0);
			Console.WriteLine("Count: " + response.Count);
		}


		[TestMethod]
		public void GetReportRequestListTest()
		{
			GetReportRequestListResult response = reportHandler.GetReportRequestList();

			Assert.IsTrue(response.ReportRequestInfo.Count > 0);

			foreach (var item in response.ReportRequestInfo)
			{
				Console.WriteLine("ReportRequestId: " + item.ReportRequestId);
				Console.WriteLine("ReportType: " + item.ReportType);
				Console.WriteLine("GeneratedReportId: " + item.GeneratedReportId);
				Console.WriteLine("SubmittedDate: " + item.SubmittedDate);
				Console.WriteLine("ReportProcessingStatus: " + item.ReportProcessingStatus);
				Console.WriteLine("---------------------------------------------- ");
			}
			//50033017756
		}


		[TestMethod] 
		public void GetReportTest()
		{

 
			GetReportResult response = reportHandler.GetReport("10772964819017755");

			Console.Write(response.Content); 
			Assert.IsTrue(response.Content.Contains("sku"));
		}


		[TestMethod]
		public void GetReportRequestInfoStatus_ReportFailed()
		{

			ReportRequestInfo reportRequestInfo = new ReportRequestInfo();
			reportRequestInfo.ReportRequestId = "";
			reportRequestInfo.ReportProcessingStatus = "_CANCELLED_";
			
			ReportRequestInfoStatus status =  reportHandler.GetReportRequestInfoStatus(reportRequestInfo);

			Console.Write(status.ToString());
			Assert.AreEqual(ReportRequestInfoStatus.ReportFailed, status);
		}

		[TestMethod]
		public void GetFilePathTest()
		{
			string response = reportHandler.CreateFileLocation("c:/temp", "TYPE", DateTime.Now.AddDays(-1), DateTime.Now);

			Console.Write(response);

		}

	}
	
}
