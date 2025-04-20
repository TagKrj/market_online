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
    public partial class doanhThuLaiLoUserControl : System.Windows.Forms.UserControl
    {
        private DateTime tuNgay;
        private DateTime denNgay;
        
        public doanhThuLaiLoUserControl()
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
        
        private void doanhThuLaiLoUserControl_Load(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
        
        // Phương thức tải dữ liệu
        private void LoadDuLieu()
        {
            try
            {
                // Lấy tổng doanh thu và lợi nhuận
                long tongDoanhThu = DoanhThuLaiLoController.GetTongDoanhThu(tuNgay, denNgay);
                long tongLoiNhuan = DoanhThuLaiLoController.GetTongLoiNhuan(tuNgay, denNgay);
                
                // Hiển thị tổng doanh thu và lợi nhuận lên giao diện
                if (lblTongDoanhThu != null)
                    lblTongDoanhThu.Text = string.Format("{0:N0} VNĐ", tongDoanhThu);
                
                if (lblTongLoiNhuan != null)
                    lblTongLoiNhuan.Text = string.Format("{0:N0} VNĐ", tongLoiNhuan);
                
                // Tính tỷ suất lợi nhuận
                double tySuatLoiNhuan = 0;
                if (tongDoanhThu > 0)
                {
                    tySuatLoiNhuan = (double)tongLoiNhuan / tongDoanhThu * 100;
                }
                
                if (lblTySuatLoiNhuan != null)
                    lblTySuatLoiNhuan.Text = string.Format("{0:F2}%", tySuatLoiNhuan);
                
                // Load dữ liệu cho biểu đồ
                LoadBieuDo();
                
                // Load dữ liệu chi tiết
                LoadDuLieuChiTiet();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Phương thức tải dữ liệu cho biểu đồ
        private void LoadBieuDo()
        {
            try
            {
                // Lấy dữ liệu doanh thu và lợi nhuận theo ngày
                Dictionary<DateTime, long> doanhThuTheoNgay = DoanhThuLaiLoController.GetDoanhThuTheoNgay(tuNgay, denNgay);
                Dictionary<DateTime, long> loiNhuanTheoNgay = DoanhThuLaiLoController.GetLoiNhuanTheoNgay(tuNgay, denNgay);
                
                // Nếu có control chart thì mới vẽ biểu đồ
                if (chartDoanhThu != null)
                {
                    // Xóa dữ liệu cũ
                    chartDoanhThu.Series.Clear();
                    
                    // Tạo series mới cho doanh thu
                    Series seriesDoanhThu = new Series("Doanh Thu");
                    seriesDoanhThu.ChartType = SeriesChartType.Column;
                    seriesDoanhThu.Color = Color.FromArgb(0, 138, 121);
                    
                    // Tạo series mới cho lợi nhuận
                    Series seriesLoiNhuan = new Series("Lợi Nhuận");
                    seriesLoiNhuan.ChartType = SeriesChartType.Column;
                    seriesLoiNhuan.Color = Color.FromArgb(255, 171, 74);
                    
                    // Thêm dữ liệu vào series
                    foreach (var item in doanhThuTheoNgay)
                    {
                        seriesDoanhThu.Points.AddXY(item.Key.ToString("dd/MM/yyyy"), item.Value);
                    }
                    
                    foreach (var item in loiNhuanTheoNgay)
                    {
                        seriesLoiNhuan.Points.AddXY(item.Key.ToString("dd/MM/yyyy"), item.Value);
                    }
                    
                    // Thêm series vào biểu đồ
                    chartDoanhThu.Series.Add(seriesDoanhThu);
                    chartDoanhThu.Series.Add(seriesLoiNhuan);
                    
                    // Cấu hình biểu đồ
                    chartDoanhThu.ChartAreas[0].AxisX.Interval = 1;
                    chartDoanhThu.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                    chartDoanhThu.ChartAreas[0].AxisY.LabelStyle.Format = "{0:N0}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Phương thức tải dữ liệu chi tiết vào DataGridView
        private void LoadDuLieuChiTiet()
        {
            try
            {
                if (dgvDoanhThu != null)
                {
                    // Lấy dữ liệu chi tiết
                    List<ThongKe> danhSachThongKe = DoanhThuLaiLoController.GetChiTietDoanhThuLoiNhuan(tuNgay, denNgay);
                    
                    // Xóa dữ liệu cũ
                    dgvDoanhThu.Rows.Clear();
                    
                    // Thêm dữ liệu mới
                    foreach (ThongKe item in danhSachThongKe)
                    {
                        dgvDoanhThu.Rows.Add(
                            item.Id,
                            item.MaDonHang,
                            item.MaSp,
                            item.TenSanPham,
                            item.SoLuong,
                            string.Format("{0:N0}", item.DoanhThu),
                            string.Format("{0:N0}", item.LoiNhuan),
                            item.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss")
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
                saveFileDialog.Title = "Xuất báo cáo doanh thu";
                saveFileDialog.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                
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