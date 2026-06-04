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
              <th style="width: 40px;"></th>
              <th>ID</th>
              <th>Sana</th>
              <th>Ta'minotchi</th>
              <th>Jami summa</th>
              <th>Status</th>
              <th>Amallar</th>
            </tr>
          </thead>
          <tbody>
            <template v-for="doc in documents" :key="doc.id">
              <tr :class="{ 'row-expanded': expandedRowId === doc.id }" @click="toggleExpand(doc.id)">
                <td>
                  <span class="expand-icon" :class="{ 'is-active': expandedRowId === doc.id }">
                    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
                      <polyline points="9 18 15 12 9 6"></polyline>
                    </svg>
                  </span>
                </td>
                <td>{{ doc.id }}</td>
                <td>{{ doc.date ? doc.date.slice(0, 10) : '' }}</td>
                <td style="font-weight: 500;">{{ getSupplierName(doc.supplierId) }}</td>
                <td style="font-weight: 600;">{{ formatSum(doc.totalSum) }}</td>
                <td>
                  <span class="status-badge" :class="statusClass(doc.status)">
                    {{ statusLabel(doc.status) }}
                  </span>
                </td>
                <td class="actions" @click.stop>
                  <div class="action-dropdown-wrapper">
                    <button @click="toggleRow(doc.id)" :class="['icon-btn', { expanded: expandedDocId === doc.id }]" title="Amallarni ko'rsatish">
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="6 9 12 15 18 9"></polyline>
                      </svg>
                    </button>
                    <div v-if="expandedDocId === doc.id" class="action-dropdown">
                      <button @click="$router.push(`/outcome-documents/edit/${doc.id}`)" class="dropdown-btn">
                        Tahrirlash
                      </button>
                      <button @click="deleteDocument(doc.id)" class="dropdown-btn danger">
                        O'chirish
                      </button>
                    </div>
                  </div>
                </td>
              </tr>
              <tr v-if="expandedRowId === doc.id" class="nested-row">
                <td colspan="7">
                  <div class="nested-container">
                    <div v-if="loadingDetails" class="nested-loading">Yuklanmoqda...</div>
                    <table v-else class="nested-table">
                      <thead>
                        <tr>
                          <th>Mahsulot ID</th>
                          <th>Mahsulot nomi</th>
                          <th>Miqdori</th>
                          <th>Narxi</th>
                          <th>Jami</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="item in documentDetails[doc.id]?.tables" :key="item.id">
                          <td>{{ item.productId }}</td>
                          <td>{{ getProductName(item.productId) }}</td>
                          <td>{{ item.quantity }}</td>
                          <td>{{ formatSum(item.price) }}</td>
                          <td style="color: #f1f5f9;">{{ formatSum(item.totalSum) }}</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </td>
              </tr>
            </template>
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
const isEditMode = ref(false);
const currentId = ref(null);
const expandedDocId = ref(null);
const expandedRowId = ref(null);
const documentDetails = reactive({});
const loadingDetails = ref(false);

const toggleRow = (id) => {
  expandedDocId.value = expandedDocId.value === id ? null : id;
};

const toggleExpand = async (id) => {
  if (expandedRowId.value === id) {
    expandedRowId.value = null;
    return;
  }
  expandedRowId.value = id;
  if (!documentDetails[id]) {
    loadingDetails.value = true;
    try {
      const response = await api.get(`/api/OutcomeDocument/Get?id=${id}`);
      documentDetails[id] = response.data?.data || response.data;
    } catch (err) {
      console.error('Error fetching details:', err);
    } finally {
      loadingDetails.value = false;
    }
  }
};

const statusLabel = (status) => {
  const map = {
    1: "Yaratildi",
    2: "Tasdiqlandi",
    3: "Rad etildi",
    4: "O'zgartirildi",
    5: "O'chirildi"
  };
  return map[status] ?? 'Noma\'lum';
};

const statusClass = (status) => {
  const map = { 1: 'created', 2: 'accepted', 3: 'not-accepted', 4: 'modified', 5: 'deleted' };
  return map[status] ?? '';
};

const totalSum = computed(() =>
  documents.value.reduce((sum, d) => sum + (d.docSum || 0), 0)
);

const paidSum = computed(() =>
  documents.value
    .filter(d => d.statusId === 2) // StatusIdConst.ACCEPT
    .reduce((sum, d) => sum + (d.docSum || 0), 0)
);

