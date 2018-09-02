using BigElectron.Common;
using BigElectron.MWS.Handlers;
using BigElectron.MWS.Handlers.Reports;
using CsvHelper;
using MarketplaceWebService.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weir.Inventory.ConsoleApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			ILogger nLogger = LogManager.GetLogger("Inventory.Console Logger");

			nLogger.Info("Starting console app");

			string outputLocation = "c:/Temp";
			string keyFileLocation = "c:/Temp";
			string asinFilterListLocation = "c:/Temp";

			var appSettings = ConfigurationManager.AppSettings;
			outputLocation = appSettings["outputLocation"];
			keyFileLocation = appSettings["keyFileLocation"];
			asinFilterListLocation = appSettings["asinFilterListLocation"];

			nLogger.Info("outputLocation: " + outputLocation);
			nLogger.Info("keyFileLocation: " + keyFileLocation);
			nLogger.Info("asinFilterListLocation: " + asinFilterListLocation);

			string keyData = File.ReadAllText(keyFileLocation);
			ServiceContext serviceContext = new ServiceContext();
			serviceContext = JsonConvert.DeserializeObject<ServiceContext>(keyData);


			Console.WriteLine("month (1-12)");
			string m = Console.ReadLine();
			nLogger.Info("month: " + m);
			Console.WriteLine("year ");
			string y = Console.ReadLine();
			nLogger.Info("year: " + y);

			int month;
			int year;
			DateTime date = DateTime.Now.Date;
			if (!Int32.TryParse(m, out month)) month = date.Month;
			if (!Int32.TryParse(y, out year)) year = date.Year;

			DateTime startDate = new DateTime(year, month, 1);
			DateTime endDate = startDate.AddMonths(1);

			try
			{
				ReportHandler reportHandler = new ReportHandler(serviceContext, nLogger);
				ReportManager reportManager = new ReportManager(reportHandler, serviceContext, nLogger);
				ReportBuilder reportBuilder = new ReportBuilder();

				nLogger.Info("getting ordersReport");
				GetReportResult ordersReport = reportManager.GetReport(ReportType._GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_, startDate, endDate, outputLocation);
				nLogger.Info("getting inventoryReport");
				GetReportResult inventoryReport = reportManager.GetReport(ReportType._GET_FBA_MYI_ALL_INVENTORY_DATA_, DateTime.Now.Date.AddDays(-1), DateTime.Now.Date, outputLocation);

				nLogger.Info("getting ordersTable");
				DataTable ordersTable = Util.StringToDataTable(ordersReport.Content);
				nLogger.Info("getting inventoryTable");
				DataTable inventoryTable = Util.StringToDataTable(inventoryReport.Content);
				nLogger.Info("calling JoinInventoryAndOrders");
				IEnumerable<SalesInventoryReportItem> reportItems = reportBuilder.JoinInventoryAndOrders(inventoryTable, ordersTable, startDate);
				nLogger.Info(string.Format("SalesInventoryReportItem report created, Count: {0}", reportItems.Count()));

				nLogger.Info("GetAsinFilterList");
				IList<string> asinFilterList = reportManager.GetAsinFilterList(asinFilterListLocation);
				nLogger.Info(string.Format("asinFilterList, Count: {0}", asinFilterList.Count()));

				nLogger.Info("filtering by asins");
				reportItems = reportManager.FilterReportByAsins(reportItems.ToList(), asinFilterList);

				string writePath = reportHandler.CreateFileLocation(outputLocation, serviceContext.SellerId, "InventoryAndOrders", startDate, endDate);
				nLogger.Info("SalesInventoryReportItem report path: " + writePath);

				Directory.CreateDirectory(Path.GetDirectoryName(writePath));
				using (var csv = new CsvWriter(new StreamWriter(writePath)))
				{
					csv.WriteRecords(reportItems);
				}
				nLogger.Info("csv written");
			}

			catch (Exception e)
			{
				nLogger.Error(e);
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("Report Complete.");
			Console.ReadKey();
		}
	}

	
}
