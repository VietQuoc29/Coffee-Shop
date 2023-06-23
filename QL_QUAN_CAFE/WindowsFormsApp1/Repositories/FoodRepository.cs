using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;

namespace WindowsFormsApp1.Repositories
{
    public class FoodRepository
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        public string AddFood(string name,string idcategory, string price)
        {
            if (CheckField(name,idcategory,price))
            {
                try
                {
                    cm = new SqlCommand("INSERT INTO FOOD(name,idcategory,price)VALUES(@name,@idcategory,@price)", dbcon.connect());
                    cm.Parameters.AddWithValue("@name", name);
                    cm.Parameters.AddWithValue("@idcategory", idcategory);
                    cm.Parameters.AddWithValue("@price", price);

                    dbcon.open();
                    cm.ExecuteNonQuery();
                    dbcon.close();
                    return "Success";
                }
                catch
                {
                    return "Exception";
                }
            }
            else
            {
                return "emptyfield";
            }
        }
        public bool CheckField(string name, string idcategory, string price)
        {
            if (name == "" || idcategory == "" || price == "")
            {
                return false;
            }
            else return true;
        }
    }
}
