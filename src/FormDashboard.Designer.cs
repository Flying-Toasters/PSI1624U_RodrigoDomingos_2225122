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
            this.btnAdicionarMembro = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.ColunaNome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaTelefone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaPlanoAtivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColunaEditar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColunaRemover = new System.Windows.Forms.DataGridViewButtonColumn();
            this.sthenosDBDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sthenosDBDataSet = new WindowsFormsApp1.SthenosDBDataSet();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSet)).BeginInit();
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
            this.tabPage1.Controls.Add(this.btnAdicionarMembro);
            this.tabPage1.Controls.Add(this.dataGridView2);
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
            this.btnAtribuirPlano.Location = new System.Drawing.Point(7, 212);
            this.btnAtribuirPlano.Name = "btnAtribuirPlano";
            this.btnAtribuirPlano.Size = new System.Drawing.Size(100, 23);
            this.btnAtribuirPlano.TabIndex = 3;
            this.btnAtribuirPlano.Text = "Atribuir Plano";
            this.btnAtribuirPlano.UseVisualStyleBackColor = true;
            this.btnAtribuirPlano.Visible = false;
            this.btnAtribuirPlano.Click += new System.EventHandler(this.btnAtribuirPlano_Click);
            // 
            // btnAdicionarMembro
            // 
            this.btnAdicionarMembro.Location = new System.Drawing.Point(7, 182);
            this.btnAdicionarMembro.Name = "btnAdicionarMembro";
            this.btnAdicionarMembro.Size = new System.Drawing.Size(100, 23);
            this.btnAdicionarMembro.TabIndex = 2;
            this.btnAdicionarMembro.Text = "Adicionar Membro";
            this.btnAdicionarMembro.UseVisualStyleBackColor = true;
            this.btnAdicionarMembro.Visible = false;
            this.btnAdicionarMembro.Click += new System.EventHandler(this.btnAdicionarMembro_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColunaNome,
            this.ColunaEmail,
            this.ColunaTelefone,
            this.ColunaPlanoAtivo,
            this.ColunaEditar,
            this.ColunaRemover});
            this.dataGridView2.DataSource = this.sthenosDBDataSetBindingSource;
            this.dataGridView2.Location = new System.Drawing.Point(7, 7);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(645, 150);
            this.dataGridView2.TabIndex = 1;
            // 
            // ColunaNome
            // 
            this.ColunaNome.HeaderText = "Nome";
            this.ColunaNome.Name = "ColunaNome";
            // 
            // ColunaEmail
            // 
            this.ColunaEmail.HeaderText = "Email";
            this.ColunaEmail.Name = "ColunaEmail";
            // 
            // ColunaTelefone
            // 
            this.ColunaTelefone.HeaderText = "Telefone";
            this.ColunaTelefone.Name = "ColunaTelefone";
            // 
            // ColunaPlanoAtivo
            // 
            this.ColunaPlanoAtivo.HeaderText = "Plano Ativo";
            this.ColunaPlanoAtivo.Name = "ColunaPlanoAtivo";
            // 
            // ColunaEditar
            // 
            this.ColunaEditar.HeaderText = "Editar";
            this.ColunaEditar.Name = "ColunaEditar";
            this.ColunaEditar.Text = "Editar";
            this.ColunaEditar.UseColumnTextForButtonValue = true;
            this.ColunaEditar.Visible = false;
            // 
            // ColunaRemover
            // 
            this.ColunaRemover.HeaderText = "Remover";
            this.ColunaRemover.Name = "ColunaRemover";
            this.ColunaRemover.Text = "Remover";
            this.ColunaRemover.UseColumnTextForButtonValue = true;
            this.ColunaRemover.Visible = false;
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
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Planos";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(768, 400);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Aulas";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(768, 400);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Pagamentos";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(768, 400);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Eventos";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(768, 400);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Equipamentos";
            this.tabPage6.UseVisualStyleBackColor = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sthenosDBDataSet)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnAtribuirPlano;
        private System.Windows.Forms.Button btnAdicionarMembro;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaNome;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaTelefone;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColunaPlanoAtivo;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaEditar;
        private System.Windows.Forms.DataGridViewButtonColumn ColunaRemover;
    }
}