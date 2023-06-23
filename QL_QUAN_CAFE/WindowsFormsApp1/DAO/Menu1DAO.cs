using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    public class Menu1DAO
    {
       private static Menu1DAO instance;

        public static Menu1DAO Instance
        {
            get { if (instance == null) instance = new Menu1DAO(); return instance; }
            private set { Menu1DAO.instance = value; }
        }
        private Menu1DAO() { }
        public List<Menu1> GetListMenuByTable(int id)
        {
            List<Menu1> listMenu = new List<Menu1>();
            DataTable data = DataProvider.Instance.ExecuteQuery("select fod.NAME,bilinf.COUNT,fod.PRICE,fod.PRICE*bilinf.COUNT as TotalPrice from dbo.BILLINFO as bilinf, BILL as bil,dbo.FOOD as fod where bilinf.IDBILL = bil.ID and bilinf.IDFOOD = fod.ID and bil.STATUS=0 and bil.IDTABLE = " + id);
            foreach(DataRow item in data.Rows)
            {
                Menu1 menu = new Menu1(item);
                listMenu.Add(menu);

            }
            return listMenu;
        }
    }
}
