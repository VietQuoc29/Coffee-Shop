using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.FormsInAdmin
{
    public partial class FormBill : Form
    {
        public FormBill()
        {
            InitializeComponent();
        }

        private void FormBill_Load(object sender, EventArgs e)
        {
            LoadTheme();
            StyleDatagridview();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        private void LoadTheme()
        {
            label1.ForeColor = ThemeColor.SecondaryColor;
            label2.ForeColor = ThemeColor.PrimaryColor;
            btnViewBill.BackColor = ThemeColor.PrimaryColor;
            btnViewBill.ForeColor = Color.White;
            btnViewBill.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnFirstBillPage.BackColor = ThemeColor.PrimaryColor;
            btnFirstBillPage.ForeColor = Color.White;
            btnFirstBillPage.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnLastBillPage.BackColor = ThemeColor.PrimaryColor;
            btnLastBillPage.ForeColor = Color.White;
            btnLastBillPage.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnNextBillPage.BackColor = ThemeColor.PrimaryColor;
            btnNextBillPage.ForeColor = Color.White;
            btnNextBillPage.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnPreviousBillPage.BackColor = ThemeColor.PrimaryColor;
            btnPreviousBillPage.ForeColor = Color.White;
            btnPreviousBillPage.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
        }

        void StyleDatagridview() //Custome Bảng
        {
            dtgvBill.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dtgvBill.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvBill.DefaultCellStyle.SelectionBackColor = ThemeColor.PrimaryColor;
            dtgvBill.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvBill.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            dtgvBill.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvBill.RowHeadersVisible = false;
            dtgvBill.EnableHeadersVisualStyles = false;
            dtgvBill.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvBill.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dtgvBill.ColumnHeadersHeight = 35;
            dtgvBill.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Bold);
            dtgvBill.ColumnHeadersDefaultCellStyle.BackColor = ThemeColor.SecondaryColor;
            dtgvBill.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeColor.SecondaryColor;
            dtgvBill.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvBill.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }

        void LoadDateTimePickerBill()
        {
            dtpkFromDate.Format = DateTimePickerFormat.Custom;
            dtpkFromDate.CustomFormat = "dd/MM/yyyy";
            dtpkToDate.Format = DateTimePickerFormat.Custom;
            dtpkToDate.CustomFormat = "dd/MM/yyyy";
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = today;
            dtpkToDate.Value = dtpkFromDate.Value.AddDays(+1);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
                lastPage++;

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
                page--;

            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillListByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page < sumRecord)
                page++;

            txbPageBill.Text = page.ToString();
        }
    }
}
