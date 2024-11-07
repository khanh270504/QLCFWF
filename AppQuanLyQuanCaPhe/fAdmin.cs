using AppQuanLyQuanCaPhe.DAO;
using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AppQuanLyQuanCaPhe
{
	public partial class fAdmin : Form
	{
		BindingSource foodList = new BindingSource();
		public fAdmin()
		{
			InitializeComponent();
			dtgvFood.DataSource = foodList;

			LoadListBillByDate(dtpFromDate.Value, dtpToDate.Value);
			LoadListFood();
			LoadCategoryIntoCombobox(cbFoodCategory);
			AddFoodBinding();


		}
		
		#region methos
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
		void AddFoodBinding()
		{
			txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true,DataSourceUpdateMode.Never));
			txbFoodId.DataBindings.Add(new Binding("Text",dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
			nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
		}
		void LoadCategoryIntoCombobox(ComboBox cb)
		{
			cb.DataSource = CategoryDAO.Instance.GetListCategory();
			cb.DisplayMember = "Name";

		}

		#endregion

		#region events
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

	}
}
