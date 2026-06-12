<template>
  <div class="document-report-page">
    <div class="section-header">
      <h2 class="text-2xl font-bold text-white">Soliishtirma dalolatnoma (Hisobot)</h2>
    </div>

    <!-- Toolbar -->
    <div class="toolbar">
      <label>Dan:</label>
      <input type="date" v-model="filters.dateFrom" />
      <label>Gacha:</label>
      <input type="date" v-model="filters.dateTo" />
      <button @click="fetchList" :disabled="loading">
          <span v-if="loading" class="spinner-small"></span>
        {{ loading ? 'Yuklanmoqda...' : 'Yangilash' }}
      </button>
    </div>

    <!-- Summary cards -->
    <div class="summary-grid">
      <div class="scard">
        <div class="label">Ochilish qoldig'i (Debet)</div>
        <div class="value blue">{{ fmt(totalOpeningDebit) }}</div>
      </div>
      <div class="scard">
        <div class="label">Davr kirim</div>
        <div class="value positive">{{ fmt(totalDebit) }}</div>
      </div>
      <div class="scard">
        <div class="label">Davr chiqim</div>
        <div class="value negative">{{ fmt(totalCredit) }}</div>
      </div>
      <div class="scard">
        <div class="label">Yopilish qoldig'i</div>
        <div class="value" :class="{ 'positive': (totalClosingDebit - totalClosingCredit) >= 0, 'negative': (totalClosingDebit - totalClosingCredit) < 0 }">
          {{ fmt(totalClosingDebit - totalClosingCredit) }}
        </div>
      </div>
    </div>

    <!-- Jadval -->
    <div class="data-panel">
      <div v-if="loading" class="loading-overlay">Yuklanmoqda...</div>
      <div v-else class="overflow-x-auto">
        <table class="report-table">
          <thead>
            <tr class="group-header">
              <th rowspan="2">Kontragent (Kodi)</th>
              <th rowspan="2">INN</th>
              <th colspan="2" class="group-border">Ochilish qoldig'i</th>
              <th colspan="2" class="group-border">Davr aylanmasi</th>
              <th colspan="2" class="group-border">Yopilish qoldig'i</th>
            </tr>
            <tr class="sub-header">
              <th class="text-right">Debet</th>
              <th class="text-right">Kredit</th>
              <th class="text-right">Debet</th>
              <th class="text-right">Kredit</th>
              <th class="text-right">Debet</th>
              <th class="text-right">Kredit</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="list.length === 0 && !loading">
              <td colspan="8" class="empty-cell">Ma'lumot yo'q</td>
            </tr>
            <tr v-for="(item, idx) in list" :key="idx" class="report-row">
              <td class="supplier-cell">{{ item.supplierName }}</td>
              <td>{{ item.inn || '—' }}</td>
              <td class="text-right debit-val">{{ fmt(item.openingDebit) }}</td>
              <td class="text-right credit-val">{{ fmt(item.openingCredit) }}</td>
              <td class="text-right debit-val">{{ fmt(item.debit) }}</td>
              <td class="text-right credit-val">{{ fmt(item.credit) }}</td>
              <td class="text-right" :class="{ 'debit-val': item.closingDebit > 0 }">
                {{ fmt(item.closingDebit) }}
              </td>
              <td class="text-right" :class="{ 'credit-val': item.closingCredit > 0 }">
                {{ fmt(item.closingCredit) }}
              </td>
            </tr>
          </tbody>
          <tfoot>
            <tr class="footer-row">
              <td colspan="2" class="text-right font-bold">JAMI:</td>
              <td class="text-right debit-val">{{ fmt(totalOpeningDebit) }}</td>
              <td class="text-right credit-val">{{ fmt(totalOpeningCredit) }}</td>
              <td class="text-right debit-val">{{ fmt(totalDebit) }}</td>
              <td class="text-right credit-val">{{ fmt(totalCredit) }}</td>
              <td class="text-right debit-val">{{ fmt(totalClosingDebit) }}</td>
              <td class="text-right credit-val">{{ fmt(totalClosingCredit) }}</td>
            </tr>
          </tfoot>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { documentReportService,
         type DocumentReportListDto,
         type DocumentReportFilterDto } from './documentReportService';

const list = ref<DocumentReportListDto[]>([]);
const loading = ref(false);

const today = new Date();
const firstDay = new Date(today.getFullYear(), 0, 1);

const filters = ref<DocumentReportFilterDto>({
  dateFrom: firstDay.toISOString().split('T')[0],
  dateTo: today.toISOString().split('T')[0],
});

