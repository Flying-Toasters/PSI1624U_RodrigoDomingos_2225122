using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;


namespace WindowsFormsApp1
{
    public static class DatabaseInitializer
    {
        private const string MasterConnection =
  @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;TrustServerCertificate=True";



        public static bool InicializarBD()
        {
            try
            {
                if (!BDExiste())
                    CriarBD();



                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                "Não foi possível inicializar a base de dados.\n\n" + ex.Message,
                "Erro de arranque",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return false;
            }
        }



        private static bool BDExiste()
        {
            using (SqlConnection conn = new SqlConnection(MasterConnection))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM sys.databases WHERE name = 'SthenosDB'", conn))
                {
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }



        private static void CriarBD()
        {




            using (SqlConnection conn = new SqlConnection(MasterConnection))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("CREATE DATABASE SthenosDB", conn))
                    cmd.ExecuteNonQuery();
            }

            string sthenosConnection = @"Server=(localdb)\MSSQLLocalDB;Database=SthenosDB;Trusted_Connection=True;TrustServerCertificate=True";
            string[] blocos = ObterScriptSQL().Split(new[] { "\r\nGO", "\nGO" },
  StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection conn = new SqlConnection(sthenosConnection))
            {
                conn.Open();


                foreach (string bloco in blocos)
                {
                    string sql = bloco.Trim();
                    if (string.IsNullOrEmpty(sql)) continue;



                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                        cmd.ExecuteNonQuery();
                }
            }
        }
        



        private static string ObterScriptSQL()
        {
            string caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scriptsbd", "SQLQuery1.sql");
            return File.ReadAllText(caminho);
        }
    }
}