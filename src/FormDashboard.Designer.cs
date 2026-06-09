namespace WindowsFormsApp1
{
    partial class FormDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDashboard));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnAtribuirPlano = new System.Windows.Forms.Button();
            this.dgvMembros = new System.Windows.Forms.DataGridView();
            this.ColunaNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaTelefone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaPlanoAtivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaEditar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColunaRemover = new System.Windows.Forms.DataGridViewButtonColumn();
            this.idmembroDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sthenosDBDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sthenosDBDataSet = new WindowsFormsApp1.SthenosDBDataSet();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvPlanos = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnAdicionarAula = new System.Windows.Forms.Button();
            this.dgvAulas = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dgvPagamentos = new System.Windows.Forms.DataGridView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.dgvEventos = new System.Windows.Forms.DataGridView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.dgvEquipamentos = new System.Windows.Forms.DataGridView();
            this.planosAssinaturaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.planosAssinaturaTableAdapter = new WindowsFormsApp1.SthenosDBDataSetTableAdapters.PlanosAssinaturaTableAdapter();
            this.RemoverAula = new System.Windows.Forms.DataGridViewButtonColumn();
            this.InscreverAula = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSet)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanos)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAulas)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.planosAssinaturaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAtribuirPlano);
            this.tabPage1.Controls.Add(this.dgvMembros);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Membros";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnAtribuirPlano
            // 
            this.btnAtribuirPlano.Location = new System.Drawing.Point(3, 182);
            this.btnAtribuirPlano.Name = "btnAtribuirPlano";
            this.btnAtribuirPlano.Size = new System.Drawing.Size(100, 23);
            this.btnAtribuirPlano.TabIndex = 3;
            this.btnAtribuirPlano.Text = "Atribuir Plano";
            this.btnAtribuirPlano.UseVisualStyleBackColor = true;
            this.btnAtribuirPlano.Visible = false;
            this.btnAtribuirPlano.Click += new System.EventHandler(this.btnAtribuirPlano_Click);
            // 
            // dgvMembros
            // 
            this.dgvMembros.AutoGenerateColumns = false;
            this.dgvMembros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaNome,
            this.ColunaEmail,
            this.ColunaTelefone,
            this.ColunaPlanoAtivo,
            this.ColunaEditar,
            this.ColunaRemover,
            this.idmembroDataGridViewTextBoxColumn});
            this.dgvMembros.DataMember = "Membros";
            this.dgvMembros.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvMembros.Location = new System.Drawing.Point(7, 7);
            this.dgvMembros.Name = "dgvMembros";
            this.dgvMembros.ReadOnly = true;
            this.dgvMembros.Size = new System.Drawing.Size(755, 150);
            this.dgvMembros.TabIndex = 1;
            this.dgvMembros.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMembros_CellContentClick);
            // 
            // ColunaNome
            // 
            this.ColunaNome.HeaderText = "Nome";
            this.ColunaNome.Name = "ColunaNome";
            this.ColunaNome.ReadOnly = true;
            // 
            // ColunaEmail
            // 
            this.ColunaEmail.HeaderText = "Email";
            this.ColunaEmail.Name = "ColunaEmail";
            this.ColunaEmail.ReadOnly = true;
            // 
            // ColunaTelefone
            // 
            this.ColunaTelefone.HeaderText = "Telefone";
            this.ColunaTelefone.Name = "ColunaTelefone";
            this.ColunaTelefone.ReadOnly = true;
            // 
            // ColunaPlanoAtivo
            // 
            this.ColunaPlanoAtivo.HeaderText = "Plano Ativo";
            this.ColunaPlanoAtivo.Name = "ColunaPlanoAtivo";
            this.ColunaPlanoAtivo.ReadOnly = true;
            // 
            // ColunaEditar
            // 
            this.ColunaEditar.HeaderText = "Editar";
            this.ColunaEditar.Name = "ColunaEditar";
            this.ColunaEditar.ReadOnly = true;
            this.ColunaEditar.Text = "Editar";
            this.ColunaEditar.UseColumnTextForButtonValue = true;
            this.ColunaEditar.Visible = false;
            // 
            // ColunaRemover
            // 
            this.ColunaRemover.HeaderText = "Remover";
            this.ColunaRemover.Name = "ColunaRemover";
            this.ColunaRemover.ReadOnly = true;
            this.ColunaRemover.Text = "Remover";
            this.ColunaRemover.UseColumnTextForButtonValue = true;
            this.ColunaRemover.Visible = false;
            // 
            // idmembroDataGridViewTextBoxColumn
            // 
            this.idmembroDataGridViewTextBoxColumn.DataPropertyName = "id_membro";
            this.idmembroDataGridViewTextBoxColumn.HeaderText = "id_membro";
            this.idmembroDataGridViewTextBoxColumn.Name = "idmembroDataGridViewTextBoxColumn";
            this.idmembroDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sthenosDBDataSetBindingSource
            // 
            this.sthenosDBDataSetBindingSource.DataSource = this.sthenosDBDataSet;
            this.sthenosDBDataSetBindingSource.Position = 0;
            // 
            // sthenosDBDataSet
            // 
            this.sthenosDBDataSet.DataSetName = "SthenosDBDataSet";
            this.sthenosDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvPlanos);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Planos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvPlanos
            // 
            this.dgvPlanos.AutoGenerateColumns = false;
            this.dgvPlanos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlanos.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvPlanos.Location = new System.Drawing.Point(6, 6);
            this.dgvPlanos.Name = "dgvPlanos";
            this.dgvPlanos.ReadOnly = true;
            this.dgvPlanos.Size = new System.Drawing.Size(445, 150);
            this.dgvPlanos.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnAdicionarAula);
            this.tabPage3.Controls.Add(this.dgvAulas);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(768, 400);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Aulas";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnAdicionarAula
            // 
            this.btnAdicionarAula.Location = new System.Drawing.Point(6, 162);
            this.btnAdicionarAula.Name = "btnAdicionarAula";
            this.btnAdicionarAula.Size = new System.Drawing.Size(75, 23);
            this.btnAdicionarAula.TabIndex = 1;
            this.btnAdicionarAula.Text = "Adicionar Aula";
            this.btnAdicionarAula.UseVisualStyleBackColor = true;
            this.btnAdicionarAula.Click += new System.EventHandler(this.btnAdicionarAula_Click);
            // 
            // dgvAulas
            // 
            this.dgvAulas.AutoGenerateColumns = false;
            this.dgvAulas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAulas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RemoverAula,
            this.InscreverAula});
            this.dgvAulas.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvAulas.Location = new System.Drawing.Point(6, 6);
            this.dgvAulas.Name = "dgvAulas";
            this.dgvAulas.ReadOnly = true;
            this.dgvAulas.Size = new System.Drawing.Size(756, 150);
            this.dgvAulas.TabIndex = 0;
            this.dgvAulas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAulas_CellContentClick);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dgvPagamentos);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(768, 400);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Pagamentos";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvPagamentos
            // 
            this.dgvPagamentos.AutoGenerateColumns = false;
            this.dgvPagamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPagamentos.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvPagamentos.Location = new System.Drawing.Point(112, 47);
            this.dgvPagamentos.Name = "dgvPagamentos";
            this.dgvPagamentos.Size = new System.Drawing.Size(240, 150);
            this.dgvPagamentos.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.dgvEventos);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(768, 400);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Eventos";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // dgvEventos
            // 
            this.dgvEventos.AutoGenerateColumns = false;
            this.dgvEventos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEventos.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvEventos.Location = new System.Drawing.Point(165, 64);
            this.dgvEventos.Name = "dgvEventos";
            this.dgvEventos.Size = new System.Drawing.Size(240, 150);
            this.dgvEventos.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.dgvEquipamentos);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(768, 400);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Equipamentos";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // dgvEquipamentos
            // 
            this.dgvEquipamentos.AutoGenerateColumns = false;
            this.dgvEquipamentos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEquipamentos.DataSource = this.sthenosDBDataSetBindingSource;
            this.dgvEquipamentos.Location = new System.Drawing.Point(178, 57);
            this.dgvEquipamentos.Name = "dgvEquipamentos";
            this.dgvEquipamentos.Size = new System.Drawing.Size(240, 150);
            this.dgvEquipamentos.TabIndex = 0;
            // 
            // planosAssinaturaBindingSource
            // 
            this.planosAssinaturaBindingSource.DataMember = "PlanosAssinatura";
            this.planosAssinaturaBindingSource.DataSource = this.sthenosDBDataSetBindingSource;
            // 
            // planosAssinaturaTableAdapter
            // 
            this.planosAssinaturaTableAdapter.ClearBeforeFill = true;
            // 
            // RemoverAula
            // 
            this.RemoverAula.HeaderText = "";
            this.RemoverAula.Name = "RemoverAula";
            this.RemoverAula.ReadOnly = true;
            this.RemoverAula.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RemoverAula.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.RemoverAula.Text = "Remover Aula";
            this.RemoverAula.UseColumnTextForButtonValue = true;
            // 
            // InscreverAula
            // 
            this.InscreverAula.HeaderText = "";
            this.InscreverAula.Name = "InscreverAula";
            this.InscreverAula.ReadOnly = true;
            this.InscreverAula.Text = "Inscrever Aula";
            this.InscreverAula.UseColumnTextForButtonValue = true;
            // 
            // FormDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormDashboard";
            this.Text = "FormDashboard";
            this.Load += new System.EventHandler(this.FormDashboard_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMembros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSet)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanos)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAulas)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEventos)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEquipamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.planosAssinaturaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.BindingSource sthenosDBDataSetBindingSource;
        private SthenosDBDataSet sthenosDBDataSet;
        private System.Windows.Forms.DataGridView dgvMembros;
        private System.Windows.Forms.Button btnAtribuirPlano;
        private System.Windows.Forms.DataGridView dgvPlanos;
        private System.Windows.Forms.DataGridView dgvAulas;
        private System.Windows.Forms.DataGridView dgvPagamentos;
        private System.Windows.Forms.DataGridView dgvEventos;
        private System.Windows.Forms.DataGridView dgvEquipamentos;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaNome;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaTelefone;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaPlanoAtivo;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaEditar;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaRemover;
        private System.Windows.Forms.DataGridViewTextBoxColumn idmembroDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource planosAssinaturaBindingSource;
        private SthenosDBDataSetTableAdapters.PlanosAssinaturaTableAdapter planosAssinaturaTableAdapter;
        private System.Windows.Forms.Button btnAdicionarAula;
        private System.Windows.Forms.DataGridViewButtonColumn RemoverAula;
        private System.Windows.Forms.DataGridViewButtonColumn InscreverAula;
    }
}