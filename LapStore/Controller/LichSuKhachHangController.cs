using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using LapStore.Model;

namespace LapStore.Controller
{
    public class LichSuKhachHangController
    {
        // Phương thức lấy thông tin khách hàng dựa trên từ khóa tìm kiếm
        public static KhachHang GetKhachHangByKeyword(string keyword)
        {
            try
            {
                string query = "SELECT * FROM KhachHang WHERE MaKH LIKE @keyword OR TenKH LIKE @keyword";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@keyword", "%" + keyword + "%")
                };

                DataTable data = Database.ExecuteQuery(query, parameters);
                if (data.Rows.Count > 0)
                {
                    DataRow row = data.Rows[0];
                    KhachHang khachHang = new KhachHang
                    {
                        MaKH = row["MaKH"].ToString(),
                        TenKH = row["TenKH"].ToString(),
                        DiaChi = row["DiaChi"].ToString(),
                        SoDT = row["SoDT"].ToString(),
                        Email = row["Email"].ToString()
                    };
                    return khachHang;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm khách hàng: " + ex.Message);
            }
        }

        // Phương thức lấy danh sách hóa đơn của một khách hàng
        public static List<HoaDon> GetHoaDonByKhachHangId(string maKhachHang)
        {
            try
            {
                List<HoaDon> hoaDons = new List<HoaDon>();
                string query = "SELECT * FROM HoaDon WHERE MaKH = @maKH ORDER BY NgayLap DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maKH", maKhachHang)
                };

                DataTable data = Database.ExecuteQuery(query, parameters);
                foreach (DataRow row in data.Rows)
                {
                    HoaDon hoaDon = new HoaDon
                    {
                        MaHD = row["MaHD"].ToString(),
                        NgayLap = Convert.ToDateTime(row["NgayLap"]),
                        TongTien = Convert.ToDecimal(row["TongTien"]),
                        MaKH = row["MaKH"].ToString()
                    };
                    hoaDons.Add(hoaDon);
                }
                return hoaDons;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn: " + ex.Message);
            }
        }

        // Phương thức lấy chi tiết các sản phẩm trong một hóa đơn
        public static List<ChiTietHoaDon> GetChiTietHoaDon(string maHoaDon)
        {
            try
            {
                List<ChiTietHoaDon> chiTiets = new List<ChiTietHoaDon>();
                string query = @"SELECT ct.*, sp.TenSP 
                                FROM ChiTietHoaDon ct 
                                JOIN SanPham sp ON ct.MaSP = sp.MaSP 
                                WHERE ct.MaHD = @maHD";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maHD", maHoaDon)
                };

                DataTable data = Database.ExecuteQuery(query, parameters);
                foreach (DataRow row in data.Rows)
                {
                    ChiTietHoaDon chiTiet = new ChiTietHoaDon
                    {
                        MaHD = row["MaHD"].ToString(),
                        MaSP = row["MaSP"].ToString(),
                        TenSP = row["TenSP"].ToString(),
                        SoLuong = Convert.ToInt32(row["SoLuong"]),
                        DonGia = Convert.ToDecimal(row["DonGia"]),
                        ThanhTien = Convert.ToDecimal(row["ThanhTien"])
                    };
                    chiTiets.Add(chiTiet);
                }
                return chiTiets;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết hóa đơn: " + ex.Message);
            }
        }

        // Phương thức lấy tổng chi tiêu của khách hàng
        public static decimal GetTongChiTieu(string maKhachHang)
        {
            try
            {
                string query = "SELECT SUM(TongTien) FROM HoaDon WHERE MaKH = @maKH";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maKH", maKhachHang)
                };

                object result = Database.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDecimal(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng chi tiêu: " + ex.Message);
            }
        }

        // Phương thức lấy số lượng hóa đơn của khách hàng
        public static int GetSoLuongHoaDon(string maKhachHang)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM HoaDon WHERE MaKH = @maKH";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maKH", maKhachHang)
                };

                object result = Database.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đếm số lượng hóa đơn: " + ex.Message);
            }
        }

        // Phương thức lấy danh sách sản phẩm đã mua của khách hàng
        public static List<SanPhamDaMua> GetSanPhamDaMua(string maKhachHang)
        {
            try
            {
                List<SanPhamDaMua> sanPhams = new List<SanPhamDaMua>();
                string query = @"SELECT sp.MaSP, sp.TenSP, SUM(ct.SoLuong) as TongSoLuong, 
                                SUM(ct.ThanhTien) as TongThanhTien 
                                FROM HoaDon hd 
                                JOIN ChiTietHoaDon ct ON hd.MaHD = ct.MaHD 
                                JOIN SanPham sp ON ct.MaSP = sp.MaSP 
                                WHERE hd.MaKH = @maKH 
                                GROUP BY sp.MaSP, sp.TenSP 
                                ORDER BY TongSoLuong DESC";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@maKH", maKhachHang)
                };

                DataTable data = Database.ExecuteQuery(query, parameters);
                foreach (DataRow row in data.Rows)
                {
                    SanPhamDaMua sanPham = new SanPhamDaMua
                    {
                        MaSP = row["MaSP"].ToString(),
                        TenSP = row["TenSP"].ToString(),
                        TongSoLuong = Convert.ToInt32(row["TongSoLuong"]),
                        TongThanhTien = Convert.ToDecimal(row["TongThanhTien"])
                    };
                    sanPhams.Add(sanPham);
                }
                return sanPhams;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm đã mua: " + ex.Message);
            }
        }
    }
}

