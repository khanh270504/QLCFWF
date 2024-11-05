using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class Table
	{
		public Table(int id, string name, string status)
		{
			this.ID = id;
			this.Name = name;
			this.Status = status;
		}
		public Table(DataRow row)
		{
			this.ID = (int)row["ID"];
			this.Name = row["Name"].ToString();
			this.Status = row["Status"].ToString();

		}

		private int iD;

		public int ID
		{
			get { return iD; }

			set { iD = value; }
		}
		private string status;
		public string Status
		{
			get { return status; }
			set { status = value; }
		}
		private string name;
		public string Name
		{
			get { return name; }
			set { name = value; }


		}
	}
}
