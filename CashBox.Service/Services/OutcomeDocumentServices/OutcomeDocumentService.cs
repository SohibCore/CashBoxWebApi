using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Repository.Entity;
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
                Status = createOutcomeDocumentDto.Status
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
                Status = outcomeDocument.Status,
                TotalSum = outcomeDocument.TotalSum
            };
        }

        public Task<List<OutcomeDocumentDto>> GetListAsync(OutcomeDocumentFilterDto outcomeDocumentFilterDto)
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

            if (outcomeDocumentFilterDto.Status.HasValue)
                outcomeDocument = outcomeDocument.Where(x => x.Status == outcomeDocumentFilterDto.Status);
        }

        public Task UpdateAsync(int id, UpdateOutcomeDocumentDto updateOutcomeDocumentDto)
        {
            throw new NotImplementedException();
        }
    }
}
