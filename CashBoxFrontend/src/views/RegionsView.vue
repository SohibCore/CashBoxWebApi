<template>
  <div class="page-card wide-card regions-page">
    <div class="section-header">
      <div>
        <h2>Viloyatlar</h2>
        <p>Viloyat qo‘shish, ro‘yxatni ko‘rish va yangilash.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="createNewRegion">
      <h3>Yangi viloyat</h3>
      <div class="form-grid">
        <label>
          To‘liq nom
          <input v-model="newRegion.fullName" required maxlength="500" />
        </label>
        <label>
          Qisqa nom
          <input v-model="newRegion.shortName" required maxlength="300" />
        </label>
        <label>
          Kod
          <input v-model="newRegion.code" required maxlength="9" />
        </label>
      </div>
      <button type="submit">Saqlash</button>
      <p v-if="createError" class="error">{{ createError }}</p>
    </form>

    <div class="data-panel">
      <h3>Viloyatlar ro‘yxati</h3>
      <p v-if="regions.length === 0" class="empty-hint">Viloyatlar topilmadi</p>
      <table v-else>
        <thead>
          <tr>
            <th>ID</th>
            <th>To‘liq nom</th>
            <th>Qisqa nom</th>
            <th>Kod</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="region in regions" :key="region.id">
            <td>{{ region.id || '-' }}</td>
            <td>{{ region.fullName || '-' }}</td>
            <td>{{ region.shortName || '-' }}</td>
            <td>{{ region.code || '-' }}</td>
            <td class="actions">
              <div class="action-dropdown-wrapper">
                <button
                  type="button"
                  @click="toggleRow(region.id)"
                  :class="['icon-btn', { expanded: expandedRegionId === region.id }]"
                  title="Amallarni ko'rsatish"
                >
                  <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <polyline points="6 9 12 15 18 9"></polyline>
                  </svg>
                </button>
                <div v-if="expandedRegionId === region.id" class="action-dropdown">
                  <button type="button" @click="startEdit(region)" class="dropdown-btn">
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                      <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                    </svg>
                    Tahrirlash
                  </button>
                  <button type="button" @click="deleteRow(region.id)" class="dropdown-btn danger">
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

    <div v-if="editRegion" class="entity-form">
      <h3>Viloyatni yangilash</h3>
      <div class="form-grid">
        <label>
          To‘liq nom
          <input v-model="editRegion.fullName" required maxlength="500" />
        </label>
        <label>
          Qisqa nom
          <input v-model="editRegion.shortName" required maxlength="300" />
        </label>
        <label>
          Kod
          <input v-model="editRegion.code" required maxlength="9" />
        </label>
      </div>
      <div class="button-row">
        <button type="button" @click="saveEdit">Yangilash</button>
        <button type="button" class="btn-secondary" @click="cancelEdit">Bekor qilish</button>
      </div>
      <p v-if="updateError" class="error">{{ updateError }}</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { getRegions, createRegion, updateRegion, deleteRegion, extractApiData } from '../api';

export default {
  setup() {
    const regions = ref([]);
    const expandedRegionId = ref(null);
    const newRegion = ref({ fullName: '', shortName: '', code: '' });
    const editRegion = ref(null);
    const createError = ref('');
    const updateError = ref('');

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) {
          return obj[key];
        }
      }
      return null;
    };

    const mapRegion = (item) => ({
      id: getField(item, ['id', 'Id']),
      fullName: getField(item, ['fullName', 'FullName', 'name', 'Name']) || '',
      shortName: getField(item, ['shortName', 'ShortName']) || '',
      code: getField(item, ['code', 'Code']) || ''
    });

    const loadRegions = async () => {
      try {
        const response = await getRegions();
        const data = extractApiData(response);
        const rawList = Array.isArray(data)
          ? data
          : Array.isArray(data?.items)
          ? data.items
          : Array.isArray(data?.value)
          ? data.value
          : [];
        regions.value = rawList.map(mapRegion);
      } catch (error) {
        console.error('Error loading regions:', error);
        regions.value = [];
      }
    };

    const toggleRow = (id) => {
      expandedRegionId.value = expandedRegionId.value === id ? null : id;
    };

    const createNewRegion = async () => {
      createError.value = '';
      try {
        await createRegion(newRegion.value);
        newRegion.value = { fullName: '', shortName: '', code: '' };
        await loadRegions();
      } catch (error) {
        console.error('Error creating region:', error);
        createError.value =
          error.response?.data?.message ||
          error.response?.data?.title ||
          'Viloyat yaratishda xatolik yuz berdi.';
      }
    };

    const startEdit = (region) => {
      expandedRegionId.value = null;
      editRegion.value = { ...region };
    };

    const saveEdit = async () => {
      updateError.value = '';
      if (!editRegion.value?.id) return;
      const payload = {
        fullName: editRegion.value.fullName,
        shortName: editRegion.value.shortName,
        code: editRegion.value.code
      };
      try {
        await updateRegion(editRegion.value.id, payload);
        editRegion.value = null;
        await loadRegions();
      } catch (error) {
        console.error('Error updating region:', error);
        updateError.value =
          error.response?.data?.message ||
          error.response?.data?.title ||
          'Viloyatni yangilashda xatolik yuz berdi.';
      }
    };

    const deleteRow = async (id) => {
      expandedRegionId.value = null;
      try {
        await deleteRegion(id);
        if (editRegion.value?.id === id) {
          editRegion.value = null;
        }
        await loadRegions();
      } catch (error) {
        console.error('Error deleting region:', error);
      }
    };

    const cancelEdit = () => {
      editRegion.value = null;
      updateError.value = '';
    };

    onMounted(loadRegions);

    return {
      regions,
      expandedRegionId,
      newRegion,
      editRegion,
      createError,
      updateError,
      toggleRow,
      createNewRegion,
      startEdit,
      saveEdit,
      deleteRow,
      cancelEdit
    };
  }
};
</script>

<style scoped>
.regions-page.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.regions-page.wide-card {
  max-width: none;
  width: 100%;
  margin: 0;
  box-sizing: border-box;
}

.section-header {
  margin-bottom: 1.5rem;
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
