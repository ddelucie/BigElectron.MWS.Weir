using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BigElectron.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigElectron.MWS.Common.Tests
{

	[TestClass]
	public class UtilTests
	{
		[TestMethod][Ignore]
		public void SendEmailTest()
		{
			Util.SendEmail("TEST EMAIL", "XXXX", new List<string> { "ddelucie@hotmail.com" });
		}

		[TestMethod]
		public void StringToDataTableTest1()
		{
			string data = File.ReadAllText(@"report_samples\_GET_FBA_MYI_ALL_INVENTORY_DATA_.txt");


			DataTable table = Util.StringToDataTable(data);


			Assert.IsFalse(table.HasErrors);
			Assert.IsTrue(table.Rows.Count > 0);
			Assert.AreEqual(13, table.Rows.Count);
			Assert.AreEqual("sku", table.Columns[0].ColumnName);
			Assert.AreEqual("Acne_Moisturizer", table.Rows[0].ItemArray[0]);
		}


		[TestMethod]
		public void StringToDataTableTest2()
		{
			string data  = File.ReadAllText(@"report_samples\_GET_FBA_FULFILLMENT_CURRENT_INVENTORY_DATA_.txt");


			DataTable table = Util.StringToDataTable(data);


			Assert.IsFalse(table.HasErrors);
			Assert.IsTrue(table.Rows.Count > 0);
			Assert.AreEqual(239, table.Rows.Count);
			Assert.AreEqual("snapshot-date", table.Columns[0].ColumnName);
			Assert.AreEqual("2018-08-18T00:00:00-07:00", table.Rows[0].ItemArray[0]);
		}
	}
}
