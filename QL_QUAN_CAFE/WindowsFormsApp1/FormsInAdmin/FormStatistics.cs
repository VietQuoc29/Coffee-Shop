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

namespace WindowsFormsApp1.FormsInAdmin
{
    public partial class FormStatistics : Form
    {
        private SqlConnection conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=Test;Integrated Security=True");
        public FormStatistics()
        {
            InitializeComponent();
        }

        private void FormStatistics_Load(object sender, EventArgs e)
        {
            ThongKeDoanhThu();
            comboBox1.Text = Convert.ToString(DateTime.Now.Month);
        }

        private void ThongKeDoanhThu()
        {           
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select month(datecheckin) thang, sum(totalprice) tongdoanhthu from bill group by month(datecheckin)", conn);
            conn.Open();
            da.Fill(dt);
            conn.Close();
            chartDoanhThu.ChartAreas["ChartArea1"].AxisX.Title = "Tháng";
            chartDoanhThu.ChartAreas["ChartArea1"].AxisY.Title = "Doanh thu";
            //chartDoanhThu.Series["Doanh thu"]["DrawingStyle"] = "Cylinder";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                chartDoanhThu.Series["Doanh thu"].Points.AddXY(dt.Rows[i]["thang"], dt.Rows[i]["tongdoanhthu"]);
            }
        }

        private void ThongKeMon(int month)
        {
            chartFood.Series["Series1"].Points.Clear();
            DataTable dt1 = new DataTable();
            SqlDataAdapter da1 = new SqlDataAdapter("select name tenmon, sum(COUNT) soluong from BILLINFO, FOOD, BILL where FOOD.ID=BILLINFO.IDFOOD and bill.ID=BILLINFO.IDBILL and month(DATECHECKIN)='" + month + "' group by name ", conn);
            conn.Open();
            da1.Fill(dt1);
            conn.Close();
            chartFood.ChartAreas["ChartArea1"].AxisX.Title = "Món";
            chartFood.ChartAreas["ChartArea1"].AxisY.Title = "Số lượng";
            chartFood.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                chartFood.Series["Series1"].Points.AddXY(dt1.Rows[i]["tenmon"], dt1.Rows[i]["soluong"]);               
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int month = int.Parse(comboBox1.Text);
            ThongKeMon(month);
        }
    }
}
