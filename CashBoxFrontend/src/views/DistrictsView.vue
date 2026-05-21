<template>
  <div class="page-card wide-card districts-page">
    <div class="section-header">
      <div>
        <h2>Tumanlar</h2>
        <p>Tuman qo‘shish, ro‘yxatni ko‘rish va yangilash.</p>
      </div>
    </div>

    <div class="section-actions">
      <router-link to="/districts/new" class="toggle-create">
        <span>+</span> Yangi tuman qo'shish
      </router-link>
      <span class="user-count">{{ districts.length }} ta tuman</span>
    </div>

    <div class="data-panel">
      <h3>Tumanlar ro‘yxati</h3>
      <p v-if="districts.length === 0" class="empty-hint">Tumanlar topilmadi</p>
      <table v-else>
        <thead>
          <tr>
            <th>ID</th>
            <th>To‘liq nom</th>
            <th>Kod</th>
            <th>Viloyat</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="district in districts" :key="district.id" @dblclick="startEdit(district)" style="cursor: pointer;">
            <td>{{ district.id || '-' }}</td>
            <td>{{ district.fullName || '-' }}</td>
            <td>{{ district.code || '-' }}</td>
            <td>{{ district.region || '-' }}</td>
            <td class="actions">
              <div class="action-dropdown-wrapper">
                <button
                  type="button"
                  @click="toggleRow(district.id)"
                  :class="['icon-btn', { expanded: expandedDistrictId === district.id }]"
                  title="Amallarni ko'rsatish"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="6 9 12 15 18 9"></polyline>
                  </svg>
                </button>
                <div v-if="expandedDistrictId === district.id" class="action-dropdown">
                  <button type="button" @click="startEdit(district)" class="dropdown-btn">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                      <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                    </svg>
                    Tahrirlash
                  </button>
                  <button type="button" @click="deleteRow(district.id)" class="dropdown-btn danger">
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
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getDistricts, deleteDistrict, extractApiData, getRegions } from '../api';

export default {
  setup() {
    const router = useRouter();
    const districts = ref([]);
    const regionsMap = ref({});
    const expandedDistrictId = ref(null);

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) {
          return obj[key];
        }
      }
      return null;
    };

    const loadRegionsMap = async () => {
      try {
        const response = await getRegions();
        const data = extractApiData(response);
        const rawList = Array.isArray(data) ? data : (data?.items || data?.value || []);
        const map = {};
        rawList.forEach(item => {
          const id = getField(item, ['id', 'Id']);
          const name = getField(item, ['fullName', 'FullName', 'name', 'Name']);
          if (id) map[id] = name;
        });
        regionsMap.value = map;
      } catch (error) {
        console.error('Error loading regions for map:', error);
      }
    };

    const mapDistrict = (item) => {
      const regionId = getField(item, ['regionId', 'RegionId']);
      return {
        id: getField(item, ['id', 'Id']),
        fullName: getField(item, ['fullName', 'FullName', 'name', 'Name']) || '',
        code: getField(item, ['code', 'Code']) || '',
        region: regionsMap.value[regionId] || getField(item, ['region', 'Region', 'regionName', 'RegionName']) || regionId || '-'
      };
    };

    const loadDistricts = async () => {
      try {
        const response = await getDistricts();
        const data = extractApiData(response);
        const rawList = Array.isArray(data)
          ? data
          : Array.isArray(data?.items)
          ? data.items
          : Array.isArray(data?.value)
          ? data.value
          : [];
        districts.value = rawList.map(mapDistrict);
      } catch (error) {
        console.error('Error loading districts:', error);
        districts.value = [];
      }
    };

    const toggleRow = (id) => {
      expandedDistrictId.value = expandedDistrictId.value === id ? null : id;
    };

    const startEdit = (district) => {
      expandedDistrictId.value = null;
      router.push(`/districts/edit/${district.id}`);
    };

    const deleteRow = async (id) => {
      expandedDistrictId.value = null;
      try {
        await deleteDistrict(id);
        await loadDistricts();
      } catch (error) {
        console.error('Error deleting district:', error);
      }
    };

    onMounted(async () => {
      await loadRegionsMap();
      await loadDistricts();
    });

    return {
      districts,
      expandedDistrictId,
      toggleRow,
      startEdit,
      deleteRow,
      regionsMap
    };
  }
};
</script>

<style scoped>
.districts-page.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.districts-page.wide-card {
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
}

.user-count {
  color: #475569;
  font-weight: 600;
}

.entity-form {
  margin-bottom: 1.5rem;
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
}

.entity-form h3 {
  margin-top: 0;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

label {
  display: flex;
  flex-direction: column;
  font-weight: 600;
  color: #0f172a;
}

input {
  width: 100%;
  padding: 0.75rem;
  margin-top: 0.5rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.7rem;
  font-size: 0.95rem;
  box-sizing: border-box;
}

.entity-form > button[type='submit'],
.button-row button {
  margin-top: 1rem;
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  cursor: pointer;
  font-weight: 500;
}

.button-row {
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
}

.btn-secondary {
  background: #e5e7eb !important;
  color: #111827 !important;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
  font-size: 0.95rem;
}

.data-panel {
  overflow-x: visible;
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
}

.data-panel h3 {
  margin-top: 0;
}

.empty-hint {
  margin: 0;
  color: #64748b;
  font-size: 0.95rem;
}

table {
  width: 100%;
  min-width: 0;
  border-collapse: collapse;
  table-layout: auto;
}

thead th {
  text-align: left;
  padding: 0.65rem 0.5rem;
  border-bottom: 1px solid #e2e8f0;
  background: #f9fafb;
  font-weight: 600;
  font-size: 0.82rem;
  line-height: 1.3;
  white-space: normal;
}

tbody td {
  padding: 0.75rem 0.5rem;
  border-bottom: 1px solid #f1f5f9;
  font-size: 0.88rem;
  line-height: 1.35;
  vertical-align: top;
  word-break: break-word;
}

.actions {
  display: flex;
  gap: 0.5rem;
  white-space: nowrap;
  word-break: normal;
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
  margin: 0;
  border: 1px solid transparent;
  background: #ffffff;
  color: #0f172a;
  padding: 0.35rem 0.6rem;
  font-size: 0.78rem;
  border-radius: 0.4rem;
  cursor: pointer;
  transition: background 0.2s ease, border-color 0.2s ease;
}

.dropdown-btn svg {
  width: 12px;
  height: 12px;
}

.dropdown-btn:hover {
  background: #eff6ff;
  border-color: #cbd5e1;
}

.dropdown-btn.danger {
  color: white;
  background: #dc2626;
  border-color: #dc2626;
}

.dropdown-btn.danger:hover {
  background: #b91c1c;
  border-color: #b91c1c;
}

.icon-btn {
  margin: 0;
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

.icon-btn:hover {
  background: #1d4ed8;
}

.icon-btn.expanded svg {
  transform: rotate(180deg);
}
</style>
