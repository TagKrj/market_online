using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LapStore.Model;

namespace LapStore.Controller
{
    public class DoanhThuLaiLoController
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

        // Phương thức lấy danh sách doanh thu lãi lỗ theo từ khóa tìm kiếm
        public static List<ThongKeDoanhThuLaiLo> GetDoanhThuLaiLoByKeyword(string keyword)
        {
            try
            {
                List<ThongKeDoanhThuLaiLo> danhSachThongKe = new List<ThongKeDoanhThuLaiLo>();
                string query = @"
                    SELECT hd.MaHD, hd.NgayLap, kh.TenKH, 
                           SUM(ct.SoLuong * sp.GiaVon) AS TienVon,
                           SUM(ct.ThanhTien) AS TienBan,
                           (SUM(ct.ThanhTien) - SUM(ct.SoLuong * sp.GiaVon)) AS LoiNhuan
                    FROM HoaDon hd
                    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                    JOIN ChiTietHoaDon ct ON hd.MaHD = ct.MaHD
                    JOIN SanPham sp ON ct.MaSP = sp.MaSP
                    WHERE (kh.MaKH LIKE @keyword OR kh.TenKH LIKE @keyword)
                    GROUP BY hd.MaHD, hd.NgayLap, kh.TenKH
                    ORDER BY hd.NgayLap DESC";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@keyword", "%" + keyword + "%")
                };

                DataTable data = Database.ExecuteQuery(query, parameters);
                int stt = 1;
                foreach (DataRow row in data.Rows)
                {
                    ThongKeDoanhThuLaiLo thongKe = new ThongKeDoanhThuLaiLo
                    {
                        STT = stt++,
                        MaHD = row["MaHD"].ToString(),
                        TenKH = row["TenKH"].ToString(),
                        TienVon = Convert.ToDecimal(row["TienVon"]),
                        TienBan = Convert.ToDecimal(row["TienBan"]),
                        LoiNhuan = Convert.ToDecimal(row["LoiNhuan"])
                    };
                    danhSachThongKe.Add(thongKe);
                }
                return danhSachThongKe;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu doanh thu lãi lỗ: " + ex.Message);
            }
        }

        // Phương thức lấy tất cả doanh thu lãi lỗ
        public static List<ThongKeDoanhThuLaiLo> GetAllDoanhThuLaiLo()
        {
            try
            {
                List<ThongKeDoanhThuLaiLo> danhSachThongKe = new List<ThongKeDoanhThuLaiLo>();
                string query = @"
                    SELECT hd.MaHD, hd.NgayLap, kh.TenKH, 
                           SUM(ct.SoLuong * sp.GiaVon) AS TienVon,
                           SUM(ct.ThanhTien) AS TienBan,
                           (SUM(ct.ThanhTien) - SUM(ct.SoLuong * sp.GiaVon)) AS LoiNhuan
                    FROM HoaDon hd
                    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
                    JOIN ChiTietHoaDon ct ON hd.MaHD = ct.MaHD
                    JOIN SanPham sp ON ct.MaSP = sp.MaSP
                    GROUP BY hd.MaHD, hd.NgayLap, kh.TenKH
                    ORDER BY hd.NgayLap DESC";

                DataTable data = Database.ExecuteQuery(query);
                int stt = 1;
                foreach (DataRow row in data.Rows)
                {
                    ThongKeDoanhThuLaiLo thongKe = new ThongKeDoanhThuLaiLo
                    {
                        STT = stt++,
                        MaHD = row["MaHD"].ToString(),
                        TenKH = row["TenKH"].ToString(),
                        TienVon = Convert.ToDecimal(row["TienVon"]),
                        TienBan = Convert.ToDecimal(row["TienBan"]),
                        LoiNhuan = Convert.ToDecimal(row["LoiNhuan"])
                    };
                    danhSachThongKe.Add(thongKe);
                }
                return danhSachThongKe;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu doanh thu lãi lỗ: " + ex.Message);
            }
        }

        // Phương thức tính tổng vốn
        public static decimal TinhTongVon(List<ThongKeDoanhThuLaiLo> danhSach)
        {
            return danhSach.Sum(item => item.TienVon);
        }

        // Phương thức tính tổng tiền bán
        public static decimal TinhTongTienBan(List<ThongKeDoanhThuLaiLo> danhSach)
        {
            return danhSach.Sum(item => item.TienBan);
        }

        // Phương thức tính tổng lợi nhuận
        public static decimal TinhTongLoiNhuan(List<ThongKeDoanhThuLaiLo> danhSach)
        {
            return danhSach.Sum(item => item.LoiNhuan);
        }
    }
}
