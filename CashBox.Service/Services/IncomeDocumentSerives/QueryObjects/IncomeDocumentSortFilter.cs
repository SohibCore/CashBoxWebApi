using CashBox.Service.Services.IncomeDocumentServices;

namespace CashBox.Service.Services.IncomeDocumentServes.QueryObjects
{
    public static class IncomeDocumentSortFilter
    {
        public static IQueryable<IncomeDocumentListDto> SortFilter(this IQueryable<IncomeDocumentListDto> query, IncomeDocumentFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id);
            if (filter.SupplierId.HasValue)
                query = query.Where(x => x.SupplierId == filter.SupplierId);
            if (filter.DocSum.HasValue)
                query = query.Where(x => x.DocSum == filter.DocSum.Value);
            if (filter.StatusId.HasValue)
                query = query.Where(x => x.StatusId == filter.StatusId);

            query = query.OrderByDescending(x => x.DocOn);

            return query;
        }
    }
}
