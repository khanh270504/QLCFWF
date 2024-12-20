﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	
	internal class Bill
	{
		public Bill(int id,DateTime? dateCheckOut, DateTime? dateCheckIn, int status,int discount = 0)
		{
			this.ID = id;
			this.DateCheckOut = dateCheckOut;
			this.Status = status;
			this.dateCheckIn = dateCheckIn;
			this.discount = discount;
		}
		public Bill(DataRow row)
		{
			this.ID = (int)row["ID"];
			this.dateCheckIn = (DateTime?)row["CheckIn"];
			var dataCheckoutTemp = row ["CheckOut"];
			if (dataCheckoutTemp.ToString() !="")
			{
				this.DateCheckOut = (DateTime?)dataCheckoutTemp;
			}
			this.Status = (int)row["Status"];
			if (row["Discount"].ToString() != "")
			this.Discount = (int)row["Discount"];
 			

		}
		private int discount;
		private int status;
		public int Status
		{
			get { return status; }
			set { status = value; }
		}

		public DateTime? DateCheckOut
		{ 
			get { return dateCheckOut; }
			set {  dateCheckOut = value; }
		}

		public int ID
		{ 
			get { return iD; }
			set {  iD = value; }
		}

		public DateTime? DateCheckIn 
		{ 
			get { return dateCheckIn; }
			set { dateCheckIn = value; }
		}

		public int Discount 
		{
			get { return discount; } 
			set {  discount = value; }
		}
		

		private DateTime? dateCheckOut;
		private int iD;
		private DateTime? dateCheckIn;


	}
}
