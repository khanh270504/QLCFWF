﻿using AppQuanLyQuanCaPhe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppQuanLyQuanCaPhe
{
	public partial class fLogin : Form
	{
		public fLogin()
		{
			InitializeComponent();
		}

		private void btnLogin_Click(object sender, EventArgs e)
		{
			string userName = txbLogin.Text;
			string passWord = txbPassWord.Text;
			if (Login(userName,passWord))
			{
				Account loginAccount = AccountDAO.Instace.GetAccountByUserName(userName);
				fTableManager f = new fTableManager(loginAccount);
				this.Hide();
				f.ShowDialog();
				this.Show();
			}
			else
			{
				MessageBox.Show("Sai tên tài khoản hoặc mật khẩu !!!", "Thông báo");
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MessageBox.Show("Bạn có thực sự muốn thoát chương trình?",
					"Thông báo",
					MessageBoxButtons.OKCancel,
					MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
			{
				e.Cancel = true;
			}

		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
		bool Login(string userName ,string passWord)
		{
			return AccountDAO.Instace.Login(userName,passWord);
		}

		private void txbLogin_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
