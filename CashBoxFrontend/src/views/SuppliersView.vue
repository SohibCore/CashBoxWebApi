<template>
  <div class="page-card wide-card suppliers-page">
    <div class="section-header"> 
      <div>
        <h2>Ta'minotchilar</h2>
        <p>Ta'minotchi qo'shish, ro'yxatni ko'rish va yangilash.</p>
      </div>
    </div>

    <div class="section-actions">
      <router-link to="/suppliers/new" class="toggle-create">
        <span>+</span> Yangi ta'minotchi qo'shish
      </router-link>
      <span class="user-count">{{ suppliers.length }} ta ta'minotchi</span>
    </div>

    <div class="data-panel">
      <h3>Ta'minotchilar ro'yxati</h3>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>INN</th>
            <th>Code</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in suppliers" :key="item.id" @dblclick="startEdit(item)">
            <td>{{ item.id || '-' }}</td>
            <td>{{ item.inn || '-' }}</td>
            <td>{{ item.code || '-' }}</td>
            <td class="actions">
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
                  <button type="button" @click="deleteRow(item.id)" class="dropdown-btn danger">
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
import { getSuppliers, deleteSupplier, extractApiData } from '../api';

export default {
  setup() {
    const router = useRouter();
    const suppliers = ref([]);
    const expandedSupplierId = ref(null);

    const toggleRow = (id) => {
      expandedSupplierId.value = expandedSupplierId.value === id ? null : id;
    };

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) {
          return obj[key];
        }
      }
      return null;
    };

    const loadSuppliers = async () => {
      try {
        const response = await getSuppliers();
        const result = extractApiData(response);
        const raw = Array.isArray(result) ? result : [];
        
        suppliers.value = raw.map((s) => ({
          id: getField(s, ['id', 'Id', 'supplierId']),
          inn: getField(s, ['inn', 'Inn']) || '',
          code: getField(s, ['code', 'Code']) || '',
          fullName: getField(s, ['fullName', 'FullName']) || '',
          shortName: getField(s, ['shortName', 'ShortName']) || getField(s, ['fullName', 'FullName']) || ''
        }));
      } catch (error) {
        console.error("Load suppliers error:", error);
      }
    };

    const startEdit = (s) => {
      expandedSupplierId.value = null;
      router.push(`/suppliers/edit/${s.id}`);
    };

    const deleteRow = async (id) => {
      if (!window.confirm("O'chirishni tasdiqlaysizmi?")) return;
      expandedSupplierId.value = null;
      try {
        await deleteSupplier(id);
        await loadSuppliers();
      } catch (error) {
        console.error("Delete supplier error:", error);
      }
    };

    onMounted(loadSuppliers);

    return {
      suppliers,
      expandedSupplierId,
      toggleRow,
      startEdit,
      deleteRow
    };
  }
};
</script>

<style scoped>
.suppliers-page.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.suppliers-page.wide-card {
  max-width: none;
  width: 100%;
  margin: 0;
  box-sizing: border-box;
}

.section-header {
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
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  border: none;
  cursor: pointer;
  text-decoration: none;
  font-size: inherit;
  font-family: inherit;
  font-weight: 600;
}

.user-count {
  color: #475569;
  font-weight: 600;
}

.data-panel {
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
}

.data-panel h3 {
  margin-top: 0;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  text-align: left;
  padding: 0.65rem 0.5rem;
  border-bottom: 1px solid #e2e8f0;
  background: #f9fafb;
  font-weight: 600;
  font-size: 0.82rem;
  color: #475569;
}

tbody td {
  padding: 0.75rem 0.5rem;
  border-bottom: 1px solid #f1f5f9;
  font-size: 0.88rem;
}

tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
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
  border: 1px solid transparent;
  background: #ffffff;
  color: #0f172a;
  padding: 0.35rem 0.6rem;
  font-size: 0.78rem;
  border-radius: 0.4rem;
  cursor: pointer;
}

.dropdown-btn:hover {
  background: #eff6ff;
  border-color: #cbd5e1;
}

.dropdown-btn.danger {
  color: white;
  background: #dc2626;
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
</style>