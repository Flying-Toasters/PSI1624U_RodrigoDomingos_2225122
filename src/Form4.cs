using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void btnSignUpMem_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("As palavras-passe não coincidem.");
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string query = @"INSERT INTO Instrutores
                    (nome, apelido, email, palavra_passe)
                    VALUES (@nome, @apelido, @email, @password)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", textBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@apelido", textBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@password",
                            DatabaseHelper.HashPassword(textBox2.Text.Trim()));

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Instrutor registado com sucesso!");
                new Form2().Show();
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void lblMudarInstrutores_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "SKY" && textBox5.Text == "GRAY" && textBox1.Text == "DAYS" && textBox2.Text == "VOID" && textBox3.Text == "STRANGER")
            {
                new Form7().Show();
                Hide();
            }
        }
    }
}
