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
using System.Windows.Forms.VisualStyles;

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
            
            this.planosAssinaturaTableAdapter.Fill(this.sthenosDBDataSet.PlanosAssinatura);
            bool isAdmin = Global.GlobalVar == "Administradores";
            btnAtribuirPlano.Visible = isAdmin;
            ColunaEditar.Visible = isAdmin;
            ColunaRemover.Visible = isAdmin;
            btnAdicionarAula.Visible = isAdmin;
            RemoverAula.Visible = isAdmin;
            

            CarregarMembros();
            CarregarPlanos();
            CarregarAulas();
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

        private void CarregarPlanos()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT nome_plano AS [Nome do Plano], duracao_meses AS [Duração (meses)],
                                   preco AS [Preço (€)], CASE ativo WHEN 1 THEN 'Ativo' ELSE 'Inativo' END
                                   AS [Estado] FROM PlanosAssinatura ORDER BY duracao_meses";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvPlanos.AutoGenerateColumns = true;
                    dgvPlanos.DataSource = dt;
                    dgvPlanos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvPlanos.ReadOnly = true;
                    dgvPlanos.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar planos: " + ex.Message);
            }
        }

        private void CarregarAulas()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT a.id_aula, a.nome_aula AS [Aula], i.nome + ' ' + i.apelido AS [Instrutor],
                                   FORMAT(a.data_hora, 'dd/MM/yyyy HH:mm') AS [Data/Hora], a.duracao_min
                                   AS [Duração (min)], a.vagas AS [Vagas], ISNULL(a.local, '-') AS [Local]
                                   FROM Aulas a JOIN Instrutores i ON i.id_instrutor = a.id_instrutor ORDER BY
                                   a.data_hora";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAulas.AutoGenerateColumns = true;
                    dgvAulas.DataSource = dt;
                    dgvAulas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvAulas.ReadOnly = true;
                    dgvAulas.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar aulas: " + ex.Message);
            }
        }

        private void btnAdicionarAula_Click(object sender, EventArgs e)
        {
            using (Form dlg = new Form())
            {
                dlg.Text = "Adicionar Aula";
                dlg.Size = new System.Drawing.Size(340, 310);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;

                Label lblNome = new Label { Text = "Nome da Aula:", Left = 10, Top = 12, Width = 90 };

                TextBox txtNome = new TextBox { Left = 110, Top = 12, Width = 200 };

                Label lblInst = new Label { Text = "Instrutor:", Left = 10, Top = 45, Width = 90 };

                ComboBox cbInst = new ComboBox { Left = 110, Top = 45, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

                Label lblDH = new Label { Text = "Data/Hora:", Left = 10, Top = 90, Width = 90 };

                DateTimePicker dtpDH = new DateTimePicker { Left = 110, Top = 90, Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = false, Value = DateTime.Now };

                Label lblDur = new Label { Text = "Duração (min):", Left = 10, Top = 117, Width = 90 };

                NumericUpDown nudDur = new NumericUpDown { Left = 110, Top = 117, Width = 80, Minimum = 15, Maximum = 240, Value = 60, Increment = 15 };

                Label lblVagas = new Label { Text = "Vagas:", Left = 10, Top = 152, Width = 90 };

                NumericUpDown nudVagas = new NumericUpDown { Left = 110, Top = 152, Width = 80, Minimum = 1, Maximum = 200, Value = 20 };

                Label lblLocal = new Label { Text = "Local:", Left = 10, Top = 187, Width = 90 };

                TextBox txtLocal = new TextBox { Left = 110, Top = 187, Width = 200 };

                Button btnOk = new Button { Text = "Guardar", Left = 120, Top = 232, Width = 90 };


                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        string sql = "SELECT id_instrutor, nome + ' ' + apelido AS label FROM Instrutores WHERE ativo=1 ORDER BY nome";
                        SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        cbInst.DisplayMember = "label";
                        cbInst.ValueMember = "id_instrutor";
                        cbInst.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar instrutores: " + ex.Message);
                    return;
                }

                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNome.Text) || cbInst.SelectedValue == null)
                    {
                        MessageBox.Show("Nome e instrutor são obrigatórios.");
                        return;
                    }

                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            string sql = @"
                            INSERT INTO Aulas (nome_aula, id_instrutor, data_hora, duracao_min, vagas, local)
                            VALUES (@nome, @instrutor, @datahora, @duracao, @vagas, @local)";

                            SqlCommand cmd = new SqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@instrutor", (int)cbInst.SelectedValue);
                            cmd.Parameters.AddWithValue("@datahora", dtpDH.Value);
                            cmd.Parameters.AddWithValue("@duracao", (int)nudDur.Value);
                            cmd.Parameters.AddWithValue("@vagas", (int)nudVagas.Value);
                            cmd.Parameters.AddWithValue("@local", string.IsNullOrWhiteSpace(txtLocal.Text) ? (object)DBNull.Value : txtLocal.Text.Trim());

                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Aula adicionada com sucesso.");
                        dlg.Close();
                        CarregarAulas();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao adicionar aula: " + ex.Message);
                    }
                };

                dlg.Controls.AddRange(new Control[]

                {
                    lblNome, txtNome, lblInst, cbInst, lblDH, dtpDH, lblDur, nudDur, lblVagas, nudVagas, lblLocal, txtLocal, btnOk
                });
                dlg.ShowDialog(this);
            }
        }

        private void dgvAulas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int idAula = (int)((DataRowView)dgvAulas.Rows[e.RowIndex].DataBoundItem).Row["id_aula"];
            if (dgvAulas.Columns[e.ColumnIndex].Name == "RemoverAula")
            {
                if (MessageBox.Show("Remover esta aula?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        SqlCommand cmdInsc = new SqlCommand("DELETE FROM Inscricoes WHERE id_aula = @id", conn);
                        cmdInsc.Parameters.AddWithValue("@id", idAula);
                        cmdInsc.ExecuteNonQuery();
                        SqlCommand cmdaula = new SqlCommand("DELETE FROM Aulas WHERE id_aula = @id", conn);
                        cmdaula.Parameters.AddWithValue("@id", idAula);
                        cmdaula.ExecuteNonQuery();
                    }
                    MessageBox.Show("Aula removida.");
                    CarregarAulas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover aula: " + ex.Message);
                }
            }

            if (dgvAulas.Columns[e.ColumnIndex].Name == "InscreverAula")
            {
                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        SqlCommand cmdCheck = new SqlCommand(@"SELECT COUNT(*) FROM Inscricoes WHERE id_membro = @m AND id_aula = @a", conn);
                        cmdCheck.Parameters.AddWithValue("@m", Session.UserId);
                        cmdCheck.Parameters.AddWithValue("@a", idAula);
                        int jaInscrito = (int)cmdCheck.ExecuteScalar();
                        if (jaInscrito > 0)
                        {
                            MessageBox.Show("Já estás inscrito nesta aula.");
                            return;
                        }

                        SqlCommand cmdVagas = new SqlCommand(@"SELECT a.vagas - COUNT (i.id_inscricao) FROM AULAS a LEFT JOIN Inscricoes i ON i.id_aula = a.id_aula WHERE a.id_aula = @a GROUP BY a.vagas", conn);
                        cmdVagas.Parameters.AddWithValue("@a", idAula);
                        object resultado = cmdVagas.ExecuteScalar();
                        int vagasLivres = resultado != null ? (int)resultado : 0;

                        if (vagasLivres <= 0)
                        {
                            MessageBox.Show("Não há vagas disponíveis nesta aula.");
                            return;
                        }

                        SqlCommand cmdInsc = new SqlCommand(@"INSERT INTO Inscricoes (id_membro, id_aula)
                                                              VALUES (@m, @a)", conn);
                        cmdInsc.Parameters.AddWithValue("@m", Session.UserId);
                        cmdInsc.Parameters.AddWithValue("@a", idAula);
                        cmdInsc.ExecuteNonQuery();
                    }
                    MessageBox.Show("Inscrição realizada com sucesso.");
                    CarregarAulas();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inscrever: " + ex.Message);
                }
            }





        }


    }
}
