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
            CarregarPagamentos();
            CarregarEventos();
            CarregarEquipamentos();
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
                                   LEFT JOIN PlanosAssinatura pa ON pa.id_plano = p.id_plano WHERE m.ativo = 1";

                    if (Global.GlobalVar == "Membros")
                    {
                        sql += " AND m.id_membro = @idMembro";
                    }

                    sql += " ORDER BY m.nome";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                    if (Global.GlobalVar == "Membros")
                    {
                        da.SelectCommand.Parameters.AddWithValue("@idMembro", Session.UserId);
                    }

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
                    string sql = @"SELECT id_plano, nome_plano AS [Nome do Plano], ISNULL(descricao, '-') AS [Descrição], duracao_meses AS [Duração (meses)],
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

                    if (dgvAulas.Columns["id_aula"] != null)
                        dgvAulas.Columns["id_aula"].Visible = false;
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

            string colName = dgvAulas.Columns[e.ColumnIndex].Name;

            if (colName == "RemoverAula")
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

            if (colName == "InscreverAula")
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

            if (colName == "aulaParticipantes")
            {
                MostrarParticipantes("Participantes da aula", @"SELECT i.id_inscricao, m.nome + ' ' + m.apelido AS Membro, m.email AS Email, CASE i.presenca WHEN 1 THEN 'Sim' ELSE 'Não' END AS [Presença] FROM INSCRICOES i JOIN Membros m ON m.id_membro = i.id_membro WHERE i.id_aula = @id ORDER BY m.nome", idAula);
            }
        }

        private void CarregarPagamentos()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT m.nome + ' ' + m.apelido AS [Membro], pa.nome_plano AS [Plano], p.valor
                                   AS [Valor (€)], FORMAT(p.data_pagamento, 'dd/MM/yyyy') AS [Data Pagamento],
                                   FORMAT (p.data_inicio, 'dd/MM/yyyy') AS [Início],
                                   FORMAT (p.data_fim, 'dd/MM/yyyy') AS [Fim], p.estado AS [Estado]
                                   FROM Pagamentos p JOIN Membros m ON m.id_membro = p.id_membro
                                   JOIN PlanosAssinatura pa ON pa.id_plano = p.id_plano
                                   ";

                    if (Global.GlobalVar == "Membros")
                        sql += " WHERE p.id_membro = @idMembro";
                    sql += " ORDER BY p.data_pagamento DESC";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    if (Global.GlobalVar == "Membros")
                        da.SelectCommand.Parameters.AddWithValue("@idMembro", Session.UserId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvPagamentos.AutoGenerateColumns = true;
                    dgvPagamentos.DataSource = dt;
                    dgvPagamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvPagamentos.ReadOnly = true;
                    dgvPagamentos.AllowUserToAddRows = false;

                    if (dgvPagamentos.Columns["id_pagamento"] != null)
                        dgvPagamentos.Columns["id_pagamento"].Visible = false;

                    foreach (DataGridViewRow row in dgvPagamentos.Rows)
                    {
                        string estado = row.Cells["Estado"]?.Value?.ToString();
                        if (estado == "Pendente")
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(255, 235, 156);
                        else if (estado == "Cancelado")
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(198, 239, 206);
                    }

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar pagamentos: " + ex.Message);
            }
        }

        private void CarregarEventos()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT id_evento, nome_evento AS [Evento], ISNULL(tipo, '-') AS [Tipo], ISNULL (local, '-') AS [Local],
                                   FORMAT (data_inicio, 'dd/MM/yyyy HH:mm') AS [Início], FORMAT (data_fim, 'dd/MM/yyyy HH:mm') AS [Fim],
                                   vagas AS [Vagas] FROM Eventos ORDER BY data_inicio";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvEventos.AutoGenerateColumns = true;
                    dgvEventos.DataSource = dt;
                    dgvEventos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvEventos.ReadOnly = true;
                    dgvEventos.AllowUserToAddRows = false;

                    if (dgvEventos.Columns["id_evento"] != null)
                        dgvEventos.Columns["id_evento"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar eventos: " + ex.Message);
            }
        }

        private void btnAdicionarEvento_Click(object sender, EventArgs e)
        {
            using (Form dlg = new Form())
            {
                dlg.Text = "Adicionar Evento";
                dlg.Size = new System.Drawing.Size(340, 360);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;

                

                Label lblNome = new Label { Text = "Nome do Evento:", Left = 10, Top = 12, Width = 90 };

                TextBox txtNome = new TextBox { Left = 110, Top = 12, Width = 200 };

                Label lblTipo = new Label { Text = "Tipo:", Left = 10, Top = 45, Width = 90 };

                ComboBox cbTipo = new ComboBox { Left = 110, Top = 45, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cbTipo.Items.AddRange(new object[] { "Torneio", "Campeonato", "Workshop" });

                Label lblInicio = new Label { Text = "Início:", Left = 10, Top = 90, Width = 90 };

                DateTimePicker dtpInicio = new DateTimePicker { Left = 110, Top = 90, Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = false, Value = DateTime.Now };

                Label lblFim = new Label { Text = "Fim:", Left = 10, Top = 120, Width = 90 };

                DateTimePicker dtpFim = new DateTimePicker { Left = 110, Top = 120, Width = 200, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = false, Value = DateTime.Now };

                Label lblVagas = new Label { Text = "Vagas:", Left = 10, Top = 152, Width = 90 };

                NumericUpDown nudVagas = new NumericUpDown { Left = 110, Top = 152, Width = 80, Minimum = 1, Maximum = 200, Value = 20 };

                Label lblLocal = new Label { Text = "Local:", Left = 10, Top = 187, Width = 90 };

                TextBox txtLocal = new TextBox { Left = 110, Top = 187, Width = 200 };

                Label lblDesc = new Label { Text = "Descrição:", Left = 10, Top = 222, Width = 90 };

                TextBox txtDesc = new TextBox { Left = 110, Top = 222, Width = 200, Height = 50, Multiline = true };

                Button btnOk = new Button { Text = "Guardar", Left = 120, Top = 282, Width = 90 };

                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        MessageBox.Show("O nome do evento é obrigatório.");
                        return;
                    }

                    if (dtpFim.Value <= dtpInicio.Value)
                    {
                        MessageBox.Show("A data de fim deve ser posterior à data de início.");
                        return;
                    }

                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            string sql = @"INSERT INTO Eventos (nome_evento, descricao, local, data_inicio, data_fim, vagas, tipo)
                                           VALUES (@nome, @desc, @local, @inicio, @fim, @vagas, @tipo)";
                            SqlCommand cmd = new SqlCommand(sql, conn);

                            cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@desc", string.IsNullOrWhiteSpace(txtDesc.Text) ? (object)DBNull.Value : txtDesc.Text.Trim());
                            cmd.Parameters.AddWithValue("@local", string.IsNullOrWhiteSpace(txtLocal.Text) ? (object)DBNull.Value : txtLocal.Text.Trim());
                            cmd.Parameters.AddWithValue("@inicio", dtpInicio.Value);
                            cmd.Parameters.AddWithValue("@fim", dtpFim.Value);
                            cmd.Parameters.AddWithValue("@vagas", (int)nudVagas.Value);
                            cmd.Parameters.AddWithValue("@tipo", cbTipo.SelectedItem.ToString());
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Evento adicionado com sucesso.");
                        dlg.Close();
                        CarregarEventos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao adicionar evento: " + ex.Message);
                    }
                };

                dlg.Controls.AddRange(new Control[] { lblNome, txtNome, lblTipo, cbTipo, lblLocal, txtLocal, lblInicio, dtpInicio, lblFim, dtpFim, lblVagas, nudVagas, lblDesc, txtDesc, btnOk });
                dlg.ShowDialog(this);
            }
        }

        private void dgvEventos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int idEvento = (int)((DataRowView)dgvEventos.Rows[e.RowIndex].DataBoundItem).Row["id_evento"];

            string colName = dgvEventos.Columns[e.ColumnIndex].Name;

            if (colName == "RemoverEvento")
            {
                if (MessageBox.Show("Resmover este evento?", "Confirmar", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

                try
                {
                    using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {
                        SqlCommand cmdInsc = new SqlCommand("DELETE FROM InscricoesEventos WHERE id_evento = @id", conn);
                        cmdInsc.Parameters.AddWithValue("@id", idEvento);
                        cmdInsc.ExecuteNonQuery();
                        SqlCommand cmdEvento = new SqlCommand("DELETE FROM Eventos WHERE id_evento = @id", conn);
                        cmdEvento.Parameters.AddWithValue("@id", idEvento);
                        cmdEvento.ExecuteNonQuery();
                    }
                    MessageBox.Show("Evento removido.");
                    CarregarEventos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao remover evento: " + ex.Message);
                }
                return;
            }

            if (colName == "InscreverEvento")
            {
                try
                {
                   using (SqlConnection conn = DatabaseHelper.GetConnection())
                    {

                        SqlCommand cmdCheck = new SqlCommand(@"SELECT COUNT(*) FROM InscricoesEventos WHERE id_membro = @m AND id_evento = @ev", conn);
                        cmdCheck.Parameters.AddWithValue("@m", Session.UserId);
                        cmdCheck.Parameters.AddWithValue("@ev", idEvento);
                        int jaInscrito = (int)cmdCheck.ExecuteScalar();
                        if (jaInscrito > 0)
                        {
                            MessageBox.Show("Já estás inscrito neste evento.");
                            return;
                        }

                        SqlCommand cmdVagas = new SqlCommand(@"SELECT ev.vagas - COUNT(ie.id_inscricao_ev) FROM Eventos ev LEFT JOIN InscricoesEventos ie ON ie.id_evento = ev.id_evento
                                                               WHERE ev.id_evento = @ev GROUP BY ev.vagas", conn);

                        cmdVagas.Parameters.AddWithValue("@ev", idEvento);
                        object resultado = cmdVagas.ExecuteScalar();
                        int vagasLivres = resultado != null ? (int)resultado : 0;

                        if (vagasLivres <= 0)
                        {
                            MessageBox.Show("Não há vagas disponíveis para este evento.");
                            return;
                        }

                        SqlCommand cmdInsc = new SqlCommand(@"INSERT INTO InscricoesEventos (id_membro, id_evento) VALUES (@m, @ev)", conn);
                        cmdInsc.Parameters.AddWithValue("@m", Session.UserId);
                        cmdInsc.Parameters.AddWithValue("@ev", idEvento);
                        cmdInsc.ExecuteNonQuery();
                    }
                    MessageBox.Show("Inscrição realizada com sucesso.");
                    CarregarEventos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao inscrever: " + ex.Message);
                }
                return;
            }

            if (colName == "Participantes")
            {
                MostrarParticipantes("Participantes do Evento", @"SELECT m.nome + ' ' + m.apelido AS Membro, m.email AS Email, FORMAT (ie.data_inscricao, 'dd/MM/yyyy HH:mm') AS [Inscrito em] FROM InscricoesEventos ie JOIN Membros m ON m.id_membro = ie.id_membro
                                       WHERE ie.id_evento = @id ORDER BY m.nome", idEvento);
            }
        }

        private void MostrarParticipantes(string titulo, string sql, int id)
        {

            bool comPresenca = titulo.Contains("aula");

            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }

                using (Form dlg = new Form())
                {
                    dlg.Text = titulo;
                    dlg.Size = new System.Drawing.Size(660, 380);
                    dlg.StartPosition = FormStartPosition.CenterParent;




                    DataGridView grid = new DataGridView();
                    grid.Dock = DockStyle.Fill;
                    grid.AllowUserToAddRows = false;
                    grid.AutoGenerateColumns = false;
                    grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    grid.DataSource = dt;

                            if (comPresenca)
                        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "id_inscricao", Visible = false, DataPropertyName = "id_inscricao" });

                            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Membro", HeaderText = "Membro", DataPropertyName = "Membro", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, MinimumWidth = 150 });

                            grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Email", HeaderText = "Email", DataPropertyName = "Email", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, MinimumWidth = 180 });

                    if (comPresenca)
                    {
                        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Presença", HeaderText = "Presença", DataPropertyName = "Presença", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });

                        DataGridViewButtonColumn btnP = new DataGridViewButtonColumn();
                        btnP.Name = "TogglePresenca";
                        btnP.HeaderText = "Marcar Presença";
                        btnP.Text = "Marcar/Desmarcar";
                        btnP.UseColumnTextForButtonValue = true;
                        btnP.Width = 140;
                        grid.Columns.Add(btnP);

                        grid.CellContentClick += (s, ev) =>
                        {
                            if (ev.RowIndex < 0) return;
                            if (grid.Columns[ev.ColumnIndex].Name != "TogglePresenca") return;

                            int idInscricao = (int)((DataRowView)grid.Rows[ev.RowIndex].DataBoundItem).Row["id_inscricao"];

                            try
                            {
                                using (SqlConnection conn = DatabaseHelper.GetConnection())
                                {
                                    SqlCommand cmd = new SqlCommand(@"UPDATE Inscricoes SET presenca = CASE presenca WHEN 1 THEN 0 ELSE 1 END WHERE id_inscricao = @id", conn);

                                    cmd.Parameters.AddWithValue("@id", idInscricao);
                                    cmd.ExecuteNonQuery();

                                    SqlCommand cmdLer = new SqlCommand(@"SELECT presenca FROM Inscricoes WHERE id_inscricao = @id", conn);

                                    cmdLer.Parameters.AddWithValue("@id", idInscricao);

                                    bool nova = Convert.ToBoolean(cmdLer.ExecuteScalar());

                                    ((DataRowView)grid.Rows[ev.RowIndex].DataBoundItem).Row["Presença"] = nova ? "Sim" : "Não";
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Erro ao atualizar presença: " + ex.Message);
                            }
                        };
                    }
                    else
                    {
                        grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "Inscrito em", HeaderText = "Inscrito em", DataPropertyName = "Inscrito em", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
                    }

                    grid.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        Label lbl = new Label
                        {
                            Text = "Ainda não há inscrições.",
                            Dock = DockStyle.Top,
                            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                            Height = 30
                        };
                        dlg.Controls.Add(lbl);
                    }

                    dlg.Controls.Add(grid);
                    dlg.ShowDialog(this);      

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar participantes: " + ex.Message);
            }
        }

        private void CarregarEquipamentos()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    string sql = @"SELECT nome AS Nome, ISNULL(categoria, '-') AS Categoria, quantidade AS Quantidade, estado AS Estado, ISNULL(observacoes,'-') AS Observações
                                   FROM Equipamentos ORDER BY categoria, nome";

                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    

                    dgvEquipamentos.AutoGenerateColumns = true;
                    dgvEquipamentos.DataSource = dt;
                    dgvEquipamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvEquipamentos.ReadOnly = true;
                    dgvEquipamentos.AllowUserToAddRows = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar equipamentos: " + ex.Message);
            }
        }

        private void btnAdicionarEquipamento_Click(object sender, EventArgs e)
        {
            AbrirDialogoEquipamento(0, "", "", 1, "Bom", "");
        }

        private void AbrirDialogoEquipamento(int idEquip, string nome, string categoria, int quantidade, string estado, string observacoes)
        {
            bool isEdicao = idEquip > 0;

            using (Form dlg = new Form())
            {
                dlg.Text = isEdicao ? "Editar Equipamento" : "Adicionar Equipamento";
                dlg.Size = new System.Drawing.Size(340, 310);
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                dlg.MaximizeBox = false;

                int lx = 10, cx = 120, w = 185, ly = 12, step = 35;

                Label lblNome = new Label { Text = "Nome:", Left = lx, Top = ly, Width = 100 };
                TextBox txtNome = new TextBox { Left = cx, Top = ly, Width = w, Text = nome };
                Label lblCat = new Label { Text = "Categoria:", Left = lx, Top = ly + step, Width = 100 };
                TextBox txtCat = new TextBox { Left = cx, Top = ly + step, Width = w, Text = categoria };
                Label lblQtd = new Label { Text = "Quantidade:", Left = lx, Top = ly + step * 2, Width = 100 };
                NumericUpDown nudQtd = new NumericUpDown { Left = cx, Top = ly + step * 2, Width = 80, Minimum = 0, Maximum = 9999, Value = quantidade };
                Label lblEst = new Label { Text = "Estado:", Left = lx, Top = ly + step * 3, Width = 100 };
                ComboBox cbEst = new ComboBox { Left = cx, Top = ly + step * 3, Width = w, DropDownStyle = ComboBoxStyle.DropDownList };
                cbEst.Items.AddRange(new object[] { "Bom", "Degradado", "Avariado" });
                cbEst.SelectedItem = cbEst.Items.Contains(estado) ? estado : "Bom";
                Label lblObs = new Label { Text = "Observações:", Left = lx, Top = ly + step * 4, Width = 100 };
                TextBox txtObs = new TextBox { Left = cx, Top = ly + step * 4, Width = w, Height = 45, Multiline = true, Text = observacoes };
                Button btnOk = new Button { Text = "Guardar", Left = 120, Top = ly + step * 4 + 55, Width = 85 };

                btnOk.Click += (s, ev) =>
                {
                    if (string.IsNullOrWhiteSpace(txtNome.Text))
                    {
                        MessageBox.Show("O nome é obrigatório.");
                        return;
                    }

                    try
                    {
                        using (SqlConnection conn = DatabaseHelper.GetConnection())
                        {
                            SqlCommand cmd;
                            if (isEdicao)
                            {
                                cmd = new SqlCommand(@"UPDATE Equipamentos SET nome=@nome, categoria=@cat, quantidade=@qtd, estado=@est, observacoes=@obs WHERE id_equipamento=@id", conn);

                                cmd.Parameters.AddWithValue("@id", idEquip);
                            }
                            else
                            {
                                cmd = new SqlCommand(@"INSERT INTO Equipamentos (nome, categoria, quantidade, estado, observacoes) VALUES(@nome, @cat, @qtd, @est, @obs)", conn);

                            }

                            cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                            cmd.Parameters.AddWithValue("@cat", string.IsNullOrWhiteSpace(txtCat.Text) ? (object)DBNull.Value : txtCat.Text.Trim());
                            cmd.Parameters.AddWithValue("@qtd", (int)nudQtd.Value);
                            cmd.Parameters.AddWithValue("@est", cbEst.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@obs", string.IsNullOrWhiteSpace(txtObs.Text) ? (object)DBNull.Value : txtObs.Text.Trim());

                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show(isEdicao ? "Equipamento atualizado" : "Equipamento adicionado.");
                        dlg.Close();
                        CarregarEquipamentos();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao guardar: " + ex.Message);
                    }
                };

                dlg.Controls.AddRange(new Control[] { lblNome, txtNome, lblCat, txtCat, lblQtd, nudQtd, lblEst, cbEst, lblObs, txtObs, btnOk });
                dlg.ShowDialog();
            }
        }


    }
}
