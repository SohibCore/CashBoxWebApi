using CashBox.Repository.Dtos.IncomeDocumentDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.AccountServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.IncomeDocumentSerives
{
    public class IncomeDocumentService : IIncomeDocumentService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public IncomeDocumentService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }
        public async Task CreateAsync(CreateIncomeDocumentDto createIncomeDocumentDto)
        {
            var incomeDocument = new IncomeDocument
            {
                Date = createIncomeDocumentDto.Date,
                SupplierId = createIncomeDocumentDto.SupplierId,
                ProductId = createIncomeDocumentDto.ProductId,
                //OrganizationId = createIncomeDocumentDto.OrganizationId,
                Price = createIncomeDocumentDto.Price,
                Quantity = createIncomeDocumentDto.Quantity,
                TotalSum = createIncomeDocumentDto.TotalSum,
                Status = createIncomeDocumentDto.Status,
            };
            await _context.AddAsync(incomeDocument);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var incomeDocument = await _context.IncomeDocuments.FindAsync(id);

            if (incomeDocument == null)
                throw new KeyNotFoundException();

            _context.IncomeDocuments.Remove(incomeDocument);
            await _context.SaveChangesAsync();
        }

        public async Task<IncomeDocumentDto> GetAsync(int id)
        {
            var incomeDocument = await _context.IncomeDocuments.FindAsync(id);

            if (incomeDocument == null)
                throw new KeyNotFoundException();

            return new IncomeDocumentDto
            {
                Id = incomeDocument.Id,
                Date = incomeDocument.Date,
                SupplierId = incomeDocument.SupplierId,
                ProductId = incomeDocument.ProductId,
                //OrganizationId = incomeDocument.OrganizationId,
                Price = incomeDocument.Price,
                Quantity = incomeDocument.Quantity,
                TotalSum = incomeDocument.TotalSum,
                Status = incomeDocument.Status,
            };

        }

        public async Task<List<IncomeDocumentDto>> GetListAsync(IncomeDocumentFilterDto incomeDocumentFilterDto)
        {
            var incomeDocument = _context.IncomeDocuments.AsQueryable();

            if (incomeDocumentFilterDto.Id.HasValue)
                incomeDocument = incomeDocument.Where(x => x.Id == incomeDocumentFilterDto.Id);

            if (incomeDocumentFilterDto.SupplierId.HasValue)
                incomeDocument = incomeDocument.Where(x => x.SupplierId == incomeDocumentFilterDto.SupplierId);

            if (incomeDocumentFilterDto.ProductId.HasValue)
                incomeDocument = incomeDocument.Where(x => x.ProductId == incomeDocumentFilterDto.ProductId);

            //if (incomeDocumentFilterDto.OrganizationId.HasValue)
            //    incomeDocument = incomeDocument.Where(x => x.OrganizationId == incomeDocumentFilterDto.OrganizationId);

            if (incomeDocumentFilterDto.Price.HasValue)
                incomeDocument = incomeDocument.Where(x => x.Price == incomeDocumentFilterDto.Price);

            if (incomeDocumentFilterDto.Quantity.HasValue)
                incomeDocument = incomeDocument.Where(x => x.Quantity == incomeDocumentFilterDto.Quantity);

            if (incomeDocumentFilterDto.TotalSum.HasValue)
                incomeDocument = incomeDocument.Where(x => x.TotalSum == incomeDocumentFilterDto.TotalSum);

            return await incomeDocument.Select(u => new IncomeDocumentDto
            {
                Id = u.Id,
                SupplierId = u.SupplierId,
                Date = u.Date,
                //OrganizationId = u.OrganizationId,
                Price = u.Price,
                Quantity = u.Quantity,
                TotalSum = u.TotalSum,
                ProductId = u.ProductId,
                Status = u.Status,
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateIncomeDocument updateIncomeDocument)
        {
            var incomeDocument = await _context.IncomeDocuments.FindAsync(id);

            if (incomeDocument == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (updateIncomeDocument.SupplierId.HasValue)
                incomeDocument.SupplierId = updateIncomeDocument.SupplierId.Value;

            //if (updateIncomeDocument.OrganizationId.HasValue && updateIncomeDocument.OrganizationId != 0)
            //    incomeDocument.OrganizationId = updateIncomeDocument.OrganizationId.Value;

            if (updateIncomeDocument.Price.HasValue)
                incomeDocument.Price = updateIncomeDocument.Price.Value;

            if (updateIncomeDocument.Quantity.HasValue)
                incomeDocument.Quantity = updateIncomeDocument.Quantity.Value;

            if (updateIncomeDocument.TotalSum.HasValue)
                incomeDocument.TotalSum = updateIncomeDocument.TotalSum.Value;

            if (updateIncomeDocument.ProductId.HasValue)
                incomeDocument.ProductId = updateIncomeDocument.ProductId.Value;

            if (updateIncomeDocument.Date.HasValue)
                incomeDocument.Date = updateIncomeDocument.Date.Value;

            if (updateIncomeDocument.Status.HasValue)
                incomeDocument.Status = updateIncomeDocument.Status.Value;

            //product.ModifiedAt = DateTime.UtcNow;
            //product.ModifiedUserId = _account.UserId;

            await _context.SaveChangesAsync();
        }
    }
}
