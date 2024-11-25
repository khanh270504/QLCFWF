using AppQuanLyQuanCaPhe.DAO;
using AppQuanLyQuanCaPhe.DTO;
using Microsoft.Reporting.WinForms;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

using Excel = Microsoft.Office.Interop.Excel;

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
			//foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
			string searchText = txbSearchFoodName.Text.Trim();

			
			if (string.IsNullOrEmpty(searchText))
			{
				MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			
			if (int.TryParse(searchText, out _))
			{
				MessageBox.Show("Từ khóa tìm kiếm không được là số. Vui lòng nhập tên món ăn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			List<Food> listFood = SearchFoodByName(searchText);
			if (listFood.Count == 0)
			{
				MessageBox.Show("Không tìm thấy món ăn phù hợp với từ khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}		
			foodList.DataSource = listFood;
		}
		private void btnSearchCategory_Click(object sender, EventArgs e)
		{
			//dtgvCatagory.DataSource = SearchCategoryByName(txbSearchCategory.Text);
			string searchText = txbSearchCategory.Text.Trim();

			if (string.IsNullOrEmpty(searchText))
			{
				MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (int.TryParse(searchText, out _))
			{
				MessageBox.Show("Từ khóa tìm kiếm không được là số. Vui lòng nhập tên danh mục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			var categoryList = SearchCategoryByName(searchText);

			if (categoryList == null || categoryList.Count == 0)
			{
				MessageBox.Show("Không tìm thấy danh mục phù hợp với từ khóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			dtgvCatagory.DataSource = categoryList;
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


        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
           
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;

          
            Excel.Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Excel.Worksheet worksheet = workbook.Sheets[1];
            worksheet.Name = "Bảng thống kê";

          
            int columnCount = dtgvBill.Columns.Count;

            
            worksheet.Cells[1, 1] = "BẢNG THỐNG KÊ DOANH THU QUÁN CÀ PHÊ";
            Excel.Range titleRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, columnCount]]; 
            titleRange.Merge(); // Gộp ô
            titleRange.Font.Size = 16;
            titleRange.Font.Bold = true;
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            titleRange.WrapText = true;  // Đảm bảo văn bản được xuống dòng khi quá dài

            
            worksheet.Cells[2, 1] = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy}";
            Excel.Range dateRange = worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[2, columnCount]];
            dateRange.Merge(); // Gộp ô
            dateRange.Font.Size = 12;
            dateRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            dateRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

           
            Excel.Range headerRange = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[4, columnCount]];
            headerRange.Font.Bold = true;
            headerRange.Font.Size = 12;
            headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

            
            for (int i = 1; i <= dtgvBill.Columns.Count; i++)
            {
                worksheet.Cells[4, i] = dtgvBill.Columns[i - 1].HeaderText;
            }

          
            for (int i = 0; i < dtgvBill.Rows.Count; i++)
            {
                for (int j = 0; j < dtgvBill.Columns.Count; j++)
                {
                    var cellValue = dtgvBill.Rows[i].Cells[j].Value;
                    var cell = worksheet.Cells[i + 5, j + 1];

                   
                    if (cellValue is int || cellValue is decimal || cellValue is double)
                    {
                        cell.Value = cellValue;
                    }
                    else if (cellValue is DateTime)
                    {
                        cell.NumberFormat = "dd/MM/yyyy"; 
                        cell.Value = cellValue;
                    }
                    else
                    {
                        cell.Value = cellValue?.ToString(); 
                    }
                }
            }

           
            Excel.Range dataRange = worksheet.Range[worksheet.Cells[4, 1], worksheet.Cells[dtgvBill.Rows.Count + 4, columnCount]];
            dataRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            dataRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

            
            worksheet.Columns.AutoFit();

           
            MessageBox.Show("Xuất dữ liệu ra Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }





    }

}

