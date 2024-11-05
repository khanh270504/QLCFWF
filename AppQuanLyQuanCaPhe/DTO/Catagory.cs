using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DTO
{
	internal class Category
	{
		public Category(int id, string name)
		{
			this.ID = id;
			this.Name = name;
		}
		public Category(DataRow row)
		{
			this.ID = (int)row["ID"];
			this.Name = (string)row["Name"];
		}
		private int iD;
		private string name;

		public int ID
		{
			get { return iD; }
			set { iD = value; }
		}
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}
