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
using BigElectron.MWS.Handlers.Reports;
using System.Data;
using System.Collections.Generic;

namespace BigElectron.MWS.Common.Tests
{
	[TestClass]
	public class ReportBuilderTests
	{
 
		public ReportBuilderTests()
		{
			 
		}


		[TestMethod]
		public void JoinInventoryAndOrdersTest()
		{
			string inventory = File.ReadAllText(@"report_samples\_GET_FBA_MYI_ALL_INVENTORY_DATA_.txt");
			DataTable inventoryTable = Util.StringToDataTable(inventory);

			string orders = File.ReadAllText(@"report_samples\_GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_.txt");
			DataTable ordersTable = Util.StringToDataTable(orders);

			ReportBuilder reportBuilder = new ReportBuilder();


			var report = reportBuilder.JoinInventoryAndOrders(inventoryTable, ordersTable, DateTime.Parse("7/1/2018"));


			foreach (var item in report)
			{
				Console.WriteLine(item.ASIN);
				Console.WriteLine(item.TotalInventory);
				Console.WriteLine(item.UnfulfillableInventory);
				Console.WriteLine(item.ReservedInventory);
				Console.WriteLine(item.InboundInventory);
				Console.WriteLine(item.MonthlySales);
				Console.WriteLine(item.MonthYear);
			}
			
			Assert.IsNotNull(report);

			var inventoryitem = report.First(r => r.ASIN == "B01B93T49O");

			Assert.AreEqual("B01B93T49O", inventoryitem.ASIN);
			Assert.AreEqual( 23, inventoryitem.MonthlySales);
			Assert.AreEqual("072018", inventoryitem.MonthYear);

			Assert.IsTrue(0 < report.Count());
			Assert.AreEqual(13, report.Count());
			Assert.AreEqual(1, report.First().ReservedInventory);
			Assert.AreEqual(100, report.First().TotalInventory);
			Assert.AreEqual(20, report.First().UnfulfillableInventory);
			//Assert.AreEqual(1, report.First().MonthlySales);
			//Assert.AreEqual(3, report.ElementAt(1).MonthlySales);
		}



		[TestMethod]
		public void IsEmptyContent_NotEmpty()
		{
			string inventory = File.ReadAllText(@"report_samples\_GET_FBA_MYI_ALL_INVENTORY_DATA_.txt");
			GetReportResult getReportResult = new GetReportResult();
			getReportResult.Content = inventory;

			Assert.IsFalse(getReportResult.IsEmptyContent());
		}
		[TestMethod]
		public void IsEmptyContent_OneLineHeadersOnly()
		{
			string inventory = File.ReadAllText(@"report_samples\_GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_empty.txt");
			GetReportResult getReportResult = new GetReportResult();
			getReportResult.Content = inventory;

			Assert.IsTrue(getReportResult.IsEmptyContent());
		}


		public void IsEmptyContent_Empty()
		{
			string inventory = "";
			GetReportResult getReportResult = new GetReportResult();
			getReportResult.Content = inventory;

			Assert.IsTrue(getReportResult.IsEmptyContent());
		}
	}
	
}
