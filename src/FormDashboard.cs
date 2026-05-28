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
    public partial class FormDashboard : Form
    {
        public static string userRole = null;
        public FormDashboard()
        {
            InitializeComponent();
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            bool isAdmin = Global.GlobalVar == "Administradores";
            btnAtribuirPlano.Enabled = isAdmin;
            if (isAdmin)
            {
                btnAtribuirPlano.Visible = true;
            }
            ColunaEditar.Visible = isAdmin;
            ColunaRemover.Visible = isAdmin;


            CarregarMembros();
        }

        private void CarregarMembros()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT m.id_membro, m.nome + '' + m.apelido AS Nome, m.email AS Email, ISNULL(m.telefone, '-') AS Telefone,
                                   ISNULL(pa.nome_plano, 'Sem plano') AS [Plano Ativo]
                                   FROM Membros m LEFT JOIN Pagamentos p ON p.id_membro = m.id_membro
                                   AND p.estado = 'Pago' AND p.data_fim >= CAST(GETDATE() AS DATE)
                                   LEFT JOIN PlanosAssinatura pa ON pa.id_plano = p.id_plano WHERE m.ativo = 1
                                   ORDER BY m.nome";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvMembros.AutoGenerateColumns = false;
                    dgvMembros.DataSource = dt;

                    ColunaNome.DataPropertyName = "Nome";
                    ColunaEmail.DataPropertyName = "Email";
                    ColunaTelefone.DataPropertyName = "Telefone";

                    ColunaPlanoAtivo.DataPropertyName = "Plano Ativo";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar membros: " + ex.Message);
            }
        }



        private void btnAtribuirPlano_Click(object sender, EventArgs e)
        {
            if (dgvMembros.CurrentRow == null)
            {
                return;
            }

            int idMembro = (int)
                ((DataRowView)dgvMembros.CurrentRow.DataBoundItem)
                .Row["id_membro"];
            using (Form dlg = new Form())
            {
                dlg.Text = "Atribuir Plano";
                dlg.Size = new System.Drawing.Size(320, 200);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;

                Label lbl = new Label { Text = "Plano:", Left = 10, Top = 15, Width = 60 };
                ComboBox cbx = new ComboBox
                {
                    Left = 75,
                    Top = 12,
                    Width = 200,
                    DropDownStyle = ComboBoxStyle.DropDownList
                };

                Label lblData = new Label
                {
                    Text = "Início:", Left = 10, Top = 50, Width = 60
                };

                DateTimePicker dtp = new DateTimePicker { Left = 75, Top = 47, Width = 200, Value = DateTime.Today };

                Button btnOk = new Button { Text = "Guardar", Left = 110, Top = 110, Width = 80 };

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        string sql = "SELECT id_plano, nome_plano + ' (' + CAST(preco AS NVARCHAR) + '€)' AS label FROM PlanosAssinatura WHERE ativo = 1";
                        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        cbx.DisplayMember = "label";
                        cbx.ValueMember = "id_plano";
                        cbx.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                    return;
                }

                btnOk.Click += (s, ev) =>
                {
                    if (cbx.SelectedValue == null)
                        return;

                    int idPlano = (int)cbx.SelectedValue;
                    DateTime inicio = dtp.Value.Date;

                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            string sqlDur = "SELECT duracao_meses, preco FROM PlanosAssinatura WHERE id_plano=@id";
                            SqlCommand cmdDur = new SqlCommand(sqlDur, conn);

                            cmdDur.Parameters.AddWithValue("@id", idPlano);

                            SqlDataReader dr = cmdDur.ExecuteReader();
                            int duracao = 1;
                            decimal preco = 0;
                            if (dr.Read())
                            {
                                duracao = (int)dr["duracao_meses"];
                                preco = (decimal)dr["preco"];
                            }
                            dr.Close();

                            DateTime fim = inicio.AddMonths(duracao).AddDays(-1);

                            string sqllns = @"INSERT INTO Pagamentos(id_membro, id_plano, valor, data_inicio, data_fim, estado)
                                              VALUES (@m, @p, @v, @di, @df, 'Pago')";


                            SqlCommand cmd = new SqlCommand(sqllns, conn);

                            cmd.Parameters.AddWithValue("@m", idMembro);
                            cmd.Parameters.AddWithValue("@p", idPlano);
                            cmd.Parameters.AddWithValue("@v", preco);
                            cmd.Parameters.AddWithValue("@di", inicio);
                            cmd.Parameters.AddWithValue("@df", fim);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Plano atribuído com sucesso.");
                        dlg.Close();
                        CarregarMembros();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao atribuir plano: " + ex.Message);
                    }
                };


                dlg.Controls.AddRange(new Control[] { lbl, cbx, lblData, dtp, btnOk });
                dlg.ShowDialog(this);
            }
        }

        private void dgvMembros_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idMembro = (int)
                ((DataRowView)dgvMembros.Rows[e.RowIndex].DataBoundItem)
                .Row["id_membro"];

            if (e.ColumnIndex == ColunaRemover.Index)
            {
                if (MessageBox.Show("Remover membro?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            string sql = "UPDATE Membros SET ativo=0 WHERE id_membro = @id";
                            SqlCommand cmd = new SqlCommand(sql, conn);

                            cmd.Parameters.AddWithValue("@id", idMembro);
                            cmd.ExecuteNonQuery();
                        }
                        CarregarMembros();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro: " + ex.Message);
                    }
                }
            }
        }


    }
}
