namespace CashBox.Integrations.UzasboServices.Dtos
{
    public class UserByInnResponseDto
    {
        public int Ns10Code { get; set; }
        public string Oked { get; set; } = null!;
        public string DirectorTin { get; set; } = null!;
        public long DirectorPinfl { get; set; } 
        public string Director { get; set; } = null!;
        public string Accountant { get; set; } = null!;
        public int IsBudget { get; set; }
        public int TaxpayerType { get; set; }
        public bool IsItd { get; set; }
        public string? PersonalNum { get; set; }
        public bool SelfEmployment { get; set; }
        public bool PrivateNotary { get; set; }
        public bool PeasantFarm { get; set; }
    }
}
