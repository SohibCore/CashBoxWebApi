<template>
  <div class="income-documents-page">
    <div class="section-header"> 
      <h2>Kirim hujjatlari</h2>
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

    <div class="section-actions">
      <span class="user-count">{{ filteredDocuments.length }} ta hujjat</span>
    </div>

    <div class="data-panel">
      <div class="table-header">
        <h3>Hujjatlar ro'yxati</h3>
        <router-link to="/income-documents/new" class="btn-primary">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi hujjat qo'shish
        </router-link>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Ta'minotchi</th>
            <th>Jami summa</th>
            <th>Sana</th>
            <th>Status</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredDocuments.length === 0" class="no-data-row">
            <td colspan="7">Ma'lumot yo'q</td>
          </tr>
          <template v-for="doc in filteredDocuments" :key="doc.id">
            <tr @dblclick="router.push(`/income-documents/edit/${doc.id}`)">
              <td>{{ doc.id }}</td>
              <td style="font-weight: 500;">{{ doc.supplierName || doc.supplierId }}</td>
              <td style="font-weight: 600;">{{ formatSum(doc.totalSum) }}</td>
              <td>{{ formatDate(doc.date) }}</td>
              <td>
                <span :class="['badge', statusClass(doc.status)]">
                  {{ statusLabel(doc.status) }}
                </span>
              </td>
              <td class="actions" @click.stop>
                <div class="action-dropdown-wrapper">
                  <button
                    type="button"
                    @click="toggleDropdown(doc.id)"
                    :class="['icon-btn', { expanded: expandedDocId === doc.id }]"
                  >
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="6 9 12 15 18 9"></polyline>
                    </svg>
                  </button>
                  <div v-if="expandedDocId === doc.id" class="action-dropdown">
                    <button type="button" @click="handleAccept(doc.id)" class="dropdown-btn success-btn">
                      Tasdiqlash
                    </button>
                    <button type="button" @click="handleReject(doc.id)" class="dropdown-btn warning-btn">
                      Rad etish
                    </button>
                    <button type="button" @click="openEditModal(doc)" class="dropdown-btn">
                      Tahrirlash
                    </button>
                    <button type="button" @click="deleteItem(doc.id)" class="dropdown-btn danger">
                      O'chirish
                    </button>
                  </div>
                </div>
              </td>
            </tr>
          </template>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { incomeDocumentService } from './incomeDocumentService';

const router = useRouter();
const documents = ref([]);
const products = ref([]);
const expandedDocId = ref(null);
const filters = reactive({ status: null });

const toggleDropdown = (id) => {
  expandedDocId.value = expandedDocId.value === id ? null : id;
};

const loadMetaData = async () => {
  try {
    const res = await incomeDocumentService.getProducts();
    products.value = res.data?.data || res.data || [];
  } catch (err) {
    console.error('Mahsulotlarni yuklashda xato:', err);
  }
};

const getProductName = (id) => {
  const p = products.value.find(x => (x.id || x.Id) === id);
  return p ? p.name : 'Mahsulot #' + id;
};

const statusLabel = (status) => {
  const map = { 1: "Yaratildi", 2: "Tasdiqlandi", 3: "Rad etildi", 4: "O'zgartirildi", 5: "O'chirildi" };
  return map[status] ?? 'Noma\'lum';
};

const statusClass = (status) => {
  const map = { 1: 'created', 2: 'accepted', 3: 'not-accepted', 4: 'modified', 5: 'deleted' };
  return map[status] ?? '';
};

const totalSum = computed(() => documents.value.reduce((sum, d) => sum + (d.totalSum || 0), 0));
const paidSum = computed(() => documents.value.filter(d => d.status === 2).reduce((sum, d) => sum + (d.totalSum || 0), 0));
const unpaidSum = computed(() => documents.value.filter(d => [1, 3, 4].includes(d.status)).reduce((sum, d) => sum + (d.totalSum || 0), 0));
const formatSum = (val) => new Intl.NumberFormat('uz-UZ').format(val) + ' UZS';

const filteredDocuments = computed(() => {
  if (filters.status === null) return documents.value;
  return documents.value.filter(doc => doc.status === filters.status);
});

const loadDocuments = async () => {
  try {
    const res = await incomeDocumentService.getList();
    const raw = res.data?.data || res.data || [];
    documents.value = raw.map(doc => ({
      id: doc.id || doc.Id,
      supplierId: doc.supplierId || doc.SupplierId,
      supplierName: doc.supplierName || doc.SupplierName,
      totalSum: doc.docSum ?? doc.DocSum ?? 0,
      date: doc.docOn ?? doc.DocOn,
      status: doc.statusId ?? doc.StatusId ?? 1,
      statusName: doc.statusName || doc.StatusName
    }));
  } catch (err) {
    console.error('Load documents error:', err);
  }
};

