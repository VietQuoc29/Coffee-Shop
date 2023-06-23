using NUnit.Framework;
using System;
using WindowsFormsApp1;
using WindowsFormsApp1.Repositories;
using System.Data.SqlClient;

namespace Test
{
    class AddFood
    {
        SqlCommand cm = new SqlCommand();
        FoodRepository repository = new FoodRepository();
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
        public void AddSuccess()
        {
            var result = repository.AddFood("Cơm nắm","3","20000");
            Assert.AreEqual(result, "Success");
        }
        [Test]
        public void AddFail()
        {
            var result = repository.AddFood("","","");
            Assert.AreEqual(result, "emptyfield");
        }
    }
}