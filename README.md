# Sthenos — Sistema de Gestão para Ginásios
 
Sistema de gestão desktop para ginásios e academias de pequena e média dimensão, desenvolvido em **C# / Windows Forms** com **SQL Server**.
 
## Funcionalidades
 
- Gestão de membros, instrutores e administradores com permissões diferenciadas
- Planos de assinatura (Mensal · Trimestral · Anual) e registo de pagamentos
- Agendamento de aulas com controlo de vagas e registo de presenças
- Feedback de instrutores a membros
- Gestão de eventos desportivos (Torneios, Campeonatos, Workshops)
- Inventário de equipamentos
- Relatórios de frequência e de eventos realizados
 
## Estrutura do Repositório
 
```
PSI1624U_RodrigoDomingos_2225122/
├── README.md
├── .gitignore
├── docs/
│   ├── originais/                                              
│   ├── PSI1624U_RodrigoDomingos_2225122_PropostaPreProjeto.PDF
│   ├── PSI1624U_RodrigoDomingos_2225122_RelatorioFinal.PDF
│   └── PSI1624U_RodrigoDomingos_2225122_ManualUtilizador.PDF
│   
├── scriptsbd/                                                  
├── src/                                                        
└── dist/                                                       
```
 
## Execução
 
### 1. Requisitos
 
- Windows 10 / 11
- [.NET Framework 4.7.2+](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net472) (normalmente já incluído no Windows)
- SQL Server LocalDB — incluído no Visual Studio, ou instalável via [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
 
### 2. Executar
 
Abra a pasta `dist/` e execute **WindowsFormsApp1.exe**. Não é necessário instalar nem compilar nada mais.
 
## Credenciais Padrão
 
| Campo | Valor |
|---|---|
| E-mail | `exemplo@gmail.com` |
| Palavra-passe | `exemplo123` |
 
 
## Compilar a partir do código-fonte (opcional)
 
Abra `src/WindowsFormsApp1.sln` no Visual Studio 2019+ e execute **Build → Build Solution** (`Ctrl+Shift+B`).
 
> A string de ligação está definida em `src/DatabaseHelper.cs`. Se utilizar uma instância SQL Server diferente de `(localdb)\MSSQLLocalDB`, altere o valor de `ConnectionString` antes de compilar.

PSI1624U_RodrigoDomingos_2225122
