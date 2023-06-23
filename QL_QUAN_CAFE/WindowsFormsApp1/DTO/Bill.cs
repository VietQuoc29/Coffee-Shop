using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.DTO
{
   public class Bill
    {
        int id;
        private DateTime? dateCheckin;
        private DateTime? dateCheckOut;
        private int status;
        private int discount;
        public Bill(int id,DateTime? dateCheckin,DateTime? dateCheckOut,int status, int discount = 0)
        {
            this.Id = id;
            this.DateCheckin = dateCheckin;
            this.DateCheckOut = dateCheckOut;
            this.Status = status;
            this.Discount = discount;
        }
        public Bill(DataRow row)
        {
            this.Id = (int)row["id"];
            this.DateCheckin = (DateTime)row["dateCheckin"];
            var dateCheckOutTemp = row["dateCheckOut"];
            if (dateCheckOutTemp.ToString() != "")
            {
                this.DateCheckOut = (DateTime)dateCheckOutTemp;
            }        
            this.Status = (int)row["status"];
            if (row["discount"].ToString() !="")
                this.Discount = (int)row["discount"];
        }
       

        public DateTime? DateCheckin { get => dateCheckin; set => dateCheckin = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int Id { get => id; set => id = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}
