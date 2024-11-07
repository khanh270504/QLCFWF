using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class BillDAO
	{
		private static BillDAO instance;

		public static BillDAO Instance
		{
			get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
			private set { BillDAO.instance = value; }
		}
		private BillDAO() { }
		public int GetUncheckBillIDByTableID(int id)
		{
			DataTable data = DataProvider.Instance.ExecuteQuery("select * from Bill where TableId = "+ id +" and Status = 0");
			if (data.Rows.Count > 0)
			{
				Bill bill = new Bill(data.Rows[0]);
				return bill.ID;

			}
			return -1;
		}
		public void  InsertBill(int id)
		{
			DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @TableID ", new object[] { id });
		}
		public void CheckOut(int id,int discount,float totalPrice)
		{
			string query = "update Bill set CheckOut = getdate() , status = 1, " + " discount = " + discount + ", totalPrice = " + totalPrice + " where id = " + id ;
			DataProvider.Instance.ExecuteNonQuery(query);
		}
		public DataTable GetBillListByDate(DateTime checkIn,DateTime checkOut)
		{
			return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDay @FromDate , @ToDate ",new object[] {checkIn,checkOut});
		}
		public int GetMaxIDBill()
		{
			try
			{
			    return (int)DataProvider.Instance.ExecuteScalar("select max(ID) from Bill");
			}
			catch
			{
				return 1;
			}
			

		}

	}
}
