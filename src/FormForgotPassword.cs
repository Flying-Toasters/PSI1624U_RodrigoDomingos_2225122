using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormForgotPassword : Form
    {
        public static string userName = null;
        public static string userRole = null;
        string randomCode;
        public static string to;
        public FormForgotPassword()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string from, pass, messageBody;
            Random rand = new Random();
            randomCode = (rand.Next(99999)).ToString();
            MailMessage message = new MailMessage();
            to = email;
            from = "sthenoshelp@gmail.com";
            pass = "vksz yrpo vbcd prsx";
            messageBody = "O seu código é: " + randomCode;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = "Repôr palavra-passe Sthenos";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);




            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    using (var cmd = new SqlCommand("SELECT nome, apelido FROM Membros WHERE email = @e AND ativo = 1", conn))
                    {
                        cmd.Parameters.AddWithValue("@e", email);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userName = reader["nome"].ToString();
                                userRole = "Membro";
                            }
                        }
                    }
                    if (userRole == null)
                    {
                        using (var cmd = new SqlCommand("SELECT nome, apelido FROM Instrutores WHERE email = @e AND ativo = 1", conn))
                        {
                            cmd.Parameters.AddWithValue("@e", email);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userName = reader["nome"].ToString();
                                    userRole = "Instrutor";
                                }
                            }
                        }
                    }
                    if (userRole == null)
                    {
                        using (var cmd = new SqlCommand("SELECT nome, apelido FROM Administradores WHERE email = @e", conn))
                        {
                            cmd.Parameters.AddWithValue("@e", email);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userName = reader["nome"].ToString();
                                    userRole = "Admin";
                                }
                            }
                        }
                    }

                }
                if (userRole != null)
                {
                    smtp.Send(message);
                }
                
                MessageBox.Show("Código enviado para " + email + ".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (randomCode == (textBox2.Text).ToString())
            {
                to = textBox1.Text;
                new FormResetPassword().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Código inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            new FormMembrosLogIn().Show();
            Hide();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }
    }
}
