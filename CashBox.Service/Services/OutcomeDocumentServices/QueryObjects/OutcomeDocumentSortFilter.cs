using CashBox.Service.Services.OutcomeDocumentService;

namespace CashBox.Service.Services.OutcomeDocumentServices.QueryObjects
{
    public static class OutcomeDocumentSortFilter
    {
        public static IQueryable<OutcomeDocumentListDto> SortFilter(this IQueryable<OutcomeDocumentListDto> query, OutcomeDocumentFilterDto filter)
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
