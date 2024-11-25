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

using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
namespace AppQuanLyQuanCaPhe
{
	public partial class fTableManager : Form
	{
		private Account loginAccount;

		public Account LoginAccount 
		{ 
			get { return loginAccount; }
			set {  loginAccount = value;ChangeAccount(LoginAccount.Type);  }
		}

		public fTableManager(Account acc)
		{
			InitializeComponent();

			this.LoginAccount = acc;
			LoadTable();
			LoadCategory();
			LoadComboboxTable(cbSwitchTable);
		}

		#region Method
		void ChangeAccount(int type)
		{
			adminToolStripMenuItem.Enabled = type == 1;
			đăngNhậpToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
		}
		
		
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
		void LoadComboboxTable(ComboBox cb)
		{
			cb.DataSource = TableDAO.Instance.LoadTableList();
			cb.DisplayMember = "Name";
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
			fAccountProfile f = new fAccountProfile(loginAccount);
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
			f.loginAccount = LoginAccount;
			f.InsertFood += F_InsertFood;
			f.DeleteFood += F_DeleteFood;
			f.UpdateFood += F_UpdateFood;
			f.ShowDialog();
		}

		private void F_InsertFood(object sender, EventArgs e)
		{
			LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
			if(lsvBill.Tag != null)
			ShowBill((lsvBill.Tag as Table).ID);
		}

		private void F_UpdateFood(object sender, EventArgs e)
		{
			LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
			if (lsvBill.Tag != null)
				ShowBill((lsvBill.Tag as Table).ID);
		}

		private void F_DeleteFood(object sender, EventArgs e)
		{
			LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
			if (lsvBill.Tag != null)
				ShowBill((lsvBill.Tag as Table).ID);
			LoadTable();
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
			if(table == null)
			{
				MessageBox.Show("Hãy chọn bàn");
				return;
			}	
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
            // Lấy thông tin từ bàn và hóa đơn
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            List<AppQuanLyQuanCaPhe.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(table.ID);
            int discount = (int)nmDisCount.Value;
            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc chắn muốn thanh toán hóa đơn cho bàn {0} \n Với Giá gốc là {1} đ , Giảm giá là {2} % \n Số tiền phải thanh toán là {3} đ ", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo !!!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    // Cập nhật tình trạng hóa đơn là đã thanh toán
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);

                    // Hiển thị lại hóa đơn và bàn
                    ShowBill(table.ID);
                    LoadTable();
					ExportBillToExcel(table.Name, totalPrice, discount, finalTotalPrice, listBillInfo);
                }
            }
        }
        private void ExportBillToExcel(string tableName, double totalPrice, int discount, double finalTotalPrice, List<AppQuanLyQuanCaPhe.DTO.Menu> listBillInfo)
        {
            // Khởi tạo ứng dụng Excel
            var excelApp = new Excel.Application();
            excelApp.Visible = true;

            // Tạo Workbook và Worksheet
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Sheets[1];
            worksheet.Name = "Hóa đơn";

            // Đặt tiêu đề và định dạng cho các ô
            worksheet.Cells[1, 1] = "HÓA ĐƠN BÁN HÀNG";
            Excel.Range titleRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]];
            titleRange.Merge();
            titleRange.Font.Size = 16;
            titleRange.Font.Bold = true;
            titleRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            titleRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);
            titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            titleRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

            worksheet.Cells[2, 1] = "Bàn: " + tableName;
            worksheet.Cells[3, 1] = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy}";

            // Thêm tiêu đề cột cho chi tiết hóa đơn
            worksheet.Cells[8, 1] = "Tên món ăn";
            worksheet.Cells[8, 2] = "Số lượng";
            worksheet.Cells[8, 3] = "Đơn giá";
            worksheet.Cells[8, 4] = "Tổng tiền";

            // Định dạng tiêu đề cột
            Excel.Range headerRange = worksheet.Range[worksheet.Cells[8, 1], worksheet.Cells[8, 4]];
            headerRange.Font.Bold = true;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

            // Thêm thông tin chi tiết hóa đơn
            int row = 9; // Bắt đầu từ dòng 9 (sau tiêu đề)
            foreach (var item in listBillInfo)
            {
                worksheet.Cells[row, 1] = item.FoodName;
                worksheet.Cells[row, 2] = item.Count;
                worksheet.Cells[row, 3] = string.Format("{0:N0}", item.Price);  // Định dạng giá
                worksheet.Cells[row, 4] = string.Format("{0:N0}", item.TotalPrice); // Định dạng tổng tiền
                row++;
            }

            // Thêm giá gốc, giảm giá và tổng tiền phải thanh toán ở dưới bảng chi tiết hóa đơn
            worksheet.Cells[row + 1, 1] = $"Giá gốc: {string.Format("{0:N0}", totalPrice)} đ";
            worksheet.Cells[row + 2, 1] = $"Giảm giá: {discount} %";
            worksheet.Cells[row + 3, 1] = $"Tổng tiền phải thanh toán: {string.Format("{0:N0}", finalTotalPrice)} đ";

            // Định dạng các ô thông tin tổng tiền
            worksheet.Range[worksheet.Cells[row + 1, 1], worksheet.Cells[row + 3, 1]].Font.Bold = true;
            worksheet.Range[worksheet.Cells[row + 1, 1], worksheet.Cells[row + 3, 1]].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
            worksheet.Range[worksheet.Cells[row + 1, 1], worksheet.Cells[row + 3, 1]].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

            // Định dạng cột và các ô
            worksheet.Columns.AutoFit();
            worksheet.Rows.AutoFit();

            // Thêm đường viền cho các ô chi tiết hóa đơn
            Excel.Range detailRange = worksheet.Range[worksheet.Cells[9, 1], worksheet.Cells[row - 1, 4]];
            detailRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

          
            // Thông báo đã lưu thành công
            MessageBox.Show($"Hóa đơn đã được xuất ra file Excel!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        private void btnSwitchTable_Click(object sender, EventArgs e)
		{
			
			int id1 = (lsvBill.Tag as Table).ID;

			int id2 = (cbSwitchTable.SelectedItem as Table).ID;
			if (MessageBox.Show(string.Format("Bạn có thực sự muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông Báo",MessageBoxButtons.OKCancel)==System.Windows.Forms.DialogResult.OK)
			{

				TableDAO.Instance.SwitchTable(id1, id2);
				LoadTable();
			}


		}
		#endregion
	
	}
}
