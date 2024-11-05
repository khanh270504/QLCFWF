using AppQuanLyQuanCaPhe.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class AccountDAO
	{
		private static AccountDAO instace;

		internal static AccountDAO Instace
		{
			get { if (instace == null) instace = new AccountDAO(); return instace; }
			private set { instace = value; }
		}
		private AccountDAO() { }
		public bool Login(string userName, string passWord)
		{
			string query = "dbo.USP_Login @userName , @passWord";
			DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });
			return result.Rows.Count > 0;
		}
	}
}
