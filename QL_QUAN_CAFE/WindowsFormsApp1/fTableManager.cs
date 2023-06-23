using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class fTableManager : Form
    {
        //Tạo thư viện user32.DLL để kéo thả di chuyển form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        //

        private Account loginAccount;

        public Account LoginAccount { get => loginAccount; set { loginAccount = value; ChangeAccount(loginAccount.Type); } }

        public fTableManager(Account acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
            label1.Text = "    " + DateTime.Now.ToString();
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
        }

        #region Method

        void ChangeAccount(int type)
        {
            btnOpenFormAdmin.Enabled = type == 1;
            lblDispalyName.Text = "(" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }
        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach(Table item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth,  Height=TableDAO.TableHeight };
                btn.Click += Btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "0":
                        {
                            item.Status = "Trống";
                            btn.BackColor = Color.FromArgb(255, 243, 35);
                            btn.BackgroundImage = Resources.round_table__1_;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                            break;
                        }
                    default:
                        {
                            item.Status = "Đã có khách";
                            btn.BackColor = Color.Beige;
                            btn.BackgroundImage = Resources.round_table__1_;
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                            break;
                        }
                }
                btn.Text = item.Name + Environment.NewLine + item.Status;
                flpTable.Controls.Add(btn);
                
              
            }
        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<Menu1> listMenu = Menu1DAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach(Menu1 item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                lsvBill.Items.Add(lsvItem);
                totalPrice += item.TotalPrice;
            }
            totalPrice = totalPrice - (totalPrice * (float)nmDiscount.Value / 100);
            CultureInfo culture = new CultureInfo("vi-VN");
            txbTotalPrice.Text = totalPrice.ToString("c",culture);
        }

        void LoadComboboxTable (ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion
        #region Event
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableID);
        }
        private void btnOpenFormAdmin_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin(LoginAccount);
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;
            f.ShowDialog();
        }
        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
            { ShowBill((lsvBill.Tag as Table).ID); }
            LoadTable();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", ""));
            double finalTotalPrice = totalPrice;

            if (idBill != -1)
            {
                if(MessageBox.Show(string.Format("Bạn có chắc thanh toán hoá đơn cho {0}", table.Name),"Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Information)==System.Windows.Forms.DialogResult.OK)
                {
                    printPreviewDialog1.Document = printDocument1;
                    printPreviewDialog1.ShowDialog();
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    
                    ShowBill(table.ID);
                    LoadTable();
                }    
            }          
        }
        //private void SnapShotPNG(ListView source, string destination)
        //{
        //    var lastHeight = source.Height;
        //    source.Height = 10000;
        //    Graphics canvas = source.CreateGraphics();
        //    Bitmap bmp = new Bitmap(1000, 1000, canvas);
        //    source.DrawToBitmap(bmp, new Rectangle(0, 0, 1000, 1000));
        //    bmp.Save(destination);
        //    source.Height = lastHeight;
        //}
        private void btnOpenFormProfile_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        void f_UpdateAccount(object sender, AccountEvent e)
        {
           lblDispalyName.Text = "(" + e.Acc.DisplayName + ")";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text ="    " + DateTime.Now.ToString();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {

                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }

            ShowBill(table.ID);
            LoadTable();
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;
            int id2 = (cbSwitchTable.SelectedItem as Table).ID;

            if(MessageBox.Show(String.Format("Bạn có thật sự muốn chuyển {0} qua {1}?", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),"Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {              
                    TableDAO.Instance.SwitchTable(id1, id2);
                    LoadTable();                       
            }    
        }

        private void fTableManager_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode.Equals(Keys.T))
                {
                    btnCheck_Click(this, new EventArgs());
                }
            }
        }




        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            LoadTable();
            LoadCategory();
            LoadComboboxTable(cbSwitchTable);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
           
            List<Menu1> listMenu = Menu1DAO.Instance.GetListMenuByTable((lsvBill.Tag as Table).ID);
            CultureInfo culture = new CultureInfo("vi-VN");
            
            e.Graphics.DrawString("HÓA ĐƠN BÁN HÀNG", new Font("Bookman Old Style", 24, FontStyle.Bold), Brushes.Black, new Point(255, 100));
            e.Graphics.DrawString("Ngày hóa đơn: " + DateTime.Now.ToShortDateString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(25, 160));
            e.Graphics.DrawString("Tên nhân viên: " + LoginAccount.DisplayName.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(25, 190));
            e.Graphics.DrawString("Tên bàn: " + (lsvBill.Tag as Table).Name.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(25, 220));            
            e.Graphics.DrawString("------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 235));
            e.Graphics.DrawString("Tên món ăn: ", new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(30, 255));
            e.Graphics.DrawString("Số lượng: ", new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(380, 255));
            e.Graphics.DrawString("Đơn giá: ", new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(510, 255));
            e.Graphics.DrawString("Thành tiền: ", new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(660, 255));
            e.Graphics.DrawString("------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, 270));
            int yPos = 295;
            float totalPrice=0;
            foreach (Menu1 item in listMenu)
            {
                e.Graphics.DrawString(item.FoodName.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(30, yPos));
                e.Graphics.DrawString(item.Count.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(405, yPos));
                e.Graphics.DrawString(item.Price.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(525, yPos));
                e.Graphics.DrawString(item.TotalPrice.ToString(), new Font("Bookman Old Style", 12, FontStyle.Regular), Brushes.Black, new Point(675, yPos));
                totalPrice += item.TotalPrice;
                yPos += 30;

            }
            e.Graphics.DrawString("------------------------------------------------------------------------------------------------------------------------------------", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(25, yPos));
            e.Graphics.DrawString("Tổng tiền: "+ totalPrice.ToString("c", culture), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(600, yPos+30));
            e.Graphics.DrawString("Giảm giá: " + nmDiscount.Value.ToString(culture)+"%", new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(600, yPos + 60));
            e.Graphics.DrawString("Số tiền phải trả: " + txbTotalPrice.Text.ToString(culture), new Font("Arial", 12, FontStyle.Regular), Brushes.Black, new Point(600, yPos + 90)); 
        }
       
        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }

        private void nmDiscount_ValueChanged(object sender, EventArgs e)
        {
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }
    }
}
