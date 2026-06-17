using CashBox.Core;
using CashBox.Service.Services.AccountServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.DocumentReportServices
{
    public class DocumentReportService : IDocumentReportService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public DocumentReportService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<List<DocumentReportListDto>> GetHeaderReportAsync(DocumentReportFilterDto filter)
        {
            var beginCredit = _context.IncomeDocuments
                 .Include(x => x.Tables)
                 .Where(x => /*x.OrganizationId == _account.OrganizationId
                          &&*/ x.StatusId == StatusIdConst.ACCEPT && x.DocOn < filter.DateFrom)
                 .SelectMany(x => x.Tables)
                 .GroupBy(x => new
                 {
                     x.IncomeDocument.Supplier.Inn,
                     x.IncomeDocument.SupplierId,
                     x.IncomeDocument.Supplier.Code,
                 })
                 .Select(x => new DocumentReportListDto
                 {
                     Inn = x.Key.Inn,
                     SupplierId = x.Key.SupplierId,
                     SupplierName = x.Key.Code,
                     OpeningCredit = x.Sum(a => a.TotalSum),
                     OpeningDebit = 0,
                     Credit = 0,
                     Debit = 0,
                     ClosingCredit = 0,
                     ClosingDebit = 0,
                 });
            var beginDebit = _context.OutcomeDocuments
                 .Include(x => x.Tables)
                 .Where(x => x.OrganizationId == _account.OrganizationId
                          && x.StatusId == StatusIdConst.ACCEPT && x.DocOn < filter.DateFrom)
                 .SelectMany(x => x.Tables)
                 .GroupBy(x => new
                 {
                     x.OutcomeDocument.Supplier.Inn,
                     x.OutcomeDocument.SupplierId,
                     x.OutcomeDocument.Supplier.Code,
                 })
                 .Select(x => new DocumentReportListDto
                 {
                     Inn = x.Key.Inn,
                     SupplierId = x.Key.SupplierId,
                     SupplierName = x.Key.Code,
                     OpeningCredit = 0,
                     OpeningDebit = x.Sum(a => a.TotalSum),
                     Credit = 0,
                     Debit = 0,
                     ClosingCredit = 0,
                     ClosingDebit = 0,
                 });
            var credit = _context.IncomeDocuments
                 .Include(x => x.Tables)
                 .Where(x => x.OrganizationId == _account.OrganizationId
                          && x.StatusId == StatusIdConst.ACCEPT && filter.DateFrom <= x.DocOn && x.DocOn <= filter.DateTo)
                 .SelectMany(x => x.Tables)
                 .GroupBy(x => new
                 {
                     x.IncomeDocument.Supplier.Inn,
                     x.IncomeDocument.SupplierId,
                     x.IncomeDocument.Supplier.Code,
                 })
                 .Select(x => new DocumentReportListDto
                 {
                     Inn = x.Key.Inn,
                     SupplierId = x.Key.SupplierId,
                     SupplierName = x.Key.Code,
                     OpeningCredit = 0,
                     OpeningDebit = 0,
                     Credit = x.Sum(a => a.TotalSum),
                     Debit = 0,
                     ClosingCredit = 0,
                     ClosingDebit = 0,
                 });
            var debit = _context.OutcomeDocuments
                 .Include(x => x.Tables)
                 .Where(x => x.OrganizationId == _account.OrganizationId
                          && x.StatusId == StatusIdConst.ACCEPT && filter.DateFrom <= x.DocOn && x.DocOn <= filter.DateTo)
                 .SelectMany(x => x.Tables)
                 .GroupBy(x => new
                 {
                     x.OutcomeDocument.Supplier.Inn,
                     x.OutcomeDocument.SupplierId,
                     x.OutcomeDocument.Supplier.Code,
                 })
                 .Select(x => new DocumentReportListDto
                 {
                     Inn = x.Key.Inn,
                     SupplierId = x.Key.SupplierId,
                     SupplierName = x.Key.Code,
                     OpeningCredit = 0,
                     OpeningDebit = 0,
                     Credit = 0,
                     Debit = x.Sum(a => a.TotalSum),
                     ClosingCredit = 0,
                     ClosingDebit = 0,
                 });

            var result = await beginCredit.Concat(beginDebit).Concat(credit).Concat(debit)
                .GroupBy(x => new
                {
                    x.Inn,
                    x.SupplierId,
                    x.SupplierName
                }).
                Select(x => new DocumentReportListDto()
                {
                    Inn = x.Key.Inn,
                    SupplierId = x.Key.SupplierId,
                    SupplierName = x.Key.SupplierName,
                    OpeningCredit = x.Sum(a => a.OpeningCredit),
                    OpeningDebit = x.Sum(a => a.OpeningDebit),
                    Credit = x.Sum(a => a.Credit),
                    Debit = x.Sum(a => a.Debit),

                    ClosingDebit = (x.Sum(a => a.OpeningDebit) + x.Sum(a => a.Debit)) - (x.Sum(a => a.OpeningCredit) + x.Sum(a => a.Credit)) > 0 ? (x.Sum(a => a.OpeningDebit) + x.Sum(a => a.Debit)) - (x.Sum(a => a.OpeningCredit) + x.Sum(a => a.Credit)) : 0,
                    ClosingCredit = (x.Sum(a => a.OpeningDebit) + x.Sum(a => a.Debit)) - (x.Sum(a => a.OpeningCredit) + x.Sum(a => a.Credit)) < 0 ? -1 * ((x.Sum(a => a.OpeningDebit) + x.Sum(a => a.Debit)) - (x.Sum(a => a.OpeningCredit) + x.Sum(a => a.Credit))) : 0,
                }).ToListAsync();

            return result;
        }
    }
}
