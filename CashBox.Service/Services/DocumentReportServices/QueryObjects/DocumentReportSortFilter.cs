namespace CashBox.Service.Services.DocumentReportServices.QueryObjects
{
    public static class DocumentReportSortFilter
    {
        public static IQueryable<DocumentReportListDto> SortFilter(this IQueryable<DocumentReportListDto> query, DocumentReportFilterDto filter)
        {
            if (filter.Id.HasValue)
                query = query.Where(x => x.Id == filter.Id);
            if (filter.Price.HasValue)
                query = query.Where(x => x.Price == filter.Price);
            if (filter.Debit.HasValue)
                query = query.Where(x => x.Debit == filter.Debit.Value);
            if (filter.Credit.HasValue)
                query = query.Where(x => x.Credit == filter.Credit);
            if (filter.OpeningDebit.HasValue)
                query = query.Where(x => x.OpeningDebit == filter.OpeningDebit);
            if (filter.OpeningCredit.HasValue)
                query = query.Where(x => x.OpeningCredit == filter.OpeningCredit);

            //query = query.OrderByDescending(x => x.DocOn);

            return query;
        }
    }
}
