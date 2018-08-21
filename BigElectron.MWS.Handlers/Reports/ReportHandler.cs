using MarketplaceWebService;
using MarketplaceWebService.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigElectron.MWS.Handlers
{
	public class ReportHandler
	{

		ServiceContext serviceContext;
		ILogger nLogger;
		MarketplaceWebServiceConfig config;
		MarketplaceWebService.MarketplaceWebService service;

		public ReportHandler(ServiceContext serviceContext, ILogger nLogger)
		{
			this.serviceContext = serviceContext;
			this.nLogger = nLogger;


			config = new MarketplaceWebServiceConfig();
			config.ServiceURL = serviceContext.MwsServiceUrl;

			service = new MarketplaceWebServiceClient(
				   serviceContext.AccessKey,
				   serviceContext.SecretKey,
				   serviceContext.ApplicationName,
				   serviceContext.AppVersion,
				   config);
		}

		public RequestReportResult RequestReport(string reportType, DateTime startDate, DateTime endDate, string reportOptions = "")
		{
			if (string.IsNullOrWhiteSpace(reportType)) throw new ArgumentNullException("reportType is empty");

			nLogger.Info("RequestReport start for reportType: " + reportType);

			RequestReportRequest request = new RequestReportRequest();
			request.ReportType = reportType;
			request.Merchant = serviceContext.SellerId; ;
			request.MWSAuthToken = serviceContext.MwsAuthToken; ; // Optional
			request.StartDate = startDate;
			request.EndDate = endDate;
			request.ReportOptions = reportOptions;
			RequestReportResponse response = service.RequestReport(request);

			return response.RequestReportResult;
		}

		public GetReportResult GetReport(string reportId)
		{
			if (string.IsNullOrWhiteSpace(reportId)) throw new ArgumentNullException("reportId is empty");

			nLogger.Info("GetReport for : " + reportId);

			GetReportRequest req = new GetReportRequest();
			req.MWSAuthToken = serviceContext.MwsAuthToken;
			if (string.IsNullOrEmpty(req.MWSAuthToken)) req.MWSAuthToken = "MWSAuthToken";
			req.Merchant = serviceContext.SellerId;
			req.ReportId = reportId;

			GetReportResponse response = null;

			try
			{
				response = service.GetReport(req);
			}
			catch (Exception e)
			{
				nLogger.Error("GetReport Failed");
				throw;
			}

			return response.GetReportResult;
		}



		public ReportRequestInfo PollReportStatus(string reportRequestId, out ReportRequestInfoStatus reportRequestInfoStatus)
		{
			if (string.IsNullOrEmpty(reportRequestId)) throw new ArgumentNullException("reportRequestId is empty");
			ReportRequestInfo reportRequestInfo = null;
			reportRequestInfoStatus = ReportRequestInfoStatus.Unknown;
			int sleepTime = 5000;
			for (int i = 0; i < 6; i++)
			{
				nLogger.Info("GetReportRequestInfo polling for reportRequestId: " + reportRequestId);

				reportRequestInfo = GetReportRequestInfo(reportRequestId);

				reportRequestInfoStatus = GetReportRequestInfoStatus(reportRequestInfo);
				if (reportRequestInfoStatus == ReportRequestInfoStatus.ReportPending || reportRequestInfoStatus == ReportRequestInfoStatus.Unknown)
				{
					Thread.Sleep(sleepTime);
					sleepTime = sleepTime + sleepTime;
				}
				else break;
			}

			return reportRequestInfo;
		}

		public GetReportCountResult GetReportCount()
		{
			nLogger.Info("GetReportCount start");
			GetReportCountRequest request = new GetReportCountRequest();
			request.MWSAuthToken = serviceContext.MwsAuthToken;
			request.Merchant = serviceContext.SellerId;
			GetReportCountResponse response = service.GetReportCount(request);
			return response.GetReportCountResult;
		}


		public GetReportRequestListResult GetReportRequestList()
		{
			nLogger.Info("GetReportRequestList start");
			GetReportRequestListRequest request = new GetReportRequestListRequest();
			request.MWSAuthToken = serviceContext.MwsAuthToken;
			request.Merchant = serviceContext.SellerId;
			GetReportRequestListResponse response = service.GetReportRequestList(request);
			return response.GetReportRequestListResult;
		}

		/// <summary>
		/// Get a ReportRequestInfo for a reportRequestId
		/// </summary>
		/// <param name="reportRequestId"></param>
		/// <returns></returns>
		public ReportRequestInfo GetReportRequestInfo(string reportRequestId)
		{
			if (string.IsNullOrWhiteSpace(reportRequestId)) throw new ArgumentNullException("reportRequestId is empty");

			nLogger.Info("GetReportRequestResult start");
			GetReportRequestListResult reportRequestList = GetReportRequestList();
			return reportRequestList.ReportRequestInfo.FirstOrDefault(rr => rr.ReportRequestId == reportRequestId);
		}

		public ReportRequestInfoStatus GetReportRequestInfoStatus(ReportRequestInfo reportRequestInfo)
		{
			/*
			 *	_SUBMITTED_
				_IN_PROGRESS_
				_CANCELLED_
				_DONE_
				_DONE_NO_DATA_
			 */
			if (reportRequestInfo == null) return ReportRequestInfoStatus.ReportPending;
			if (reportRequestInfo.ReportProcessingStatus == "_SUBMITTED_") return ReportRequestInfoStatus.ReportPending;
			if (reportRequestInfo.ReportProcessingStatus == "_IN_PROGRESS_") return ReportRequestInfoStatus.ReportPending;
			if (reportRequestInfo.ReportProcessingStatus == "_CANCELLED_") return ReportRequestInfoStatus.ReportFailed;
			if (reportRequestInfo.ReportProcessingStatus == "_DONE_") return ReportRequestInfoStatus.ReportReady;
			if (reportRequestInfo.ReportProcessingStatus == "_DONE_NO_DATA_") return ReportRequestInfoStatus.ReportFailed;
			return ReportRequestInfoStatus.ReportFailed;
		}


		public string CreateFileLocation(string basePath, string reportType, DateTime startDate, DateTime endDate, string reportOptions = "")
		{
			string fileLocation = Path.Combine(basePath, reportType, startDate.Date.ToString("yyyy-MM-dd") + "--" + endDate.Date.ToString("yyyy-MM-dd") + ".txt");
			return fileLocation;
		}

		public void WriteToFile(string fileContents, string fileLocation)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));
			File.WriteAllText(fileLocation, fileContents);
		}
	}
}
