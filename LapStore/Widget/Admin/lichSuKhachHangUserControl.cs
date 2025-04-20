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
    public partial class lichSuKhachHangUserControl : UserControl
    {
        private KhachHang khachHangHienTai = null;
        private List<HoaDon> danhSachHoaDon = new List<HoaDon>();
        private List<SanPhamDaMua> danhSachSanPham = new List<SanPhamDaMua>();

        public lichSuKhachHangUserControl()
        {
            InitializeComponent();
            SetupDataGridViews();
        }

        // Thiết lập các DataGridView
        private void SetupDataGridViews()
        {
            // Thiết lập DataGridView cho hóa đơn
            guna2DataGridView1.AutoGenerateColumns = false;
            guna2DataGridView1.Columns.Clear();
            guna2DataGridView1.Columns.Add("MaHD", "Mã Hóa Đơn");
            guna2DataGridView1.Columns.Add("NgayLap", "Ngày");
            guna2DataGridView1.Columns.Add("TongTien", "Tổng Tiền");

            // Thiết lập DataGridView cho sản phẩm đã mua
            guna2DataGridView2.AutoGenerateColumns = false;
            guna2DataGridView2.Columns.Clear();
            guna2DataGridView2.Columns.Add("MaSP", "Mã SP");
            guna2DataGridView2.Columns.Add("TenSP", "Tên Sản Phẩm");
            guna2DataGridView2.Columns.Add("SoLuong", "Số Lượng");
        }

        // Xử lý sự kiện khi nhấn nút tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string tuKhoa = txtTimKiem.Text.Trim();
                if (string.IsNullOrEmpty(tuKhoa))
                {
                    MessageBox.Show("Vui lòng nhập mã hoặc tên khách hàng để tìm kiếm!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tìm kiếm khách hàng
                khachHangHienTai = LichSuKhachHangController.GetKhachHangByKeyword(tuKhoa);
                if (khachHangHienTai == null)
                {
                    MessageBox.Show("Không tìm thấy khách hàng với thông tin đã nhập!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Hiển thị thông tin khách hàng
                label2.Text = khachHangHienTai.TenKH;

                // Load dữ liệu hóa đơn và sản phẩm đã mua
                LoadDuLieuKhachHang(khachHangHienTai.MaKH);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức load dữ liệu khách hàng
        private void LoadDuLieuKhachHang(string maKhachHang)
        {
            try
            {
                // Load danh sách hóa đơn
                danhSachHoaDon = LichSuKhachHangController.GetHoaDonByKhachHangId(maKhachHang);

                // Hiển thị danh sách hóa đơn lên DataGridView
                guna2DataGridView1.Rows.Clear();
                foreach (HoaDon hoaDon in danhSachHoaDon)
                {
                    guna2DataGridView1.Rows.Add(
                        hoaDon.MaHD,
                        hoaDon.NgayLap.ToString("dd/MM/yyyy"),
                        string.Format("{0:N0} VNĐ", hoaDon.TongTien)
                    );
                }

                // Load danh sách sản phẩm đã mua
                danhSachSanPham = LichSuKhachHangController.GetSanPhamDaMua(maKhachHang);

                // Hiển thị danh sách sản phẩm đã mua lên DataGridView
                guna2DataGridView2.Rows.Clear();
                foreach (SanPhamDaMua sanPham in danhSachSanPham)
                {
                    guna2DataGridView2.Rows.Add(
                        sanPham.MaSP,
                        sanPham.TenSP,
                        sanPham.TongSoLuong
                    );
                }

                // Cập nhật tiêu đề của DataGridView
                label3.Text = "THỐNG KÊ SẢN PHẨM ĐÃ MUA";
                label4.Text = "LỊCH SỬ HOÁ ĐƠN CỦA KHÁCH HÀNG";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện khi nhấn vào một hóa đơn
        private void guna2DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < danhSachHoaDon.Count)
            {
                try
                {
                    string maHoaDon = danhSachHoaDon[e.RowIndex].MaHD;

                    // Lấy chi tiết hóa đơn
                    List<ChiTietHoaDon> chiTietHoaDons = LichSuKhachHangController.GetChiTietHoaDon(maHoaDon);

                    // Hiển thị chi tiết hóa đơn (có thể hiển thị trong form riêng hoặc MessageBox)
                    string chiTiet = "Chi tiết hóa đơn " + maHoaDon + ":\n\n";
                    foreach (ChiTietHoaDon ct in chiTietHoaDons)
                    {
                        chiTiet += string.Format("{0} - {1} - SL: {2} - Đơn giá: {3:N0} VNĐ - Thành tiền: {4:N0} VNĐ\n",
                            ct.MaSP, ct.TenSP, ct.SoLuong, ct.DonGia, ct.ThanhTien);
                    }

                    MessageBox.Show(chiTiet, "Chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}