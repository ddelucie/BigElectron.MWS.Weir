using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigElectron.MWS.Handlers.Reports
{
	public class ReportBuilder
	{


		public IEnumerable<SalesInventoryReportItem> JoinInventoryAndOrders(DataTable inventory, DataTable orders, DateTime ordersDate)
		{
			var ordersEnumerable = orders.AsEnumerable();
			var groupedOrders = from o in ordersEnumerable
								group o["quantity-purchased"] by o["sku"] into g
								select new { sku = g.Key, quantity = g.ToList().Count};

			string monthYear = "";
			//if (ordersEnumerable != null && ordersEnumerable.Any())
			//{
			//	DateTime orderDate = Convert.ToDateTime(ordersEnumerable.First()["purchase-date"]);
			//	monthYear = orderDate.Month.ToString("d2") + orderDate.Year.ToString();
			//}
			monthYear = ordersDate.Month.ToString("d2") + ordersDate.Year.ToString();
			

			var results = from inventoryData in inventory.AsEnumerable()
						  join ordersData in groupedOrders.AsEnumerable() on inventoryData["sku"] equals ordersData.sku into g
						  from inventoryorders in g.DefaultIfEmpty()
						  select new SalesInventoryReportItem
						  {
							  ASIN = inventoryData["ASIN"].ToString(),
							  TotalInventory = Convert.ToInt32(inventoryData["afn-total-quantity"]),
							  UnfulfillableInventory = Convert.ToInt32(inventoryData["afn-unsellable-quantity"]),
							  InboundInventory = Convert.ToInt32(inventoryData["afn-inbound-shipped-quantity"]),
							  ReservedInventory = Convert.ToInt32(inventoryData["afn-reserved-quantity"]),
							  MonthlySales = inventoryorders == null ? 0 : inventoryorders.quantity,
							  MonthYear = monthYear
						  };

			return results;
		}

	}
}
