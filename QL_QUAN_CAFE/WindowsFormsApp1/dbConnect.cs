using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    //to get connection string between application and database   
    public class dbConnect
    {
        SqlCommand cm = new SqlCommand();
        private SqlConnection cn = new SqlConnection(@"Data Source=QUOCTRAN\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True");
        public SqlConnection connect()
        {
            return cn;
        }

        public void open()
        {
            if (cn.State == System.Data.ConnectionState.Closed)
                cn.Open();
        }

        public void close()
        {
            if (cn.State == System.Data.ConnectionState.Open)
                cn.Close();
        }

        public void executeQuery(string sql)
        {
            try
            {
                open();
                cm = new SqlCommand(sql, connect());
                cm.ExecuteNonQuery();
                close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Coffee Shop");
            }
        }
        public int Query(string sql)
        {
            try
            {
                open();
                cm = new SqlCommand(sql, connect());
                int res = cm.ExecuteNonQuery();
                close();
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Coffee Shop");
                return -1;
            }
        }
    }
}