using CashBox.Core;
using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.OutcomeDocumentServices;
using CashBox.Service.Services.OutcomeDocumentServices.QueryObjects;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.OutcomeDocumentService
{
    public class OutcomeDocumentService : IOutcomeDocumentService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;

        public OutcomeDocumentService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task<List<OutcomeDocumentListDto>> GetListAsync(OutcomeDocumentFilterDto filter)
        {
            var result = await _context.OutcomeDocuments
                .Where(x => x.StatusId != StatusIdConst.DELETE /*&& x.OrganizationId == _account.OrganizationId*/)
                .Select(x => new OutcomeDocumentListDto
                {
                    Id = x.Id,
                    DocOn = x.DocOn,
                    SupplierId = x.SupplierId,
                    SupplierName = x.Supplier.Code,
                    StatusId = x.StatusId,
                    StatusName = x.Status.ShortName,
                    DocSum = x.DocSum
                }).SortFilter(filter)
                .ToListAsync();

            return result;
        }

        public async Task<OutcomeDocumentDto> GetAsync(long id)
        {
            var outcomeDocument = await _context.OutcomeDocuments
                .Include(x => x.Tables)
                .Include(x => x.Supplier)
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == _account.OrganizationId && x.StatusId != StatusIdConst.DELETE);

            if (outcomeDocument == null)
                throw new KeyNotFoundException($"{id} topilmadi yoki sizning tashkilotingizga tegishli emas");

            return new OutcomeDocumentDto
            {
                Id = outcomeDocument.Id,
                DocOn = outcomeDocument.DocOn,
                SupplierId = outcomeDocument.SupplierId,
                DocSum = outcomeDocument.DocSum,
                StatusName = outcomeDocument.Status.ShortName,
                SupplierName = outcomeDocument.Supplier.Code,

                Tables = outcomeDocument.Tables.Select(t => new OutcomeDocumentTableDto
                {
                    Id = t.Id,
                    ProductId = t.ProductId,
                    ProductName = t.Product?.Name,
                    Quantity = t.Quantity,
                    Price = t.Price,
                    TotalSum = t.Price * t.Quantity
                }).ToList()
            };
        }

        public async Task<long> CreateAsync(CreateOutcomeDocumentDlDto dto)
        {
            var entity = new OutcomeDocument
            {
                DocOn = DateTime.SpecifyKind(dto.DocOn, DateTimeKind.Utc),
                SupplierId = dto.SupplierId,
                DocSum = dto.Tables.Sum(x => x.Price * x.Quantity),
                OrganizationId = _account.OrganizationId,
                StatusId = StatusIdConst.CREATED,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = _account.UserId,

                Tables = dto.Tables.Select(t => new OutcomeDocumentTable
                {
                    ProductId = t.ProductId,
                    Quantity = t.Quantity,
                    Price = t.Price,
                    TotalSum = t.Price * t.Quantity
                }).ToList()
            };
            await _context.OutcomeDocuments.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(UpdateOutcomeDocumentDto dto)
        {
            var entity = await _context.OutcomeDocuments
                 .Include(x => x.Tables)
                 .FirstOrDefaultAsync(x => x.Id == dto.Id && x.OrganizationId == _account.OrganizationId &&
                 x.StatusId != StatusIdConst.DELETE);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.DocOn = DateTime.SpecifyKind(dto.DocOn, DateTimeKind.Utc);
            entity.SupplierId = dto.SupplierId;
            entity.DocSum = dto.Tables.Sum(x => x.Price * x.Quantity);

            entity.StatusId = StatusIdConst.MODIFIED;
            entity.ModifiedAt = DateTime.UtcNow;
            entity.ModifiedUserId = _account.UserId;

            _context.OutcomeDocumentTables.RemoveRange(entity.Tables);

            entity.Tables = dto.Tables.Select(t => new OutcomeDocumentTable
            {
                OwnerId = entity.Id,
                ProductId = t.ProductId,
                Quantity = t.Quantity,
                Price = t.Price,
                TotalSum = t.Price * t.Quantity
            }).ToList();

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.OutcomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.DELETE;
            await _context.SaveChangesAsync();
        }

        public async Task<long> Accept(long id)
        {
            var entity = await _context.OutcomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.ACCEPT;
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<long> NotAccept(long id)
        {
            var entity = await _context.OutcomeDocuments.FindAsync(id);

            if (entity == null)
                throw new KeyNotFoundException();

            entity.StatusId = StatusIdConst.NOT_ACCEPT;
            await _context.SaveChangesAsync();
            return entity.Id;
        }

    }
}
