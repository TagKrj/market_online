using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    internal class KhoHangController
    {
        public static List<SanPhamKho> GetAllSanPhamKho()
        {
            List<SanPhamKho> danhSachSanPham = new List<SanPhamKho>();
            string query = @"SELECT SP.maSp, SP.tenSp, SP.hinhAnh, DM.tenDanhMuc, SP.soLuong, SP.giaNhap, SP.giaBan, SP.created_at
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        ORDER BY SP.soLuong ASC";

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
                            SanPhamKho sanPham = new SanPhamKho
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                HinhAnh = reader["hinhAnh"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaNhap = Convert.ToInt64(reader["giaNhap"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                NgayTao = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm trong kho: " + ex.Message);
            }

            return danhSachSanPham;
        }

        // Lấy danh sách sản phẩm sắp hết hàng (số lượng < 10)
        public static List<SanPhamKho> GetSanPhamSapHetHang()
        {
            List<SanPhamKho> danhSachSanPham = new List<SanPhamKho>();
            string query = @"SELECT SP.maSp, SP.tenSp, SP.hinhAnh, DM.tenDanhMuc, SP.soLuong, SP.giaNhap, SP.giaBan, SP.created_at
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE SP.soLuong < 10
                        ORDER BY SP.soLuong ASC";

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
                            SanPhamKho sanPham = new SanPhamKho
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                HinhAnh = reader["hinhAnh"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaNhap = Convert.ToInt64(reader["giaNhap"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                NgayTao = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm sắp hết hàng: " + ex.Message);
            }

            return danhSachSanPham;
        }

        // Lấy danh sách sản phẩm hết hàng (số lượng = 0)
        public static List<SanPhamKho> GetSanPhamHetHang()
        {
            List<SanPhamKho> danhSachSanPham = new List<SanPhamKho>();
            string query = @"SELECT SP.maSp, SP.tenSp, SP.hinhAnh, DM.tenDanhMuc, SP.soLuong, SP.giaNhap, SP.giaBan, SP.created_at
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE SP.soLuong = 0
                        ORDER BY SP.created_at DESC";

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
                            SanPhamKho sanPham = new SanPhamKho
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                HinhAnh = reader["hinhAnh"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaNhap = Convert.ToInt64(reader["giaNhap"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                NgayTao = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm hết hàng: " + ex.Message);
            }

            return danhSachSanPham;
        }

        // Lấy danh sách sản phẩm theo danh mục
        public static List<SanPhamKho> GetSanPhamTheoDanhMuc(string maDanhMuc)
        {
            List<SanPhamKho> danhSachSanPham = new List<SanPhamKho>();
            string query = @"SELECT SP.maSp, SP.tenSp, SP.hinhAnh, DM.tenDanhMuc, SP.soLuong, SP.giaNhap, SP.giaBan, SP.created_at
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE SP.maDm = @maDanhMuc
                        ORDER BY SP.soLuong ASC";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maDanhMuc", maDanhMuc);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SanPhamKho sanPham = new SanPhamKho
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                HinhAnh = reader["hinhAnh"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaNhap = Convert.ToInt64(reader["giaNhap"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                NgayTao = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy danh sách sản phẩm theo danh mục: " + ex.Message);
            }

            return danhSachSanPham;
        }

        // Cập nhật số lượng sản phẩm trong kho
        public static bool CapNhatSoLuongSanPham(string maSp, int soLuongMoi)
        {
            bool ketQua = false;
            string query = "UPDATE SANPHAM SET soLuong = @soLuongMoi WHERE maSp = @maSp";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@maSp", maSp);
                    cmd.Parameters.AddWithValue("@soLuongMoi", soLuongMoi);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    ketQua = rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhật số lượng sản phẩm: " + ex.Message);
            }

            return ketQua;
        }

        // Tìm kiếm sản phẩm trong kho
        public static List<SanPhamKho> SearchSanPhamKho(string keyword)
        {
            List<SanPhamKho> danhSachSanPham = new List<SanPhamKho>();
            string query = @"SELECT SP.maSp, SP.tenSp, SP.hinhAnh, DM.tenDanhMuc, SP.soLuong, SP.giaNhap, SP.giaBan, SP.created_at
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        WHERE SP.maSp LIKE @keyword OR SP.tenSp LIKE @keyword OR DM.tenDanhMuc LIKE @keyword
                        ORDER BY SP.soLuong ASC";

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
                            SanPhamKho sanPham = new SanPhamKho
                            {
                                MaSp = reader["maSp"].ToString(),
                                TenSp = reader["tenSp"].ToString(),
                                HinhAnh = reader["hinhAnh"].ToString(),
                                TenDanhMuc = reader["tenDanhMuc"].ToString(),
                                SoLuong = Convert.ToInt32(reader["soLuong"]),
                                GiaNhap = Convert.ToInt64(reader["giaNhap"]),
                                GiaBan = Convert.ToInt64(reader["giaBan"]),
                                NgayTao = Convert.ToDateTime(reader["created_at"])
                            };
                            danhSachSanPham.Add(sanPham);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi tìm kiếm sản phẩm trong kho: " + ex.Message);
            }

            return danhSachSanPham;
        }

        // Lấy tổng số sản phẩm trong kho
        public static int GetTongSoSanPham()
        {
            int tongSoSanPham = 0;
            string query = "SELECT COUNT(*) FROM SANPHAM";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    tongSoSanPham = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng số sản phẩm: " + ex.Message);
            }

            return tongSoSanPham;
        }

        // Lấy tổng số lượng của tất cả sản phẩm
        public static int GetTongSoLuongTatCaSanPham()
        {
            int tongSoLuong = 0;
            string query = "SELECT SUM(soLuong) FROM SANPHAM";

            try
            {
                using (SqlConnection conn = Database.GetConnection())
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        tongSoLuong = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy tổng số lượng sản phẩm: " + ex.Message);
            }

            return tongSoLuong;
        }

        // Lấy số lượng sản phẩm theo danh mục
        public static Dictionary<string, int> GetSoLuongTheoDanhMuc()
        {
            Dictionary<string, int> soLuongTheoDanhMuc = new Dictionary<string, int>();
            string query = @"SELECT DM.tenDanhMuc, SUM(SP.soLuong) as tongSoLuong
                        FROM SANPHAM SP
                        JOIN DANHMUC DM ON SP.maDm = DM.id
                        GROUP BY DM.tenDanhMuc
                        ORDER BY tongSoLuong DESC";

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
                            string tenDanhMuc = reader["tenDanhMuc"].ToString();
                            int tongSoLuong = Convert.ToInt32(reader["tongSoLuong"]);
                            soLuongTheoDanhMuc.Add(tenDanhMuc, tongSoLuong);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lấy số lượng theo danh mục: " + ex.Message);
            }

            return soLuongTheoDanhMuc;
        }
    }

    // Class chứa thông tin sản phẩm trong kho
    public class SanPhamKho
    {
        public string MaSp { get; set; }
        public string TenSp { get; set; }
        public string HinhAnh { get; set; }
        public string TenDanhMuc { get; set; }
        public int SoLuong { get; set; }
        public long GiaNhap { get; set; }
        public long GiaBan { get; set; }
        public DateTime NgayTao { get; set; }
    }
}