using MarketplaceWebService;

namespace BigElectron.MWS.Handlers

{
	public class ServiceContext
	{
		private string _appVersion = "1.0";
		private string _applicationName = "BigElectron";
		private string _mwsServiceUrl = "https://mws.amazonservices.com";

		public string MwsServiceUrl
		{
			get { return _mwsServiceUrl; }
			set { _mwsServiceUrl = value; }
		}
		public string AccessKey { get; set; }
		public string SecretKey { get; set; }
		public string ApplicationName
		{
			get { return _applicationName; }
			set { _applicationName = value; }
		}
		public string AppVersion
		{
			get { return _appVersion; }
			set { _appVersion = value; }
		}
		public string SellerId { get; set; }
		public string MwsAuthToken { get; set; }

		private int myVar;



	}
}