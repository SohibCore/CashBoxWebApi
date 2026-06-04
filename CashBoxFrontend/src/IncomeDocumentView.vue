<template>
  <main class="main">
    <div class="page-header">
      <h1 class="page-title">Kirim hujjatlari</h1>
      <p class="page-sub">Kirim hujjatlarini qo'shish, ko'rish va boshqarish.</p>
    </div>

    <!-- STAT CARDS -->
    <div class="stat-row">
      <div class="stat-card blue">
        <div class="stat-icon">💰</div>
        <div class="stat-info">
          <div class="stat-label">Jami summa</div>
          <div class="stat-val">{{ formatPrice(totalSum) }} UZS</div>
        </div>
      </div>
      <div class="stat-card green">
        <div class="stat-icon">✅</div>
        <div class="stat-info">
          <div class="stat-label">To'langan</div>
          <div class="stat-val">{{ formatPrice(paidSum) }} UZS</div>
        </div>
      </div>
      <div class="stat-card amber">
        <div class="stat-icon">⏳</div>
        <div class="stat-info">
          <div class="stat-label">To'lanmagan</div>
          <div class="stat-val">{{ formatPrice(unpaidSum) }} UZS</div>
        </div>
      </div>
    </div>

    <!-- ACTION ROW -->
    <div class="action-row">
      <button class="add-btn" @click="$router.push('/income-documents/new')">
        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/>
        </svg>
        Yangi hujjat qo'shish
      </button>
      <div class="right">
        <select class="filter-select" v-model="filterStatus">
          <option value="all">Barcha statuslar</option>
          <option value="1">To'langan</option>
          <option value="0">To'lanmagan</option>
        </select>
        <span class="count-text">{{ filteredDocuments.length }} ta hujjat</span>
      </div>
    </div>

    <!-- TABLE -->
    <div class="table-card">
      <div class="table-head-row">
        <span class="table-card-title">Hujjatlar ro'yxati</span>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Ta'minotchi</th>
            <th>Mahsulot</th>
            <th>Miqdori</th>
            <th>Narxi</th>
            <th>Jami summa</th>
            <th>Sana</th>
            <th>Status</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="doc in filteredDocuments" :key="doc.id">
            <td>{{ doc.id }}</td>
            <td>{{ doc.supplierName || 'SUP-001' }}</td>
            <td>{{ doc.productName || '—' }}</td>
            <td>{{ doc.quantity }}</td>
            <td>{{ formatPrice(doc.price) }}</td>
            <td>{{ formatPrice(doc.totalSum) }}</td>
            <td>{{ doc.date ? doc.date.slice(0, 10) : '—' }}</td>
            <td>
              <span :class="['badge', doc.status === 1 ? 'badge-success' : 'badge-warning']">
                {{ doc.status === 1 ? 'To\'langan' : 'To\'lanmagan' }}
              </span>
            </td>
            <td><button class="action-btn">▾</button></td>
          </tr>
          <tr v-if="filteredDocuments.length === 0">
            <td colspan="9" style="text-align:center; padding: 40px; color: #475569;">Hujjatlar topilmadi</td>
          </tr>
        </tbody>
      </table>
    </div>
  </main>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import axios from 'axios';

const documents = ref([]);
const filterStatus = ref('all');

const fetchDocuments = async () => {
  try {
    const response = await axios.get('http://localhost:5107/api/IncomeDocument/GetList', {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    });
    documents.value = response.data?.data || response.data || [];
  } catch (error) {
    console.error('Fetch error:', error);
  }
};

const filteredDocuments = computed(() => {
  if (filterStatus.value === 'all') return documents.value;
  return documents.value.filter(d => d.status.toString() === filterStatus.value);
});

const totalSum = computed(() => documents.value.reduce((s, d) => s + (d.totalSum || 0), 0));
const paidSum = computed(() => documents.value.filter(d => d.status === 1).reduce((s, d) => s + (d.totalSum || 0), 0));
const unpaidSum = computed(() => documents.value.filter(d => d.status === 0).reduce((s, d) => s + (d.totalSum || 0), 0));

const formatPrice = (val) => new Intl.NumberFormat('uz-UZ').format(val || 0);

onMounted(fetchDocuments);
</script>

<style scoped>
.main { flex: 1; padding: 24px 28px; background: #0d1117; min-height: 100vh; }
.page-header { text-align: center; margin-bottom: 24px; }
.page-title { font-size: 22px; font-weight: 700; color: #f1f5f9; }
.page-sub { font-size: 13px; color: #94a3b8; margin-top: 4px; }

.stat-row { display: grid; grid-template-columns: repeat(3, 1fr); gap: 12px; margin-bottom: 20px; }
.stat-card { background: #111827; border: 1px solid rgba(255,255,255,0.07); border-radius: 12px; padding: 16px 18px; display: flex; align-items: center; gap: 14px; position: relative; overflow: hidden; }
.stat-card::before { content: ''; position: absolute; left: 0; top: 0; bottom: 0; width: 3px; }
.stat-card.blue::before { background: #3b82f6; }
.stat-card.green::before { background: #10b981; }
.stat-card.amber::before { background: #f59e0b; }
.stat-icon { font-size: 26px; }
.stat-label { font-size: 11px; color: #475569; text-transform: uppercase; letter-spacing: 0.7px; margin-bottom: 3px; }
.stat-val { font-size: 20px; font-weight: 700; font-family: 'Space Grotesk', sans-serif; color: #f1f5f9; }

.action-row { display: flex; align-items: center; gap: 12px; margin-bottom: 20px; }
.add-btn { display: flex; align-items: center; gap: 6px; background: linear-gradient(135deg,#3b82f6,#2563eb); border: none; border-radius: 9px; color: white; padding: 9px 16px; cursor: pointer; box-shadow: 0 2px 12px rgba(59,130,246,0.3); }
.filter-select { background: rgba(255,255,255,0.04); border: 1px solid rgba(255,255,255,0.07); border-radius: 8px; padding: 8px 12px; color: #94a3b8; outline: none; }
.count-text { font-size: 13px; color: #475569; margin-left: auto; }

.table-card { background: #111827; border: 1px solid rgba(255,255,255,0.07); border-radius: 12px; overflow: hidden; }
.table-head-row { padding: 14px 18px; border-bottom: 1px solid rgba(255,255,255,0.07); }
.table-card-title { font-size: 14px; font-weight: 600; color: #f1f5f9; }

table { width: 100%; border-collapse: collapse; }
thead tr { background: #0f172a; }
thead th { padding: 11px 16px; text-align: left; font-size: 11px; color: #475569; text-transform: uppercase; letter-spacing: 0.6px; }
tbody td { padding: 13px 16px; font-size: 13.5px; color: #94a3b8; border-bottom: 1px solid rgba(255,255,255,0.07); }
tbody tr:hover { background: rgba(255,255,255,0.03); }

.badge { padding: 3px 10px; border-radius: 20px; font-size: 11px; font-weight: 600; }
.badge-warning { background: rgba(245,158,11,0.15); color: #f59e0b; border: 1px solid rgba(245,158,11,0.25); }
.badge-success { background: rgba(16,185,129,0.15); color: #10b981; border: 1px solid rgba(16,185,129,0.25); }
.action-btn { background: #3b82f6; border: none; border-radius: 7px; color: white; padding: 4px 8px; cursor: pointer; }
</style>