using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    internal class DoanhThuNhomHangController
    {
        public static List<ThongKeDanhMuc> getAllThongKeDanhMucs()
        {
            List<ThongKeDanhMuc> ThongKeDanhMucs = new List<ThongKeDanhMuc>();

            string query = @"
            SELECT
                d.id AS DanhMucId,         -- ID của danh mục
                d.tenDanhMuc,             -- Tên danh mục
                SUM(s.soLuong) AS TongSoLuong -- Tổng số lượng sản phẩm trong danh mục này
            FROM
                DANHMUC d                 -- Alias 'd' cho bảng DANHMUC
            JOIN
                SANPHAM s ON d.id = s.maDm -- Kết nối DANHMUC và SANPHAM trên cột id và maDm
            GROUP BY
                d.id, d.tenDanhMuc        -- Gom nhóm kết quả theo ID và tên danh mục
            ORDER BY
                d.id;
        ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ThongKeDanhMucs.Add(new ThongKeDanhMuc
                        {
                            DanhMucId = reader["DanhMucId"].ToString(), // Sửa lại tên cột
                            TenDanhMuc = reader["tenDanhMuc"].ToString(), // Sửa lại tên cột
                            TongSoLuong = (int)reader["TongSoLuong"],
                        });
                    }
                }
            }

            return ThongKeDanhMucs;
        }

        public static List<ThongKeDanhMuc> cboThongKeDanhMucs(string text)
        {
            List<ThongKeDanhMuc> ThongKeDanhMucs = new List<ThongKeDanhMuc>();

            string query = @"
            SELECT
                d.id AS DanhMucId,
                d.tenDanhMuc,
                SUM(s.soLuong) AS TongSoLuong
            FROM
                DANHMUC d
            JOIN
                SANPHAM s ON d.id = s.maDm
            WHERE
                d.id = @DanhMucId -- Thêm điều kiện lọc theo tham số @DanhMucId
            GROUP BY
                d.id, d.tenDanhMuc
            ORDER BY
                d.id;
            ";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@DanhMucId",  text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ThongKeDanhMucs.Add(new ThongKeDanhMuc
                        {
                            DanhMucId = reader["DanhMucId"].ToString(), // Sửa lại tên cột
                            TenDanhMuc = reader["tenDanhMuc"].ToString(), // Sửa lại tên cột
                            TongSoLuong = (int)reader["TongSoLuong"],
                        });
                    }
                }
            }

            return ThongKeDanhMucs;
        }
    }

    // Class để chứa thông tin thống kê doanh thu sản phẩm
}