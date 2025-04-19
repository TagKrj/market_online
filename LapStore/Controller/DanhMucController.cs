using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LapStore.Model;

namespace LapStore.Controller
{
    internal class DanhMucController
    {
        public static List<DanhMuc> getAllDanhMucs()
        {
            List<DanhMuc> DanhMucs = new List<DanhMuc>();

            using (SqlConnection conn = Database.GetConnection()) // Sử dụng DatabaseHelper
            {
                string query = "SELECT * FROM DANHMUC";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DanhMucs.Add(new DanhMuc
                        {
                            id = reader["id"].ToString(),
                            tenDanhMuc = reader["tenDanhMuc"].ToString(),
                        });
                    }
                }
            }

            return DanhMucs;
        }

        public static void AddDanhMucs(DanhMuc DanhMuc)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "INSERT INTO DANHMUC(id, tenDanhMuc) " +
                               "VALUES (@id, @tenDanhMuc)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", DanhMuc.id);
                    cmd.Parameters.AddWithValue("@tenDanhMuc", DanhMuc.tenDanhMuc);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void UpdateDanhMucs(DanhMuc DanhMuc)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "UPDATE DANHMUC SET tenDanhMuc = @tenDanhMuc WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", DanhMuc.id);
                    cmd.Parameters.AddWithValue("@tenDanhMuc", DanhMuc.tenDanhMuc);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteDanhMucs(DanhMuc DanhMuc)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "DELETE FROM DANHMUC WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", DanhMuc.id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static List<DanhMuc> SearchDanhMucs(string searchValue)
        {
            List<DanhMuc> DanhMucs = new List<DanhMuc>();

            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT * FROM DANHMUC WHERE id LIKE @search";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchValue + "%");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DanhMucs.Add(new DanhMuc
                            {
                                id = reader["id"].ToString(),
                                tenDanhMuc = reader["tenDanhMuc"].ToString(),
                            });
                        }
                    }
                }
            }

            return DanhMucs;
        }
        public bool CheckMa(string maSP)
        {
            using (SqlConnection conn = Database.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM DANHMUC WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@maSP", maSP);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
