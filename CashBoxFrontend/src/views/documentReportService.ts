import { api } from '../api'; // endi ishlaydi

export interface DocumentReportListDto {
  supplierId: number;
  supplierName: string;
  inn: string;
  openingDebit: number;
  openingCredit: number;
  debit: number;
  credit: number;
  closingDebit: number;
  closingCredit: number;
}

export interface DocumentReportFilterDto {
  dateFrom: string;
  dateTo: string;
}

export const documentReportService = {
  async getHeaderReport(filter: DocumentReportFilterDto): Promise<DocumentReportListDto[]> {
    const res = await api.get('/api/DocumentReport/GetList', {
      params: {
        DateFrom: filter.dateFrom,
        DateTo: filter.dateTo,
      }
    });
    return res.data?.data ?? res.data?.Data ?? (Array.isArray(res.data) ? res.data : []);
  }
};