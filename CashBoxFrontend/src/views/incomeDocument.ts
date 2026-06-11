// Ro'yxat uchun (GetList endpoint'dan keladi)
export interface IncomeDocumentListDto {
  id: number;
  docOn: string;           // sana (ISO string)
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
export interface IncomeDocumentDetailDto {
  id: number;
  docOn: string;
  supplierName: string;
  docSum: number;
  statusId: number;
  statusName: string;
  tables: IncomeDocumentItemDto[];  // Mahsulotlar ro'yxati
}

// Har bir mahsulot qatori
export interface IncomeDocumentItemDto {
  id: number;
  productId: number;
  productName?: string;
  price: number;
  quantity: number;
  totalSum: number;
}

// Yaratish/tahrirlash uchun
export interface IncomeDocumentCreateDto {
  docOn: string;
  supplierId: number;
  tables: IncomeDocumentItemCreateDto[];
}

export interface IncomeDocumentItemCreateDto {
  productId: number;
  price: number;
  quantity: number;
}