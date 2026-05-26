using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormDashboard : Form
    {
        public static string userRole = null;
        public FormDashboard()
        {
            InitializeComponent();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            if (Global.GlobalVar == "Administradores")
            {
                btnAdicionarMembro.Visible = true;
                btnAtribuirPlano.Visible = true;
                ColunaEditar.Visible = true;
                ColunaRemover.Visible = true;
            }
        }

        private void btnAdicionarMembro_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yeah bro, For You page. Chinese buddy car flying through the windshield, Ukraine buddy legs, it's For You");
        }

        private void btnAtribuirPlano_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yeah bro, Summer Games Fest. Roguelites, AAA, gacha game update, friendslop, live service. Its Summer Games Fest");
        }
    }
}
