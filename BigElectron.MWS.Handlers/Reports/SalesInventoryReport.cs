using System;
using System.Collections.Generic;

namespace BigElectron.MWS.Handlers.Reports
{
	public class SalesInventoryReport
	{
		public IList<SalesInventoryReportItem> SalesInventoryReportItems { get; set; }
	}

	public class SalesInventoryReportItem
	{
		public string ASIN { get; set; }
		public int TotalInventory { get; set; }
		public int InboundInventory { get; set; }
		public int ReservedInventory { get; set; }
		public int UnfulfillableInventory { get; set; }
		//public DateTime SnapshotDate { get; set; }
		public int MonthlySales { get; set; }
		public string MonthYear { get; set; }
	}

}
