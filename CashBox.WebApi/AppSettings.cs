using CashBox.Service.Configurations;

namespace CashBox.WebApi
{
    public class AppSettings
    {
        public static AppSettings Instance { get; set; } = null!;
        public Jwt Jwt { get; set; } = null!;
        public UzasboSetting UzasboSetting { get; set; } = null!;

        public static void Init(AppSettings instance)
        {
            Instance = instance;
        }
    }

    public class Jwt
    {
        public string Key { get; set; } = null!;
    }
}
