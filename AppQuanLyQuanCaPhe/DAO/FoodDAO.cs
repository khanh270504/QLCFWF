using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class FoodDAO
	{
		private static FoodDAO instance;

		public static FoodDAO Instance
		{
			get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
			private set { FoodDAO.instance = value; }
		}

		private FoodDAO() { }
		public List<Food>GetFoodByCategoryID(int id)
		{
			List<Food>list= new List<Food>();
			string query = "select  * from Food where CategoryID = " + id ;
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach(DataRow item in data.Rows)
			{
				Food food = new Food(item);
				list.Add(food);

			}
			return list;
		}
		public List<Food> GetListFood()
		{
				List<Food>list= new List<Food>();
			string query = "select  * from Food  "  ;
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach(DataRow item in data.Rows)
			{
				Food food = new Food(item);
				list.Add(food);

			}
			return list;
		}
		public bool InsertFood(string name , int id , float price)
		{
			string query = string.Format("INSERT dbo.Food( Name, CategoryID, Price ) VALUES  ( N'{0}',{1},{2}) ",name,id,price);
			int result = DataProvider.Instance.ExecuteNonQuery(query);
			return result > 0;
		}
		public bool UpdateFood(int idFood,string name, int id, float price)
		{
			string query = string.Format("update dbo.Food set Name =  N'{0}', CategoryID = {1}, Price= {2} where ID = {3}", name, id, price,idFood);
			int result = DataProvider.Instance.ExecuteNonQuery(query);
			return result > 0;
		}
		public bool DeleteFood(int idFood)
		{
			BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood);
			string query = string.Format("Delete dbo.Food where id = {0} ", idFood);
			int result = DataProvider.Instance.ExecuteNonQuery(query);
			return result > 0;
		}
		public List<Food> SearchFoodByName(string name)
		{
			List<Food> list =  new List<Food>();
			string query = string.Format("select * from dbo.Food where dbo.fuConvertToUnsign1(name) like N'%' + dbo.fuConvertToUnsign1(N'{0}') +'%'", name);
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach (DataRow item in data.Rows)
			{
				Food food = new Food(item);
				list.Add(food);

			}
			return list;
		}

	}
}
