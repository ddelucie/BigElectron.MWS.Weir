using BigElectron.MWS.Handlers;
using MarketplaceWebService.Model;
using System;
using System.Collections.Generic;
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

			if (reportRequestInfoStatus.Equals(ReportRequestInfoStatus.ReportReady))
			{
				report = reportHandler.GetReport(reportRequestInfo.GeneratedReportId);
				reportHandler.WriteToFile(report.Content, fileLocation);
			}

			return report;
		}

	}
}
