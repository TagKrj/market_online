using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    internal class ThongKeTheoMaGiamGiaController
    {
        public static List<ThongKeGiamGia> getAllThongKeGiamGias()
        {
            List<ThongKeGiamGia> ThongKeGiamGias = new List<ThongKeGiamGia>();

            string query = @"
            SELECT
                d.maGiamGia AS MaGiamGiaId,         -- ID của danh mục
                d.tenGiamGia,             -- Tên danh mục
                SUM(s.soLuong) AS TongSoLuong -- Tổng số lượng sản phẩm trong danh mục này
            FROM
                GIAMGIA d                 -- Alias 'd' cho bảng DANHMUC
            JOIN
                SANPHAM s ON d.maGiamGia = s.maGiamGia -- Kết nối DANHMUC và SANPHAM trên cột id và maDm
            GROUP BY
                d.maGiamGia, d.tenGiamGia        -- Gom nhóm kết quả theo ID và tên danh mục
            ORDER BY
                d.maGiamGia;
        ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ThongKeGiamGias.Add(new ThongKeGiamGia
                        {
                            GiamGiaId = reader["MaGiamGiaId"].ToString(), // Sửa lại tên cột
                            TenGiamGia = reader["tenGiamGia"].ToString(), // Sửa lại tên cột
                            TongSoLuong = (int)reader["TongSoLuong"],
                        });
                    }
                }
            }

            return ThongKeGiamGias;
        }
        public static List<ThongKeGiamGia> cboThongKeGiamGias(string text)
        {
            List<ThongKeGiamGia> ThongKeGiamGias = new List<ThongKeGiamGia>();

            string query = @"
            SELECT
                d.maGiamGia AS MaGiamGiaId,         -- ID của danh mục
                d.tenGiamGia,             -- Tên danh mục
                SUM(s.soLuong) AS TongSoLuong -- Tổng số lượng sản phẩm trong danh mục này
            FROM
                GIAMGIA d                 -- Alias 'd' cho bảng DANHMUC
            JOIN
                SANPHAM s ON d.maGiamGia = s.maGiamGia
            WHERE
                d.maGiamGia = @maGiamGiaId -- Thêm điều kiện lọc theo tham số @DanhMucId
            GROUP BY
                d.maGiamGia, d.tenGiamGia        -- Gom nhóm kết quả theo ID và tên danh mục
            ORDER BY
                d.maGiamGia;
            ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@maGiamGiaId", text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ThongKeGiamGias.Add(new ThongKeGiamGia
                        {
                            GiamGiaId = reader["MaGiamGiaId"].ToString(), // Sửa lại tên cột
                            TenGiamGia = reader["tenGiamGia"].ToString(), // Sửa lại tên cột
                            TongSoLuong = (int)reader["TongSoLuong"],
                        });
                    }
                }
            }

            return ThongKeGiamGias;
        }
    }
}
