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

			string fileLocation = "c:/Temp";
			string keyFileLocation = "c:/Temp";

			var appSettings = ConfigurationManager.AppSettings;
			keyFileLocation = appSettings["KeyFileLocation"];

			Console.WriteLine("keyFileLocation: " +keyFileLocation);

			string keyData = File.ReadAllText(keyFileLocation);
			ServiceContext serviceContext = new ServiceContext();
			serviceContext = JsonConvert.DeserializeObject<ServiceContext>(keyData);


			Console.WriteLine("month (1-12)");
			Console.ReadLine();
			nLogger.Info("month: " + args[0]);
			Console.WriteLine("year ");
			Console.ReadLine();
			nLogger.Info("year: " + args[1]);

			int month;
			int year;
			DateTime date = DateTime.Now.Date;
			if (!Int32.TryParse(args[0], out month)) month = date.Month;
			if (!Int32.TryParse(args[1], out year)) year = date.Year;

			DateTime startDate = new DateTime(year, month, 1);
			DateTime endDate = startDate.AddMonths(1);

			try
			{
				ReportHandler reportHandler = new ReportHandler(serviceContext, nLogger);
				ReportManager reportManager = new ReportManager();
				ReportBuilder reportBuilder = new ReportBuilder();

				nLogger.Info("getting ordersReport");
				GetReportResult ordersReport = reportManager.GetReport(reportHandler, ReportType._GET_FLAT_FILE_ORDERS_DATA_, startDate, endDate, fileLocation);
				nLogger.Info("getting inventoryReport");
				GetReportResult inventoryReport = reportManager.GetReport(reportHandler, ReportType._GET_FBA_MYI_ALL_INVENTORY_DATA_, DateTime.Now.Date.AddDays(-1), DateTime.Now.Date, fileLocation);

				nLogger.Info("getting ordersTable");
				DataTable ordersTable = Util.StringToDataTable(ordersReport.Content);
				nLogger.Info("getting inventoryTable");
				DataTable inventoryTable = Util.StringToDataTable(inventoryReport.Content);
				nLogger.Info("calling JoinInventoryAndOrders");
				IEnumerable<SalesInventoryReportItem> reportItems = reportBuilder.JoinInventoryAndOrders(inventoryTable, ordersTable);
				nLogger.Info("SalesInventoryReportItem report created");
				
				string writePath = reportHandler.CreateFileLocation(fileLocation, "InventoryAndOrders", startDate, endDate);
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

			//Console.ReadKey();
		}
	}

	
}
