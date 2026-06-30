using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormMembrosLogIn : Form
    {
        public FormMembrosLogIn()
        {
            InitializeComponent();
        }

        private void lblMudarInstrutores_Click(object sender, EventArgs e)
        {
            new FormInstrutoresLogIn().Show();
            Hide();
        }

        private void btnSignUpMem_Click(object sender, EventArgs e)
        {
            new FormMembrosSignIn().Show();
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
                    string query = @"SELECT id_membro FROM Membros 
                                     WHERE email=@email AND palavra_passe=@password";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@password", passwordHash);

                        object resultado = cmd.ExecuteScalar();

                        if(resultado != null)
                        {
                            Session.UserId = (int)resultado;
                            Session.Role = "Membro";
                            Global.GlobalVar = "Membros";

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

        private void FormMembrosLogIn_Load(object sender, EventArgs e)
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

            string pasta = Application.StartupPath;

            if (textBox2.UseSystemPasswordChar == true)
            {
                textBox2.UseSystemPasswordChar = false;
                pictureBox2.BackgroundImage = Image.FromFile(Path.Combine(pasta, "eye_15732976.png"));
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
                pictureBox2.BackgroundImage = Image.FromFile(Path.Combine(pasta, "eye_15732967.png"));
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
           
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
