using CashBox.Core;
using CashBox.Repository.Dtos.DocumentReportDto;
using CashBox.Repository.Dtos.DocumentReportDtos;
using CashBox.Repository.Entity.Reports;
using CashBox.Service.Services.AccountServices;
using CashBox.Service.Services.DocumentReportServices.QueryObjects;
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

        public async Task<List<DocumentReportListDto>> GetListAsync(DocumentReportFilterDto filter)
        {
            var result = await _context.DocumentReports
                .Where(x => x.StatusId != StatusIdConst.DELETE && x.OrganizationId == _account.OrganizationId)
                .Select(x => new DocumentReportListDto
                {
                    Id = x.Id,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Code,
                    Inn = x.Supplier.Inn,
                    OpeningDebit = x.OpeningDebit,
                    OpeningCredit = x.OpeningCredit,
                    Debit = x.Debit,
                    Credit = x.Credit,
                    ClosingCredit = x.ClosingCredit,
                    ClosingDebit = x.ClosingDebit
                }).SortFilter(filter)
                .ToListAsync();

            return result;
        }
        public async Task<DocumentReportDto> GetAsync(long id)
        {
            var documentReport = await _context.DocumentReports
                .Include(x => x.IncomeReport)
                .Include(x => x.OutcomeReport)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.StatusId != StatusIdConst.DELETE && x.Id == id && x.OrganizationId == _account.OrganizationId);

            if (documentReport == null)
                throw new KeyNotFoundException();

            return new DocumentReportDto
            {
                Id = documentReport.Id,
                SupplierId = documentReport.SupplierId,
                SupplierName = documentReport.Supplier.Code,
                Inn = documentReport.Supplier.Inn,
                OpeningDebit = documentReport.OpeningDebit,
                OpeningCredit = documentReport.OpeningCredit,
                Debit = documentReport.Debit,
                Credit = documentReport.Credit,
                ClosingDebit = documentReport.ClosingDebit,
                ClosingCredit = documentReport.ClosingCredit,
                InReports = documentReport.IncomeReport.Select(x => new IncomeReport
                {
                    Id = x.Id,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Code,
                    ProductId = x.ProductId,
                    //ProductName = x.
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Debit = 0,
                    Credit = x.Credit,
                }).ToList(),
                OutReports = documentReport.OutcomeReport.Select(x => new OutcomeReport
                {
                    Id = x.Id,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Code,
                    ProductId = x.ProductId,
                    //ProductName = x.
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Debit = x.Debit,
                    Credit = 0
                }).ToList()
            };
        }
        public async Task<long> CreateAsync(CreateDocumentReportDto dto)
        {
            var entity = new DocumentReport
            {
                SupplierId = dto.SupplierId,
                OpeningDebit = dto.OpeningDebit,
                OpeningCredit = dto.OpeningCredit,
                OrganizationId = _account.OrganizationId,
                Debit = dto.Debit,
                Credit = dto.Credit,
                ClosingDebit = dto.ClosingDebit,
                ClosingCredit = dto.ClosingCredit,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _account.UserId,

                IncomeReport = dto.IncomeReports.Select(x => new IncomeReportTable
                {
                    SupplierId = x.SupplierId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Debit = x.Debit,
                    Credit = x.Credit,
                }).ToList(),

                OutcomeReport = dto.OutcomeReports.Select(x => new OutcomeReportTable
                {
                    SupplierId = x.SupplierId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Debit = x.Debit,
                    Credit = x.Credit,
                }).ToList()
            };

            await _context.DocumentReports.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }
        public async Task UpdateAsync(UpdateDocumentReportDlDto dto)
        {
            var entity = await _context.DocumentReports
                .Include(x => x.IncomeReport)
                .Include(x => x.OutcomeReport)
                .FirstOrDefaultAsync(x => x.StatusId != StatusIdConst.DELETE && x.Id == dto.Id && x.OrganizationId == _account.OrganizationId);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.SupplierId = dto.SupplierId;
            entity.OpeningCredit = dto.OpeningCredit;
            entity.OpeningDebit = dto.OpeningDebit;
            entity.Debit = dto.Debit;
            entity.Credit = dto.Credit;
            entity.ClosingCredit = dto.ClosedCredit;
            entity.ClosingDebit = dto.ClosingDebit;

            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedUserId = _account.UserId;

            _context.RemoveRange(entity.IncomeReport);
            _context.RemoveRange(entity.OutcomeReport);

            entity.IncomeReport = dto.IncomeUpdate.Select(x => new IncomeReportTable
            {
                SupplierId = x.SupplierId,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                Debit = x.Debit,
                Credit = x.Credit,
            }).ToList();

            entity.OutcomeReport = dto.OutcomeUpdate.Select(x => new OutcomeReportTable
            {
                SupplierId = x.SupplierId,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                Debit = x.Debit,
                Credit = x.Credit,
            }).ToList();

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(long id)
        {
            var entity = await _context.DocumentReports.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.DELETE;
            await _context.SaveChangesAsync();
        }
    }
}
