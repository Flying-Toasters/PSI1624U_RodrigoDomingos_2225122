using System;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace WindowsFormsApp1
{
    public static class DatabaseInitializer
    {
        private const string MasterConnection =
  @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;TrustServerCertificate=True";



        public static bool InicializarBD()
        {
            try
            {
                if (!BDExiste())
                    CriarBD();



                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                "Não foi possível inicializar a base de dados.\n\n" + ex.Message,
                "Erro de arranque",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return false;
            }
        }



        private static bool BDExiste()
        {
            using (SqlConnection conn = new SqlConnection(MasterConnection))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM sys.databases WHERE name = 'SthenosDB'", conn))
                {
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }



        private static void CriarBD()
        {




            using (SqlConnection conn = new SqlConnection(MasterConnection))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("CREATE DATABASE SthenosDB", conn))
                    cmd.ExecuteNonQuery();
            }

            string sthenosConnection = @"Server=(localdb)\MSSQLLocalDB;Database=SthenosDB;Trusted_Connection=True;TrustServerCertificate=True";
            string[] blocos = ObterScriptSQL().Split(new[] { "\r\nGO", "\nGO" },
  StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection conn = new SqlConnection(sthenosConnection))
            {
                conn.Open();


                foreach (string bloco in blocos)
                {
                    string sql = bloco.Trim();
                    if (string.IsNullOrEmpty(sql)) continue;



                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                        cmd.ExecuteNonQuery();
                }
            }
        }
        



        private static string ObterScriptSQL()
        {
            return @"
CREATE TABLE Membros (
    id_membro       INT IDENTITY(1,1) PRIMARY KEY,
    nome            NVARCHAR(100) NOT NULL,
    apelido         NVARCHAR(100) NOT NULL,
    email           NVARCHAR(150) NOT NULL UNIQUE,
    palavra_passe   NVARCHAR(256) NOT NULL,
    telefone        NVARCHAR(20),
    morada          NVARCHAR(200),
    data_nascimento DATE,
    data_registo    DATETIME DEFAULT GETDATE(),
    ativo           BIT DEFAULT 1
)



CREATE TABLE Instrutores (
    id_instrutor    INT IDENTITY(1,1) PRIMARY KEY,
    nome            NVARCHAR(100) NOT NULL,
    apelido         NVARCHAR(100) NOT NULL,
    email           NVARCHAR(150) NOT NULL UNIQUE,
    palavra_passe   NVARCHAR(256) NOT NULL,
    telefone        NVARCHAR(20),
    especialidade   NVARCHAR(100),
    data_registo    DATETIME DEFAULT GETDATE(),
    ativo           BIT DEFAULT 1
)



CREATE TABLE Administradores (
    id_admin        INT IDENTITY(1,1) PRIMARY KEY,
    nome            NVARCHAR(100) NOT NULL,
    apelido         NVARCHAR(100) NOT NULL,
    email           NVARCHAR(150) NOT NULL UNIQUE,
    palavra_passe   NVARCHAR(256) NOT NULL,
    data_registo    DATETIME DEFAULT GETDATE()
)



CREATE TABLE PlanosAssinatura (
    id_plano        INT IDENTITY(1,1) PRIMARY KEY,
    nome_plano      NVARCHAR(100) NOT NULL,
    descricao       NVARCHAR(300),
    duracao_meses   INT NOT NULL,
    preco           DECIMAL(10,2) NOT NULL,
    ativo           BIT DEFAULT 1
)



CREATE TABLE Pagamentos (
    id_pagamento    INT IDENTITY(1,1) PRIMARY KEY,
    id_membro       INT NOT NULL FOREIGN KEY REFERENCES Membros(id_membro),
    id_plano        INT NOT NULL FOREIGN KEY REFERENCES PlanosAssinatura(id_plano),
    valor           DECIMAL(10,2) NOT NULL,
    data_pagamento  DATETIME DEFAULT GETDATE(),
    data_inicio     DATE NOT NULL,
    data_fim        DATE NOT NULL,
    estado          NVARCHAR(20) DEFAULT 'Pago'
)



CREATE TABLE Aulas (
    id_aula         INT IDENTITY(1,1) PRIMARY KEY,
    nome_aula       NVARCHAR(100) NOT NULL,
    id_instrutor    INT NOT NULL FOREIGN KEY REFERENCES Instrutores(id_instrutor),
    data_hora       DATETIME NOT NULL,
    duracao_min     INT DEFAULT 60,
    vagas           INT DEFAULT 20,
    local           NVARCHAR(100),
    descricao       NVARCHAR(300)
)



CREATE TABLE Inscricoes (
    id_inscricao    INT IDENTITY(1,1) PRIMARY KEY,
    id_membro       INT NOT NULL FOREIGN KEY REFERENCES Membros(id_membro),
    id_aula         INT NOT NULL FOREIGN KEY REFERENCES Aulas(id_aula),
    data_inscricao  DATETIME DEFAULT GETDATE(),
    presenca        BIT DEFAULT 0
)



CREATE TABLE FeedbackMembros (
    id_feedback     INT IDENTITY(1,1) PRIMARY KEY,
    id_membro       INT NOT NULL FOREIGN KEY REFERENCES Membros(id_membro),
    id_instrutor    INT NOT NULL FOREIGN KEY REFERENCES Instrutores(id_instrutor),
    id_aula         INT FOREIGN KEY REFERENCES Aulas(id_aula),
    titulo          NVARCHAR(150),
    mensagem        NVARCHAR(1000),
    data_feedback   DATETIME DEFAULT GETDATE()
)



CREATE TABLE Eventos (
    id_evento       INT IDENTITY(1,1) PRIMARY KEY,
    nome_evento     NVARCHAR(150) NOT NULL,
    descricao       NVARCHAR(500),
    local           NVARCHAR(200),
    data_inicio     DATETIME NOT NULL,
    data_fim        DATETIME,
    vagas           INT DEFAULT 50,
    tipo            NVARCHAR(50)
)



CREATE TABLE InscricoesEventos (
    id_inscricao_ev INT IDENTITY(1,1) PRIMARY KEY,
    id_membro       INT NOT NULL FOREIGN KEY REFERENCES Membros(id_membro),
    id_evento       INT NOT NULL FOREIGN KEY REFERENCES Eventos(id_evento),
    data_inscricao  DATETIME DEFAULT GETDATE()
)



CREATE TABLE Equipamentos (
    id_equipamento  INT IDENTITY(1,1) PRIMARY KEY,
    nome            NVARCHAR(100) NOT NULL,
    categoria       NVARCHAR(100),
    quantidade      INT DEFAULT 0,
    estado          NVARCHAR(50) DEFAULT 'Bom',
    observacoes     NVARCHAR(300)
)



INSERT INTO PlanosAssinatura (nome_plano, descricao, duracao_meses, preco) VALUES
('Mensal',     'Plano de acesso mensal',   1,  50.00),
('Trimestral', 'Plano de 3 meses',         3,  130.00),
('Anual',      'Plano anual com desconto', 12, 450.00)



INSERT INTO Administradores (nome, apelido, email, palavra_passe) VALUES
('Admin', 'Sthenos', 'sthenoshelp@gmail.com',
'0372b1ea4f4a5679764993ce0bb18fc00e2f8e1a05c4b67698d7adce31c2d1b9')
";
        }
    }
}