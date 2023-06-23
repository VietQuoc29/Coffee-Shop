using NUnit.Framework;
using WindowsFormsApp1;
using System.Data.SqlClient;
using System;

namespace Test
{
    [TestFixture]
    public class Login
    {
        SqlCommand cm = new SqlCommand();
        private SqlConnection cn = new SqlConnection(@"Data Source=QUOCTRAN\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True");
        public SqlDataReader dr;
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
        public int Query(string sql)
        {
            try
            {
                open();
                cm = new SqlCommand(sql, connect());
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    close();
                    return 1;

                }
                else
                {
                    close();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Fail()
        {
            Assert.Less(Query("SELECT username FROM Account WHERE username ='no' AND password ='no'"),1);
        }

        [Test]
        public void AdminSucess()
        {
            Assert.Greater(Query("SELECT username FROM Account WHERE username ='N2' AND password ='1' AND type = '1'"), 0);
        }

        [Test]
        public void StaffSuccess()
        {
            Assert.Greater(Query("SELECT username FROM Account WHERE username ='staff' AND password ='1' AND type ='0' "), 0);
        }
    }
}