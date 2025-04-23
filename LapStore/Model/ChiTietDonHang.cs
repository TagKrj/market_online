using System;

namespace LapStore.Model
{
    public class ChiTietDonHang
    {
        public string Id { get; set; }
        public string MaDonHang { get; set; }
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public int SoLuong { get; set; }
        public long GiaBan { get; set; }
        public string HinhAnh { get; set; }

        // Constructor mặc định
        public ChiTietDonHang() { }

        // Constructor có tham số
        public ChiTietDonHang(string id, string maDonHang, string maSp, int soLuong, long giaBan)
        {
            Id = id;
            MaDonHang = maDonHang;
            MaSp = maSp;
            SoLuong = soLuong;
            GiaBan = giaBan;
        }
    }
}
