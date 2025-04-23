using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LapStore.Controller;
using LapStore.View;
using LapStore.Widget;
using LapStore.Widget.Admin;

namespace LapStore
{
    public partial class adminHome : Form
    {
        public adminHome()
        {
            InitializeComponent();
            danhMucUserControl uc = new danhMucUserControl();
            AddUserControl(uc);
        }
     
        private void AddUserControl(System.Windows.Forms.UserControl uc)
        {
            // Xóa các control cũ trong panel
            panelChuyen.Controls.Clear();

            // Thiết lập Dock để chiếm toàn bộ không gian
            uc.Dock = DockStyle.Fill;

            // Thêm UserControl vào panel
            panelChuyen.Controls.Add(uc);
            
            uc.BringToFront();
        }
        private void SetUserImage(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                imageAdmin.ImageLocation = imagePath;
            }
            else
            {
                imageAdmin.Image = null;
                imageAdmin.BackColor = Color.Gray; // Nếu không có ảnh, đổi màu PictureBox thành đỏ
            }
        }

     
       

        private void adminHome_Load(object sender, EventArgs e)
        {
            if (UserController.CurrentUser != null)
            {
                txt_tenAdmin.Text = UserController.CurrentUser.HoTen;
                SetUserImage(UserController.CurrentUser.HinhAnh);
              
            }
        }

        private void btn_dangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close(); // Đóng form hiện tại
            }
        }

        private void btn_donHang_Click(object sender, EventArgs e)
        {
            donHangAdmin uc = new donHangAdmin();
            AddUserControl(uc);
        }

        private void btn_DanhMuc_Click(object sender, EventArgs e)
        {
            danhMucUserControl uc = new danhMucUserControl();
            AddUserControl(uc);
        }

        private void btn_KhoHang_Click(object sender, EventArgs e)
        {
            ThongKeTheoMaGiamGia uc = new ThongKeTheoMaGiamGia();
            AddUserControl(uc);
        }

        private void btn_SanPham_Click(object sender, EventArgs e)
        {
            sanPhamUserControl uc = new sanPhamUserControl();
            AddUserControl(uc);
        }

        private void btn_NhaCungCap_Click(object sender, EventArgs e)
        {
            nhaCungCapUserControl uc = new nhaCungCapUserControl();
            AddUserControl(uc);
        }

        private void btn_DTThoiGian_Click(object sender, EventArgs e)
        {
            doanhthuThoiGianUserControl uc = new doanhthuThoiGianUserControl();
            AddUserControl(uc);
        }

        private void btn_KhachHang_Click(object sender, EventArgs e)
        {
            khachHangUserControl uc = new khachHangUserControl();
            AddUserControl(uc);
        }

        private void btn_NhanVien_Click(object sender, EventArgs e)
        {
            nhanVienUserControl uc = new nhanVienUserControl();
            AddUserControl(uc);
        }

        private void btn_LSKhachHang_Click(object sender, EventArgs e)
        {
           lichSuKhachHangUserControl uc = new lichSuKhachHangUserControl();
            AddUserControl(uc);
        }

        private void btn_KhuyenMai_Click(object sender, EventArgs e)
        {
            GiamGiaUserControl uc = new GiamGiaUserControl();
            AddUserControl(uc);
        }

        private void btn_DTNhomHang_Click(object sender, EventArgs e)
        {
            doanhThuNhomHangUserControl uc = new doanhThuNhomHangUserControl();
            AddUserControl(uc);
        }

        private void btn_DTLaiLo_Click(object sender, EventArgs e)
        {
            doanhThuLaiLoUserControl uc = new doanhThuLaiLoUserControl();
            AddUserControl(uc);
        }
    }
}
