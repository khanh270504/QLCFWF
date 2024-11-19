using AppQuanLyQuanCaPhe.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
		public bool UpdateAccount(string userName,string displayName,string pass,string newPass)
		{
			int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @UserName , @DisplayName , @Password , @NewPassword ",new object[] {userName,displayName,pass,newPass});
			return result > 0;
		}
		public DataTable GetListAccount()
		{
			return DataProvider.Instance.ExecuteQuery("SELECT UserName, DisplayName, TypeID FROM dbo.Account");
		}
		public Account GetAccountByUserName(string userName)
		{
			string query = "select * from Account where UserName = @userName";
			DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { userName });
			foreach (DataRow item in data.Rows)
			{
				return new Account(item);
			}
			return null; 
	    }
		public bool InsertAccount(string name, string displayName, int type)
		{
			string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, TypeID ,Password )VALUES  ( N'{0}', N'{1}', {2},N'{3}')", name, displayName, type,"12345");
			int result = DataProvider.Instance.ExecuteNonQuery(query);

			return result > 0;
		}

		public bool UpdateAccount(string name, string displayName, int type)
		{
			string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', TypeID = {2} WHERE UserName = N'{0}'", name, displayName, type);
			int result = DataProvider.Instance.ExecuteNonQuery(query);

			return result > 0;
		}

		public bool DeleteAccount(string name)
		{
			string query = string.Format("Delete Account where UserName = N'{0}'", name);
			int result = DataProvider.Instance.ExecuteNonQuery(query);

			return result > 0;
		}

		public bool ResetPassword(string name)
		{
			string query = string.Format("update account set password = N'12345' where UserName = N'{0}'", name);
			int result = DataProvider.Instance.ExecuteNonQuery(query);

			return result > 0;
		}
	}
}
