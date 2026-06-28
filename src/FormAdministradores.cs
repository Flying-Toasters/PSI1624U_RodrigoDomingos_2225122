using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormAdministradores : Form
    {
        public FormAdministradores()
        {
            InitializeComponent();
        }

        private void btnLogInAdmin_Click(object sender, EventArgs e)
        {

            string email = textBox1.Text.Trim();
            string passwordHash = DatabaseHelper.HashPassword(textBox2.Text.Trim());

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = @"SELECT id_admin FROM Administradores 
                                     WHERE email=@email AND palavra_passe=@password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", passwordHash);

                        object resultado = cmd.ExecuteScalar();

                        if (resultado != null)
                        {
                            MessageBox.Show("Acesso autorizado.");
                            Session.UserId = (int)resultado;
                            Global.GlobalVar = "Administradores";
                            new FormDashboard().Show();
                            Hide();
                        }
                        else
                        {
                            MessageBox.Show("Acesso negado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

    }
}
