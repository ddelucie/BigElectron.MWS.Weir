﻿using System;
using System.Collections.Generic;
using System.Linq;
using BigElectron.MWS.Handlers;
using BigElectron.MWS.Handlers.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace Weir.Inventory.ConsoleApp.Tests
{
	[TestClass]
	public class ReportManagerTests
	{
		Mock<ILogger> mocknLogger = new Mock<ILogger>();
		Mock<ReportHandler> mockReportHandler;
		ReportManager reportManager;
		ServiceContext serviceContext = new ServiceContext();

		public ReportManagerTests()
		{
			mockReportHandler = new Mock<ReportHandler>(new ServiceContext(), mocknLogger.Object);
			reportManager = new ReportManager(mockReportHandler.Object, serviceContext, mocknLogger.Object);
		}

		[TestMethod]
		public void GetAsinFilterList_EmptyFile()
		{

			IList<string> filter =  reportManager.GetAsinFilterList(@"C:\Temp\Asin\empty.txt");

			Assert.AreEqual(0, filter.Count);
		}

		[TestMethod]
		public void GetAsinFilterList_MissingFile()
		{

			IList<string> filter = reportManager.GetAsinFilterList(@"C:\Temp\Asin\missing.txt");

			Assert.AreEqual(0, filter.Count);
		}

		[TestMethod]
		public void GetAsinFilterList_ValidFile()
		{

			IList<string> filter = reportManager.GetAsinFilterList(@"C:\Temp\Asin\filter.txt");

			Assert.AreEqual(2, filter.Count);
		}



		[TestMethod]
		public void FilterReportByAsins_ItemsInFilter()
		{

			IList<string> filter = reportManager.GetAsinFilterList(@"C:\Temp\Asin\filter.txt");

			IList<SalesInventoryReportItem> reportItems = new List<SalesInventoryReportItem>();
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "111111111" });
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "0763644323" });
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "XX63644321" });


			reportItems = reportManager.FilterReportByAsins(reportItems.ToList(), filter);


			Assert.AreEqual(1, reportItems.Count);
			Assert.AreEqual("0763644323", reportItems.First().ASIN);
		}



		[TestMethod]
		public void FilterReportByAsins_EmptyFilter()
		{

			IList<string> filter = reportManager.GetAsinFilterList(@"C:\Temp\Asin\empty.txt");

			IList<SalesInventoryReportItem> reportItems = new List<SalesInventoryReportItem>();
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "111111111" });
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "0763644323" });
			reportItems.Add(new SalesInventoryReportItem() { ASIN = "XX63644321" });


			reportItems = reportManager.FilterReportByAsins(reportItems.ToList(), filter);


			Assert.AreEqual(3, reportItems.Count);

		}

		[TestMethod]
		public void FilterReportByAsins_EmptyReport()
		{

			IList<string> filter = reportManager.GetAsinFilterList(@"C:\Temp\Asin\filter.txt");
			IList<SalesInventoryReportItem> reportItems = new List<SalesInventoryReportItem>();


			reportItems = reportManager.FilterReportByAsins(reportItems.ToList(), filter);


			Assert.AreEqual(0, reportItems.Count);

		}

		//[TestMethod]
		//public void _EmptyReport()
		//{



		//	reportItems = reportManager.GetPriorReport();


		//	Assert.AreEqual(0, reportItems.Count);

		//}

	}
}
