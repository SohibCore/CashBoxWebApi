using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.OutcomeDocumentService
{
    public class OutcomeDocumentService : IOutcomeDocumentService
    {
        private readonly AppDbContext _context;
        public OutcomeDocumentService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(CreateOutcomeDocumentDto createOutcomeDocumentDto)
        {
            var outcomeDocument = new OutcomeDocument
            {
                Date = createOutcomeDocumentDto.Date,
                SupplierId = createOutcomeDocumentDto.SupplierId,
                ProductId = createOutcomeDocumentDto.ProductId,
                Price = createOutcomeDocumentDto.Price,
                Quantity = createOutcomeDocumentDto.Quantity,
                TotalSum = createOutcomeDocumentDto.TotalSum,
            };
            await _context.OutcomeDocuments.AddAsync(outcomeDocument);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var outcomeDocument = await _context.OutcomeDocuments.FindAsync(id);

            if (outcomeDocument == null)
                throw new KeyNotFoundException();

            _context.OutcomeDocuments.Remove(outcomeDocument);
            await _context.SaveChangesAsync();
        }

        public async Task<OutcomeDocumentDto> GetAsync(int id)
        {
            var outcomeDocument = await _context.OutcomeDocuments.FindAsync(id);

            if (outcomeDocument == null)
                throw new KeyNotFoundException();

            return new OutcomeDocumentDto
            {
                Id = outcomeDocument.Id,
                Date = outcomeDocument.Date,
                SupplierId = outcomeDocument.SupplierId,
                Price = outcomeDocument.Price,
                ProductId = outcomeDocument.ProductId,
                Quantity = outcomeDocument.Quantity,
                TotalSum = outcomeDocument.TotalSum
            };
        }

        public async Task<List<OutcomeDocumentDto>> GetListAsync(OutcomeDocumentFilterDto outcomeDocumentFilterDto)
        {
            var outcomeDocument = _context.OutcomeDocuments.AsQueryable();

            if (outcomeDocumentFilterDto.Id.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.Id == outcomeDocumentFilterDto.Id);

            if (outcomeDocumentFilterDto.ProductId.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.ProductId == outcomeDocumentFilterDto.ProductId);

            if (outcomeDocumentFilterDto.SupplierId.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.SupplierId == outcomeDocumentFilterDto.SupplierId);

            if (outcomeDocumentFilterDto.Date.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.Date == outcomeDocumentFilterDto.Date);

            if (outcomeDocumentFilterDto.Price.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.Price == outcomeDocumentFilterDto.Price);

            if (outcomeDocumentFilterDto.Quantity.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.Quantity == outcomeDocumentFilterDto.Quantity);

            if (outcomeDocumentFilterDto.TotalSum.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.TotalSum == outcomeDocumentFilterDto.TotalSum);

            return await outcomeDocument.Select(u => new OutcomeDocumentDto
            {
                Id = u.Id,
                ProductId = u.ProductId,
                SupplierId = u.SupplierId,
                Date = u.Date,
                Price = u.Price,
                Quantity = u.Quantity,
                TotalSum = u.TotalSum,
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateOutcomeDocumentDto updateOutcomeDocumentDto)
        {
            var outcomeDocument = await _context.OutcomeDocuments.FindAsync(id);

            if (outcomeDocument == null)
                throw new KeyNotFoundException();

            if (updateOutcomeDocumentDto.ProductId.HasValue)
                outcomeDocument.ProductId = updateOutcomeDocumentDto.ProductId.Value;

            if(updateOutcomeDocumentDto.SupplierId.HasValue)
                outcomeDocument.SupplierId = updateOutcomeDocumentDto.SupplierId.Value;

            if(updateOutcomeDocumentDto.Date.HasValue)
                outcomeDocument.Date = updateOutcomeDocumentDto.Date.Value;

            if(updateOutcomeDocumentDto.Price.HasValue)
                outcomeDocument.Price = updateOutcomeDocumentDto.Price.Value;

            if(updateOutcomeDocumentDto.Quantity.HasValue)
                outcomeDocument.Quantity = updateOutcomeDocumentDto.Quantity.Value;

            if(updateOutcomeDocumentDto.TotalSum.HasValue)
                outcomeDocument.TotalSum = updateOutcomeDocumentDto.TotalSum.Value;

            await _context.SaveChangesAsync();
        }
    }
}
