<template>
  <div class="page-container">
    <div class="page-card">
      <!-- Header -->
      <div class="header-section">
        <div>
          <h1>Chiqim hujjatlari</h1>
          <p class="subtitle">Chiqim hujjatlarini boshqarish</p>
        </div>
        <button class="btn btn-primary" @click="$router.push('/outcome-documents/new')">
          + Yangi hujjat qo'shish
        </button>
      </div>

      <div class="stats-cards">
        <div class="stat-card total">
          <div class="stat-icon">💰</div>
          <div class="stat-info">
            <span class="stat-label">Jami summa</span>
            <span class="stat-value">{{ formatSum(totalSum) }}</span>
          </div>
        </div>
        <div class="stat-card paid">
          <div class="stat-icon">✅</div>
          <div class="stat-info">
            <span class="stat-label">To'langan</span>
            <span class="stat-value">{{ formatSum(paidSum) }}</span>
          </div>
        </div>
        <div class="stat-card unpaid">
          <div class="stat-icon">⏳</div>
          <div class="stat-info">
            <span class="stat-label">To'lanmagan</span>
            <span class="stat-value">{{ formatSum(unpaidSum) }}</span>
          </div>
        </div>
      </div>

      <div class="total-count" style="margin-bottom: 20px;">
        Jami: {{ documents.length }} ta hujjat
      </div>

      <!-- Table -->
      <div class="table-container">
        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Sana</th>
              <th>Ta'minotchi</th>
              <th>Mahsulot</th>
              <th>Narx</th>
              <th>Miqdor</th>
              <th>Jami summa</th>
              <th>Status</th>
              <th>Amallar</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="doc in documents" :key="doc.id" @dblclick="$router.push(`/outcome-documents/edit/${doc.id}`)">
              <td>{{ doc.id }}</td>
              <td>{{ doc.date ? doc.date.slice(0, 10) : '' }}</td>
              <td>{{ getSupplierName(doc.supplierId) }}</td>
              <td>{{ getProductName(doc.productId) }}</td>
              <td>{{ doc.price.toLocaleString() }}</td>
              <td>{{ doc.quantity }}</td>
              <td>{{ doc.totalSum.toLocaleString() }}</td>
              <td>
                <span class="status-badge" :class="'status-' + doc.status">
                  {{ statusLabels[doc.status] }}
                </span>
              </td>
              <td class="actions">
                <div class="action-dropdown-wrapper">
                  <button @click.stop="toggleRow(doc.id)" :class="['icon-btn', { expanded: expandedDocId === doc.id }]" title="Amallarni ko'rsatish">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="6 9 12 15 18 9"></polyline>
                    </svg>
                  </button>
                  <div v-if="expandedDocId === doc.id" class="action-dropdown">
                    <button @click="$router.push(`/outcome-documents/edit/${doc.id}`)" class="dropdown-btn">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                        <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                      </svg>
                      Tahrirlash
                    </button>
                    <button @click="deleteDocument(doc.id)" class="dropdown-btn danger">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                      </svg>
                      O'chirish
                    </button>
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted, computed } from 'vue';
import axios from 'axios';

// Axios instance sozlamalari
const api = axios.create({
  baseURL: 'http://localhost:5107',
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  // GET request larda Content-Type o'chirish
  if (config.method === 'get') {
    delete config.headers['Content-Type'];
  }
  return config;
});

// Holatlar (State)
const documents = ref([]);
const suppliers = ref([]);
const products = ref([]);
const statusLabels = { 0: 'To\'lanmagan', 1: 'To\'langan' };
const showModal = ref(false);
const isEditMode = ref(false);
const currentId = ref(null);
const expandedDocId = ref(null);

const toggleRow = (id) => {
  expandedDocId.value = expandedDocId.value === id ? null : id;
};

const totalSum = computed(() =>
  documents.value.reduce((sum, d) => sum + (d.totalSum || 0), 0)
);

const paidSum = computed(() =>
  documents.value
    .filter(d => d.status === 1)
    .reduce((sum, d) => sum + (d.totalSum || 0), 0)
);

const unpaidSum = computed(() =>
  documents.value
    .filter(d => d.status === 0)
    .reduce((sum, d) => sum + (d.totalSum || 0), 0)
);

const formatSum = (val) =>
  new Intl.NumberFormat('uz-UZ').format(val) + ' UZS';

// API Funksiyalari
const loadMetaData = async () => {
  try {
    const [sRes, pRes] = await Promise.all([
      api.get('/api/Supplier/GetList'),
      api.get('/api/Product/GetList')
    ]);
    suppliers.value = sRes.data?.data || sRes.data || [];
    products.value = pRes.data?.data || pRes.data || [];
  } catch (err) {
    console.error('Metadata yuklashda xato:', err);
  }
};

const getSupplierName = (id) => 
  suppliers.value.find(s => (s.id || s.Id) === id)?.code || id || '-';

const getProductName = (id) => 
  products.value.find(p => (p.id || p.Id) === id)?.name || id || '-';

const initialForm = {
  date: '',
  supplierId: null,
  productId: null,
  price: null,
  quantity: null,
  totalSum: null,
  status: 0
};
const formData = reactive({ ...initialForm });
const fetchDocuments = async () => {
  try {
    const response = await api.get('/api/OutcomeDocument/GetList');
    documents.value = response.data?.data || response.data || [];
  } catch (error) {
    console.error('Fetch error:', error.response?.status, error.response?.data);
    alert('Xatolik: ' + (error.response?.status || error.message));
  }
};

