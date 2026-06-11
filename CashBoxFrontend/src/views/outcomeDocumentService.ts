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

export const outcomeDocumentService = {
  // Hujjatlar ro'yxatini olish
  getList(filter?: any) {
    return api.get('/api/OutcomeDocument/GetList', { params: filter });
  },
  // ID bo'yicha bitta hujjatni olish
  getById(id: string | number) {
    return api.get(`/api/OutcomeDocument/Get/${id}`);
  },
  // Yangi chiqim hujjati yaratish
  create(data: any) {
    return api.post('/api/OutcomeDocument/Create', data);
  },
  // Mavjud hujjatni yangilash
  update(data: any) {
    return api.put('/api/OutcomeDocument/Update', data);
  },
  // Hujjatni o'chirish
  delete(id: number | string) {
    return api.delete(`/api/OutcomeDocument/Delete/${id}`);
  },
  accept(id: number | string) { 
    return api.patch(`/api/OutcomeDocument/Accept/${id}`);
  },
  notAccept(id: number | string) { 
    return api.patch(`/api/OutcomeDocument/NotAccept/${id}`);
  },
  getSuppliers() { return api.get('/api/Supplier/GetList'); },
  getProducts() { return api.get('/api/Product/GetList'); }
};