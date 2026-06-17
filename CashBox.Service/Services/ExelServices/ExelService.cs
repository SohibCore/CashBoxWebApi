using CashBox.Repository.Entity;
using CashBox.Service.Services.AccountServices;
using OfficeOpenXml;

namespace CashBox.Service.Services.ExelServices
{
    public class ExelService : IExelService
    {
        private readonly AccountService _account;

        public ExelService(AccountService account)
        {
            _account = account;
        }

        public byte[] ExportProducts(List<Product> data)
        {
            ExcelPackage.License.SetNonCommercialOrganization("CashBox"); //.NET 8 dan yuqori bo'lganlar uchun 
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; .NET 5-6-7 uchun 

            using var package = new ExcelPackage();
            var exel = package.Workbook.Worksheets.Add("Mahsulotlar");

            exel.Cells[1, 1].Value = "ID";
            exel.Cells[1, 2].Value = "NOMI";
            exel.Cells[1, 3].Value = "KODI";
            exel.Cells[1, 4].Value = "TASHKILOT";
            exel.Cells[1, 5].Value = "YETKAZILGAN";

            for (int i = 0; i < data.Count; i++)
            {
                int row = i + 2;

                exel.Cells[row, 1].Value = data[i].Id;
                exel.Cells[row, 2].Value = data[i].Name;
                exel.Cells[row, 3].Value = data[i].Code;
                exel.Cells[row, 4].Value = data[i].OrganizationId;
                exel.Cells[row, 5].Value = data[i].DeliveredAt.ToString();
            }

            exel.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}
