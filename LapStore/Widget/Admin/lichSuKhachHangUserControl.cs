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
    public partial class lichSuKhachHangUserControl : System.Windows.Forms.UserControl
    {
        private string selectedMaKhachHang = "";
        private string selectedMaDonHang = "";

        public lichSuKhachHangUserControl()
        {
            InitializeComponent();
        }

        private void lichSuKhachHangUserControl_Load(object sender, EventArgs e)
        {
            LoadDanhSachKhachHang();
            LoadLichSuMuaHangTatCa();
        }

        // Phương thức tải danh sách khách hàng vào combobox
        private void LoadDanhSachKhachHang()
        {
            try
            {
                if (cbbKhachHang != null)
                {
                    // Lấy danh sách khách hàng
                    List<UserModel> danhSachKhachHang = UserController.getAllUser();

                    // Thêm item "Tất cả"
                    cbbKhachHang.Items.Clear();
                    cbbKhachHang.Items.Add("Tất cả");

                    // Thêm các khách hàng vào combobox
                    foreach (UserModel user in danhSachKhachHang)
                    {
                        cbbKhachHang.Items.Add(user.HoTen + " - " + user.Id);
                    }

                    // Chọn item đầu tiên
                    cbbKhachHang.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải lịch sử mua hàng của tất cả khách hàng
        private void LoadLichSuMuaHangTatCa()
        {
            try
            {
                if (dgvLichSuMuaHang != null)
                {
                    // Lấy dữ liệu lịch sử mua hàng tất cả khách hàng
                    List<LichSuMuaHang> danhSachLichSu = LichSuKhachHangController.GetLichSuMuaHangTatCaKhachHang();

                    // Xóa dữ liệu cũ
                    dgvLichSuMuaHang.Rows.Clear();

                    // Thêm dữ liệu mới
                    foreach (LichSuMuaHang item in danhSachLichSu)
                    {
                        dgvLichSuMuaHang.Rows.Add(
                            item.MaDonHang,
                            item.MaKhachHang,
                            item.TenKhachHang,
                            string.Format("{0:N0} VNĐ", item.TongTien),
                            item.TrangThai,
                            item.NgayMua.ToString("dd/MM/yyyy HH:mm:ss"),
                            item.PhuongThucThanhToan
                        );
                    }

                    // Hiển thị thông tin tổng hợp
                    UpdateThongTinTongHop("");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử mua hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải lịch sử mua hàng của một khách hàng cụ thể
        private void LoadLichSuMuaHangKhachHang(string maKhachHang)
        {
            try
            {
                if (dgvLichSuMuaHang != null)
                {
                    // Lấy dữ liệu lịch sử mua hàng của khách hàng
                    List<LichSuMuaHang> danhSachLichSu = LichSuKhachHangController.GetLichSuMuaHangKhachHang(maKhachHang);

                    // Xóa dữ liệu cũ
                    dgvLichSuMuaHang.Rows.Clear();

                    // Thêm dữ liệu mới
                    foreach (LichSuMuaHang item in danhSachLichSu)
                    {
                        dgvLichSuMuaHang.Rows.Add(
                            item.MaDonHang,
                            item.MaKhachHang,
                            item.TenKhachHang,
                            string.Format("{0:N0} VNĐ", item.TongTien),
                            item.TrangThai,
                            item.NgayMua.ToString("dd/MM/yyyy HH:mm:ss"),
                            item.PhuongThucThanhToan
                        );
                    }

                    // Hiển thị thông tin tổng hợp
                    UpdateThongTinTongHop(maKhachHang);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử mua hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải chi tiết đơn hàng
        private void LoadChiTietDonHang(string maDonHang)
        {
            try
            {
                if (dgvChiTietDonHang != null)
                {
                    // Lấy dữ liệu chi tiết đơn hàng
                    List<ChiTietDonHang> danhSachChiTiet = LichSuKhachHangController.GetChiTietDonHang(maDonHang);

                    // Xóa dữ liệu cũ
                    dgvChiTietDonHang.Rows.Clear();

                    // Thêm dữ liệu mới
                    foreach (ChiTietDonHang item in danhSachChiTiet)
                    {
                        // Nếu có hình ảnh thì tạo đối tượng Image
                        Image img = null;
                        if (!string.IsNullOrEmpty(item.HinhAnh) && System.IO.File.Exists(item.HinhAnh))
                        {
                            try
                            {
                                using (var bmpTemp = new Bitmap(item.HinhAnh))
                                {
                                    img = new Bitmap(bmpTemp, new Size(80, 80));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi khi tải hình ảnh: " + ex.Message);
                            }
                        }

                        dgvChiTietDonHang.Rows.Add(
                            img,
                            item.MaSp,
                            item.TenSp,
                            item.SoLuong,
                            string.Format("{0:N0} VNĐ", item.GiaBan),
                            string.Format("{0:N0} VNĐ", item.SoLuong * item.GiaBan)
                        );
                    }

                    // Hiển thị thông tin đơn hàng
                    UpdateThongTinDonHang(maDonHang);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết đơn hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cập nhật thông tin tổng hợp của khách hàng
        private void UpdateThongTinTongHop(string maKhachHang)
        {
            try
            {
                if (string.IsNullOrEmpty(maKhachHang))
                {
                    // Nếu là tất cả khách hàng
                    if (lblTongDonHang != null)
                        lblTongDonHang.Text = "Tổng số đơn hàng: " + dgvLichSuMuaHang.Rows.Count;

                    // Tính tổng tiền từ datagridview
                    long tongTien = 0;
                    foreach (DataGridViewRow row in dgvLichSuMuaHang.Rows)
                    {
                        string tienStr = row.Cells[3].Value?.ToString().Replace("VNĐ", "").Replace(".", "").Trim();
                        if (!string.IsNullOrEmpty(tienStr))
                        {
                            tongTien += long.Parse(tienStr);
                        }
                    }

                    if (lblTongTien != null)
                        lblTongTien.Text = "Tổng tiền mua hàng: " + string.Format("{0:N0} VNĐ", tongTien);

                    // Reset thông tin khách hàng
                    if (lblThongTinKhachHang != null)
                        lblThongTinKhachHang.Text = "Thông tin khách hàng: Tất cả";
                }
                else
                {
                    // Nếu là khách hàng cụ thể
                    int tongDonHang = LichSuKhachHangController.GetTongSoDonHang(maKhachHang);
                    long tongTien = LichSuKhachHangController.GetTongTienMuaHang(maKhachHang);

                    if (lblTongDonHang != null)
                        lblTongDonHang.Text = "Tổng số đơn hàng: " + tongDonHang;

                    if (lblTongTien != null)
                        lblTongTien.Text = "Tổng tiền mua hàng: " + string.Format("{0:N0} VNĐ", tongTien);

                    // Hiển thị thông tin khách hàng
                    UserModel khachHang = LichSuKhachHangController.GetThongTinKhachHang(maKhachHang);
                    if (khachHang != null && lblThongTinKhachHang != null)
                    {
                        lblThongTinKhachHang.Text = $"Khách hàng: {khachHang.HoTen} | SĐT: {khachHang.Sdt} | Email: {khachHang.Email}";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật thông tin tổng hợp: " + ex.Message);
            }
        }

        // Cập nhật thông tin đơn hàng
        private void UpdateThongTinDonHang(string maDonHang)
        {
            try
            {
                // Tính tổng tiền từ chi tiết đơn hàng
                long tongTien = 0;
                foreach (DataGridViewRow row in dgvChiTietDonHang.Rows)
                {
                    string tienStr = row.Cells[5].Value?.ToString().Replace("VNĐ", "").Replace(".", "").Trim();
                    if (!string.IsNullOrEmpty(tienStr))
                    {
                        tongTien += long.Parse(tienStr);
                    }
                }

                if (lblThongTinDonHang != null)
                {
                    // Tìm thông tin đơn hàng từ DataGridView lịch sử
                    foreach (DataGridViewRow row in dgvLichSuMuaHang.Rows)
                    {
                        if (row.Cells[0].Value?.ToString() == maDonHang)
                        {
                            string trangThai = row.Cells[4].Value?.ToString();
                            string ngayMua = row.Cells[5].Value?.ToString();
                            string phuongThuc = row.Cells[6].Value?.ToString();

                            lblThongTinDonHang.Text = $"Đơn hàng: {maDonHang} | Tổng tiền: {string.Format("{0:N0} VNĐ", tongTien)} | Trạng thái: {trangThai} | Ngày mua: {ngayMua} | Phương thức: {phuongThuc}";
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật thông tin đơn hàng: " + ex.Message);
            }
        }

        // Xử lý sự kiện thay đổi khách hàng
        private void cbbKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbKhachHang.SelectedIndex <= 0)
            {
                selectedMaKhachHang = "";
                LoadLichSuMuaHangTatCa();
            }
            else
            {
                string selectedItem = cbbKhachHang.SelectedItem.ToString();
                // Lấy mã khách hàng từ chuỗi được chọn (format: Tên - Mã)
                selectedMaKhachHang = selectedItem.Substring(selectedItem.LastIndexOf(" - ") + 3);
                LoadLichSuMuaHangKhachHang(selectedMaKhachHang);
            }
        }

        // Xử lý sự kiện khi click vào một đơn hàng
        private void dgvLichSuMuaHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                selectedMaDonHang = dgvLichSuMuaHang.Rows[e.RowIndex].Cells[0].Value.ToString();
                LoadChiTietDonHang(selectedMaDonHang);
            }
        }

        // Xử lý sự kiện tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                if (string.IsNullOrEmpty(keyword))
                {
                    // Nếu từ khóa trống, tải lại dữ liệu ban đầu
                    if (string.IsNullOrEmpty(selectedMaKhachHang))
                    {
                        LoadLichSuMuaHangTatCa();
                    }
                    else
                    {
                        LoadLichSuMuaHangKhachHang(selectedMaKhachHang);
                    }
                    return;
                }

                // Tìm kiếm lịch sử mua hàng
                List<LichSuMuaHang> ketQuaTimKiem = LichSuKhachHangController.SearchLichSuMuaHang(keyword);

                if (dgvLichSuMuaHang != null)
                {
                    // Xóa dữ liệu cũ
                    dgvLichSuMuaHang.Rows.Clear();

                    // Thêm dữ liệu mới
                    foreach (LichSuMuaHang item in ketQuaTimKiem)
                    {
                        // Nếu đang lọc theo khách hàng cụ thể
                        if (!string.IsNullOrEmpty(selectedMaKhachHang) && item.MaKhachHang != selectedMaKhachHang)
                        {
                            continue;
                        }

                        dgvLichSuMuaHang.Rows.Add(
                            item.MaDonHang,
                            item.MaKhachHang,
                            item.TenKhachHang,
                            string.Format("{0:N0} VNĐ", item.TongTien),
                            item.TrangThai,
                            item.NgayMua.ToString("dd/MM/yyyy HH:mm:ss"),
                            item.PhuongThucThanhToan
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện xuất báo cáo
        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Xuất báo cáo lịch sử mua hàng";
                saveFileDialog.FileName = $"BaoCaoLichSuMuaHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Xuất báo cáo
                    // Phần này cần thêm thư viện để xuất Excel nếu cần
                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}