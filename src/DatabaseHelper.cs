using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace WindowsFormsApp1
{

    public static class DatabaseHelper
    {

        private const string ConnectionString =
            @"Server=(localdb)\\MSSQLLocalDB;Database=SthenosDB;Trusted_Connection=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                    return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }
    }
}