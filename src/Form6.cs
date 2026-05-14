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

namespace WindowsFormsApp1
{
    public partial class Form6 : Form
    {
        string username = Form5.to;
        string userRole = Form5.userRole;
        public Form6()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text)
            {
                if (userRole == "Membro")
                {
                    SqlConnection conn = DatabaseHelper.GetConnection();
                    SqlCommand cmd = new SqlCommand($"UPDATE Membros SET password = {DatabaseHelper.HashPassword(textBox2.Text.Trim())} WHERE username={username}", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
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
