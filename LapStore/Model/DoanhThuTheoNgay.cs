using System;

namespace LapStore.Model
{
    /// <summary>
    /// Lớp mô hình chứa thông tin doanh thu theo ngày
    /// </summary>
    public class DoanhThuTheoNgay
    {
        public string Id { get; set; }
        public string MaDonHang { get; set; }
        public DateTime CreatedAt { get; set; }
        public long DoanhThu { get; set; }
    }
}