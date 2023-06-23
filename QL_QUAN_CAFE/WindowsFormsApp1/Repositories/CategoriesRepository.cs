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
    public class CategoryRepository
    {
        SqlCommand cm = new SqlCommand();
        dbConnect dbcon = new dbConnect();
        public string AddCategory(string name)
        {
            if (CheckField(name))
            {
                try
                {
                    cm = new SqlCommand("INSERT INTO FOODCATEGORY(name)VALUES(@name)", dbcon.connect());
                    cm.Parameters.AddWithValue("@name", name);

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
        public bool CheckField(string name)
        {
            if (name == "")
            {
                return false;
            }
            else return true;
        }
    }
}
