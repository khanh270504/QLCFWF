using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class Food
	{
		public Food(int id,string name,int categoryID,float price)
		{
			this.Id = id;
			this.Name = name;
			this.CategoryID = categoryID;
			this.Price = price;

		}
		public Food(DataRow row)
		{
			this.Id = (int)row["ID"];
			this.Name = row["Name"].ToString();
			this.CategoryID = (int)row["CategoryID"];
			this.Price = (int)row["Price"];

		}
		private int id;
		private string name;
		private int categoryID;
		private float price;

		public int Id 
		{ 
			get { return id; }
			set {  id = value; }
		}
		public string Name
		{ 
			get { return name; }
			set {  name = value; } 
		}
		public int CategoryID 
		{ 
			get { return categoryID; }
			set {  categoryID = value; }
		}
		public float Price 
		{ 
			get { return price; }
			set {  price = value; } 
		}
	}
}
