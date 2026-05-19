using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Form6 : Form
    {
        string userEmail = Form5.to;
        string userRole = Form5.userRole;
        public Form6()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text)
            {
                string hashedPassword = DatabaseHelper.HashPassword(textBox2.Text.Trim());

                if (userRole == "Membro")
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Membros SET palavra_passe = @password WHERE email = @email", conn))
                    {
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", userEmail);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Palavra-passe alterada com sucesso");
                }
                else if (userRole == "Instrutor")
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Instrutores SET palavra_passe = @password WHERE email = @email", conn))
                    {
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", userEmail);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Palavra-passe alterada com sucesso");
                }
                else if (userRole == "Admin")
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Administradores SET palavra_passe = @password WHERE email = @email", conn))
                    {
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@email", userEmail);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Palavra-passe alterada com sucesso");
                }
            }
            else
            {
                MessageBox.Show("As palavras-passe não coincidem", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
