<template>
  <div class="currencies-page">
    <div class="page-card wide-card">
    <div class="section-header"> 
      <div>
        <h2>Valyutalar</h2>
        <p>Mavjud valyutalarni boshqarish va yangilarini qo'shish.</p>
      </div>
    </div>

    <div class="section-actions">
      <span class="user-count">{{ currencies.length }} ta valyuta</span> <!-- 'toggle-create' bu yerdan olib tashlandi -->
    </div>

    <div class="data-panel">
      <div class="table-header"> <!-- Yangi table-header divi qo'shildi -->
        <h3>Valyutalar ro'yxati</h3>
        <!-- "+ Yangi valyuta yaratish" tugmasi bu yerga ko'chirildi -->
        <router-link to="/currencies/new" class="btn-primary">
          <svg
            width="14"
            height="14"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="3"
          >
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Yangi valyuta yaratish
        </router-link>
      </div>
      <table>
        <thead>
          <tr>
            <th>Kodi</th>
            <th>To'liq nomi</th>
            <th>Qisqa nomi</th>
            <th class="actions-head">Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="currency in currencies" :key="currency.id" @dblclick="startEdit(currency.id)">
            <td>{{ currency.code }}</td>
            <td>{{ currency.fullName }}</td>
            <td>{{ currency.shortName }}</td>
            <td class="actions">
              <div class="action-dropdown-wrapper">
                <button @click="toggleRow(currency.id)" :class="['icon-btn', { expanded: expandedCurrencyId === currency.id }]" title="Amallarni ko'rsatish">
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="6 9 12 15 18 9"></polyline>
                  </svg>
                </button>
                <div v-if="expandedCurrencyId === currency.id" class="action-dropdown">
                  <button @click="startEdit(currency.id)" class="dropdown-btn">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                      <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                    </svg>
                    Tahrirlash
                  </button>
                  <button @click="deleteCurrencyRow(currency.id)" class="dropdown-btn danger">
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
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { getCurrencies, deleteCurrency, extractApiData, getField } from '../api';

export default {
  name: 'CurrenciesView',
  setup() {
    const router = useRouter();
    const currencies = ref([]);
    const expandedCurrencyId = ref(null);

    const loadCurrencies = async () => {
      try {
        const response = await getCurrencies();
        const result = extractApiData(response);
        const rawData = Array.isArray(result) ? result : [];

        currencies.value = rawData.map(item => ({
          id: getField(item, ['id', 'Id']),
          code: getField(item, ['code', 'Code']),
          fullName: getField(item, ['fullName', 'FullName', 'name', 'Name']),
          shortName: getField(item, ['shortName', 'ShortName', 'symbol', 'Symbol'])
        }));
      } catch (error) {
        console.error('Error loading currencies:', error);
      }
    };

    const startEdit = (id) => {
      router.push(`/currencies/edit/${id}`);
      expandedCurrencyId.value = null;
    };

    const deleteCurrencyRow = async (id) => {
      if (confirm('Rostdan ham ushbu valyutani o\'chirmoqchimisiz?')) {
        try {
          await deleteCurrency(id);
          await loadCurrencies();
        } catch (error) {
          console.error('Error deleting currency:', error);
        }
      }
    };

    const toggleRow = (id) => {
      expandedCurrencyId.value = expandedCurrencyId.value === id ? null : id;
    };

    onMounted(loadCurrencies);

    return {
      currencies,
      expandedCurrencyId,
      startEdit,
      deleteCurrencyRow,
      toggleRow
    };
  }
};
</script>

<style scoped>
/* Sahifaning umumiy fonini belgilaymiz */
.currencies-page {
  background: #0d1117 !important;
  min-height: 100%;
  width: 100%;
  box-sizing: border-box;
}

.page-card {
  /* Karta fonini va hoshiyasini dark theme ga moslaymiz */
  background: #111827;
  padding: 24px;
  border-radius: 12px;
  border: 1px solid rgba(255, 255, 255, 0.07);
  width: 100%;
  box-sizing: border-box;
}

.wide-card {
  max-width: none; /* Sahifani eniga to'liq uzaytiramiz */
  margin: 0;
}

.section-header { /* Boshqa sahifalarga mos header stillari */
  margin-bottom: 1.5rem;
}

.section-header h2 { color: #f1f5f9; margin: 0; }
.section-header p { color: #94a3b8; margin: 4px 0 0; }

/* Amallar qatori (filtr va hisoblagich) */
.section-actions {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  gap: 1rem;
  align-items: center;
  margin-bottom: 1rem;
}

.user-count { /* Foydalanuvchi sonini ko'rsatuvchi stil */
  color: #94a3b8;
  font-weight: 600;
}

/* Jadval kartochkasi stillari */
.data-panel {
  background: #111827; /* Jadval cardining foni */
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.07); /* Jadval cardining hoshiyasi */
  overflow: hidden; /* Ichki kontentni o'ramaydigan stillar */
}

/* Jadval sarlavhasi (header) va tugma stillari */
.table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07); /* Header ostidagi chiziq */
}

.table-header h3 { /* Jadval sarlavhasi matni */
  margin: 0;
  color: #f1f5f9;
  font-size: 15px;
}

/* Tugma stillari */
.btn-primary {
  background: #3b82f6;
  color: #fff;
  border: none;
  border-radius: 8px;
  padding: 7px 14px;
  font-size: 13px;
  display: flex;
  align-items: center;
  gap: 6px;
  cursor: pointer;
  text-decoration: none;
  font-weight: 600;
  transition: background 0.2s;
}

.btn-primary:hover {
  background: #2563eb;
}

th {
  text-align: left;
  padding: 0.75rem;
  background-color: #0f172a;
  color: #475569;
  font-size: 11px;
  text-transform: uppercase;
  letter-spacing: 0.6px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  font-weight: 600;
}

td {
  padding: 0.75rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  color: #94a3b8;
  vertical-align: middle;
  font-size: 13.5px;
}

tbody tr {
  cursor: pointer;
  transition: background-color 0.2s;
}
tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.03);
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
  background: #1e293b;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 0.5rem;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  z-index: 100;
  display: flex;
  gap: 0.25rem;
  padding: 0.25rem;
  white-space: nowrap;
}

.dropdown-btn {
  padding: 0.4rem 0.6rem;
  font-size: 0.75rem;
  display: flex;
  align-items: center;
  gap: 0.4rem;
  background: transparent;
  border: 1px solid transparent;
  color: #f1f5f9;
  border-radius: 0.35rem;
}

.icon-btn {
  border: none;
  background: #2563eb; /* Ko'k rang */
  color: white;
  padding: 0.5rem;
  border-radius: 0.4rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background 0.2s ease;
}

.icon-btn.expanded {
  background: #1d4ed8; /* Kengaytirilganda biroz to'qroq ko'k */
  color: white;
}

.icon-btn:hover {
  background: #1d4ed8; /* Hover holatida to'qroq ko'k */
}
</style>