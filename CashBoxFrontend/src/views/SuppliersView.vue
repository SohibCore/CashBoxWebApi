<template>
  <div class="page-card wide-card suppliers-page">
    <div class="section-header"> 
      <div class="header-centered">
        <h2>Ta'minotchilar</h2>
        <p>Ta'minotchi qo'shish, ro'yxatni ko'rish va boshqarish.</p>
      </div>
    </div>

    <div class="section-actions">
      <div class="search-and-count">
        <input type="text" v-model="searchQuery" placeholder="Nomi yoki INN bo'yicha qidirish..." class="search-input" />
        <span class="user-count">{{ filteredSuppliers.length }} ta ta'minotchi</span>
      </div>
    </div>

    <div class="data-panel">
      <div class="table-header">
        <h3>Ta'minotchilar ro'yxati</h3>
        <router-link to="/suppliers/new" class="btn-primary">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi ta'minotchi qo'shish
        </router-link>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Kodi</th>
            <th>INN</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredSuppliers.length === 0">
            <td colspan="4" style="text-align: center; padding: 2rem; color: #64748b;">Ma'lumot yo'q</td>
          </tr>
          <tr v-for="item in filteredSuppliers" :key="item.id" @dblclick="startEdit(item)" style="cursor: pointer;">
            <td>{{ item.id || '-' }}</td>
            <td>{{ item.code || '-' }}</td>
            <td>{{ item.inn || '-' }}</td>
            <td class="actions" @click.stop>
              <div class="action-dropdown-wrapper">
                <button
                  type="button"
                  @click="toggleRow(item.id)"
                  :class="['icon-btn', { expanded: expandedSupplierId === item.id }]"
                  title="Amallarni ko'rsatish"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="6 9 12 15 18 9"></polyline>
                  </svg>
                </button>
                <div v-if="expandedSupplierId === item.id" class="action-dropdown">
                  <button type="button" @click="startEdit(item)" class="dropdown-btn">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                      <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                    </svg>
                    Tahrirlash
                  </button>
                  <button type="button" @click="deleteItem(item.id)" class="dropdown-btn danger">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="3 6 5 6 21 6"></polyline>
                      <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                      <line x1="10" y1="11" x2="10" y2="17"></line>
                      <line x1="14" y1="11" x2="14" y2="17"></line>
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
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import NavigationHistory from './NavigationHistory.vue';
import { getSuppliers, deleteSupplier, extractApiData, getField } from '../api';

const router = useRouter();
const suppliers = ref([]);
const expandedSupplierId = ref(null);
const searchQuery = ref('');

const filteredSuppliers = computed(() => {
  const q = searchQuery.value.toLowerCase();
  if (!q) return suppliers.value;
  return suppliers.value.filter(s => 
    (s.name || '').toLowerCase().includes(q) || 
    (s.fullName || '').toLowerCase().includes(q) ||
    (s.inn || '').toLowerCase().includes(q)
  );
});

const toggleRow = (id) => {
  expandedSupplierId.value = expandedSupplierId.value === id ? null : id;
};

const loadSuppliers = async () => {
  try {
    const res = await getSuppliers();
    const result = extractApiData(res);
    const raw = Array.isArray(result) ? result : [];
    suppliers.value = raw.map(s => ({
      id: getField(s, ['id', 'Id']),
      name: getField(s, ['name', 'Name']),
      fullName: getField(s, ['fullName', 'FullName']),
      code: getField(s, ['code', 'Code']),
      inn: getField(s, ['inn', 'Inn'])
    }));
  } catch (err) {
    console.error('Load suppliers error:', err);
  }
};

const startEdit = (s) => {
  expandedSupplierId.value = null;
  router.push(`/suppliers/edit/${s.id}`);
};

const deleteItem = async (id) => {
  if (!window.confirm("O'chirishni tasdiqlaysizmi?")) return;
  expandedSupplierId.value = null;
  try {
    await deleteSupplier(id);
    await loadSuppliers();
  } catch (err) {
    console.error("Delete supplier error:", err);
  }
};

onMounted(() => {
  loadSuppliers();
});
</script>

<style scoped>
.suppliers-page.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; width: 100%; max-width: 1400px; margin: 0 auto; box-sizing: border-box; }
.section-header { margin-bottom: 1.5rem; color: #f1f5f9; }
.header-centered { text-align: center; width: 100%; }
.section-actions { display: flex; flex-wrap: wrap; justify-content: space-between; gap: 1rem; align-items: center; margin-bottom: 1rem; }
.search-and-count { display: flex; align-items: center; gap: 1rem; }
.search-input { padding: 0.75rem 1rem; border: 1px solid rgba(255, 255, 255, 0.1); background: rgba(255, 255, 255, 0.05); color: #f1f5f9; border-radius: 0.75rem; font-size: 0.95rem; outline: none; }
.user-count { color: #94a3b8; font-weight: 600; }
.data-panel { background: #111827; border-radius: 0.5rem; border: 1px solid rgba(255, 255, 255, 0.07); overflow: hidden; }
.table-header { display: flex; align-items: center; justify-content: space-between; padding: 12px 16px; border-bottom: 1px solid rgba(255, 255, 255, 0.07); }
.table-header h3 { margin: 0; color: #f1f5f9; font-size: 15px; }
.btn-primary { background: #3b82f6; color: #fff; border-radius: 8px; padding: 7px 14px; font-size: 13px; display: flex; align-items: center; gap: 6px; text-decoration: none; font-weight: 600; }
table { width: 100%; border-collapse: collapse; }
thead th { text-align: left; padding: 0.8rem; border-bottom: 1px solid rgba(255, 255, 255, 0.07); background-color: #0f172a; color: #475569; font-weight: 600; text-transform: uppercase; letter-spacing: 0.6px; font-size: 11px; }
tbody td { padding: 0.9rem 0.8rem; border-bottom: 1px solid rgba(255, 255, 255, 0.07); color: #94a3b8; font-size: 13.5px; }
tbody tr:hover { background-color: rgba(255, 255, 255, 0.03); }
.actions { display: flex; gap: 0.5rem; white-space: nowrap; }
.action-dropdown-wrapper { position: relative; display: inline-block; }
.action-dropdown { position: absolute; top: 100%; right: 0; margin-top: 0.3rem; display: flex; gap: 0.4rem; background: #1e293b; padding: 0.4rem; border-radius: 0.5rem; border: 1px solid rgba(255, 255, 255, 0.1); z-index: 10; white-space: nowrap; }
.dropdown-btn { display: inline-flex; align-items: center; gap: 0.3rem; background: transparent; color: #f1f5f9; padding: 0.35rem 0.6rem; font-size: 0.78rem; border-radius: 0.4rem; cursor: pointer; border: 1px solid transparent; }
.dropdown-btn:hover { background: rgba(59, 130, 246, 0.15); border-color: rgba(59, 130, 246, 0.3); }
.dropdown-btn.danger { color: white; background: #dc2626; border-color: #dc2626; }
.icon-btn { border: none; background: #2563eb; color: white; padding: 0.5rem; border-radius: 0.4rem; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: background 0.2s ease; }
.icon-btn.expanded svg { transform: rotate(180deg); }
</style>