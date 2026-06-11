export interface OutcomeDocumentListDto {
  id: number;
  docOn: string;
  supplierName: string;
  docSum: number;
  statusId: number;
  statusName: string;
  productName?: string;
  quantity?: number;
  price?: number;
}

export interface OutcomeDocumentDetailDto {
  id: number;
  docOn: string;
  supplierName: string;
  docSum: number;
  statusId: number;
  statusName: string;
  tables: OutcomeDocumentItemDto[];
}

export interface OutcomeDocumentItemDto {
  id: number;
  productId: number;
  productName?: string;
  price: number;
  quantity: number;
  totalSum: number;
}