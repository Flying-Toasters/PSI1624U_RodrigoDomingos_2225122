using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormInstrutoresLogIn : Form
    {
        public FormInstrutoresLogIn()
        {
            InitializeComponent();
        }

        private void lblMudarMembros_Click(object sender, EventArgs e)
        {
            new FormMembrosLogIn().Show();
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FormInstrutoresSignIn().Show();
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
                    string query = @"SELECT COUNT(*) FROM Instrutores 
                                     WHERE email=@email AND palavra_passe=@password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", passwordHash);

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            Global.GlobalVar = "Instrutores";
                            new FormDashboard().Show();
                            Hide();
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

            if (RememberMe.Checked)
            {
                Properties.Settings.Default.userEmail = textBox1.Text;
                Properties.Settings.Default.userPassword = textBox2.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.userEmail = string.Empty;
                Properties.Settings.Default.userPassword = string.Empty;
                Properties.Settings.Default.Save();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            new FormForgotPassword().Show();
            Hide();
        }

        private void FormInstrutoresLogIn_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.userEmail != string.Empty)
            {
                textBox1.Text = Properties.Settings.Default.userEmail;
                textBox2.Text = Properties.Settings.Default.userPassword;
                RememberMe.Checked = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
                pictureBox2.BackgroundImage = Image.FromFile("C:\\Users\\2225122\\Downloads\\sthenos_projeto_atualizado\\github\\src\\img\\eye_15732976.png");
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
                pictureBox2.BackgroundImage = Image.FromFile("C:\\Users\\2225122\\Downloads\\sthenos_projeto_atualizado\\github\\src\\img\\eye_15732967.png");
            }
        }
    }
}
