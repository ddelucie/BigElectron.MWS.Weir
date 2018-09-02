using BigElectron.MWS.Handlers;
using BigElectron.MWS.Handlers.Reports;
using MarketplaceWebService.Model;
using NLog;
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
		private ServiceContext serviceContext;
		private ILogger nLogger;

		public ReportManager(ReportHandler reportHandler, ServiceContext serviceContext, ILogger nLogger)
		{
			this.reportHandler = reportHandler;
			this.serviceContext = serviceContext;
			this.nLogger = nLogger;
		}

		public GetReportResult GetReport(string reportType, DateTime startDate, DateTime endDate, string fileLocation)
		{
			GetReportResult report = new GetReportResult();

			ReportRequestInfoStatus reportRequestInfoStatus;
			RequestReportResult requestReportResult = reportHandler.RequestReport(reportType, startDate, endDate);
			ReportRequestInfo reportRequestInfo = reportHandler.PollReportStatus(requestReportResult.ReportRequestInfo.ReportRequestId, out reportRequestInfoStatus);

			string writePath = reportHandler.CreateFileLocation(fileLocation, serviceContext.SellerId, reportType, startDate, endDate);

			if (reportRequestInfoStatus.Equals(ReportRequestInfoStatus.ReportReady))
			{
				nLogger.Info("ReportReady, getting report");
				report = reportHandler.GetReport(reportRequestInfo.GeneratedReportId);
				if (!report.IsEmptyContent()) reportHandler.WriteToFile(report.Content, writePath);
			}
			if (report.IsEmptyContent())
			{
				nLogger.Info("getting report from disc");
				if (File.Exists(writePath)) report.Content = File.ReadAllText(writePath);
			}
			if (report.IsEmptyContent())
			{
				nLogger.Info("GetPriorReport");
				report = GetPriorReport(reportType, startDate, endDate);
				if (!report.IsEmptyContent()) reportHandler.WriteToFile(report.Content, writePath);
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

		public GetReportResult GetPriorReport(string reportType, DateTime startDate, DateTime endDate)
		{
			GetReportResult report = new GetReportResult();
			var reportRequestList = reportHandler.GetReportRequestList(50m, reportType);
			var priorReportRequestInfo = reportRequestList.ReportRequestInfo
				.Where(r => r.StartDate.Date == startDate.Date.AddDays(-1)  && r.ReportProcessingStatus == "_DONE_")
				.OrderBy(r => r.SubmittedDate).FirstOrDefault();
			if (priorReportRequestInfo != null)
			{
				report = reportHandler.GetReport(priorReportRequestInfo.GeneratedReportId);
			}
			return report;
		}

	}
}
