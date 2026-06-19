using CashBox.Repository.Dtos.ProductDtos;
using CashBox.Repository.Entity;
using MediatR;
using Repository.Data;

namespace CashBox.Service.Applications.Products.Commands
{
    public record CreateProductCommand(CreateProductDto dto) : IRequest<string>;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, string>
    {
        private readonly AppDbContext _context;
        public CreateProductHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.dto.Name,
                Code = request.dto.Code,
                OrganizationId = request.dto.OrganizationId,
                DeliveredAt = request.dto.DeliveredAt,
                CreatedAt = request.dto.CreateAt,
                CreatedUserId = request.dto.CreatedUserId,
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return ($"{product.Id} li mahsulot yaratildi");
        }
    }
}
