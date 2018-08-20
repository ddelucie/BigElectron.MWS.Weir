using BigElectron.Common;
using BigElectron.MWS.Handlers;
using BigElectron.MWS.Handlers.Reports;
using MarketplaceWebService.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weir.Inventory.ConsoleApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			string fileLocation = "c:/Temp";
			string keyFileLocation = "c:/Temp";

			var appSettings = ConfigurationManager.AppSettings;
			keyFileLocation = appSettings["KeyFileLocation"];


			ServiceContext serviceContext = new ServiceContext();



			Console.WriteLine("month (1-12)");
			Console.ReadLine();
			Console.WriteLine("year ");
			Console.ReadLine();

			int month;
			int year;
			DateTime date = DateTime.Now.Date;
			if (!Int32.TryParse(args[0], out month)) month = date.Month;
			if (!Int32.TryParse(args[1], out year)) year = date.Year;

			DateTime startDate = new DateTime(year, month, 1);
			DateTime endDate = startDate.AddMonths(1);


			ILogger nLogger = LogManager.GetLogger("Inventory.Console Logger");

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
				reportBuilder.JoinInventoryAndOrders(inventoryTable, ordersTable);
			}

			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}
	}

	
}