const saveDocument = async () => {
  try {
    if (isEditMode.value) {
      await api.put(`/api/OutcomeDocument/Update?id=${currentId.value}`, formData);
    } else {
      await api.post('/api/OutcomeDocument/Create', formData);
    }
    showModal.value = false;
    fetchDocuments();
  } catch (error) {
    console.error('Save error:', error);
    alert('Saqlashda xatolik yuz berdi');
  }
};

const deleteDocument = async (id) => {
  if (!confirm('Haqiqatan ham ushbu hujjatni o\'chirmoqchimisiz?')) return;
  try {
    await api.delete(`/api/OutcomeDocument/Delete?id=${id}`);
    fetchDocuments();
  } catch (error) {
    console.error('Delete error:', error);
    alert('O\'chirishda xatolik yuz berdi');
  }
};

onMounted(async () => {
  await loadMetaData();
  await fetchDocuments();
});

</script>

<style scoped>
.page-container {
  background-color: #f4f6f9;
  min-height: 100vh;
  padding: 24px;
  font-family: inherit;
}

.page-card {
  background: white;
  padding: 24px;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
}

/* Header */
.header-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  gap: 12px;
  margin-bottom: 24px;
}

.header-section h1 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  color: #1e293b;
}

.subtitle {
  margin: 4px 0 0;
  font-size: 14px;
  color: #64748b;
}

/* Filter Section */
.filter-section {
  margin-bottom: 20px;
}

.filter-panel {
  display: flex;
  gap: 10px;
  align-items: center;
}

.filter-row {
  display: flex;
  gap: 10px;
  align-items: center;
}

.input-small { max-width: 150px; }
.input-medium { max-width: 180px; }

.total-count {
  margin-top: 12px;
  font-size: 14px;
  font-weight: 500;
  color: #475569;
}

/* Base UI Elements */
.input {
  height: 38px;
  border: 1px solid #e2e8f0;
  border-radius: 8px;
  padding: 8px 12px;
  width: 100%;
  font-size: 14px;
  outline: none;
  transition: border-color 0.2s;
}

.input:focus {
  border-color: #2563eb;
}

.btn {
  height: 38px;
  padding: 0 16px;
  border-radius: 8px;
  border: none;
  font-weight: 500;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  transition: opacity 0.2s;
}

.btn-primary { background-color: #2563eb; color: white; }
.btn-gray { background-color: #f1f5f9; color: #475569; }
.btn-icon { width: 38px; padding: 0; font-size: 18px; }
.btn:hover { opacity: 0.9; }

/* Table */
.table-container {
  overflow-x: auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 8px;
}

.data-table thead tr {
  background-color: #f0f2f5;
  color: #333;
}

.data-table th {
  padding: 12px 16px;
  text-align: left;
  font-size: 13px;
  font-weight: 600;
  color: inherit;
}

.data-table td {
  padding: 12px 16px;
  border-bottom: 1px solid #f1f5f9;
  font-size: 14px;
}

.data-table tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
}

.data-table tbody tr:hover {
  background-color: #f0f7ff;
  cursor: pointer;
}

/* Status Badges */
.status-badge {
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
}

.status-0 { background-color: #fef3c7; color: #d97706; } /* Unpaid */
.status-1 { background-color: #d1fae5; color: #065f46; } /* Paid */

/* Action Links */
.actions {
  display: flex;
  gap: 0.5rem;
  white-space: nowrap;
}

.action-dropdown-wrapper {
  position: relative;
  display: inline-block;
}

.action-dropdown {
  position: absolute;
  top: 100%;
  right: 0;
  margin-top: 0.3rem;
  display: flex;
  gap: 0.4rem;
  background: white;
  padding: 0.4rem;
  border-radius: 0.5rem;
  border: 1px solid #e2e8f0;
  box-shadow: 0 2px 6px rgba(15, 23, 42, 0.1);
  z-index: 10;
  white-space: nowrap;
}

.dropdown-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  margin: 0;
  border: 1px solid transparent;
  background: #ffffff;
  color: #0f172a;
  padding: 0.35rem 0.6rem;
  font-size: 0.78rem;
  border-radius: 0.4rem;
  cursor: pointer;
  transition: background 0.2s ease, border-color 0.2s ease;
}

.dropdown-btn:hover {
  background: #eff6ff;
  border-color: #cbd5e1;
}

.dropdown-btn.danger {
  color: white;
  background: #dc2626;
}

.icon-btn.expanded svg {
  transform: rotate(180deg);
}

/* Modal */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-box {
  background: white;
  border-radius: 12px;
  width: 480px;
  padding: 24px;
}

.modal-header h3 {
  margin: 0 0 20px;
  font-size: 18px;
  font-weight: 700;
}

.modal-body {
  display: flex;
  flex-direction: column;
  gap: 16px;
  margin-bottom: 24px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-group label {
  font-size: 13px;
  font-weight: 600;
  color: #64748b;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}

.stats-cards {
  display: flex;
  gap: 16px;
  margin-bottom: 24px;
}
.stat-card {
  flex: 1;
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px;
  border-radius: 12px;
  background: white;
  box-shadow: 0 1px 3px rgba(0,0,0,0.1);
}
.stat-card.total  { border-left: 4px solid #2563eb; }
.stat-card.paid   { border-left: 4px solid #16a34a; }
.stat-card.unpaid { border-left: 4px solid #d97706; }

.stat-icon { font-size: 28px; }
.stat-info { display: flex; flex-direction: column; }
.stat-label { font-size: 13px; color: #64748b; }
.stat-value { font-size: 18px; font-weight: 700; color: #1e293b; }
</style>