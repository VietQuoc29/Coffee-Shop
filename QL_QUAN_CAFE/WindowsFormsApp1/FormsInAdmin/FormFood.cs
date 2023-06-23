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
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.FormsInAdmin
{
    public partial class FormFood : Form
    {
        BindingSource foodList = new BindingSource();
        public FormFood()
        {
            InitializeComponent();
        }

        private void FormFood_Load(object sender, EventArgs e)
        {
            dtgvFood.DataSource = foodList;
            LoadTheme();
            StyleDatagridview();
            LoadListFood();
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbFoodCategory);
        }
        private void LoadTheme()
        {
            btnAddFood.BackColor = ThemeColor.PrimaryColor;
            btnAddFood.ForeColor = Color.White;
            btnAddFood.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnDeleteFood.BackColor = ThemeColor.PrimaryColor;
            btnDeleteFood.ForeColor = Color.White;
            btnDeleteFood.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnEditFood.BackColor = ThemeColor.PrimaryColor;
            btnEditFood.ForeColor = Color.White;
            btnEditFood.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnSearchFood.BackColor = ThemeColor.PrimaryColor;
            btnSearchFood.ForeColor = Color.White;
            btnSearchFood.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
            btnShowFood.BackColor = ThemeColor.PrimaryColor;
            btnShowFood.ForeColor = Color.White;
            btnShowFood.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;

        }

        void StyleDatagridview() //Custome Bảng
        {
            dtgvFood.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            dtgvFood.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dtgvFood.DefaultCellStyle.SelectionBackColor = ThemeColor.PrimaryColor;
            dtgvFood.DefaultCellStyle.SelectionForeColor = Color.White;
            dtgvFood.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            dtgvFood.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvFood.RowHeadersVisible = false;
            dtgvFood.EnableHeadersVisualStyles = false;
            dtgvFood.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgvFood.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dtgvFood.ColumnHeadersHeight = 40;
            dtgvFood.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 11, FontStyle.Bold);
            dtgvFood.ColumnHeadersDefaultCellStyle.BackColor = ThemeColor.SecondaryColor;
            dtgvFood.ColumnHeadersDefaultCellStyle.SelectionBackColor = ThemeColor.SecondaryColor;
            dtgvFood.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dtgvFood.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
            dtgvFood.Columns[0].HeaderText = "Mã món ăn";
            dtgvFood.Columns[0].DataPropertyName = "ID";
            dtgvFood.Columns[1].HeaderText = "Tên món ăn";
            dtgvFood.Columns[1].DataPropertyName = "Name";
            dtgvFood.Columns[2].HeaderText = "Mã danh mục";
            dtgvFood.Columns[2].DataPropertyName = "CategoryID";
            dtgvFood.Columns[3].HeaderText = "Giá tiền";
            dtgvFood.Columns[3].DataPropertyName = "Price";
        }

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value != null)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;
                    Category category = CategoryDAO.Instance.GetCategoryByID(id);
                    cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;

                }
            }
            catch  { MessageBox.Show("không tìm thấy món ăn"); }

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
    }
}
