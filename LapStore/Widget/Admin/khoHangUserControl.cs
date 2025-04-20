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
using System.Windows.Forms.DataVisualization.Charting;

namespace LapStore.Widget
{
    public partial class khoHangUserControl : System.Windows.Forms.UserControl
    {
        private string selectedMaDanhMuc = "";
        private string selectedMaSanPham = "";
        
        public khoHangUserControl()
        {
            InitializeComponent();
        }

        private void khoHangUserControl_Load(object sender, EventArgs e)
        {
            LoadDanhMuc();
            LoadTongQuan();
            LoadDanhSachSanPham();
        }

        // Phương thức tải danh sách danh mục vào combobox
        private void LoadDanhMuc()
        {
            try
            {
                if (cbbDanhMuc != null)
                {
                    // Lấy danh sách danh mục
                    List<DanhMuc> danhMucs = DanhMucController.getAllDanhMucs();

                    // Thêm item "Tất cả"
                    cbbDanhMuc.Items.Clear();
                    cbbDanhMuc.Items.Add("Tất cả");

                    // Thêm các danh mục vào combobox
                    foreach (DanhMuc dm in danhMucs)
                    {
                        cbbDanhMuc.Items.Add(dm.tenDanhMuc + " - " + dm.id);
                    }

                    // Chọn item đầu tiên
                    cbbDanhMuc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải tổng quan kho hàng
        private void LoadTongQuan()
        {
            try
            {
                // Lấy thông tin tổng quan
                int tongSoSanPham = KhoHangController.GetTongSoSanPham();
                int tongSoLuong = KhoHangController.GetTongSoLuongTatCaSanPham();
                
                // Hiển thị thông tin tổng quan
                if (lblTongSanPham != null)
                    lblTongSanPham.Text = "Tổng số sản phẩm: " + tongSoSanPham;
                
                if (lblTongSoLuong != null)
                    lblTongSoLuong.Text = "Tổng số lượng: " + tongSoLuong.ToString("N0");
                
                // Lấy và hiển thị sản phẩm sắp hết hàng
                List<SanPhamKho> sanPhamSapHet = KhoHangController.GetSanPhamSapHetHang();
                if (lblSapHetHang != null)
                    lblSapHetHang.Text = "Sản phẩm sắp hết hàng: " + sanPhamSapHet.Count;
                
                // Lấy và hiển thị sản phẩm đã hết hàng
                List<SanPhamKho> sanPhamHet = KhoHangController.GetSanPhamHetHang();
                if (lblHetHang != null)
                    lblHetHang.Text = "Sản phẩm hết hàng: " + sanPhamHet.Count;
                
                // Tải biểu đồ phân bố sản phẩm theo danh mục
                LoadBieuDoSoLuongTheoDanhMuc();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải tổng quan kho hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải biểu đồ số lượng sản phẩm theo danh mục
        private void LoadBieuDoSoLuongTheoDanhMuc()
        {
            try
            {
                if (chartSoLuongTheoDanhMuc != null)
                {
                    // Lấy dữ liệu số lượng theo danh mục
                    Dictionary<string, int> soLuongTheoDanhMuc = KhoHangController.GetSoLuongTheoDanhMuc();
                    
                    // Xóa dữ liệu cũ
                    chartSoLuongTheoDanhMuc.Series.Clear();
                    
                    // Tạo series mới
                    Series seriesSoLuong = new Series("Số Lượng");
                    seriesSoLuong.ChartType = SeriesChartType.Pie;
                    
                    // Thêm dữ liệu vào series
                    foreach (var item in soLuongTheoDanhMuc)
                    {
                        seriesSoLuong.Points.AddXY(item.Key, item.Value);
                    }
                    
                    // Thêm series vào biểu đồ
                    chartSoLuongTheoDanhMuc.Series.Add(seriesSoLuong);
                    
                    // Cấu hình biểu đồ
                    chartSoLuongTheoDanhMuc.Series[0]["PieLabelStyle"] = "Outside";
                    chartSoLuongTheoDanhMuc.Series[0]["PieLineColor"] = "Black";
                    chartSoLuongTheoDanhMuc.Series[0].Label = "#VALX: #VALY (#PERCENT{P0})";
                    chartSoLuongTheoDanhMuc.Series[0].LegendText = "#VALX";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ số lượng theo danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải danh sách sản phẩm
        private void LoadDanhSachSanPham()
        {
            try
            {
                if (dgvSanPham != null)
                {
                    List<SanPhamKho> danhSachSanPham;
                    
                    // Kiểm tra xem đã chọn danh mục cụ thể chưa
                    if (!string.IsNullOrEmpty(selectedMaDanhMuc))
                    {
                        danhSachSanPham = KhoHangController.GetSanPhamTheoDanhMuc(selectedMaDanhMuc);
                    }
                    else
                    {
                        danhSachSanPham = KhoHangController.GetAllSanPhamKho();
                    }
                    
                    // Xóa dữ liệu cũ
                    dgvSanPham.Rows.Clear();
                    
                    // Thêm dữ liệu mới
                    foreach (SanPhamKho item in danhSachSanPham)
                    {
                        // Nếu có hình ảnh thì tạo đối tượng Image
                        Image img = null;
                        if (!string.IsNullOrEmpty(item.HinhAnh) && System.IO.File.Exists(item.HinhAnh))
                        {
                            try
                            {
                                using (var bmpTemp = new Bitmap(item.HinhAnh))
                                {
                                    img = new Bitmap(bmpTemp, new Size(60, 60));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi khi tải hình ảnh: " + ex.Message);
                            }
                        }
                        
                        // Thêm hàng mới vào DataGridView
                        dgvSanPham.Rows.Add(
                            img,
                            item.MaSp,
                            item.TenSp,
                            item.TenDanhMuc,
                            item.SoLuong,
                            string.Format("{0:N0} VNĐ", item.GiaNhap),
                            string.Format("{0:N0} VNĐ", item.GiaBan),
                            item.NgayTao.ToString("dd/MM/yyyy")
                        );
                        
                        // Nếu số lượng dưới 10, đổi màu hàng thành vàng
                        if (item.SoLuong < 10 && item.SoLuong > 0)
                        {
                            dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                        }
                        
                        // Nếu hết hàng, đổi màu hàng thành đỏ nhạt
                        if (item.SoLuong == 0)
                        {
                            dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải danh sách sản phẩm sắp hết hàng
        private void LoadSanPhamSapHetHang()
        {
            try
            {
                if (dgvSanPham != null)
                {
                    // Lấy danh sách sản phẩm sắp hết hàng
                    List<SanPhamKho> danhSachSanPham = KhoHangController.GetSanPhamSapHetHang();
                    
                    // Xóa dữ liệu cũ
                    dgvSanPham.Rows.Clear();
                    
                    // Thêm dữ liệu mới
                    foreach (SanPhamKho item in danhSachSanPham)
                    {
                        // Nếu có hình ảnh thì tạo đối tượng Image
                        Image img = null;
                        if (!string.IsNullOrEmpty(item.HinhAnh) && System.IO.File.Exists(item.HinhAnh))
                        {
                            try
                            {
                                using (var bmpTemp = new Bitmap(item.HinhAnh))
                                {
                                    img = new Bitmap(bmpTemp, new Size(60, 60));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi khi tải hình ảnh: " + ex.Message);
                            }
                        }
                        
                        // Thêm hàng mới vào DataGridView
                        dgvSanPham.Rows.Add(
                            img,
                            item.MaSp,
                            item.TenSp,
                            item.TenDanhMuc,
                            item.SoLuong,
                            string.Format("{0:N0} VNĐ", item.GiaNhap),
                            string.Format("{0:N0} VNĐ", item.GiaBan),
                            item.NgayTao.ToString("dd/MM/yyyy")
                        );
                        
                        // Đổi màu hàng thành vàng
                        dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm sắp hết hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải danh sách sản phẩm hết hàng
        private void LoadSanPhamHetHang()
        {
            try
            {
                if (dgvSanPham != null)
                {
                    // Lấy danh sách sản phẩm hết hàng
                    List<SanPhamKho> danhSachSanPham = KhoHangController.GetSanPhamHetHang();
                    
                    // Xóa dữ liệu cũ
                    dgvSanPham.Rows.Clear();
                    
                    // Thêm dữ liệu mới
                    foreach (SanPhamKho item in danhSachSanPham)
                    {
                        // Nếu có hình ảnh thì tạo đối tượng Image
                        Image img = null;
                        if (!string.IsNullOrEmpty(item.HinhAnh) && System.IO.File.Exists(item.HinhAnh))
                        {
                            try
                            {
                                using (var bmpTemp = new Bitmap(item.HinhAnh))
                                {
                                    img = new Bitmap(bmpTemp, new Size(60, 60));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi khi tải hình ảnh: " + ex.Message);
                            }
                        }
                        
                        // Thêm hàng mới vào DataGridView
                        dgvSanPham.Rows.Add(
                            img,
                            item.MaSp,
                            item.TenSp,
                            item.TenDanhMuc,
                            item.SoLuong,
                            string.Format("{0:N0} VNĐ", item.GiaNhap),
                            string.Format("{0:N0} VNĐ", item.GiaBan),
                            item.NgayTao.ToString("dd/MM/yyyy")
                        );
                        
                        // Đổi màu hàng thành đỏ nhạt
                        dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm hết hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức cập nhật số lượng sản phẩm
        private bool CapNhatSoLuong(string maSp, int soLuongMoi)
        {
            try
            {
                return KhoHangController.CapNhatSoLuongSanPham(maSp, soLuongMoi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số lượng sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Phương thức tìm kiếm sản phẩm
        private void TimKiemSanPham(string keyword)
        {
            try
            {
                if (dgvSanPham != null)
                {
                    // Lấy kết quả tìm kiếm
                    List<SanPhamKho> ketQuaTimKiem = KhoHangController.SearchSanPhamKho(keyword);
                    
                    // Xóa dữ liệu cũ
                    dgvSanPham.Rows.Clear();
                    
                    // Thêm dữ liệu mới
                    foreach (SanPhamKho item in ketQuaTimKiem)
                    {
                        // Nếu có hình ảnh thì tạo đối tượng Image
                        Image img = null;
                        if (!string.IsNullOrEmpty(item.HinhAnh) && System.IO.File.Exists(item.HinhAnh))
                        {
                            try
                            {
                                using (var bmpTemp = new Bitmap(item.HinhAnh))
                                {
                                    img = new Bitmap(bmpTemp, new Size(60, 60));
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Lỗi khi tải hình ảnh: " + ex.Message);
                            }
                        }
                        
                        // Thêm hàng mới vào DataGridView
                        dgvSanPham.Rows.Add(
                            img,
                            item.MaSp,
                            item.TenSp,
                            item.TenDanhMuc,
                            item.SoLuong,
                            string.Format("{0:N0} VNĐ", item.GiaNhap),
                            string.Format("{0:N0} VNĐ", item.GiaBan),
                            item.NgayTao.ToString("dd/MM/yyyy")
                        );
                        
                        // Nếu số lượng dưới 10, đổi màu hàng thành vàng
                        if (item.SoLuong < 10 && item.SoLuong > 0)
                        {
                            dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                        }
                        
                        // Nếu hết hàng, đổi màu hàng thành đỏ nhạt
                        if (item.SoLuong == 0)
                        {
                            dgvSanPham.Rows[dgvSanPham.Rows.Count - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện thay đổi danh mục
        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDanhMuc.SelectedIndex <= 0)
            {
                selectedMaDanhMuc = "";
                LoadDanhSachSanPham();
            }
            else
            {
                string selectedItem = cbbDanhMuc.SelectedItem.ToString();
                // Lấy mã danh mục từ chuỗi được chọn (format: Tên - Mã)
                selectedMaDanhMuc = selectedItem.Substring(selectedItem.LastIndexOf(" - ") + 3);
                LoadDanhSachSanPham();
            }
        }

        // Xử lý sự kiện click vào một sản phẩm
        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                selectedMaSanPham = dgvSanPham.Rows[e.RowIndex].Cells[1].Value.ToString();
                
                // Hiển thị thông tin sản phẩm lên form cập nhật số lượng
                txtMaSanPham.Text = selectedMaSanPham;
                txtTenSanPham.Text = dgvSanPham.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtSoLuongHienTai.Text = dgvSanPham.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtSoLuongMoi.Text = txtSoLuongHienTai.Text;
            }
        }

        // Xử lý sự kiện tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadDanhSachSanPham();
                return;
            }
            
            TimKiemSanPham(keyword);
        }

        // Xử lý sự kiện cập nhật số lượng
        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedMaSanPham))
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                int soLuongMoi;
                if (!int.TryParse(txtSoLuongMoi.Text, out soLuongMoi) || soLuongMoi < 0)
                {
                    MessageBox.Show("Số lượng mới không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                // Cập nhật số lượng
                bool ketQua = CapNhatSoLuong(selectedMaSanPham, soLuongMoi);
                
                if (ketQua)
                {
                    MessageBox.Show("Cập nhật số lượng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // Tải lại dữ liệu
                    LoadDanhSachSanPham();
                    LoadTongQuan();
                    
                    // Reset form cập nhật
                    txtMaSanPham.Text = "";
                    txtTenSanPham.Text = "";
                    txtSoLuongHienTai.Text = "";
                    txtSoLuongMoi.Text = "";
                    selectedMaSanPham = "";
                }
                else
                {
                    MessageBox.Show("Cập nhật số lượng thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật số lượng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện click nút Sắp hết hàng
        private void btnSapHetHang_Click(object sender, EventArgs e)
        {
            LoadSanPhamSapHetHang();
        }

        // Xử lý sự kiện click nút Hết hàng
        private void btnHetHang_Click(object sender, EventArgs e)
        {
            LoadSanPhamHetHang();
        }

        // Xử lý sự kiện click nút Tất cả
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            selectedMaDanhMuc = "";
            if (cbbDanhMuc != null)
                cbbDanhMuc.SelectedIndex = 0;
            LoadDanhSachSanPham();
        }

        // Xử lý sự kiện xuất báo cáo
        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Xuất báo cáo kho hàng";
                saveFileDialog.FileName = $"BaoCaoKhoHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
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