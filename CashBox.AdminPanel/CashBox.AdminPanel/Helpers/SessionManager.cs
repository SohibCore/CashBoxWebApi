namespace CashBox.AdminPanel.Helpers
{
    public static class SessionManager
    {
        public static string? Token { get; set; }
        public static string? UserName { get; set; }

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);

        public static void Clear()
        {
            Token = null;
            UserName = null;
        }
    }
}