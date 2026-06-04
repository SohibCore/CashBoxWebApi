<template>
  <div class="page-card wide-card products-page">
    <div class="section-header"> 
      <div>
        <h2>Mahsulotlar</h2>
        <p>Mahsulot qo'shish, ro'yxatni ko'rish va boshqarish.</p>
      </div>
    </div>

    <div class="section-actions">
      <router-link to="/products/new" class="toggle-create">
        <span>+</span> Yangi mahsulot qo'shish
      </router-link>
      <div class="search-and-count">
        <input type="text" v-model="searchQuery" placeholder="Nom yoki kod bo'yicha qidirish..." class="search-input" />
        <span class="user-count">{{ filteredProducts.length }} ta mahsulot</span>
      </div>
    </div>

    <div class="data-panel">
      <h3>Mahsulotlar ro'yxati</h3>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Nomi</th>
            <th>Kodi</th>
            <th>Tashkilot ID</th>
            <th>Yetkazilgan</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="filteredProducts.length === 0">
            <td colspan="6" style="text-align: center; padding: 2rem; color: #64748b;">Ma'lumot yo'q</td>
          </tr>
          <tr v-for="item in filteredProducts" :key="item.id" @dblclick="startEdit(item)" style="cursor: pointer;">
            <td>{{ item.id || '-' }}</td>
            <td style="font-weight: 500;">{{ item.name || '-' }}</td>
            <td>{{ item.code || '-' }}</td>
            <td>{{ item.organizationId || '-' }}</td>
            <td>{{ formatDate(item.deliveredAt) }}</td>
            <td class="actions">
              <div class="action-dropdown-wrapper">
                <button
                  type="button"
                  @click="toggleRow(item.id)"
                  :class="['icon-btn', { expanded: expandedProductId === item.id }]"
                  title="Amallarni ko'rsatish"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="6 9 12 15 18 9"></polyline>
                  </svg>
                </button>
                <div v-if="expandedProductId === item.id" class="action-dropdown">
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

<script>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { getProducts, deleteProduct, extractApiData, getField } from '../api';

export default {
  name: 'ProductsView',
  setup() {
    const router = useRouter();
    const products = ref([]);
    const expandedProductId = ref(null);
    const searchQuery = ref('');

    const filteredProducts = computed(() => {
      const q = searchQuery.value.toLowerCase();
      if (!q) return products.value;
      return products.value.filter(p => 
        (p.name || '').toLowerCase().includes(q) || 
        (p.code || '').toLowerCase().includes(q)
      );
    });

    const toggleRow = (id) => {
      expandedProductId.value = expandedProductId.value === id ? null : id;
    };

    const loadProducts = async () => {
      try {
        const res = await getProducts();
        const result = extractApiData(res);
        const raw = Array.isArray(result) ? result : [];
        products.value = raw.map(p => ({
          id: getField(p, ['id', 'Id']),
          name: getField(p, ['name', 'Name']),
          code: getField(p, ['code', 'Code']),
          organizationId: getField(p, ['organizationId', 'OrganizationId']),
          deliveredAt: getField(p, ['deliveredAt', 'DeliveredAt'])
        }));
      } catch (err) {
        console.error('Load products error:', err);
      }
    };

    const startEdit = (p) => {
      expandedProductId.value = null;
      router.push(`/products/edit/${p.id}`);
    };

    const deleteItem = async (id) => {
      if (!window.confirm("O'chirishni tasdiqlaysizmi?")) return;
      expandedProductId.value = null;
      try {
        await deleteProduct(id);
        await loadProducts();
      } catch (err) {
        console.error("Delete product error:", err);
      }
    };

    const formatDate = (date) => {
      if (!date) return '-';
      return new Date(date).toLocaleDateString();
    };

    onMounted(() => {
      loadProducts();
    });

    return {
      products,
      searchQuery,
      filteredProducts,
      expandedProductId,
      toggleRow,
      startEdit,
      deleteItem,
      formatDate
    };
  }
};
</script>

<style scoped>
.products-page.page-card {
  background: #111827;
  padding: 1.5rem;
  border-radius: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  width: 100%;
  max-width: none; /* Sahifani eniga to'liq uzaytiramiz */
  box-sizing: border-box;
}

.section-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  margin-bottom: 1.5rem;
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
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.75rem;
  font-size: 0.95rem;
  outline: none;
}

.toggle-create {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  border: none;
  cursor: pointer;
  text-decoration: none;
  padding: 0.85rem 2rem; /* Gorizontal padding oshirildi */
  font-weight: 600; /* Qalinroq matn */
  height: 38px; /* Chiqim hujjatlaridagi tugma balandligi */
  justify-content: center; /* Kontentni vertikal markazlash */
}

.user-count {
  color: #94a3b8;
  font-weight: 600;
}

.data-panel {
  background: transparent;
  padding: 1.5rem;
  border-radius: 0.5rem;
  overflow-x: visible; /* Gorizontal scrollni olib tashlaymiz */
}

table {
  width: 100%;
  min-width: 0; /* Majburiy kenglikni olib tashlaymiz */
  border-collapse: collapse;
  table-layout: auto;
}

thead th {
  text-align: left;
  padding: 0.8rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  background-color: #0f172a; /* Kulrang header */
  color: #475569;
  font-weight: 600;
  white-space: normal;
  text-transform: uppercase;
  letter-spacing: 0.6px;
  font-size: 11px;
}

tbody td {
  padding: 0.9rem 0.8rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  color: #94a3b8;
  word-break: break-word;
  font-size: 13.5px;
}

tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
}

tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.03); /* Och ko'k hover */
}

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

.icon-btn {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.5rem;
  border-radius: 0.4rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s ease;
}
</style>