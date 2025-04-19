using LapStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LapStore.Controller
{
    internal class NhaCungCapController
    {
        public static List<NhaCungCap> getAllNhaCungCaps()
        {
            List<NhaCungCap> NhaCungCaps = new List<NhaCungCap>();

            using (SqlConnection conn = Database.GetConnection()) // Sử dụng DatabaseHelper
            {
                string query = "SELECT * FROM NhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NhaCungCaps.Add(new NhaCungCap
                        {
                            id = reader["maNhaCungCap"].ToString(),
                            tenNhaCungCap = reader["tenNhaCungCap"].ToString(),
                            diaChi = reader["diaChi"].ToString(),
                        });
                    }
                }
            }

            return NhaCungCaps;
        }

        public static void AddNhaCungCaps(NhaCungCap NhaCungCap)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "INSERT INTO NhaCungCap(maNhaCungCap, tenNhaCungCap, diaChi) " +
                               "VALUES (@maNhaCungCap, @tenNhaCungCap, @diaChi)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maNhaCungCap", NhaCungCap.id);
                    cmd.Parameters.AddWithValue("@tenNhaCungCap", NhaCungCap.tenNhaCungCap);
                    cmd.Parameters.AddWithValue("@diaChi", NhaCungCap.diaChi);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateNhaCungCaps(NhaCungCap NhaCungCap)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "UPDATE NhaCungCap SET tenNhaCungCap = @tenNhaCungCap, diaChi = @diaChi WHERE maNhaCungCap = @maNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maNhaCungCap", NhaCungCap.id);
                    cmd.Parameters.AddWithValue("@tenNhaCungCap", NhaCungCap.tenNhaCungCap);
                    cmd.Parameters.AddWithValue("@diaChi", NhaCungCap.diaChi);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteNhaCungCaps(NhaCungCap NhaCungCap)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "DELETE FROM NhaCungCap WHERE maNhaCungCap = @maNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maNhaCungCap", NhaCungCap.id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<NhaCungCap> SearchNhaCungCaps(string searchValue)
        {
            List<NhaCungCap> NhaCungCaps = new List<NhaCungCap>();

            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT * FROM NhaCungCap WHERE maNhaCungCap LIKE @search";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchValue + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NhaCungCaps.Add(new NhaCungCap
                            {
                                id = reader["maNhaCungCap"].ToString(),
                                tenNhaCungCap = reader["tenNhaCungCap"].ToString(),
                                diaChi = reader["diaChi"].ToString(),
                            });
                        }
                    }
                }
            }

            return NhaCungCaps;
        }
        public bool CheckMa(string maSP)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM NhaCungCap WHERE maNhaCungCap = @maNhaCungCap";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maNhaCungCap", maSP);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
