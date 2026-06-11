import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5107',
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export const incomeDocumentService = {
  // Hujjatlar ro'yxatini olish
  getList(filter?: any) {
    return api.get('/api/IncomeDocument/GetList', { params: filter });
  },
  // ID bo'yicha bitta hujjatni ota va bola ma'lumotlari bilan olish
  getById(id: string | number) {
    return api.get(`/api/IncomeDocument/Get/${id}`);
  },
  // Yangi kirim hujjati yaratish
  create(data: any) {
    return api.post('/api/IncomeDocument/Create', data);
  },
  // Mavjud hujjatni yangilash
  update(data: any) {
    return api.put('/api/IncomeDocument/Update', data);
  },
  // Hujjatni o'chirish
  delete(id: number | string) {
    return api.delete(`/api/IncomeDocument/Delete/${id}`);
  },
  // Hujjatni tasdiqlash yoki rad etish (Backend HttpPatch route-ga moslab)
  accept(id: number | string) { 
    return api.patch(`/api/IncomeDocument/Accept/${id}`);
  },
  notAccept(id: number | string) { 
    return api.patch(`/api/IncomeDocument/NotAccept/${id}`);
  },
  // Metadata yuklash
  getSuppliers() { return api.get('/api/Supplier/GetList'); },
  getProducts() { return api.get('/api/Product/GetList'); }
};