const handleAccept = async (id) => {
  try {
    await incomeDocumentService.accept(id);
    await loadDocuments();
    expandedDocId.value = null;
  } catch (err) {
    console.error('Accept error:', err);
  }
};

const handleReject = async (id) => {
  try {
    await incomeDocumentService.notAccept(id);
    await loadDocuments();
    expandedDocId.value = null;
  } catch (err) {
    console.error('Reject error:', err);
  }
};

const openEditModal = (doc) => {
  expandedDocId.value = null;
  router.push(`/income-documents/edit/${doc.id}`);
};

const deleteItem = async (id) => {
  if (!window.confirm("O'chirishni tasdiqlaysizmi?")) return;
  expandedDocId.value = null;
  try {
    await incomeDocumentService.delete(id);
    await loadDocuments();
  } catch (err) {
    console.error('Delete document error:', err);
  }
};

const formatDate = (date) => {
  if (!date) return '-';
  return new Date(date).toLocaleDateString();
};

onMounted(() => {
  loadDocuments();
  loadMetaData();
});
</script>

<style scoped>
.income-documents-page {
  background: #0d1117 !important;
  min-height: 100%;
  width: 100%;
  box-sizing: border-box;
}

.income-documents-page.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.income-documents-page.wide-card {
  max-width: none;
  width: 100%;
  margin: 0;
  box-sizing: border-box;
}

.section-header {
  margin-bottom: 1.5rem;
  text-align: center;
}

.section-actions {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 1rem;
  align-items: center;
  margin-bottom: 1rem;
}

.search-and-count {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.search-input {
  padding: 0.75rem 1rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.75rem;
  font-size: 0.95rem;
}

.toggle-create {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  background: #2563eb;
  color: white;
  padding: 0.85rem 2rem;
  border-radius: 0.75rem;
  border: none;
  cursor: pointer;
  text-decoration: none;
  font-size: 0.95rem;
  font-family: inherit;
  font-weight: 600;
  height: 38px;
  justify-content: center;
}

.user-count {
  color: #94a3b8;
  font-weight: 600;
}

.data-panel {
  background: #111827;
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  overflow: hidden;
}

.table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
}

.table-header h3 {
  margin: 0;
  color: #f1f5f9;
  font-size: 15px;
}

.btn-primary {
  background: #3b82f6;
  color: #fff;
  border-radius: 8px;
  padding: 7px 14px;
  font-size: 13px;
  display: flex;
  align-items: center;
  gap: 6px;
  text-decoration: none;
  font-weight: 600;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  text-align: left;
  padding: 0.65rem 0.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  background: #0f172a;
  font-weight: 600;
  font-size: 0.82rem;
  color: #475569;
  text-transform: uppercase;
  letter-spacing: 0.6px;
}

tbody td {
  padding: 0.75rem 0.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.04);
  font-size: 0.88rem;
  color: #94a3b8;
}

tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
}
tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.03);
}
.actions {
  display: flex;
  gap: 0.5rem;
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
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.4);
  z-index: 10;
  white-space: nowrap;
}

.dropdown-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  background: transparent;
  color: #f1f5f9;
  padding: 0.35rem 0.6rem;
  font-size: 0.78rem;
  border-radius: 0.4rem;
  cursor: pointer;
  border: 1px solid transparent;
}

.dropdown-btn:hover {
  background: rgba(59, 130, 246, 0.15);
  border-color: rgba(59, 130, 246, 0.3);
}

.dropdown-btn.danger {
  color: white;
  background: #dc2626;
}

.dropdown-btn.success-btn {
  background: rgba(16, 185, 129, 0.2);
  color: #10b981;
}

.dropdown-btn.warning-btn {
  background: rgba(245, 158, 11, 0.2);
  color: #f59e0b;
}

.icon-btn {
  background: #2563eb;
  color: white;
  padding: 0.5rem;
  border-radius: 0.4rem;
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}

.icon-btn.expanded svg {
  transform: rotate(180deg);
}

.badge {
  padding: 4px 10px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 600;
}
.badge.created      { background: rgba(59, 130, 246, 0.15); color: #60a5fa; border: 1px solid rgba(59, 130, 246, 0.25); }
.badge.accepted     { background: rgba(16, 185, 129, 0.15); color: #10b981; border: 1px solid rgba(16, 185, 129, 0.25); }
.badge.not-accepted { background: rgba(239, 68, 68, 0.15); color: #ef4444; border: 1px solid rgba(239, 68, 68, 0.25); }
.badge.modified     { background: rgba(168, 85, 247, 0.15); color: #a855f7; border: 1px solid rgba(168, 85, 247, 0.25); }
.badge.deleted      { background: rgba(100, 116, 139, 0.15); color: #94a3b8; border: 1px solid rgba(100, 116, 139, 0.25); }

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

.no-data-row td {
  text-align: center;
  padding: 2rem;
  color: #64748b;
}

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
  background: #0f172a;
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