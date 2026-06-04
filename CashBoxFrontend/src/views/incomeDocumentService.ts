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
  getList() {
    return api.get('/api/IncomeDocument/GetList');
  },
  // ID bo'yicha bitta hujjatni olish
  getById(id: string | number) {
    return api.get(`/api/IncomeDocument/Get/${id}`);
  },
  // Yangi hujjat yaratish
  create(data: any) {
    return api.post('/api/IncomeDocument/Create', data);
  },
  // Mavjud hujjatni tahrirlash
  update(data: any) {
    return api.put('/api/IncomeDocument/Update', data);
  },
  // Hujjatni o'chirish
  delete(id: number) {
    return api.delete(`/api/IncomeDocument/Delete/${id}`);
  },
  // Hujjatni tasdiqlash
  accept(id: number) {
    return api.post(`/api/IncomeDocument/Accept?id=${id}`);
  },
  // Tasdiqni bekor qilish
  notAccept(id: number) {
    return api.post(`/api/IncomeDocument/NotAccept?id=${id}`);
  },
  // Ta'minotchilar va mahsulotlar ro'yxati (Forma uchun)
  getSuppliers() {
    return api.get('/api/Supplier/GetList');
  },
  getProducts() {
    return api.get('/api/Product/GetList');
  }
};