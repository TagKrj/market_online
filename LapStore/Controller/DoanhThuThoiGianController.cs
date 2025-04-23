using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using LapStore.Model;

namespace LapStore.Controller
{
    internal class DoanhThuThoiGianController
    {
        public static List<DoanhThuTheoNgay> getAllDoanhThuThoiGians(string startDate, string endDate)
        {
            List<DoanhThuTheoNgay> DoanhThuTheoNgays = new List<DoanhThuTheoNgay>();

            string query = "SELECT id, maDonHang, created_at, doanhThu FROM THONGKE " +
                               "WHERE created_at >= @startDate AND created_at <= @endDate";
                using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
                {
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DoanhThuTheoNgays.Add(new DoanhThuTheoNgay
                            {
                                Id = reader["id"].ToString(), // Sửa lại tên cột
                                MaDonHang = reader["maDonHang"].ToString(), // Sửa lại tên cột
                                CreatedAt = Convert.ToDateTime(reader["created_at"]),
                                DoanhThu = reader.GetInt64(reader.GetOrdinal("doanhThu"))
                            });
                        }
                    }
                }
            return DoanhThuTheoNgays;
        }
        public static List<DoanhThuTheoNgay> searchDoanhThuThoiGians(string startDate, string endDate, string maDonHang)
        {
            List<DoanhThuTheoNgay> DoanhThuTheoNgays = new List<DoanhThuTheoNgay>();

            string query = "SELECT id, maDonHang, created_at, doanhThu FROM THONGKE " +
                           "WHERE created_at >= @startDate AND created_at <= @endDate " +
                           "AND maDonHang LIKE @maDonHang";

            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@maDonHang", "%" + maDonHang + "%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoanhThuTheoNgays.Add(new DoanhThuTheoNgay
                        {
                            Id = reader["id"].ToString(),
                            MaDonHang = reader["maDonHang"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"]),
                            DoanhThu = reader.GetInt64(reader.GetOrdinal("doanhThu"))
                        });
                    }
                }
            }
            return DoanhThuTheoNgays;
        }

    }
}