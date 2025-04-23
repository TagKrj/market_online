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
        public static List<LichSuDonHangInfo> searchLichSuDonHangInfos(string text)
        {
            List<LichSuDonHangInfo> LichSuDonHangInfos = new List<LichSuDonHangInfo>();

            string query = @"
            SELECT
                dh.id AS idDonHang,
                dh.created_at AS created_atDonHang,
                u.hoTen AS hoTenUsers
            FROM
                DONHANG dh
            INNER JOIN
                USERS u ON dh.maUser = u.id
            WHERE
                dh.maUser = @UserId";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@UserId", text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        LichSuDonHangInfos.Add(new LichSuDonHangInfo
                        {
                            IdDonHang = reader["idDonHang"].ToString(), // Sửa lại tên cột
                            HoTenUsers = reader["hoTenUsers"].ToString(), // Sửa lại tên cột
                            CreatedAtDonHang = Convert.ToDateTime(reader["created_atDonHang"]),
                        });
                    }
                }
            }

            return LichSuDonHangInfos;
        }

        public static string searchTenKH(string text)
        {
            string query = @"
            SELECT
                hoTen
            FROM
                USERS
            WHERE
                id = @UserId";
            string s = null;
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@UserId", text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        s = reader["hoTen"].ToString();
                    }
                }
            }

            return s;
        }

        public static List<ChiTietDonHang> SearchChiTietDonHangs(string text)
        {
            List<ChiTietDonHang> ChiTietDonHangs = new List<ChiTietDonHang>();
            string query = @"
            SELECT
                ctdh.*, -- Select all columns from CHITIETDONHANG
                sp.tenSp -- Select tenSp from SANPHAM
            FROM
                CHITIETDONHANG ctdh
            INNER JOIN
                SANPHAM sp ON ctdh.maSp = sp.maSp
            WHERE
                ctdh.maDonHang = @text";
            using (SqlCommand cmd = new SqlCommand(query, Database.GetConnection()))
            {
                cmd.Parameters.AddWithValue("@text", text);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ChiTietDonHangs.Add(new ChiTietDonHang
                        {
                            Id = reader["id"].ToString(), // Sửa lại tên cột
                            MaDonHang = reader["maDonHang"].ToString(), // Sửa lại tên cột
                            MaSp = reader["maSp"].ToString(),
                            TenSp = reader["tenSp"].ToString(),
                            SoLuong = Convert.ToInt32(reader["soLuong"]),
                            GiaBan = Convert.ToInt64(reader["giaBan"]),
                        });
                    }
                }
            }

            return ChiTietDonHangs;
        }
    }
}

