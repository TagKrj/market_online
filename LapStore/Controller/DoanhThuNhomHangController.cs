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
        public static Dictionary<string, long> GetDoanhThuTheoDanhMuc(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<string, long> doanhThuDanhMuc = new Dictionary<string, long>();
            string query = @"SELECT DM.tenDanhMuc, SUM(TK.doanhThu) as tongDoanhThu 
                        FROM THONGKE TK
                        JOIN SANPHAM SP ON TK.maSp = SP.maSp
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE TK.created_at BETWEEN @tuNgay AND @denNgay 
                        GROUP BY DM.tenDanhMuc
                        ORDER BY tongDoanhThu DESC";

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
                            string tenDanhMuc = reader["tenDanhMuc"].ToString();
                            long tongDoanhThu = Convert.ToInt64(reader["tongDoanhThu"]);
                            doanhThuDanhMuc.Add(tenDanhMuc, tongDoanhThu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy doanh thu theo danh mục: " + ex.Message);
            }

            return doanhThuDanhMuc;
        }

        // Lấy lợi nhuận theo từng danh mục trong khoảng thời gian
        public static Dictionary<string, long> GetLoiNhuanTheoDanhMuc(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<string, long> loiNhuanDanhMuc = new Dictionary<string, long>();
            string query = @"SELECT DM.tenDanhMuc, SUM(TK.loiNhuan) as tongLoiNhuan 
                        FROM THONGKE TK
                        JOIN SANPHAM SP ON TK.maSp = SP.maSp
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE TK.created_at BETWEEN @tuNgay AND @denNgay 
                        GROUP BY DM.tenDanhMuc
                        ORDER BY tongLoiNhuan DESC";

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
                            string tenDanhMuc = reader["tenDanhMuc"].ToString();
                            long tongLoiNhuan = Convert.ToInt64(reader["tongLoiNhuan"]);
                            loiNhuanDanhMuc.Add(tenDanhMuc, tongLoiNhuan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy lợi nhuận theo danh mục: " + ex.Message);
            }

            return loiNhuanDanhMuc;
        }

        // Lấy số lượng sản phẩm đã bán theo danh mục
        public static Dictionary<string, int> GetSoLuongBanTheoDanhMuc(DateTime tuNgay, DateTime denNgay)
        {
            Dictionary<string, int> soLuongBanDanhMuc = new Dictionary<string, int>();
            string query = @"SELECT DM.tenDanhMuc, SUM(TK.soLuong) as tongSoLuong 
                        FROM THONGKE TK
                        JOIN SANPHAM SP ON TK.maSp = SP.maSp
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE TK.created_at BETWEEN @tuNgay AND @denNgay 
                        GROUP BY DM.tenDanhMuc
                        ORDER BY tongSoLuong DESC";

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
                            string tenDanhMuc = reader["tenDanhMuc"].ToString();
                            int tongSoLuong = Convert.ToInt32(reader["tongSoLuong"]);
                            soLuongBanDanhMuc.Add(tenDanhMuc, tongSoLuong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy số lượng bán theo danh mục: " + ex.Message);
            }

            return soLuongBanDanhMuc;
        }

        // Lấy chi tiết doanh thu theo danh mục và sản phẩm 
        public static List<ThongKeDoanhThuSanPham> GetChiTietDoanhThuTheoDanhMuc(string maDm, DateTime tuNgay,
            DateTime denNgay)
        {
            List<ThongKeDoanhThuSanPham> danhSachThongKe = new List<ThongKeDoanhThuSanPham>();
            string query = @"SELECT SP.maSp, SP.tenSp, DM.tenDanhMuc, SUM(TK.soLuong) as tongSoLuong, 
                        SUM(TK.doanhThu) as tongDoanhThu, SUM(TK.loiNhuan) as tongLoiNhuan
                        FROM THONGKE TK
                        JOIN SANPHAM SP ON TK.maSp = SP.maSp
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE SP.maDm = @maDm AND TK.created_at BETWEEN @tuNgay AND @denNgay 
                        GROUP BY SP.maSp, SP.tenSp, DM.tenDanhMuc
                        ORDER BY tongDoanhThu DESC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maDm", maDm);
                    cmd.Parameters.AddWithValue("@tuNgay", tuNgay);
                    cmd.Parameters.AddWithValue("@denNgay", denNgay);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ThongKeDoanhThuSanPham thongKe = new ThongKeDoanhThuSanPham
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                TongSoLuong = Convert.ToInt32(reader["tongSoLuong"]),
                                TongDoanhThu = Convert.ToInt64(reader["tongDoanhThu"]),
                                TongLoiNhuan = Convert.ToInt64(reader["tongLoiNhuan"])
                            };
                            danhSachThongKe.Add(thongKe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy chi tiết doanh thu theo danh mục: " + ex.Message);
            }

            return danhSachThongKe;
        }

        // Lấy tất cả chi tiết doanh thu theo sản phẩm
        public static List<ThongKeDoanhThuSanPham> GetChiTietDoanhThuTatCaSanPham(DateTime tuNgay, DateTime denNgay)
        {
            List<ThongKeDoanhThuSanPham> danhSachThongKe = new List<ThongKeDoanhThuSanPham>();
            string query = @"SELECT SP.maSp, SP.tenSp, DM.tenDanhMuc, SUM(TK.soLuong) as tongSoLuong, 
                        SUM(TK.doanhThu) as tongDoanhThu, SUM(TK.loiNhuan) as tongLoiNhuan
                        FROM THONGKE TK
                        JOIN SANPHAM SP ON TK.maSp = SP.maSp
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE TK.created_at BETWEEN @tuNgay AND @denNgay 
                        GROUP BY SP.maSp, SP.tenSp, DM.tenDanhMuc
                        ORDER BY tongDoanhThu DESC";

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
                            ThongKeDoanhThuSanPham thongKe = new ThongKeDoanhThuSanPham
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                TongSoLuong = Convert.ToInt32(reader["tongSoLuong"]),
                                TongDoanhThu = Convert.ToInt64(reader["tongDoanhThu"]),
                                TongLoiNhuan = Convert.ToInt64(reader["tongLoiNhuan"])
                            };
                            danhSachThongKe.Add(thongKe);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy chi tiết doanh thu tất cả sản phẩm: " + ex.Message);
            }

            return danhSachThongKe;
        }
    }

    // Class để chứa thông tin thống kê doanh thu sản phẩm
    public class ThongKeDoanhThuSanPham
    {
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string TenDanhMuc { get; set; }
        public int TongSoLuong { get; set; }
        public long TongDoanhThu { get; set; }
        public long TongLoiNhuan { get; set; }
    }
}