const unpaidSum = computed(() =>
  documents.value
    .filter(d => [1, 3, 4].includes(d.statusId)) // StatusIdConst.CREATED, StatusIdConst.NOT_ACCEPT, StatusIdConst.MODIFIED
    .reduce((sum, d) => sum + (d.docSum || 0), 0)
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
    documents.value = (response.data?.data || response.data || []).map(doc => ({
      ...doc,
      date: doc.docOn // Backenddan kelayotgan DocOn ni date ga o'tkazamiz
    }));
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
  background-color: #0d1117;
  min-height: 100vh;
  padding: 24px;
  font-family: inherit;
}

.page-card {
  background: #111827;
  padding: 24px;
  border-radius: 12px;
  border: 1px solid rgba(255, 255, 255, 0.07);
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
  color: #f1f5f9;
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
  color: #94a3b8;
}

/* Base UI Elements */
.input {
  height: 38px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
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
  background: #111827;
  border-radius: 10px;
  border: 1px solid rgba(255, 255, 255, 0.07);
  overflow: hidden;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 8px;
}

.data-table thead tr {
  background-color: #0f172a;
}

.data-table th {
  padding: 12px 16px;
  text-align: left;
  font-size: 11px;
  font-weight: 600;
  color: #475569 !important;
  text-transform: uppercase;
  letter-spacing: 0.6px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
}

.data-table td {
  padding: 12px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  color: #94a3b8;
  font-size: 14px;
}

.data-table tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
}

.data-table tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.03);
  cursor: pointer;
}

/* Status Badges */
.status-badge {
  padding: 4px 10px;
  border-radius: 999px;
  font-size: 12px;
  font-weight: 600;
}

.status-created      { background: rgba(59, 130, 246, 0.15); color: #60a5fa; border: 1px solid rgba(59, 130, 246, 0.25); }
.status-accepted     { background: rgba(16, 185, 129, 0.15); color: #10b981; border: 1px solid rgba(16, 185, 129, 0.25); }
.status-not-accepted { background: rgba(239, 68, 68, 0.15); color: #ef4444; border: 1px solid rgba(239, 68, 68, 0.25); }
.status-modified     { background: rgba(168, 85, 247, 0.15); color: #a855f7; border: 1px solid rgba(168, 85, 247, 0.25); }
.status-deleted      { background: rgba(100, 116, 139, 0.15); color: #94a3b8; border: 1px solid rgba(100, 116, 139, 0.25); }

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
  background: #1e293b;
  padding: 0.4rem;
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  z-index: 10;
  white-space: nowrap;
}

.dropdown-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  margin: 0;
  border: 1px solid transparent;
  background: transparent;
  color: #f1f5f9;
  padding: 0.35rem 0.6rem;
  font-size: 0.78rem;
  border-radius: 0.4rem;
  cursor: pointer;
  transition: background 0.2s ease, border-color 0.2s ease;
}

.dropdown-btn:hover {
  background: rgba(59, 130, 246, 0.15);
  border-color: rgba(59, 130, 246, 0.35);
  color: #60a5fa;
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
  background: #111827;
  border: 1px solid rgba(255, 255, 255, 0.07);
}
.stat-card.total  { border-left: 4px solid #2563eb; }
.stat-card.paid   { border-left: 4px solid #16a34a; }
.stat-card.unpaid { border-left: 4px solid #d97706; }

.stat-icon { font-size: 28px; }
.stat-info { display: flex; flex-direction: column; }
.stat-label { font-size: 13px; color: #64748b; }
.stat-value { font-size: 18px; font-weight: 700; color: #f1f5f9; }

/* Nested Table Styles */
.expand-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform 0.2s;
  color: #64748b;
}
.expand-icon.is-active {
  transform: rotate(90deg);
  color: #3b82f6;
}
.nested-row td {
  padding: 0;
  background: #0a0f1e !important;
}
.nested-container {
  padding: 1rem 1rem 1.5rem 3rem;
  border-left: 2px solid #3b82f6;
  margin: 5px 0;
}
.nested-table {
  width: 100%;
  background: #111827;
  border: 1px solid rgba(255,255,255,0.05);
  border-radius: 8px;
  border-collapse: collapse;
}
.nested-table th {
  background: #1e293b;
  font-size: 10px;
  padding: 8px;
  color: #475569;
  text-transform: uppercase;
}
.nested-table td {
  font-size: 12px;
  padding: 8px;
  color: #94a3b8;
  border-bottom: 1px solid rgba(255,255,255,0.03);
}
.nested-loading {
  padding: 10px;
  color: #3b82f6;
  font-size: 13px;
}
</style>