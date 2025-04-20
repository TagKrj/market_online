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
    public partial class doanhThuNhomHangUserControl : System.Windows.Forms.UserControl
    {
        private DateTime tuNgay;
        private DateTime denNgay;
        private string selectedDanhMuc = "";

        public doanhThuNhomHangUserControl()
        {
            InitializeComponent();

            // Khởi tạo ngày mặc định (tháng hiện tại)
            tuNgay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            denNgay = DateTime.Now;

            // Khởi tạo các control
            if (dtpTuNgay != null && dtpDenNgay != null)
            {
                dtpTuNgay.Value = tuNgay;
                dtpDenNgay.Value = denNgay;
            }
        }

        private void doanhThuNhomHangUserControl_Load(object sender, EventArgs e)
        {
            LoadDanhMuc();
            LoadDuLieu();
        }

        // Phương thức load danh sách danh mục vào combobox
        private void LoadDanhMuc()
        {
            try
            {
                if (cbbDanhMuc != null)
                {
                    // Lấy danh sách danh mục từ controller của danh mục
                    List<DanhMuc> danhMucs = DanhMucController.getAllDanhMucs();

                    // Thêm item "Tất cả"
                    cbbDanhMuc.Items.Clear();
                    cbbDanhMuc.Items.Add("Tất cả");

                    // Thêm các danh mục vào combobox
                    foreach (DanhMuc danhMuc in danhMucs)
                    {
                        cbbDanhMuc.Items.Add(danhMuc.TenDanhMuc);
                    }

                    // Chọn "Tất cả" làm giá trị mặc định
                    cbbDanhMuc.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải dữ liệu
        private void LoadDuLieu()
        {
            try
            {
                // Cập nhật biểu đồ doanh thu theo danh mục
                LoadBieuDoDoanhThuTheoDanhMuc();

                // Cập nhật biểu đồ lợi nhuận theo danh mục
                LoadBieuDoLoiNhuanTheoDanhMuc();

                // Cập nhật biểu đồ số lượng bán theo danh mục
                LoadBieuDoSoLuongBanTheoDanhMuc();

                // Cập nhật bảng chi tiết doanh thu
                LoadDuLieuChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải biểu đồ doanh thu theo danh mục
        private void LoadBieuDoDoanhThuTheoDanhMuc()
        {
            try
            {
                if (chartDoanhThu != null)
                {
                    // Lấy dữ liệu doanh thu theo danh mục
                    Dictionary<string, long> doanhThuDanhMuc = DoanhThuNhomHangController.GetDoanhThuTheoDanhMuc(tuNgay, denNgay);

                    // Xóa dữ liệu cũ
                    chartDoanhThu.Series.Clear();

                    // Tạo series mới
                    Series seriesDoanhThu = new Series("Doanh Thu");
                    seriesDoanhThu.ChartType = SeriesChartType.Pie;

                    // Thêm dữ liệu vào series
                    foreach (var item in doanhThuDanhMuc)
                    {
                        seriesDoanhThu.Points.AddXY(item.Key, item.Value);
                    }

                    // Thêm series vào biểu đồ
                    chartDoanhThu.Series.Add(seriesDoanhThu);

                    // Cấu hình biểu đồ
                    chartDoanhThu.Series[0]["PieLabelStyle"] = "Outside";
                    chartDoanhThu.Series[0]["PieLineColor"] = "Black";
                    chartDoanhThu.Series[0].Label = "#PERCENT{P0}";
                    chartDoanhThu.Series[0].LegendText = "#VALX";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải biểu đồ lợi nhuận theo danh mục
        private void LoadBieuDoLoiNhuanTheoDanhMuc()
        {
            try
            {
                if (chartLoiNhuan != null)
                {
                    // Lấy dữ liệu lợi nhuận theo danh mục
                    Dictionary<string, long> loiNhuanDanhMuc = DoanhThuNhomHangController.GetLoiNhuanTheoDanhMuc(tuNgay, denNgay);

                    // Xóa dữ liệu cũ
                    chartLoiNhuan.Series.Clear();

                    // Tạo series mới
                    Series seriesLoiNhuan = new Series("Lợi Nhuận");
                    seriesLoiNhuan.ChartType = SeriesChartType.Pie;

                    // Thêm dữ liệu vào series
                    foreach (var item in loiNhuanDanhMuc)
                    {
                        seriesLoiNhuan.Points.AddXY(item.Key, item.Value);
                    }

                    // Thêm series vào biểu đồ
                    chartLoiNhuan.Series.Add(seriesLoiNhuan);

                    // Cấu hình biểu đồ
                    chartLoiNhuan.Series[0]["PieLabelStyle"] = "Outside";
                    chartLoiNhuan.Series[0]["PieLineColor"] = "Black";
                    chartLoiNhuan.Series[0].Label = "#PERCENT{P0}";
                    chartLoiNhuan.Series[0].LegendText = "#VALX";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ lợi nhuận: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải biểu đồ số lượng bán theo danh mục
        private void LoadBieuDoSoLuongBanTheoDanhMuc()
        {
            try
            {
                if (chartSoLuong != null)
                {
                    // Lấy dữ liệu số lượng bán theo danh mục
                    Dictionary<string, int> soLuongDanhMuc = DoanhThuNhomHangController.GetSoLuongBanTheoDanhMuc(tuNgay, denNgay);

                    // Xóa dữ liệu cũ
                    chartSoLuong.Series.Clear();

                    // Tạo series mới
                    Series seriesSoLuong = new Series("Số Lượng Bán");
                    seriesSoLuong.ChartType = SeriesChartType.Column;

                    // Thêm dữ liệu vào series
                    foreach (var item in soLuongDanhMuc)
                    {
                        seriesSoLuong.Points.AddXY(item.Key, item.Value);
                    }

                    // Thêm series vào biểu đồ
                    chartSoLuong.Series.Add(seriesSoLuong);

                    // Cấu hình biểu đồ
                    chartSoLuong.ChartAreas[0].AxisX.Interval = 1;
                    chartSoLuong.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ số lượng bán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Phương thức tải dữ liệu chi tiết vào DataGridView
        private void LoadDuLieuChiTiet()
        {
            try
            {
                if (dgvDoanhThu != null)
                {
                    List<ThongKeDoanhThuSanPham> danhSachThongKe;

                    // Nếu đã chọn danh mục cụ thể
                    if (!string.IsNullOrEmpty(selectedDanhMuc) && cbbDanhMuc.SelectedIndex > 0)
                    {
                        // Lấy mã danh mục từ tên danh mục
                        DanhMuc danhMuc = DanhMucController.getAllDanhMucs().FirstOrDefault(dm => dm.tenDanhMuc == selectedDanhMuc);
                        if (danhMuc != null)
                        {
                            danhSachThongKe = DoanhThuNhomHangController.GetChiTietDoanhThuTheoDanhMuc(danhMuc.id, tuNgay, denNgay);
                        }
                        else
                        {
                            danhSachThongKe = new List<ThongKeDoanhThuSanPham>();
                        }
                    }
                    else
                    {
                        // Lấy tất cả
                        danhSachThongKe = DoanhThuNhomHangController.GetChiTietDoanhThuTatCaSanPham(tuNgay, denNgay);
                    }

                    // Xóa dữ liệu cũ
                    dgvDoanhThu.Rows.Clear();

                    // Thêm dữ liệu mới
                    foreach (ThongKeDoanhThuSanPham item in danhSachThongKe)
                    {
                        dgvDoanhThu.Rows.Add(
                            item.MaSp,
                            item.TenSp,
                            item.TenDanhMuc,
                            item.TongSoLuong,
                            string.Format("{0:N0}", item.TongDoanhThu),
                            string.Format("{0:N0}", item.TongLoiNhuan)
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu chi tiết: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Xử lý sự kiện thay đổi ngày bắt đầu
        private void dtpTuNgay_ValueChanged(object sender, EventArgs e)
        {
            tuNgay = dtpTuNgay.Value;
            if (tuNgay > denNgay)
            {
                MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tuNgay = denNgay;
                dtpTuNgay.Value = tuNgay;
                return;
            }
            LoadDuLieu();
        }

        // Xử lý sự kiện thay đổi ngày kết thúc
        private void dtpDenNgay_ValueChanged(object sender, EventArgs e)
        {
            denNgay = dtpDenNgay.Value;
            if (denNgay < tuNgay)
            {
                MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                denNgay = tuNgay;
                dtpDenNgay.Value = denNgay;
                return;
            }
            LoadDuLieu();
        }

        // Xử lý sự kiện thay đổi danh mục
        private void cbbDanhMuc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbDanhMuc.SelectedIndex <= 0)
            {
                selectedDanhMuc = "";
            }
            else
            {
                selectedDanhMuc = cbbDanhMuc.SelectedItem.ToString();
            }
            LoadDuLieu();
        }

        // Xử lý sự kiện tìm kiếm
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        // Xử lý sự kiện xuất báo cáo
        private void btnXuatBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Xuất báo cáo doanh thu theo nhóm hàng";
                saveFileDialog.FileName = $"BaoCaoDoanhThuNhomHang_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

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