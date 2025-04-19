using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    public class LichSuKhachHangController
    {
        // Lấy danh sách lịch sử mua hàng của tất cả khách hàng
        public static List<LichSuMuaHang> GetLichSuMuaHangTatCaKhachHang()
        {
            List<LichSuMuaHang> lichSuList = new List<LichSuMuaHang>();
            string query = @"SELECT DH.id, DH.maUser, U.hoTen, DH.tongTien, DH.trangThai, DH.created_at, DH.phuongThucThanhToan
                            FROM DONHANG DH
                            JOIN USERS U ON DH.maUser = U.id
                            ORDER BY DH.created_at DESC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LichSuMuaHang lichSu = new LichSuMuaHang
                            {
                                MaDonHang = reader["id"].ToString(),
                                MaKhachHang = reader["maUser"].ToString(),
                                TenKhachHang = reader["hoTen"].ToString(),
                                TongTien = Convert.ToInt64(reader["tongTien"]),
                                TrangThai = reader["trangThai"].ToString(),
                                NgayMua = Convert.ToDateTime(reader["created_at"]),
                                PhuongThucThanhToan = reader["phuongThucThanhToan"].ToString()
                            };
                            lichSuList.Add(lichSu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy lịch sử mua hàng của tất cả khách hàng: " + ex.Message);
            }

            return lichSuList;
        }

        // Lấy danh sách lịch sử mua hàng của một khách hàng cụ thể
        public static List<LichSuMuaHang> GetLichSuMuaHangKhachHang(string maKhachHang)
        {
            List<LichSuMuaHang> lichSuList = new List<LichSuMuaHang>();
            string query = @"SELECT DH.id, DH.maUser, U.hoTen, DH.tongTien, DH.trangThai, DH.created_at, DH.phuongThucThanhToan
                            FROM DONHANG DH
                            JOIN USERS U ON DH.maUser = U.id
                            WHERE DH.maUser = @maKhachHang
                            ORDER BY DH.created_at DESC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maKhachHang", maKhachHang);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LichSuMuaHang lichSu = new LichSuMuaHang
                            {
                                MaDonHang = reader["id"].ToString(),
                                MaKhachHang = reader["maUser"].ToString(),
                                TenKhachHang = reader["hoTen"].ToString(),
                                TongTien = Convert.ToInt64(reader["tongTien"]),
                                TrangThai = reader["trangThai"].ToString(),
                                NgayMua = Convert.ToDateTime(reader["created_at"]),
                                PhuongThucThanhToan = reader["phuongThucThanhToan"].ToString()
                            };
                            lichSuList.Add(lichSu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy lịch sử mua hàng của khách hàng: " + ex.Message);
            }

            return lichSuList;
        }

        // Lấy chi tiết đơn hàng
        public static List<ChiTietDonHang> GetChiTietDonHang(string maDonHang)
        {
            List<ChiTietDonHang> chiTietList = new List<ChiTietDonHang>();
            string query = @"SELECT CT.id, CT.maDonHang, CT.maSp, SP.tenSp, CT.soLuong, CT.giaBan, SP.hinhAnh
                            FROM CHITIETDONHANG CT
                            JOIN SANPHAM SP ON CT.maSp = SP.maSp
                            WHERE CT.maDonHang = @maDonHang";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maDonHang", maDonHang);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChiTietDonHang chiTiet = new ChiTietDonHang
                            {
                                Id = reader["id"].ToString(),
                                MaDonHang = reader["maDonHang"].ToString(),
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                HinhAnh = reader["hinhAnh"].ToString()
                            };
                            chiTietList.Add(chiTiet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy chi tiết đơn hàng: " + ex.Message);
            }

            return chiTietList;
        }

        // Lấy thông tin khách hàng
        public static UserModel GetThongTinKhachHang(string maKhachHang)
        {
            UserModel khachHang = null;
            string query = "SELECT * FROM USERS WHERE id = @maKhachHang";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maKhachHang", maKhachHang);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            khachHang = new UserModel
                            {
                                Id = reader["id"].ToString(),
                                HoTen = reader["hoTen"].ToString(),
                                Email = reader["email"].ToString(),
                                Pass = reader["pass"].ToString(),
                                DiaChi = reader["diaChi"].ToString(),
                                Sdt = reader["sdt"].ToString(),
                                Check = Convert.ToBoolean(reader["check"]),
                                HinhAnh = reader["hinhAnh"] != DBNull.Value ? reader["hinhAnh"].ToString() : null
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy thông tin khách hàng: " + ex.Message);
            }

            return khachHang;
        }

        // Tìm kiếm lịch sử mua hàng
        public static List<LichSuMuaHang> SearchLichSuMuaHang(string keyword)
        {
            List<LichSuMuaHang> lichSuList = new List<LichSuMuaHang>();
            string query = @"SELECT DH.id, DH.maUser, U.hoTen, DH.tongTien, DH.trangThai, DH.created_at, DH.phuongThucThanhToan
                            FROM DONHANG DH
                            JOIN USERS U ON DH.maUser = U.id
                            WHERE DH.id LIKE @keyword OR U.hoTen LIKE @keyword OR U.email LIKE @keyword
                            ORDER BY DH.created_at DESC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LichSuMuaHang lichSu = new LichSuMuaHang
                            {
                                MaDonHang = reader["id"].ToString(),
                                MaKhachHang = reader["maUser"].ToString(),
                                TenKhachHang = reader["hoTen"].ToString(),
                                TongTien = Convert.ToInt64(reader["tongTien"]),
                                TrangThai = reader["trangThai"].ToString(),
                                NgayMua = Convert.ToDateTime(reader["created_at"]),
                                PhuongThucThanhToan = reader["phuongThucThanhToan"].ToString()
                            };
                            lichSuList.Add(lichSu);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi tìm kiếm lịch sử mua hàng: " + ex.Message);
            }

            return lichSuList;
        }

        // Lấy tổng số đơn hàng của khách hàng
        public static int GetTongSoDonHang(string maKhachHang)
        {
            int tongSoDonHang = 0;
            string query = "SELECT COUNT(*) FROM DONHANG WHERE maUser = @maKhachHang";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maKhachHang", maKhachHang);

                    tongSoDonHang = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng số đơn hàng: " + ex.Message);
            }

            return tongSoDonHang;
        }

        // Lấy tổng tiền mua hàng của khách hàng
        public static long GetTongTienMuaHang(string maKhachHang)
        {
            long tongTien = 0;
            string query = "SELECT SUM(tongTien) FROM DONHANG WHERE maUser = @maKhachHang AND trangThai = N'Đã thanh toán'";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maKhachHang", maKhachHang);

                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongTien = Convert.ToInt64(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng tiền mua hàng: " + ex.Message);
            }

            return tongTien;
        }
    }

    // Class chứa thông tin lịch sử mua hàng
    public class LichSuMuaHang
    {
        public string MaDonHang { get; set; }
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public long TongTien { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayMua { get; set; }
        public string PhuongThucThanhToan { get; set; }
    }
}