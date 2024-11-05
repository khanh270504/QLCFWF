using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class BillInfoDAO
	{

		private static BillInfoDAO instance;
		public static BillInfoDAO Instance
		{
			get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; } 
			private set { BillInfoDAO.instance = value; }
		}
		
		private BillInfoDAO() { }
		public List<BillInfor> GetListBillInfo(int id)
		{
			List<BillInfor > listBillInfo = new List<BillInfor>();
			DataTable data = DataProvider.Instance.ExecuteQuery("select * from BillInfo where BillID = " + id);
			foreach (DataRow item in data.Rows)
			{
				BillInfor info = new BillInfor(item);
				listBillInfo.Add(info);
			}
			return listBillInfo;
		}

		public void InsertBillInfo(int idBill, int idFood, int count)
		{
			DataProvider.Instance.ExecuteNonQuery("USP_InsertBillInfo @BillID , @FoodID , @Amount ", new object[] { idBill,idFood,count });
		}

	}
}
