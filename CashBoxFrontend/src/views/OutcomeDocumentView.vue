<template>
  <div class="income-documents-page">
    <div class="section-header"> 
      <h2>Chiqim hujjatlari</h2>
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
          <span class="stat-label">Tasdiqlangan</span>
          <span class="stat-value">{{ formatSum(acceptedSum) }}</span>
        </div>
      </div>
      <div class="stat-card unpaid">
        <div class="stat-icon">⏳</div>
        <div class="stat-info">
          <span class="stat-label">Kutilayotgan/Rad etilgan</span>
          <span class="stat-value">{{ formatSum(pendingRejectedSum) }}</span>
        </div>
      </div>
    </div>

    <div class="section-actions">
      <span class="user-count">{{ filteredDocuments.length }} ta hujjat</span>
    </div>

    <div class="data-panel">
      <div class="table-header">
        <h3>Hujjatlar ro'yxati</h3>
        <router-link to="/outcome-documents/new" class="btn-primary">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi hujjat qo'shish
        </router-link>
      </div>

      <!-- Asosiy Jadval -->
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-[#0f172a] border-b border-white/5 text-[#475569] uppercase text-xs">
              <th class="p-3">ID</th>
              <th class="p-3">Ta'minotchi</th>
              <th class="p-3">Jami summa</th>
              <th class="p-3">Sana</th>
              <th class="p-3">Status</th>
              <th class="p-3">Amallar</th>
            </tr>
          </thead>
          <tbody>
            <tr 
              v-if="filteredDocuments.length === 0" class="no-data-row">
              <td colspan="6">Ma'lumot yo'q</td>
            </tr>
            <tr 
              v-for="doc in filteredDocuments" 
              :key="doc.id" 
              @dblclick="viewDetails(doc.id)"
              class="border-b border-white/5 text-[#94a3b8] hover:bg-white/[0.02] cursor-pointer transition"
              title="Batafsil ko'rish uchun ikki marta bosing"
            >
              <td class="p-3">#{{ doc.id }}</td>
              <td class="p-3" style="font-weight: 500;">{{ doc.supplierName || doc.supplierId }}</td>
              <td class="p-3" style="font-weight: 600;">{{ formatSum(doc.docSum) }}</td>
              <td class="p-3">{{ formatDate(doc.docOn) }}</td>
              <td class="p-3">
                <span :class="statusClass(doc.statusId)" class="px-2 py-1 rounded text-xs font-medium">
                  {{ doc.statusName }}
                </span>
              </td>
              <td class="p-3">
                <div class="flex gap-2">
                  <button v-if="doc.statusId !== 3" @click.stop="$router.push('/outcome-documents/edit/' + doc.id)" class="text-blue-600 hover:underline font-medium">Tahrirlash</button>
                  <button v-if="[1,2].includes(doc.statusId)" @click.stop="deleteDoc(doc.id)" class="text-red-600 hover:underline">O'chirish</button>
                  <button @click.stop="viewDetails(doc.id)" class="text-gray-600 hover:underline">Ko'rish</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Detal ko'rinishi (Modal) -->
    <div v-if="selectedDoc || detailLoading" class="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center p-4 z-50">
      <div v-if="detailLoading" class="bg-[#111827] p-8 rounded-lg border border-white/10 shadow-2xl">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-[#94a3b8]">Ma'lumotlar yuklanmoqda...</p>
      </div>
      
      <div v-else class="bg-[#111827] rounded-lg border border-white/10 shadow-2xl w-full max-w-5xl max-h-[90vh] overflow-hidden flex flex-col">
        <div class="p-6 border-b border-white/5 flex justify-between items-center bg-[#0f172a]">
          <div>
            <h3 class="text-xl font-bold text-white">Hujjat ma'lumotlari #{{ selectedDoc.id }}</h3>
            <p class="text-sm text-[#64748b]">Ta'minotchi: <strong class="text-[#f1f5f9]">{{ selectedDoc.supplierName }}</strong> | Sana: <strong class="text-blue-500">{{ formatDate(selectedDoc.docOn) }}</strong></p>
          </div>
          <button @click="closeModal" class="text-gray-400 hover:text-gray-600 text-3xl leading-none">&times;</button>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1">
          <h4 class="font-bold mb-4 text-gray-700 uppercase text-sm tracking-wider">Mahsulotlar ro'yxati</h4>
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-[#0f172a] border-b border-white/5 text-xs font-bold text-[#475569] uppercase">
                <th class="p-2">Mahsulot</th>
                <th class="p-2 text-right w-32">Narxi</th>
                <th class="p-2 text-center w-24">Miqdori</th>
                <th class="p-2 text-right w-40">Jami</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in selectedDoc.tables" :key="item.id" class="border-b border-white/5">
                <td class="p-2 text-[#f1f5f9] font-medium">{{ item.productName || 'Mahsulot #' + item.productId }}</td>
                <td class="p-2 text-right">{{ formatSum(item.price) }}</td>
                <td class="p-2 text-center text-[#94a3b8]">{{ item.quantity }}</td>
                <td class="p-2 text-right font-bold text-blue-400">{{ formatSum(item.totalSum) }}</td>
              </tr>
            </tbody>
            <tfoot>
              <tr class="font-black bg-blue-600/5 text-xl border-t border-blue-500/20">
                <td colspan="3" class="p-4 text-right text-[#64748b]">JAMI HUJJAT SUMMASI:</td>
                <td class="p-4 text-right text-blue-500">{{ formatSum(selectedDoc.docSum) }} UZS</td>
              </tr>
            </tfoot>
          </table>
        </div>
        
        <div class="p-4 border-t border-white/5 bg-[#0f172a] flex justify-between items-center px-6">
          <div class="text-sm text-gray-500 italic">* Hujjat holati: {{ selectedDoc.statusName }}</div>
          <div class="flex gap-3">
            <button @click="closeModal" class="bg-gray-200 text-gray-700 px-6 py-2 rounded-lg font-semibold hover:bg-gray-300 transition">Yopish</button>
            <button v-if="selectedDoc.statusId !== 3" @click="handleAccept(selectedDoc.id)" class="bg-green-600 text-white px-6 py-2 rounded-lg font-semibold hover:bg-green-700 transition">Tasdiqlash</button>
            <button v-if="selectedDoc.statusId === 3" @click="handleNotAccept(selectedDoc.id)" class="bg-orange-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-orange-600 transition">Qaytarish (Not Accept)</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts"> 
import { ref, computed, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { outcomeDocumentService } from './outcomeDocumentService';
import { type OutcomeDocumentListDto } from './outcomeDocument';
import { extractApiData } from '../api'; // Assuming extractApiData is in api.js

const router = useRouter();
const documents = ref<OutcomeDocumentListDto[]>([]);
const selectedDoc = ref<any>(null); // Missing ref
const detailLoading = ref(false);   // Missing ref

const expandedDocId = ref<number | null>(null);
const filters = reactive({ status: null });
const totalSum = computed(() =>
  documents.value.reduce((sum, d) => sum + (d.docSum || 0), 0)
);
const acceptedSum = computed(() =>
  documents.value.filter(d => d.statusId === 2).reduce((sum, d) => sum + (d.docSum || 0), 0)
);
const pendingRejectedSum = computed(() =>
  documents.value.filter(d => [1, 3].includes(d.statusId)).reduce((sum, d) => sum + (d.docSum || 0), 0)
);

const filteredDocuments = computed(() => documents.value);

const fetchList = async () => {
  try {
    const res = await outcomeDocumentService.getList();
    const data = (res.data as any).data || res.data;
    documents.value = Array.isArray(data) ? data : [];
    // Ensure statusName is set if not coming from API directly
    documents.value.forEach(doc => { if (!doc.statusName) doc.statusName = statusLabel(doc.statusId); });
  } catch (err) { console.error('Xatolik:', err); }
};

const viewDetails = async (id: number) => {
  detailLoading.value = true;
  selectedDoc.value = null;
  try {
    const res = await outcomeDocumentService.getById(id);
    selectedDoc.value = res.data;
  } catch (err) { console.error('Yuklashda xatolik:', err); }
  finally { detailLoading.value = false; }
};

const closeModal = () => { selectedDoc.value = null; detailLoading.value = false; };

const handleAccept = async (id: number) => {
  if (!confirm('Ushbu hujjatni tasdiqlaysizmi?')) return;
  try {
    await outcomeDocumentService.accept(id);
    closeModal();
    fetchList();
  } catch (err) { alert('Tasdiqlashda xatolik'); }
};

const handleNotAccept = async (id: number) => {
  if (!confirm('Ushbu hujjatni "Tasdiqlanmagan" holatiga qaytarmoqchimisiz?')) return;
  try {
    await outcomeDocumentService.notAccept(id);
    closeModal();
    fetchList();
  } catch (err) { alert('Amalni bajarishda xatolik'); }
};

const deleteDoc = async (id: number) => {
  if (!confirm('Hujjatni o\'chirmoqchimisiz?')) return;
  try {
    await outcomeDocumentService.delete(id);
    fetchList();
  } catch (err) { alert('O\'chirishda xatolik'); }
};

const formatDate = (dateStr: string) => {
  if (!dateStr) return '';
  return new Date(dateStr).toLocaleDateString('ru-RU');
};

const formatSum = (val: number) => new Intl.NumberFormat('ru-RU').format(val || 0);

const statusLabel = (id: number) => {
  switch (id) {
    case 1: return 'Yaratildi';
    case 2: return 'Tasdiqlandi';
    case 3: return 'Rad etildi';
    case 4: return 'O\'zgartirildi';
    case 5: return 'O\'chirildi';
    default: return 'Noma\'lum';
  }
};

const statusClass = (id: number) => {
  switch (id) {
    case 1: return 'bg-blue-900/30 text-blue-400 border border-blue-500/30';
    case 2: return 'bg-green-900/30 text-green-400 border border-green-500/30';
    case 3: return 'bg-red-900/30 text-red-400 border border-red-500/30';
    case 4: return 'bg-purple-900/30 text-purple-400 border border-purple-500/30';
    case 5: return 'bg-gray-900/30 text-gray-400 border border-gray-500/30';
    default: return 'bg-gray-800 text-gray-400';
  }
};

onMounted(() => {
  fetchList(); // Fixed function name
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

/* Nested Table Styles (Not used in this refactor, but kept for consistency if needed later) */
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