using BigElectron.MWS.Handlers;
using BigElectron.MWS.Handlers.Reports;
using MarketplaceWebService.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weir.Inventory.ConsoleApp
{
	public class ReportManager
	{
		private ReportHandler reportHandler;

		public ReportManager(ReportHandler reportHandler)
		{
			this.reportHandler = reportHandler;
		}

		public GetReportResult GetReport(string reportType, DateTime startDate, DateTime endDate, string fileLocation)
		{
			GetReportResult report = new GetReportResult();

			ReportRequestInfoStatus reportRequestInfoStatus;
			RequestReportResult requestReportResult = reportHandler.RequestReport(reportType, startDate, endDate);
			ReportRequestInfo reportRequestInfo = reportHandler.PollReportStatus(requestReportResult.ReportRequestInfo.ReportRequestId, out reportRequestInfoStatus);

			string writePath = reportHandler.CreateFileLocation(fileLocation, reportType, startDate, endDate);

			if (reportRequestInfoStatus.Equals(ReportRequestInfoStatus.ReportReady))
			{
				report = reportHandler.GetReport(reportRequestInfo.GeneratedReportId);
				reportHandler.WriteToFile(report.Content, writePath);
			}
			else
			{
				if (File.Exists(writePath)) report.Content = File.ReadAllText(writePath);
			}

			return report;
		}

		public IList<string> GetAsinFilterList(string fileLocation)
		{
			var asinFilterList = new List<string>();
			if (string.IsNullOrWhiteSpace(fileLocation)) return asinFilterList;
			if (File.Exists(fileLocation)) 
			{
				var asins = File.ReadAllLines(fileLocation);
				asinFilterList = new List<string>(asins);
			}
			return asinFilterList;
		}


		public IList<SalesInventoryReportItem> FilterReportByAsins(IList<SalesInventoryReportItem> reportItems, IList<string> asinFilterList)
		{
			if (reportItems == null) return reportItems;
			if (!asinFilterList.Any()) return reportItems;
			reportItems = reportItems.Where(item => asinFilterList.Contains(item.ASIN)).ToList();
			return reportItems;
		}

	}
}
