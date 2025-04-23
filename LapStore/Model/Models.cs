using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Model
{
    public class KhachHang
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public string SoDT { get; set; }
        public string Email { get; set; }
    }

    public class HoaDon
    {
        public string MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
    }

    public class ChiTietHoaDon
    {
        public string MaHD { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }

    public class SanPhamDaMua
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int TongSoLuong { get; set; }
        public decimal TongThanhTien { get; set; }
    }

    // public class DanhMuc
    // {
    //     public int id { get; set; }
    //     public string maDanhMuc { get; set; }
    //     public string tenDanhMuc { get; set; }
    //     public string moTa { get; set; }
    // }

    public class ThongKeDoanhThuSanPham
    {
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string TenDanhMuc { get; set; }
        public int TongSoLuong { get; set; }
        public decimal TongDoanhThu { get; set; }
        public decimal TongLoiNhuan { get; set; }
    }

    public class ThongKeDoanhThuLaiLo
    {
        public int STT { get; set; }
        public string MaHD { get; set; }
        public string TenKH { get; set; }
        public decimal TienVon { get; set; }
        public decimal TienBan { get; set; }
        public decimal LoiNhuan { get; set; }
    }

    // public class ThongKe
    // {
    //     public string Id { get; set; }
    //     public string MaDonHang { get; set; }
    //     public string MaSp { get; set; }
    //     public string TenSanPham { get; set; }
    //     public int SoLuong { get; set; }
    //     public long DoanhThu { get; set; }
    //     public long LoiNhuan { get; set; }
    //     public DateTime CreatedAt { get; set; }
    // }
}