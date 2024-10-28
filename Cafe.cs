using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TienSuToCoffee
{
    public partial class Cafe : UserControl
    {
        MYCOFFEEEntitiesS me = DataContextSingleton.Instance;
        public Cafe()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Cafe_Load(object sender, EventArgs e)
        {
            LoadData1();
            LoadCategories();
            AddBinding();
        }
        void LoadData1()
        {
            var result = me.Foods.Select(c => new { c.id, c.name, c.idCategory, c.price });
            dgvFood.DataSource = result.ToList();

        }

        void LoadCategories()
        {
            // Truy vấn để lấy danh sách danh mục thực phẩm
            var categories = me.FoodCategories.Select(c => new { c.id, c.name }).ToList();

            // Gán dữ liệu vào ComboBox
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "name"; // Hiển thị tên trong ComboBox
            cmbCategory.ValueMember = "id";      // Lưu id khi chọn

        }

        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadData1();
        }

        void AddBinding()
        {
            txtIDFood.DataBindings.Clear();
            txtDish.DataBindings.Clear();
            cmbCategory.DataBindings.Clear();
            txtPriceFood.DataBindings.Clear();

            // Thêm Binding mới
            txtIDFood.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "ID"));
            txtDish.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "name"));
           // cmbCategory.DataBindings.Add(new Binding("SelectedValue", dgvFood.DataSource, "idCategory"));
            txtPriceFood.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "price"));

        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {

            int id = int.Parse(txtIDFood.Text);
            string foodName = txtDish.Text;
            int categoryID = (int)cmbCategory.SelectedValue;  // id của danh mục được chọn
            float price = float.Parse(txtPriceFood.Text);

            // Tạo đối tượng Food mới
            Food newFood = new Food
            {
                id = id,
                name = foodName,
                idCategory = categoryID,
                price = price
            };

            // Thêm món ăn mới vào cơ sở dữ liệu
            me.Foods.Add(newFood);
            me.SaveChanges();

            // Tải lại dữ liệu để hiển thị món ăn mới trong DataGridView
            LoadData1();

            // Cập nhật lại binding để hiển thị dữ liệu mới
            AddBinding();

            // Chọn dòng cuối cùng trong DataGridView để hiển thị thông tin mới
            if (dgvFood.Rows.Count > 0)
            {
                dgvFood.Rows[dgvFood.Rows.Count - 1].Selected = true;
                dgvFood.CurrentCell = dgvFood.Rows[dgvFood.Rows.Count - 1].Cells[0]; // Chọn ô đầu tiên của dòng cuối cùng
            }

            MessageBox.Show("Món ăn đã được thêm thành công!");
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {

            // Kiểm tra xem ô text ID món ăn có giá trị hay không
            if (!string.IsNullOrEmpty(txtIDFood.Text))
            {
                // Chuyển đổi giá trị ID món ăn thành kiểu int
                int foodID = int.Parse(txtIDFood.Text);

                // Tìm món ăn cần xóa trong cơ sở dữ liệu dựa trên ID
                var foodToDelete = me.Foods.SingleOrDefault(f => f.id == foodID);

                if (foodToDelete != null)
                {
                    // Xóa món ăn khỏi cơ sở dữ liệu
                    me.Foods.Remove(foodToDelete);
                    me.SaveChanges();

                    // Cập nhật lại DataGridView sau khi xóa
                    LoadData1();

                    // Cập nhật lại binding để hiển thị dữ liệu mới
                    AddBinding();

                    // Chọn dòng đầu tiên sau khi xóa
                    if (dgvFood.Rows.Count > 0)
                    {
                        dgvFood.Rows[0].Selected = true;
                        dgvFood.CurrentCell = dgvFood.Rows[0].Cells[0]; // Chọn ô đầu tiên của dòng đầu tiên
                    }

                    // Thông báo xóa thành công
                    MessageBox.Show("Món ăn đã được xóa thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy món ăn để xóa.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món ăn để xóa.");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtIDFood.Text))
            {
                // Chuyển đổi giá trị ID món ăn thành kiểu int
                int foodID = int.Parse(txtIDFood.Text);

                // Tìm món ăn cần sửa trong cơ sở dữ liệu dựa trên ID
                var foodToEdit = me.Foods.SingleOrDefault(f => f.id == foodID);

                if (foodToEdit != null)
                {
                    // Cập nhật thông tin món ăn
                    foodToEdit.name = txtDish.Text;
                    foodToEdit.idCategory = (int)cmbCategory.SelectedValue;  // id của danh mục được chọn
                    foodToEdit.price = float.Parse(txtPriceFood.Text);  // Giá trị từ ô nhập

                    // Lưu thay đổi vào cơ sở dữ liệu
                    me.SaveChanges();

                    // Tải lại dữ liệu để hiển thị thông tin đã sửa trong DataGridView
                    LoadData1();

                    // Cập nhật lại binding để hiển thị dữ liệu mới
                    AddBinding();

                    // Chọn dòng đã sửa trong DataGridView
                    if (dgvFood.Rows.Count > 0)
                    {
                        dgvFood.Rows[dgvFood.CurrentRow.Index].Selected = true;
                        dgvFood.CurrentCell = dgvFood.Rows[dgvFood.CurrentRow.Index].Cells[0]; // Chọn ô đầu tiên của dòng hiện tại
                    }

                    // Thông báo sửa thành công
                    MessageBox.Show("Món ăn đã được sửa thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy món ăn để sửa.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món ăn để sửa.");
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchFood.Text.Trim(); // Lấy từ khóa tìm kiếm và loại bỏ khoảng trắng

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Tìm các món ăn có tên bắt đầu bằng từ khóa tìm kiếm
                var searchResult = me.Foods
                    .Where(f => f.name.StartsWith(searchTerm))
                    .Select(c => new { c.id, c.name, c.idCategory, c.price })
                    .ToList();

                // Cập nhật DataGridView với kết quả tìm kiếm
                dgvFood.DataSource = searchResult;

                if (searchResult.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy món ăn nào tương ứng.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm.");
            }
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
