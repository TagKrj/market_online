using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using LapStore.Model;
using LapStore.Widget.User;

namespace LapStore.Controller
{
    public class DoanhThuLaiLoController
    {
        public static List<ThongKeDoanhThuLaiLo> getAllThongKeDoanhThuLaiLos()
        {
            List<ThongKeDoanhThuLaiLo> ThongKeDoanhThuLaiLos = new List<ThongKeDoanhThuLaiLo>();

            string query = @"
            SELECT
                tk.maDonHang,                 -- Mã đơn hàng từ bảng THONGKE
                u.hoTen,                      -- Họ tên người dùng từ bảng USERS
                SUM(sp.giaNhap * tk.soLuong) AS TongGiaNhap, -- Tổng giá nhập (giá nhập * số lượng)
                SUM(tk.doanhThu) AS TongDoanhThu,          -- Tổng doanh thu từ bảng THONGKE
                SUM(tk.loiNhuan) AS TongLoiNhuan           -- Tổng lợi nhuận từ bảng THONGKE
            FROM
                THONGKE tk
            INNER JOIN
                DONHANG dh ON tk.maDonHang = dh.id -- Kết nối THONGKE với DONHANG qua id đơn hàng
            INNER JOIN
                USERS u ON dh.maUser = u.id         -- Kết nối DONHANG với USERS qua id người dùng
            INNER JOIN
                SANPHAM sp ON tk.maSp = sp.maSp     -- Kết nối THONGKE với SANPHAM qua mã sản phẩm
            GROUP BY
                tk.maDonHang,                 -- Nhóm kết quả theo mã đơn hàng
                u.hoTen                       -- và họ tên người dùng
            ORDER BY
                tk.maDonHang;                 -- Sắp xếp kết quả theo mã đơn hàng
        ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ThongKeDoanhThuLaiLos.Add(new ThongKeDoanhThuLaiLo
                        {
                            MaHD = reader["maDonHang"].ToString(), // Sửa lại tên cột
                            TenKH = reader["hoTen"].ToString(), // Sửa lại tên cột
                            TienVon = reader.GetInt64(reader.GetOrdinal("TongGiaNhap")),
                            TienBan = reader.GetInt64(reader.GetOrdinal("TongDoanhThu")),
                            LoiNhuan = reader.GetInt64(reader.GetOrdinal("TongLoiNhuan")),
                        });
                    }
                }
            }

            return ThongKeDoanhThuLaiLos;
        }
        public static List<ThongKeDoanhThuLaiLo> searchThongKeDoanhThuLaiLos(string searchTermParam)
        {
            List<ThongKeDoanhThuLaiLo> ThongKeDoanhThuLaiLos = new List<ThongKeDoanhThuLaiLo>();

            string query = @"
            SELECT
                tk.maDonHang,                 -- Mã đơn hàng từ bảng THONGKE
                u.hoTen,                      -- Họ tên người dùng từ bảng USERS
                SUM(sp.giaNhap * tk.soLuong) AS TongGiaNhap, -- Tổng giá nhập (giá nhập * số lượng)
                SUM(tk.doanhThu) AS TongDoanhThu,          -- Tổng doanh thu từ bảng THONGKE
                SUM(tk.loiNhuan) AS TongLoiNhuan           -- Tổng lợi nhuận từ bảng THONGKE
            FROM
                THONGKE tk
            INNER JOIN
                DONHANG dh ON tk.maDonHang = dh.id -- Kết nối THONGKE với DONHANG qua id đơn hàng
            INNER JOIN
                USERS u ON dh.maUser = u.id         -- Kết nối DONHANG với USERS qua id người dùng
            INNER JOIN
                SANPHAM sp ON tk.maSp = sp.maSp     -- Kết nối THONGKE với SANPHAM qua mã sản phẩm
            WHERE
                -- Search term is applied to both maDonHang and hoTen using OR
                tk.maDonHang LIKE @searchTermParam
                OR u.hoTen LIKE @searchTermParam
            GROUP BY
                tk.maDonHang,                 -- Nhóm kết quả theo mã đơn hàng
                u.hoTen                       -- và họ tên người dùng
            ORDER BY
                tk.maDonHang;                 -- Sắp xếp kết quả theo mã đơn hàng
        ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@searchTermParam", "%" + searchTermParam + "%");
                using (SqlDataReader reader = cmd.ExecuteReader())

                {
                    while (reader.Read())
                    {
                        ThongKeDoanhThuLaiLos.Add(new ThongKeDoanhThuLaiLo
                        {
                            MaHD = reader["maDonHang"].ToString(), // Sửa lại tên cột
                            TenKH = reader["hoTen"].ToString(), // Sửa lại tên cột
                            TienVon = reader.GetInt64(reader.GetOrdinal("TongGiaNhap")),
                            TienBan = reader.GetInt64(reader.GetOrdinal("TongDoanhThu")),
                            LoiNhuan = reader.GetInt64(reader.GetOrdinal("TongLoiNhuan")),
                        });
                    }
                }
            }

            return ThongKeDoanhThuLaiLos;
        }
    }
}