using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class BillInfor
	{
		public BillInfor(int id, int billID,int foodID,int count) 
		{
			this.ID = id;
			this.BillID = billID;
			this.FoodID = foodID;
			this.Count = count;
		}
		public BillInfor(DataRow row)
		{
			this.ID =(int)row["ID"];
			this.BillID = (int)row["BillID"];
			this.FoodID = (int)row["FoodID"];
			this.Count = (int)row["Amount"];

		}
		private int iD;

		public int ID 
		{ 
			get {  return iD; }
			set {  iD = value; }
		}

		public int BillID
		{ 
			get { return billID; }
			set {  billID = value; }
		}

		private int billID;
		private int foodID;
		public int FoodID
		{
			get { return foodID; }
			set { foodID = value; }
	    }
		private int count;
		public int Count
		{
			get { return count; }
			set { count = value; }
		}
	}
}
