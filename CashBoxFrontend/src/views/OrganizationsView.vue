<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Tashkilotlar</h2>
        <p>Tashkilot qo‘shish, ro‘yxatni ko‘rish va yangilash.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="createNewOrganization">
      <h3>Yangi tashkilot</h3>
      <div class="form-grid">
        <label>INN<input v-model="newOrganization.inn" required maxlength="9" /></label>
        <label>To‘liq nom<input v-model="newOrganization.fullName" required /></label>
        <label>Qisqa nom<input v-model="newOrganization.shortName" required /></label>
        <label>Region ID<input type="number" v-model.number="newOrganization.regionId" required min="1" /></label>
        <label>Hudud<input v-model="newOrganization.district" required /></label>
      </div>
      <button type="submit">Saqlash</button>
      <p v-if="createError" class="error">{{ createError }}</p>
    </form>

    <div class="data-panel">
      <h3>Tashkilotlar ro‘yxati</h3>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>INN</th>
            <th>To‘liq nom</th>
            <th>Qisqa nom</th>
            <th>Region ID</th>
            <th>Hudud</th>
            <th>Amallar</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="org in organizations" :key="org.id">
            <td>{{ org.id || '-' }}</td>
            <td>{{ org.inn || '-' }}</td>
            <td>{{ org.fullName || '-' }}</td>
            <td>{{ org.shortName || '-' }}</td>
            <td>{{ org.regionId || '-' }}</td>
            <td>{{ org.district || '-' }}</td>
            <td>
              <button @click="startEdit(org)" class="icon-btn" title="Tahrirlash">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                </svg>
              </button>
              <button @click="deleteRow(org.id)" class="icon-btn danger" title="O'chirish">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <polyline points="3 6 5 6 21 6"></polyline>
                  <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                  <line x1="10" y1="11" x2="10" y2="17"></line>
                  <line x1="14" y1="11" x2="14" y2="17"></line>
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="editOrganization" class="entity-form">
      <h3>Mavjud tashkilotni yangilash</h3>
      <div class="form-grid">
        <label>INN<input v-model="editOrganization.inn" maxlength="9" /></label>
        <label>To‘liq nom<input v-model="editOrganization.fullName" /></label>
        <label>Qisqa nom<input v-model="editOrganization.shortName" /></label>
        <label>Region ID<input type="number" v-model.number="editOrganization.regionId" min="1" /></label>
        <label>Hudud<input v-model="editOrganization.district" /></label>
      </div>
      <div class="button-row">
        <button type="button" @click="saveEdit">Yangilash</button>
        <button type="button" class="danger" @click="cancelEdit">Bekor qilish</button>
      </div>
      <p v-if="updateError" class="error">{{ updateError }}</p>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { getOrganizations, createOrganization, updateOrganization, deleteOrganization } from '../api';

export default {
  setup() {
    const organizations = ref([]);
    const newOrganization = ref({ inn: '', fullName: '', shortName: '', regionId: 0, district: '' });
    const editOrganization = ref(null);
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

    const loadOrganizations = async () => {
      try {
        const response = await getOrganizations();
        const result = response.data;
        const rawOrgs = Array.isArray(result)
          ? result
          : Array.isArray(result?.data)
          ? result.data
          : Array.isArray(result?.items)
          ? result.items
          : Array.isArray(result?.value)
          ? result.value
          : [];
        organizations.value = rawOrgs.map((org) => ({
          id: getField(org, ['id', 'Id', 'organizationId', 'OrganizationId']),
          inn: getField(org, ['inn', 'Inn']) || '',
          fullName: getField(org, ['fullName', 'FullName']) || '',
          shortName: getField(org, ['shortName', 'ShortName']) || getField(org, ['fullName', 'FullName']) || '',
          regionId: getField(org, ['regionId', 'RegionId']) || 0,
          district: getField(org, ['district', 'District']) || ''
        }));
      } catch (error) {
        console.error(error);
      }
    };

    const createNewOrganization = async () => {
      createError.value = '';
      try {
        await createOrganization(newOrganization.value);
        await loadOrganizations();
        Object.assign(newOrganization.value, { inn: '', fullName: '', shortName: '', regionId: 0, district: '' });
      } catch (error) {
        console.error('Create organization error:', error.response?.data || error.message);
        if (error.response?.data?.errors) {
          createError.value = Object.values(error.response.data.errors).flat().join(' ');
        } else {
          createError.value = error.response?.data?.message || 'Tashkilot yaratishda xatolik yuz berdi.';
        }
      }
    };

    const startEdit = (org) => {
      editOrganization.value = { ...org };
    };

    const saveEdit = async () => {
      updateError.value = '';
      if (!editOrganization.value) return;
      const payload = { ...editOrganization.value };
      delete payload.id;
      try {
        await updateOrganization(editOrganization.value.id, payload);
        editOrganization.value = null;
        await loadOrganizations();
      } catch (error) {
        console.error('Update organization error:', error.response?.data || error.message);
        if (error.response?.data?.errors) {
          updateError.value = Object.values(error.response.data.errors).flat().join(' ');
        } else {
          updateError.value = error.response?.data?.message || 'Tashkilotni yangilashda xatolik yuz berdi.';
        }
      }
    };

    const deleteRow = async (id) => {
      try {
        await deleteOrganization(id);
        await loadOrganizations();
      } catch (error) {
        console.error(error);
      }
    };

    const cancelEdit = () => {
      editOrganization.value = null;
    };

    onMounted(loadOrganizations);

    return {
      organizations,
      newOrganization,
      editOrganization,
      createError,
      updateError,
      createNewOrganization,
      startEdit,
      saveEdit,
      deleteRow,
      cancelEdit
    };
  }
};
</script>

<style>
.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.wide-card {
  max-width: 1100px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
}

.entity-form {
  margin-bottom: 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 1rem;
}

label {
  display: block;
  font-weight: 600;
}

input {
  width: 100%;
  padding: 0.75rem;
  margin-top: 0.5rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.7rem;
  font-size: 0.95rem;
}

button {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  cursor: pointer;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

.data-panel {
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
}

table {
  width: 100%;
  min-width: 1000px;
  border-collapse: collapse;
}

thead th {
  text-align: left;
  padding: 0.8rem;
  border-bottom: 1px solid #e2e8f0;
  background: #f9fafb;
  font-weight: 600;
  white-space: nowrap;
}

tbody td {
  padding: 0.9rem 0.8rem;
  border-bottom: 1px solid #f1f5f9;
}

.actions {
  display: flex;
  gap: 0.5rem;
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

.icon-btn:hover {
  background: #1d4ed8;
}

.icon-btn.danger {
  background: #dc2626;
}

.icon-btn.danger:hover {
  background: #b91c1c;
}

button.danger {
  background: #dc2626;
}
</style>
