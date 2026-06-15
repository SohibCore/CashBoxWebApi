using CashBox.Service.Configurations;

namespace CashBox.WebApi
{
    public class AppSettings
    {
        public static AppSettings Instance { get; set; }
        public Jwt Jwt { get; set; }
        public UzasboSetting UzasboSetting { get; set; }

        public static void Init(AppSettings instance)
        {
            Instance = instance;
        }
    }

    public class Jwt
    {
        public string Key { get; set; }
    }
}
