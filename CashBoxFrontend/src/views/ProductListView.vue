<template>
  <div class="page-card wide-card product-page">
    <div class="section-header">
      <div>
        <h2>Mahsulotlar</h2>
        <p>Ombordagi barcha mahsulotlar ro'yxati va ularni Excelga eksport qilish.</p>
      </div>
    </div>

    <div class="section-actions">
      <div class="search-and-count">
        <input 
          type="text" 
          v-model="searchQuery" 
          placeholder="Nomi yoki kodi bo'yicha qidirish..." 
          class="search-input" 
        />
      </div>
      <div class="action-buttons">
        <button @click="exportToExcel" :disabled="isExporting" class="btn-export" title="Mahsulotlarni Excel formatida yuklab olish">
          <svg v-if="!isExporting" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
            <polyline points="7 10 12 15 17 10"></polyline>
            <line x1="12" y1="15" x2="12" y2="3"></line>
          </svg>
          <span v-else class="spinner-sm"></span>
          <span>{{ isExporting ? 'Yuklanmoqda...' : 'Excelga yuklab olish' }}</span>
        </button>
        
        <router-link to="/products/new" class="btn-primary">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi mahsulot
        </router-link>
      </div>
    </div>

    <div class="data-panel">
      <table>
        <thead>
          <tr>
            <th style="width: 80px;">ID</th>
            <th>Nomi</th>
            <th>Kodi</th>
            <th>Tashkilot ID</th>
            <th>Yetkazilgan</th>
            <th style="width: 100px;">Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="loading"><td colspan="6" class="text-center">Yuklanmoqda...</td></tr>
          <tr v-else-if="filteredProducts.length === 0"><td colspan="6" class="text-center">Mahsulotlar topilmadi</td></tr>
          <tr v-for="product in filteredProducts" :key="product.id" @dblclick="$router.push('/products/edit/' + product.id)">
            <td>#{{ product.id }}</td>
            <td class="name-cell">{{ product.name }}</td>
            <td><code>{{ product.code }}</code></td>
            <td>{{ product.organizationId }}</td>
            <td>{{ formatDate(product.deliveredAt) }}</td>
            <td class="actions">
              <button @click="$router.push('/products/edit/' + product.id)" class="icon-btn-sm blue" title="Tahrirlash">✎</button>
              <button @click="handleDelete(product.id)" class="icon-btn-sm red" title="O'chirish">🗑</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { api, getProducts, deleteProduct, extractApiData } from '../api';

const products = ref([]);
const loading = ref(false);
const isExporting = ref(false);
const searchQuery = ref('');

const loadProducts = async () => {
  loading.value = true;
  try {
    const res = await getProducts();
    products.value = extractApiData(res) || [];
  } finally {
    loading.value = false;
  }
};

const filteredProducts = computed(() => {
  const q = searchQuery.value.toLowerCase();
  return products.value.filter(p => 
    String(p.name).toLowerCase().includes(q) || 
    String(p.code).toLowerCase().includes(q)
  );
});

const exportToExcel = async () => {
  isExporting.value = true;
  try {
    // Backend: ExportController -> Products action (URL: /api/Export/products)
    const response = await api.get('/api/Export/products', {
      responseType: 'blob' 
    });
    
    const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', 'Mahsulotlar.xlsx');
    document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
  } catch (err) {
    console.error('Export error:', err);
    alert('Excel faylni yuklashda xatolik yuz berdi.');
  } finally {
    isExporting.value = false;
  }
};

const handleDelete = async (id) => {
  if (confirm("Ushbu mahsulotni o'chirishni xohlaysizmi?")) {
    await deleteProduct(id);
    await loadProducts();
  }
};

const formatDate = (d) => d ? new Date(d).toLocaleDateString() : '-';

onMounted(loadProducts);
</script>

<style scoped>
.product-page { background: #0d1117; min-height: 100%; }
.section-actions { display: flex; justify-content: space-between; margin-bottom: 1.5rem; gap: 1rem; }
.action-buttons { display: flex; gap: 12px; }
.search-input { 
  padding: 0.75rem 1.25rem; background: #111827; border: 1px solid rgba(255,255,255,0.1);
  color: white; border-radius: 0.75rem; width: 320px; outline: none;
}
.search-input:focus { border-color: #3b82f6; }

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
table { width: 100%; border-collapse: collapse; }
th { text-align: left; padding: 1rem; background: #0f172a; color: #475569; font-size: 0.7rem; text-transform: uppercase; }
td { padding: 1rem; color: #94a3b8; border-bottom: 1px solid rgba(255,255,255,0.03); font-size: 0.9rem; }
.name-cell { font-weight: 600; color: #f1f5f9; }
.spinner-sm { width: 14px; height: 14px; border: 2px solid currentColor; border-top-color: transparent; border-radius: 50%; animation: spin 0.8s linear infinite; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>