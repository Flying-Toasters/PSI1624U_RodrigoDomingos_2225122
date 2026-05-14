using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void lblMudarInstrutores_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            Hide();
        }

        private void btnSignUpMem_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string passwordHash = DatabaseHelper.HashPassword(textBox2.Text.Trim());

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = @"SELECT COUNT(*) FROM Membros 
                                     WHERE email=@email AND palavra_passe=@password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", passwordHash);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Login efetuado com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Credenciais inválidas.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            new Form5().Show();
            Hide();
        }
    }
}