const totalOpeningDebit  = computed(() => list.value.reduce((s, i) => s + (i.openingDebit  || 0), 0));
const totalOpeningCredit = computed(() => list.value.reduce((s, i) => s + (i.openingCredit || 0), 0));
const totalDebit         = computed(() => list.value.reduce((s, i) => s + (i.debit         || 0), 0));
const totalCredit        = computed(() => list.value.reduce((s, i) => s + (i.credit        || 0), 0));
const totalClosingDebit  = computed(() => list.value.reduce((s, i) => s + (i.closingDebit  || 0), 0));
const totalClosingCredit = computed(() => list.value.reduce((s, i) => s + (i.closingCredit || 0), 0));

const fetchList = async () => {
  loading.value = true;
  try {
    list.value = await documentReportService.getHeaderReport(filters.value);
  } catch (err) {
    console.error(err);
  } finally {
    loading.value = false;
  }
};

const fmt = (val: number) => {
  if (!val || val === 0) return '—';
  return new Intl.NumberFormat('uz-UZ').format(val);
};

onMounted(fetchList);
</script>

<style scoped>
.document-report-page {
  padding: 1rem;
  background: #0d1117;
  min-height: 100vh;
  color: #f1f5f9; /* Default text color */
}

.section-header {
  margin-bottom: 1.5rem;
  text-align: center;
}

/* Toolbar */
.toolbar {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 20px;
  background: #111827;
  padding: 1rem;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  flex-wrap: wrap;
}

.toolbar label {
  font-size: 0.85rem;
  color: #94a3b8;
  font-weight: 600;
}

.toolbar input[type=date] {
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #fff;
  padding: 0.6rem 1rem;
  border-radius: 0.5rem;
  outline: none;
  font-size: 0.9rem;
}

.toolbar button {
  background: #2563eb;
  color: white;
  border: none;
  padding: 0.7rem 1.5rem;
  border-radius: 0.5rem;
  cursor: pointer;
  font-weight: 600;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background-color 0.2s;
}

.toolbar button:hover:not(:disabled) {
  background: #1d4ed8;
}

.toolbar button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

/* Summary Cards */
.summary-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 12px;
  margin-bottom: 20px;
}

.scard {
  background: #1e293b;
  border-radius: 10px;
  padding: 16px;
  border: 1px solid rgba(255, 255, 255, 0.07);
}

.scard .label {
  font-size: 12px;
  color: #64748b;
  text-transform: uppercase;
  margin-bottom: 5px;
}

.scard .value {
  font-size: 20px;
  font-weight: 700;
  color: #f1f5f9; /* Default value color */
}

.value.positive {
  color: #22c55e;
}

.value.negative {
  color: #ef4444;
}

.value.blue {
  color: #3b82f6;
}

/* Table */
.data-panel {
  background: #111827;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  overflow: hidden;
  position: relative; /* For loading overlay */
}

.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(17, 24, 39, 0.8); /* #111827 with transparency */
  display: flex;
  align-items: center;
  justify-content: center;
  color: #f1f5f9;
  font-size: 1.2rem;
  font-weight: 600;
  z-index: 10;
}

.report-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.85rem;
}

.report-table th,
.report-table td {
  padding: 0.75rem 1rem;
  border: 1px solid rgba(255, 255, 255, 0.05);
}

.group-header th {
  background: #0a0f1a;
  color: #475569;
  font-weight: 700;
  text-transform: uppercase;
  font-size: 0.7rem;
  text-align: center;
}

.sub-header th {
  background: #0f172a;
  color: #94a3b8;
  font-size: 0.75rem;
}

.group-border {
  border-left: 2px solid rgba(255, 255, 255, 0.1) !important;
}

.report-table thead th {
  white-space: nowrap; /* Prevent header text from wrapping */
}

.report-row:hover {
  background: rgba(255, 255, 255, 0.02);
}

.supplier-cell {
  color: #f1f5f9;
  font-weight: 500;
}

.debit-val {
  color: #22c55e !important; /* Green for debit */
}

.credit-val {
  color: #ef4444 !important; /* Red for credit */
}

.empty-cell {
  text-align: center;
  padding: 3rem;
  color: #475569;
  font-style: italic;
}

.footer-row td {
  background: #0f172a;
  color: #f1f5f9;
  font-weight: 700;
  border-top: 2px solid rgba(255, 255, 255, 0.1);
}

.spinner-small {
  width: 14px;
  height: 14px;
  border: 2px solid rgba(255,255,255,0.3);
  border-top-color: #fff;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }
</style>