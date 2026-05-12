namespace WindowsFormsApp1
{
    /// <summary>
    /// Guarda os dados da sessão do utilizador autenticado.
    /// Deve ser limpa ao fazer logout.
    /// </summary>
    public static class Session
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        /// <summary>Membro | Instrutor | Administrador</summary>
        public static string Role { get; set; }

        public static void Clear()
        {
            UserId = 0;
            UserName = null;
            Role = null;
        }
    }
}