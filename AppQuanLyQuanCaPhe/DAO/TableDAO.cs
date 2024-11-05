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
			private set {TableDAO.instance = value; }
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
	}
}
