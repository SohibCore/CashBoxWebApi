using CashBox.Repository.Entity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.ExelServices
{
    public class ExelService : IExelService
    {
        public byte[] ExportProducts(List<Product> data)
        {
            ExcelPackage.License.SetNonCommercialOrganization("CashBox"); //.NET 8 dan yuqori bo'lganlar uchun 
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial; .NET 5-6-7 uchun 

            using var package = new ExcelPackage();
            var exel = package.Workbook.Worksheets.Add("Mahsulotlar");

            exel.Cells[1, 1].Value = "T/r";
            exel.Cells[1, 2].Value = "Nomi";
            exel.Cells[1, 3].Value = "Kodi";
            exel.Cells[1, 4].Value = "Tashkilot";
            exel.Cells[1, 5].Value = "Yetkazilgan";

            for (int i = 0; i < data.Count; i++)
            {
                int row = i + 2;

                exel.Cells[row, 1].Value = data[i].Id;
                exel.Cells[row, 2].Value = data[i].Name;
                exel.Cells[row, 3].Value = data[i].Code;
                exel.Cells[row, 4].Value = data[i].OrganizationId;
                exel.Cells[row, 5].Value = data[i].DeliveredAt.ToString();

                for (int coll = 1; coll <= 5; coll++)
                {
                    exel.Cells[row, coll].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var border = exel.Cells[row, coll];

                    border.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    border.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    // Header
                    exel.Cells[1, coll].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    exel.Cells[1, coll].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    exel.Cells[1, coll].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    exel.Cells[1, coll].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    exel.Cells[1, coll].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    exel.Cells[1, coll].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[1, coll].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[1, coll].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[1, coll].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
            }

            exel.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] ExportUsers(List<User> data)
        {
            ExcelPackage.License.SetNonCommercialPersonal("CashBox");

            using var package = new ExcelPackage();
            var exel = package.Workbook.Worksheets.Add("Foydalanuvchilar");

            exel.Cells[2, 2].Value = "T/r";
            exel.Cells[2, 3].Value = "LoginName";
            exel.Cells[2, 4].Value = "F.I.O";
            exel.Cells[2, 5].Value = "Email";
            exel.Cells[2, 6].Value = "Qisqa nomi";
            exel.Cells[2, 7].Value = "PINFL";
            exel.Cells[2, 8].Value = "Telefon raqami";
            exel.Cells[2, 9].Value = "Manzil";
            exel.Cells[2, 10].Value = "Tug'ilgan sana";
            exel.Cells[2, 11].Value = "Passport seriya";
            exel.Cells[2, 12].Value = "Tashkilot";

            for (int i = 0; i < data.Count; i++)
            {
                int row = i + 3;

                exel.Cells[row, 2].Value = data[i].Id;
                exel.Cells[row, 3].Value = data[i].UserName;
                exel.Cells[row, 4].Value = data[i].FullName;
                exel.Cells[row, 5].Value = data[i].Email;
                exel.Cells[row, 6].Value = data[i].ShortName;
                exel.Cells[row, 7].Value = data[i].Pinfl;
                exel.Cells[row, 8].Value = data[i].PhoneNumber;
                exel.Cells[row, 9].Value = data[i].Address;
                exel.Cells[row, 10].Value = data[i].DateOfBirth;
                exel.Cells[row, 11].Value = data[i].PassportSeries;
                exel.Cells[row, 12].Value = data[i].OrganizationId;

                for (int coll = 2; coll <= 12; coll++)
                {
                    exel.Cells[row, coll].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var border = exel.Cells[row, coll];

                    border.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    border.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    border.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    border.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                    // Header
                    exel.Cells[2, coll].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    exel.Cells[2, coll].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    exel.Cells[2, coll].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    exel.Cells[2, coll].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    exel.Cells[2, coll].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    exel.Cells[2, coll].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[2, coll].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[2, coll].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    exel.Cells[2, coll].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }
            }
            exel.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}
