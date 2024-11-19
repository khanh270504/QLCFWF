using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class TableDAO
	{
		private static TableDAO instance;

		internal static TableDAO Instance
		{
			get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
			private set { TableDAO.instance = value; }
		}
		public static int TableWidth = 100;
		public static int TableHeight = 80;
		private TableDAO() { }
		public List<Table> LoadTableList()
		{
			List<Table> tableList = new List<Table>();
			DataTable data = DataProvider.Instance.ExecuteQuery("USP_GETLISTTABLE");
			foreach (DataRow item in data.Rows)
			{
				Table table = new Table(item);
				tableList.Add(table);
			}
			return tableList;
		}
		public void SwitchTable(int id1, int id2)
		{
			DataProvider.Instance.ExecuteQuery("USP_SwitchTable @TableID1 , @TableID2 ", new object[] { id1, id2 });
		}
		public bool InsertTable(string name)
		{
			string query = "USP_InsertTable @Name";
			int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { name });
			return result > 0;
		}

		public bool UpdateTable(int id, string name)
		{
			string query = "USP_UpdateTable @ID , @Name";
			int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id, name });
			return result > 0;
		}

		public bool DeleteTable(int id)
		{
			string query = string.Format("USP_DeleteTableFood @ID");
			int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { id });
			return result > 0;
		}
	}
}
