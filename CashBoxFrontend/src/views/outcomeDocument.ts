// Ro'yxat uchun (GetList endpoint'dan keladi)
export interface OutcomeDocumentListDto {
  id: number;
  docOn: string;           // sana (ISO string)
  supplierId?: number;
  supplierName: string;
  docSum: number;
  statusId: number;
  statusName: string;
  // Qo'shimcha (ba'zan kelishi mumkin)
  productName?: string;
  quantity?: number;
  price?: number;
}

// Batafsil ko'rish uchun (GetById endpoint'dan keladi)
export interface OutcomeDocumentDetailDto {
  id: number;
  docOn: string;
  supplierId?: number;
  supplierName: string;
  docSum: number;
  statusId: number;
  statusName: string;
  tables: OutcomeDocumentItemDto[];  // Mahsulotlar ro'yxati
}

// Har bir mahsulot qatori
export interface OutcomeDocumentItemDto {
  id: number;
  productId: number;
  productName?: string;
  price: number;
  quantity: number;
  totalSum: number;
}