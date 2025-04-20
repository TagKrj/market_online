using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LapStore.Model;
using LapStore.Controller;

namespace LapStore.Widget
{
    public partial class doanhThuLaiLoUserControl : UserControl
    {
        private List<ThongKeDoanhThuLaiLo> danhSachThongKe = new List<ThongKeDoanhThuLaiLo>();

        public doanhThuLaiLoUserControl()
        {
            InitializeComponent();
        }

        private void doanhThuLaiLoUserControl_Load(object sender, EventArgs e)
        {
            // Load tất cả dữ liệu doanh thu lãi lỗ
            LoadAllDoanhThuLaiLo();
        }

        // Phương thức load tất cả dữ liệu doanh thu lãi lỗ
        private void LoadAllDoanhThuLaiLo()
        {
            try
            {
                // Lấy dữ liệu doanh thu lãi lỗ
                danhSachThongKe = DoanhThuLaiLoController.GetAllDoanhThuLaiLo();

                // Hiển thị dữ liệu lên DataGridView
                HienThiDuLieuLenDataGridView();

                // Tính và hiển thị tổng
                TinhVaHienThiTong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức hiển thị dữ liệu lên DataGridView
        private void HienThiDuLieuLenDataGridView()
        {
            try
            {
                // Xóa dữ liệu cũ
                guna2DataGridView1.Rows.Clear();

                // Thêm dữ liệu mới
                foreach (ThongKeDoanhThuLaiLo item in danhSachThongKe)
                {
                    guna2DataGridView1.Rows.Add(
                        item.STT,
                        item.MaHD,
                        item.TenKH,
                        string.Format("{0:N0}", item.TienVon),
                        string.Format("{0:N0}", item.TienBan),
                        string.Format("{0:N0}", item.LoiNhuan)
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi hiển thị dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tính và hiển thị tổng
        private void TinhVaHienThiTong()
        {
            try
            {
                // Tính tổng vốn
                decimal tongVon = DoanhThuLaiLoController.TinhTongVon(danhSachThongKe);
                label2.Text = string.Format("{0:N0}", tongVon);

                // Tính tổng tiền bán
                decimal tongTienBan = DoanhThuLaiLoController.TinhTongTienBan(danhSachThongKe);
                label7.Text = string.Format("{0:N0}", tongTienBan);

                // Tính tổng lợi nhuận
                decimal tongLoiNhuan = DoanhThuLaiLoController.TinhTongLoiNhuan(danhSachThongKe);
                label6.Text = string.Format("{0:N0}", tongLoiNhuan);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tính tổng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tìm kiếm doanh thu lãi lỗ theo từ khóa
        private void TimKiemDoanhThuLaiLo(string tuKhoa)
        {
            try
            {
                // Nếu từ khóa rỗng, load tất cả
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    LoadAllDoanhThuLaiLo();
                    return;
                }

                // Lấy dữ liệu doanh thu lãi lỗ theo từ khóa
                danhSachThongKe = DoanhThuLaiLoController.GetDoanhThuLaiLoByKeyword(tuKhoa);

                // Hiển thị dữ liệu lên DataGridView
                HienThiDuLieuLenDataGridView();

                // Tính và hiển thị tổng
                TinhVaHienThiTong();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi thay đổi nội dung ô tìm kiếm
        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = guna2TextBox2.Text.Trim();
            TimKiemDoanhThuLaiLo(tuKhoa);
        }

        // Xử lý sự kiện khi nhấn phím Enter trong ô tìm kiếm
        private void guna2TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string tuKhoa = guna2TextBox2.Text.Trim();
                TimKiemDoanhThuLaiLo(tuKhoa);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}