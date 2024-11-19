using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class CategoryDAO
	{
		private static CategoryDAO instance;

		public static CategoryDAO Instance
		{
			get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
			private set { CategoryDAO.instance = value; }
		}

		private CategoryDAO() { }

		public List<Category> GetListCategory()
		{

			List<Category> list = new List<Category>();
			string query = "select * from CategoryFood";
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach (DataRow item in data.Rows)
			{
				Category category = new Category(item);
				list.Add(category);
			}
			return list;



		}
		public Category GetCategoryByID(int id)
		{
			Category category = null;


			string query = "select * from CategoryFood where id = " + id;
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach (DataRow item in data.Rows)
			{
				category = new Category(item);
				return category;
			}

			return category;
		}
		public bool InsertCategory(string name)
		{
			string query = string.Format("insert into CategoryFood (Name) values (N'{0}')", name);
			int result = DataProvider.Instance.ExecuteNonQuery(query);
			return result > 0;
		}
		public bool UpdateCategory(int id, string name)
		{
			string query = string.Format("update CategoryFood set Name = N'{0}' where ID = {1}", name, id);
			int result = DataProvider.Instance.ExecuteNonQuery(query);
			return result > 0;
		}


		public bool DeteleCategory(int id)
		{
			string query = string.Format("USP_DeleteCategory @ID");
			int result = DataProvider.Instance.ExecuteNonQuery("USP_DeleteCategory @ID", new object[] { id });
			return result > 0;
		}

		public List<Category> SearchCategoryByName(string name)
		{
			List<Category> listCategory = new List<Category>();
			string query = string.Format("select * from CategoryFood where dbo.fuConvertToUnsign1(Name) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);

			DataTable table = new DataTable();
			try
			{
				table = DataProvider.Instance.ExecuteQuery(query);
			}
			catch (Exception ex)
			{
				throw ex;
			}

			foreach (DataRow row in table.Rows)
			{
				Category category = new Category(row);
				listCategory.Add(category);
			}
			return listCategory;
		}




	}
}

		
		
	     

	
	

