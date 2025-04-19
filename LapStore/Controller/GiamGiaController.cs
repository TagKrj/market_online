using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LapStore.Model;

namespace LapStore.Controller
{
    internal class GiamGiaController
    {
        public static List<GiamGia> getAllGiamGias()
        {
            List<GiamGia> GiamGias = new List<GiamGia>();

            using (SqlConnection conn = Database.GetConnection()) // Sử dụng DatabaseHelper
            {
                string query = "SELECT * FROM GIAMGIA";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GiamGias.Add(new GiamGia
                        {
                            id = reader["maGiamGia"].ToString(),
                            tenGiamGia = reader["tenGiamGia"].ToString(),
                            soGiamGia = reader["soGiamGia"].ToString(),
                        });
                    }
                }
            }

            return GiamGias;
        }

        public static void AddGiamGias(GiamGia GiamGia)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "INSERT INTO GIAMGIA(maGiamGia, tenGiamGia, soGiamGia) " +
                               "VALUES (@maGiamGia, @tenGiamGia, @soGiamGia)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maGiamGia", GiamGia.id);
                    cmd.Parameters.AddWithValue("@tenGiamGia", GiamGia.tenGiamGia);
                    cmd.Parameters.AddWithValue("@soGiamGia", GiamGia.soGiamGia);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateGiamGias(GiamGia GiamGia)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "UPDATE GIAMGIA SET tenGiamGia = @tenGiamGia, soGiamGia = @soGiamGia WHERE maGiamGia = @maGiamGia";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maGiamGia", GiamGia.id);
                    cmd.Parameters.AddWithValue("@tenGiamGia", GiamGia.tenGiamGia);
                    cmd.Parameters.AddWithValue("@soGiamGia", GiamGia.soGiamGia);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteGiamGias(GiamGia GiamGia)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "DELETE FROM GIAMGIA WHERE maGiamGia = @maGiamGia";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maGiamGia", GiamGia.id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<GiamGia> SearchGiamGias(string searchValue)
        {
            List<GiamGia> GiamGias = new List<GiamGia>();

            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT * FROM GIAMGIA WHERE maGiamGia LIKE @search";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchValue + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GiamGias.Add(new GiamGia
                            {
                                id = reader["maGiamGia"].ToString(),
                                tenGiamGia = reader["tenGiamGia"].ToString(),
                                soGiamGia = reader["soGiamGia"].ToString(),
                            });
                        }
                    }
                }
            }

            return GiamGias;
        }
        public bool CheckMa(string maSP)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM GIAMGIA WHERE maGiamGia = @maGiamGia";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maGiamGia", maSP);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
