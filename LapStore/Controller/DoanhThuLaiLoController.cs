using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    internal class DoanhThuLaiLoController
    {
        // Lấy tổng doanh thu từ ngày đến ngày
        public static long GetTongDoanhThu(DateTime tuNgay, DateTime denNgay)
        {
            long tongDoanhThu = 0;
            string query = "SELECT SUM(doanhThu) FROM THONGKE WHERE created_at BETWEEN @tuNgay AND @denNgay";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongDoanhThu = Convert.ToInt64(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng doanh thu: " + ex.Message);
            }

            return tongDoanhThu;
        }

        // Lấy tổng lợi nhuận từ ngày đến ngày
        public static long GetTongLoiNhuan(DateTime tuNgay, DateTime denNgay)
        {
            long tongLoiNhuan = 0;
            string query = "SELECT SUM(loiNhuan) FROM THONGKE WHERE created_at BETWEEN @tuNgay AND @denNgay";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongLoiNhuan = Convert.ToInt64(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng lợi nhuận: " + ex.Message);
            }

            return tongLoiNhuan;
        }

        // Lấy thông tin chi tiết doanh thu và lợi nhuận
        public static List<ThongKe> GetChiTietDoanhThuLoiNhuan(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKe> danhSachThongKe = new List<ThongKe>();
            string query = @"SELECT TK.id, TK.maDonHang, TK.maSp, SP.tenSp, TK.soLuong, TK.doanhThu, TK.loiNhuan, TK.created_at 
                     FROM THONGKE TK
                     JOIN SANPHAM SP ON TK.maSp = SP.maSp
                     WHERE TK.created_at BETWEEN @tuNgay AND @denNgay
                     ORDER BY TK.created_at DESC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKe thongKe = new ThongKe
                            {
                                Id = reader["id"].ToString(),
                                MaDonHang = reader["maDonHang"].ToString(),
                                MaSp = reader["maSp"].ToString(),
                                TenSanPham = reader["tenSp"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                DoanhThu = Convert.ToInt64(reader["doanhThu"]),
                                LoiNhuan = Convert.ToInt64(reader["loiNhuan"]),
                                CreatedAt = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachThongKe.Add(thongKe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy chi tiết doanh thu và lợi nhuận: " + ex.Message);
            }

            return danhSachThongKe;
        }

        // Lấy doanh thu theo từng ngày trong khoảng thời gian
        public static Dictionary<DateTime, long> GetDoanhThuTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<DateTime, long> doanhThuNgay = new Dictionary<DateTime, long>();
            string query = @"SELECT CAST(created_at AS DATE) as ngay, SUM(doanhThu) as tongDoanhThu 
                     FROM THONGKE 
                     WHERE created_at BETWEEN @tuNgay AND @denNgay 
                     GROUP BY CAST(created_at AS DATE)
                     ORDER BY ngay";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime ngay = Convert.ToDateTime(reader["ngay"]);
                            long tongDoanhThu = Convert.ToInt64(reader["tongDoanhThu"]);
                            doanhThuNgay.Add(ngay, tongDoanhThu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy doanh thu theo ngày: " + ex.Message);
            }

            return doanhThuNgay;
        }

        // Lấy lợi nhuận theo từng ngày trong khoảng thời gian
        public static Dictionary<DateTime, long> GetLoiNhuanTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<DateTime, long> loiNhuanNgay = new Dictionary<DateTime, long>();
            string query = @"SELECT CAST(created_at AS DATE) as ngay, SUM(loiNhuan) as tongLoiNhuan 
                     FROM THONGKE 
                     WHERE created_at BETWEEN @tuNgay AND @denNgay 
                     GROUP BY CAST(created_at AS DATE)
                     ORDER BY ngay";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime ngay = Convert.ToDateTime(reader["ngay"]);
                            long tongLoiNhuan = Convert.ToInt64(reader["tongLoiNhuan"]);
                            loiNhuanNgay.Add(ngay, tongLoiNhuan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy lợi nhuận theo ngày: " + ex.Message);
            }

            return loiNhuanNgay;
        }
    }
}
