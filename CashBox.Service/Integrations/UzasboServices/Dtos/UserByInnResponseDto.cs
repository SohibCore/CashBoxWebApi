namespace CashBox.Integrations.UzasboServices.Dtos
{
    public class UserByInnResponseDto
    {
        public int Ns10Code { get; set; }
        public int Ns11Code { get; set; }
        public string ShortName { get; set; } = null!;
        public string Tin { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string RegDate { get; set; } = null!;
        public int Na1Code { get; set; }
        public string Na1Name { get; set; } = null!;
        public int StatusCode { get; set; }
        public string StatusName { get; set; } = null!;
        public string Mfo { get; set; } = null!;
        public string Account { get; set; } = null!;
        public string Address { get; set; } = null!;
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
