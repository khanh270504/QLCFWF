using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class Menu
	{
		public Menu (string foodName,int count, int price,int totalPrice = 0)
		{
			this.FoodName = foodName;
			this.Count = count;
			this.Price = price;
			this.TotalPrice = totalPrice;

		}
		public Menu(DataRow row)
		{
			this.FoodName = row["Name"].ToString();
			this.Count = (int)row["Amount"];
			this.Price = (int) row["Price"];
			this.TotalPrice = (int)row["TotalPrice"];

		}
		private string foodName;
		private int count;
		private int totalPrice;
		private int price;

		public string FoodName 
		{
			get { return foodName; }
			set {  foodName = value; }
		}
		public int Count 
		{
			get { return count;}
			set {  count = value; }
		}
		public int TotalPrice
		{
			get { return totalPrice; }
			set { totalPrice = value; }
		}
		public int Price
		{
			get { return price; }		
			set
			{
				price = value;
			} 
		}
	}

}
