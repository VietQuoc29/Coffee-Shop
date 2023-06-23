using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.FormsInAdmin
{
    public partial class FormAccount : Form
    {
        BindingSource AccountList = new BindingSource();
        public Account loginAccount;
        public FormAccount(Account acc)
        {
            InitializeComponent();
            this.loginAccount = acc;
            dtgvAccount.DataSource = AccountList;
            //LoadAcountList();
            AddAccountBinding();
            LoadAccount();
        }

        private void FormAccount_Load(object sender, EventArgs e)
        {
            LoadTheme();
            StyleDatagridview();
        }

        private void LoadTheme()
        {
            btnAddAccount.BackColor = ThemeColor.PrimaryColor;
            btnAddAccount.ForeColor = Color.White;
            btnAddAccount.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnDeleteAccount.BackColor = ThemeColor.PrimaryColor;
            btnDeleteAccount.ForeColor = Color.White;
            btnDeleteAccount.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnEditAccount.BackColor = ThemeColor.PrimaryColor;
            btnEditAccount.ForeColor = Color.White;
            btnEditAccount.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnResetPassword.BackColor = ThemeColor.PrimaryColor;
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnShowAccount.BackColor = ThemeColor.PrimaryColor;
            btnShowAccount.ForeColor = Color.White;
            btnShowAccount.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
        }

        void StyleDatagridview() //Custome Bảng
        {
            dtgvAccount.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dtgvAccount.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvAccount.DefaultCellStyle.SelectionBackColor = ThemeColor.PrimaryColor;
            dtgvAccount.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvAccount.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            dtgvAccount.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvAccount.RowHeadersVisible = false;
            dtgvAccount.EnableHeadersVisualStyles = false;
            dtgvAccount.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvAccount.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dtgvAccount.ColumnHeadersHeight = 40;
            dtgvAccount.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Bold);
            dtgvAccount.ColumnHeadersDefaultCellStyle.BackColor = ThemeColor.SecondaryColor;
            dtgvAccount.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeColor.SecondaryColor;
            dtgvAccount.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvAccount.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAcountList()
        {
            string query = "exec dbo.USP_GetListAccountByUserName @username ";

            dtgvAccount.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { "staff"});

        }

        void LoadAccount()
        {
            AccountList.DataSource = AccountDAO.Instance.getListAccount();
            dtgvAccount.Columns[0].HeaderText = "Tên đăng nhập";
            dtgvAccount.Columns[0].DataPropertyName = "UserName";
            dtgvAccount.Columns[1].HeaderText = "Tên người dùng";
            dtgvAccount.Columns[1].DataPropertyName = "DisplayName";
            dtgvAccount.Columns[2].HeaderText = "Loại người dùng";
            dtgvAccount.Columns[2].DataPropertyName = "Type";
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại");
            }
            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
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
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }
            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmType.Value;
            AddAccount(userName, displayName, type);
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập");
            }
            else
            {
                DeleteAccount(userName);
            }
            
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmType.Value;
            EditAccount(userName, displayName, type);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            ResetPass(userName);
        }
    }
}
