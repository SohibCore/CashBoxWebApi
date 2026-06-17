<template>
  <div class="page-card wide-card outcome-page">
    <div class="section-header">
      <div>
        <h2>Chiqim hujjatlari</h2>
        <p>Sotuv va chiqim hujjatlarini boshqarish hamda hisobini yuritish.</p>
      </div>
    </div>

    <div class="stats-cards">
      <div class="stat-card total">
        <div class="stat-icon">📄</div>
        <div class="stat-info">
          <span class="stat-label">Jami hujjatlar</span>
          <span class="stat-value">{{ documents.length }} ta</span>
        </div>
      </div>
      <div class="stat-card sum">
        <div class="stat-icon">💰</div>
        <div class="stat-info">
          <span class="stat-label">Umumiy summa</span>
          <span class="stat-value">{{ formatSum(totalSum) }}</span>
        </div>
      </div>
    </div>

    <div class="section-actions">
      <div class="search-and-count">
        <input type="text" v-model="searchQuery" placeholder="Hujjat ID yoki sana..." class="search-input" />
      </div>
      <div class="action-buttons">
        <router-link to="/outcome-documents/new" class="btn-primary">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
            <line x1="12" y1="5" x2="12" y2="19"></line><line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi chiqim hujjati
        </router-link>
      </div>
    </div>

    <div class="data-panel">
      <div class="table-header">
        <h3>Barcha chiqim hujjatlari</h3>
      </div>
      <table>
        <thead>
          <tr>
            <th style="width: 80px;">ID</th>
            <th>Sana</th>
            <th>Tashkilot</th>
            <th>Jami Summa</th>
            <th style="width: 100px;">Holat</th>
            <th style="width: 80px;">Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="loading"><td colspan="6" class="text-center">Yuklanmoqda...</td></tr>
          <tr v-else-if="filteredDocs.length === 0"><td colspan="6" class="text-center">Hujjatlar topilmadi</td></tr>
          <tr v-for="doc in filteredDocs" :key="doc.id" @dblclick="$router.push('/outcome-documents/edit/' + doc.id)">
            <td>#{{ doc.id }}</td>
            <td>{{ formatDate(doc.docOn) }}</td>
            <td>{{ doc.organizationName || '-' }}</td>
            <td class="sum-cell">{{ formatSum(doc.docSum) }}</td>
            <td>
              <span :class="['badge', getStatusClass(doc.statusId)]">
                {{ doc.statusName || 'Yaratildi' }}
              </span>
            </td>
            <td class="actions">
              <button @click="$router.push('/outcome-documents/edit/' + doc.id)" class="icon-btn-sm blue" title="Tahrirlash">✎</button>
              <button @click="handleDelete(doc.id)" class="icon-btn-sm red" title="O'chirish">🗑</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { api, getOutcomeDocuments, deleteOutcomeDocument, extractApiData } from '../api'; // api import qilinganligiga ishonch hosil qiling

const documents = ref([]);
const loading = ref(false);
const searchQuery = ref('');

const totalSum = computed(() => documents.value.reduce((s, d) => s + (d.docSum || 0), 0));

const filteredDocs = computed(() => {
  const q = searchQuery.value.toLowerCase();
  return documents.value.filter(d => 
    String(d.id).includes(q) || 
    (d.docOn && d.docOn.includes(q))
  );
});

const loadDocuments = async () => {
  loading.value = true;
  try {
    const res = await getOutcomeDocuments();
    documents.value = extractApiData(res) || [];
  } finally {
    loading.value = false;
  }
};

const handleDelete = async (id) => {
  if (confirm("Ushbu hujjatni o'chirishni xohlaysizmi?")) {
    await deleteOutcomeDocument(id);
    await loadDocuments();
  }
};

const formatSum = (v) => new Intl.NumberFormat('uz-UZ').format(v || 0) + ' UZS';
const formatDate = (d) => d ? new Date(d).toLocaleDateString() : '-';
const getStatusClass = (id) => id === 2 ? 'status-green' : 'status-blue';

onMounted(loadDocuments);
</script>

<style scoped>
.outcome-page { background: #0d1117; min-height: 100%; }
.stats-cards { display: flex; gap: 1.5rem; margin-bottom: 2rem; }
.stat-card { 
  background: #111827; border: 1px solid rgba(255,255,255,0.07); 
  padding: 1.5rem; border-radius: 1rem; flex: 1; display: flex; align-items: center; gap: 1.25rem;
  border-left: 4px solid #3b82f6;
}
.stat-icon { font-size: 2rem; background: rgba(59,130,246,0.1); padding: 10px; border-radius: 12px; }
.stat-label { display: block; font-size: 0.8rem; color: #64748b; text-transform: uppercase; letter-spacing: 0.5px; }
.stat-value { font-size: 1.2rem; font-weight: 700; color: #f1f5f9; }
.section-actions { display: flex; justify-content: space-between; margin-bottom: 1.5rem; }
.search-input { 
  padding: 0.75rem 1.25rem; background: #111827; border: 1px solid rgba(255,255,255,0.1);
  color: white; border-radius: 0.75rem; width: 300px; outline: none;
}
.search-input:focus { border-color: #3b82f6; }
.action-buttons { display: flex; gap: 12px; } /* Tugmalar orasidagi masofa uchun */
.btn-export {
  background: rgba(16, 185, 129, 0.1);
  color: #10b981;
  border: 1px solid rgba(16, 185, 129, 0.2);
  padding: 0.75rem 1.5rem;
  border-radius: 0.75rem;
  display: flex;
  align-items: center;
  gap: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: 0.2s;
}
.btn-export:hover:not(:disabled) {
  background: #10b981;
  color: white;
  transform: translateY(-2px);
}
.btn-export:disabled { opacity: 0.6; cursor: not-allowed; }
.btn-primary { 
  background: linear-gradient(135deg, #3b82f6, #2563eb); color: white;
  padding: 0.75rem 1.5rem; border-radius: 0.75rem; display: flex; align-items: center; gap: 8px;
  font-weight: 600; text-decoration: none; transition: 0.2s;
}
.btn-primary:hover { transform: translateY(-2px); box-shadow: 0 4px 15px rgba(37,99,235,0.3); }
.data-panel { background: #111827; border-radius: 1rem; border: 1px solid rgba(255,255,255,0.07); overflow: hidden; }
.table-header { padding: 1.25rem; border-bottom: 1px solid rgba(255,255,255,0.07); }
table { width: 100%; border-collapse: collapse; }
th { text-align: left; padding: 1rem; background: #0f172a; color: #475569; font-size: 0.7rem; text-transform: uppercase; }
td { padding: 1rem; color: #94a3b8; border-bottom: 1px solid rgba(255,255,255,0.03); font-size: 0.9rem; }
.sum-cell { font-weight: 600; color: #f1f5f9; }
.badge { padding: 4px 10px; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.spinner-sm { width: 14px; height: 14px; border: 2px solid currentColor; border-top-color: transparent; border-radius: 50%; animation: spin 0.8s linear infinite; }
.status-blue { background: rgba(59,130,246,0.1); color: #60a5fa; }
.status-green { background: rgba(34,197,94,0.1); color: #4ade80; }
.icon-btn-sm { border: none; padding: 6px; border-radius: 6px; cursor: pointer; margin-right: 5px; background: rgba(255,255,255,0.05); color: #94a3b8; }
.icon-btn-sm.red:hover { background: #ef4444; color: white; }
.icon-btn-sm.blue:hover { background: #3b82f6; color: white; }
</style>