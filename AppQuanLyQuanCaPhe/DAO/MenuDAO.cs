using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppQuanLyQuanCaPhe.DAO
{
	internal class MenuDAO
	{
		private static MenuDAO instance;

		internal static MenuDAO Instance 
		{
			get { if(instance==null)instance=new MenuDAO();return MenuDAO.instance;}
			private set { MenuDAO.instance = value; }
		}
		private MenuDAO() { }

		public List<Menu>GetListMenuByTable(int id)
		{
			List<Menu> listMenu = new List<Menu>();

			string query = "select f.Name, bi.Amount,f.Price,f.Price*bi.Amount as totalPrice \r\nfrom dbo.Bill as b , dbo.BillInfo as bi, dbo.Food as f\r\nwhere bi.BillID = b.ID and bi.FoodID = f.ID and b.Status = 0 and b.TableID  = " + id;
			DataTable data = DataProvider.Instance.ExecuteQuery(query);
			foreach (DataRow item in data.Rows)
			{
				Menu menu = new Menu(item);
				listMenu.Add(menu);
			}
			return listMenu;
		}
	}
}
