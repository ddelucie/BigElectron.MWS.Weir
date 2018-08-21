using BigElectron.MWS.Handlers;
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
		public GetReportResult GetReport(ReportHandler reportHandler, string reportType, DateTime startDate, DateTime endDate, string fileLocation)
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

	}
}
