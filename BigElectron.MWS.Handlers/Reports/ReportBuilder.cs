﻿using System;
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
			//var ordersEnumerable = orders.AsEnumerable();
			//var groupedOrders = from o in ordersEnumerable
			//					group o["quantity"] by o["asin"] into g
			//					select new {  asin = g.Key, quantity = g.Sum(r => r.Field<int>("Amount 1") };


		var groupedOrders =  orders.AsEnumerable()
			  .GroupBy(r => r.Field<string>("asin"))
			  .Select(g =>
			  {
				  var row = orders.NewRow();

				  row["asin"] = g.Key;
				  row["quantity"] = g.Sum(r => int.Parse(r.Field<string>("quantity")));
				  return row;
			  }).CopyToDataTable();

			string monthYear = "";
			//if (ordersEnumerable != null && ordersEnumerable.Any())
			//{
			//	DateTime orderDate = Convert.ToDateTime(ordersEnumerable.First()["purchase-date"]);
			//	monthYear = orderDate.Month.ToString("d2") + orderDate.Year.ToString();
			//}
			monthYear = ordersDate.Month.ToString("d2") + ordersDate.Year.ToString();
			

			var results = from inventoryData in inventory.AsEnumerable()
						  join ordersData in groupedOrders.AsEnumerable() on inventoryData["asin"] equals ordersData.Field<string>("asin") into g
						  from inventoryorders in g.DefaultIfEmpty()
						  select new SalesInventoryReportItem
						  {
							  ASIN = inventoryData["ASIN"].ToString(),
							  TotalInventory = Convert.ToInt32(inventoryData["afn-fulfillable-quantity"]),
							  UnfulfillableInventory = Convert.ToInt32(inventoryData["afn-unsellable-quantity"]),
							  InboundInventory = Convert.ToInt32(inventoryData["afn-inbound-shipped-quantity"]),
							  ReservedInventory = Convert.ToInt32(inventoryData["afn-reserved-quantity"]),
							  MonthlySales = inventoryorders == null ? 0 : int.Parse(inventoryorders.Field<string>("quantity")),
							  MonthYear = monthYear
						  };

			return results;
		}

	}
}
