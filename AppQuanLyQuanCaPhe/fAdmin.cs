using AppQuanLyQuanCaPhe.DAO;
using AppQuanLyQuanCaPhe.DTO;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace AppQuanLyQuanCaPhe
{
	public partial class fAdmin : Form
	{
		BindingSource foodList = new BindingSource();
		BindingSource categoryList = new BindingSource();
		BindingSource tableList = new BindingSource();
		BindingSource accountList = new BindingSource();

		public Account loginAccount;
		public fAdmin()
		{
			InitializeComponent();
			dtgvFood.DataSource = foodList;
			dtgvAccount.DataSource = accountList;
			dtgvCatagory.DataSource = categoryList;
			dtgvTable.DataSource = tableList;

			LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
			LoadListFood();
			LoadListCategory();
			LoadListTable();
			LoadAccount();
			LoadCategoryIntoCombobox(cbFoodCategory);
			AddFoodBinding();
			AddCategoryBinding();
			AddTableBinding();
			AddAccountBinding();


		}
		void AddAccountBinding()
		{
			txbUserName.DataBindings.Add(new Binding("Text",dtgvAccount.DataSource,"UserName", true, DataSourceUpdateMode.Never));
			txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
			nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "TypeID", true, DataSourceUpdateMode.Never));
			

			//// Sử dụng sự kiện Format để chuyển đổi ID thành chuỗi hiển thị
			//cbAccountType.DataBindings["Text"].Format += (sender, e) =>
			//{
			//	if (e.Value != null && int.TryParse(e.Value.ToString(), out int typeId))
			//	{
			//		// Chuyển đổi ID sang chuỗi hiển thị
			//		e.Value = typeId == 1 ? "Quản lý" : "Nhân viên";
			//	}
			//};
		}


		#region methosds
		void LoadAccount()
		{
			accountList.DataSource = AccountDAO.Instace.GetListAccount();
		}
		void AddAccount(string userName, string displayName, int type)
		{
			if (AccountDAO.Instace.InsertAccount(userName, displayName, type))
			{
				MessageBox.Show("Thêm tài khoản thành công");
			}
			else
			{
				MessageBox.Show("Thêm tài khoản thất bại");
			}

			LoadAccount();
		}

		void EditAccount(string userName, string displayName, int type)
		{
			if (AccountDAO.Instace.UpdateAccount(userName, displayName, type))
			{
				MessageBox.Show("Cập nhật tài khoản thành công");
			}
			else
			{
				MessageBox.Show("Cập nhật tài khoản thất bại");
			}

			LoadAccount();
		}

		void DeleteAccount(string userName)
		{
			if(loginAccount.UserName.Equals(userName))
			{
				MessageBox.Show("Không thể xóa chính bạn");
				return;
			}	
			if (AccountDAO.Instace.DeleteAccount(userName))
			{
				MessageBox.Show("Xóa thành công");
			}
			else
			{
				MessageBox.Show("Xóa thất bại");
			}	
				

			LoadAccount();
		}


		void ResetPass(string userName)
		{
			if (AccountDAO.Instace.ResetPassword(userName))
			{
				MessageBox.Show("Đặt lại mật khẩu thành công");
			}
			else
			{
				MessageBox.Show("Đặt lại mật khẩu thất bại");
			}
		}
		List<Food>SearchFoodByName(string name)
		{
			List<Food> listFood= FoodDAO.Instance.SearchFoodByName(name);
			return listFood;
		}
		List<Category> SearchCategoryByName(string name)
		{
			List<Category> listCategory = CategoryDAO.Instance.SearchCategoryByName(name);
			return listCategory;
		}
		void LoadListBillByDate(DateTime checkIn,DateTime checkOut)
		{
			DateTime today = DateTime.Now;
			 dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
			dtpFromDate.Value = new DateTime(today.Year, today.Month, 1);
			dtpToDate.Value = dtpFromDate.Value.AddMonths(1).AddDays(-1);
		}
		void LoadListFood()
		{
			foodList.DataSource = FoodDAO.Instance.GetListFood();
		}
		void LoadListCategory()
		{
			dtgvCatagory.DataSource = CategoryDAO.Instance.GetListCategory();
		}
		void LoadListTable()
		{
			dtgvTable.DataSource = TableDAO.Instance.LoadTableList();
		}
		void AddFoodBinding()
		{
			txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true,DataSourceUpdateMode.Never));
			txbFoodId.DataBindings.Add(new Binding("Text",dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
			nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
		}
		void AddTableBinding()
		{
			txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
			txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
			cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
		}
		void AddCategoryBinding()
		{
			textBox2.DataBindings.Add(new Binding("Text",dtgvCatagory.DataSource,"Name",true,DataSourceUpdateMode.Never));
			txbCategory.DataBindings.Add(new Binding("Text",dtgvCatagory.DataSource, "ID", true, DataSourceUpdateMode.Never));
		}
		void LoadCategoryIntoCombobox(ComboBox cb)
		{
			cb.DataSource = CategoryDAO.Instance.GetListCategory();
			cb.DisplayMember = "Name";

		}
		

		#endregion

		#region events
		private void btnSearchFood_Click(object sender, EventArgs e)
		{
			foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
		}
		private void btnSearchCategory_Click(object sender, EventArgs e)
		{
			dtgvCatagory.DataSource = SearchCategoryByName(txbSearchCategory.Text);
		}
		private void btnViewBill_Click(object sender, EventArgs e)
		{
			LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
		}
		
		private void fAdmin_Load(object sender, EventArgs e)
		{
			
		}

		private void btnShowFood_Click(object sender, EventArgs e)
		{
			LoadListFood();
		}

		private void cbFoodCategory_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void cbFoodCategory_TextChanged(object sender, EventArgs e)
		{

		}

		private void txbFoodId_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (dtgvFood.SelectedCells.Count > 0)
				{
					int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
					Category category = CategoryDAO.Instance.GetCategoryByID(id);
					cbFoodCategory.SelectedItem = category;
					int index = -1;
					int i = 0;
					foreach (Category item in cbFoodCategory.Items)
					{
						if (item.ID == category.ID)
						{
							index = i; break;
						}
						i++;
					}
					cbFoodCategory.SelectedIndex = index;
				}
			}
			catch { }
		}

		private void btnAddFood_Click(object sender, EventArgs e)
		{
			string name = txbFoodName.Text;
			int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
			float price = (float)nmFoodPrice.Value;

			if(FoodDAO.Instance.InsertFood(name,categoryID,price))
			{
				MessageBox.Show("Thêm món thành công");
				LoadListFood();
				if (insertFood != null)
				{
					insertFood(this, new EventArgs());
				}
			}
			else
			{
				MessageBox.Show("Có lỗi khi thêm món ăn ");
			}
		}

		private void btnEditFood_Click(object sender, EventArgs e)
		{
			string name = txbFoodName.Text;
			int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
			float price = (float)nmFoodPrice.Value;
			int id = Convert.ToInt32(txbFoodId.Text);

			if (FoodDAO.Instance.UpdateFood(id,name, categoryID, price))
			{
				MessageBox.Show("Sửa món thành công");
				LoadListFood();
				if (updateFood != null)
				{
					updateFood(this, new EventArgs());
				}
			}
			else
			{
				MessageBox.Show("Có lỗi khi Sửa món ăn ");
			}
		}

		private void btnDeleteFood_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(txbFoodId.Text);

			if (FoodDAO.Instance.DeleteFood(id))
			{
				MessageBox.Show("Xóa món thành công");
				LoadListFood();
				if(deleteFood != null)
				{
					deleteFood(this,new EventArgs());
				}	
			}
			else
			{
				MessageBox.Show("Có lỗi khi Xóa món ");
			}
		}
		private event EventHandler insertFood;
		public event EventHandler InsertFood
		{
			add { insertFood += value; }
			remove { insertFood -= value; }
		}
		private event EventHandler deleteFood;
		public event EventHandler DeleteFood
		{
			add { deleteFood += value; }
			remove { deleteFood -= value; }
		}
		private event EventHandler updateFood;
		public event EventHandler UpdateFood
		{
			add { updateFood += value; }
			remove { updateFood -= value; }
		}
		

		#endregion

		private void btnShowCategory_Click(object sender, EventArgs e)
		{
			LoadListCategory();
		}

		private void btnAddCategory_Click(object sender, EventArgs e)
		{
			string name = textBox2.Text;


			if (CategoryDAO.Instance.InsertCategory(name))
			{
				MessageBox.Show("Thêm danh mục thành công");
				LoadListCategory();
			}
			else
			{
				MessageBox.Show("Có lỗi khi thêm danh mục ");
			}
		}

		private void btnEditCategory_Click(object sender, EventArgs e)
		{
			string name = textBox2.Text;
			int id = Convert.ToInt32(txbCategory.Text);


			if (CategoryDAO.Instance.UpdateCategory(id,name)) 
			{
				MessageBox.Show("Sửa danh mục thành công");
				LoadListCategory();
			}
			else
			{
				MessageBox.Show("Có lỗi khi sửa danh mục ");
			}
		}

		private void btnDeleteCategory_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(txbCategory.Text);


			if (CategoryDAO.Instance.DeteleCategory(id)) 
			{
				MessageBox.Show("Xóa danh mục thành công");
				LoadListCategory();
			}
			else
			{
				MessageBox.Show("Không thể xóa danh mục do bên trong còn đồ ăn ");
			}
		}

		private void btnShowTable_Click(object sender, EventArgs e)
		{
			LoadListTable();	
		}

		private void btnAddTable_Click(object sender, EventArgs e)
		{
			string name = txbTableName.Text;


			if (TableDAO.Instance.InsertTable(name))
			{
				MessageBox.Show("Thêm bàn thành công");
				LoadListTable();
			}
			else
			{
				MessageBox.Show("Có lỗi khi thêm bàn ");
			}
		}

		private void btnEditTable_Click(object sender, EventArgs e)
		{
			string name = txbTableName.Text;
			int id = Convert.ToInt32(txbTableID.Text);


			if (TableDAO.Instance.UpdateTable(id, name))
			{
				MessageBox.Show("Cập nhật bàn thành công");
				LoadListTable();
			}
			else
			{
				MessageBox.Show("Có lỗi khi cập nhật bàn ");
			}
		}

		private void btnDeleteTable_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(txbTableID.Text);


			if (TableDAO.Instance.DeleteTable(id))
			{
				MessageBox.Show("Xóa bàn thành công");
				LoadListTable();
			}
			else
			{
				MessageBox.Show("Có lỗi khi xóa bàn do bàn vẫn đang có người ");
			}

		}

		private void btnShowAccount_Click(object sender, EventArgs e)
		{
			LoadAccount();
		}

		private void btnAddAccount_Click(object sender, EventArgs e)
		{
			string userName = txbUserName.Text;
			string displayName = txbDisplayName.Text;
			int type = (int)nmAccountType.Value;

			AddAccount(userName, displayName, type);
		}

		private void btnDeleteAccount_Click(object sender, EventArgs e)
		{
			string userName = txbUserName.Text;

			DeleteAccount(userName);
		}

		private void btnEditAccount_Click(object sender, EventArgs e)
		{
			string userName = txbUserName.Text;
			string displayName = txbDisplayName.Text;
			int type = (int) nmAccountType.Value;

			EditAccount(userName, displayName, type);
		}

	


		private void reportViewer1_Load_1(object sender, EventArgs e)
		{
			
			
		
		}

		private void txbResetPassword_TextChanged(object sender, EventArgs e)
		{

		}

		private void btnResetPassword_Click(object sender, EventArgs e)
		{
			string userName = txbUserName.Text;
			ResetPass(userName);
		}
		
	}
}
