using AppQuanLyQuanCaPhe.DAO;
using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using Menu = AppQuanLyQuanCaPhe.DTO.Menu;

namespace AppQuanLyQuanCaPhe
{
	public partial class fTableManager : Form
	{
		public fTableManager()
		{
			InitializeComponent();
			LoadTable();
			LoadCategory();
		}
		#region Method
		void LoadCategory()
		{
			List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
			cbCategory.DataSource = listCategory;
			cbCategory.DisplayMember= "Name";


		}
		void LoadFoodListByCategoryID(int id)
		{
			List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
			cbFood.DataSource = listFood;
			cbFood.DisplayMember = "Name";
		}
		void LoadTable()
		{
			flpTable.Controls.Clear();
			 List<Table>tableList = TableDAO.Instance.LoadTableList();

			foreach(Table item in tableList)
			{
				Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
			    btn.Text = item.Name+ Environment.NewLine + item.Status;
				btn.Click += btn_Click;
				btn.Tag = item;
				switch(item.Status)
				{
					case "Trống":
						btn.BackColor = Color.Aqua;
						break;
					default:
						btn.BackColor = Color.LightPink;
						break;

				}	
				flpTable.Controls.Add(btn);
			}	
		}
		void ShowBill(int id)
		{
			lsvBill.Items.Clear();
			List<AppQuanLyQuanCaPhe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
			int totalPrice = 0;
			foreach(AppQuanLyQuanCaPhe.DTO.Menu item in listBillInfo)
			{
				ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
				lsvItem.SubItems.Add(item.Count.ToString());
				lsvItem.SubItems.Add(item.Price.ToString());
				lsvItem.SubItems.Add(item.TotalPrice.ToString());
				totalPrice += item.TotalPrice;
				lsvBill.Items.Add(lsvItem);
			}
			CultureInfo culture = new CultureInfo("vi-VN");
			txbTotalPrice.Text = totalPrice.ToString("c",culture);
		}

		#endregion
		#region Events
		void btn_Click(object sender, EventArgs e)
		{
			int tableID = ((sender as Button).Tag as Table).ID;
			lsvBill.Tag = (sender as Button).Tag;
			ShowBill(tableID);
		}
		private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fAccountProfile f = new fAccountProfile();
			f.ShowDialog();	
		}

		private void pnlRa_Paint(object sender, PaintEventArgs e)
		{

		}

		private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void adminToolStripMenuItem_Click(object sender, EventArgs e)
		{
			fAdmin f = new fAdmin();
			f.ShowDialog();
		}
		private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			int id = 0;
			ComboBox cb = sender as ComboBox;
			if (cb.SelectedItem == null)
			{
				return;
			}
			Category selected = cb.SelectedItem as Category;
			id = selected.ID;
			LoadFoodListByCategoryID(id);
		}
		private void btnAddFood_Click(object sender, EventArgs e)
		{
			Table table = lsvBill.Tag as Table;
			int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
			int foodID = (cbFood.SelectedItem as Food).Id;
			int count = (int)nmFoodCount.Value;
			if (idBill == -1)
			{
				BillDAO.Instance.InsertBill(table.ID);
				BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
			}
			else
			{
				BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
			}
			ShowBill(table.ID);
			LoadTable();

		}
		private void btnCheckOut_Click(object sender, EventArgs e)
		{
			Table table = lsvBill.Tag as Table;
			int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
			int discount = (int)nmDisCount.Value;
			double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
			double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;
			if(idBill != -1)
			{
				if(MessageBox.Show(string.Format("Bạn có chắc chắn muốn thanh toán hóa đơn cho bàn {0} \n Với Giá gốc là {1} Vnd , Giảm giá là {2} % \n Số tiền phải thanh toán là {3} Vnd ", table.Name,totalPrice*1000,discount,finalTotalPrice*1000),"Thông báo !!!",MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
				{
					BillDAO.Instance.CheckOut(idBill,discount);
					ShowBill(table.ID);
					LoadTable();
				}	
			}	
		}
		#endregion

		private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void txbTotalPrice_TextChanged(object sender, EventArgs e)
		{

		}

		
	}
}
