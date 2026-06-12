<template>
  <div class="page-card wide-card organizations-page">
    <NavigationHistory />
    <div class="section-header"> 
      <div>
        <h2>Tashkilotlar</h2>
        <p>Tashkilot qo‘shish, ro‘yxatni ko‘rish va yangilash.</p>
      </div>
    </div>

    <div class="section-actions">
      <div class="search-and-count">
        <input type="text" v-model="searchQuery" placeholder="Nom yoki INN bo'yicha qidirish..." class="search-input" />
        <span class="user-count">{{ (filteredOrganizations || []).length }} ta tashkilot</span>
      </div>
    </div>

    <Transition name="fade" mode="out-in">
      <div class="data-panel" :key="filteredOrganizations.length">
        <div class="table-header">
          <h3>Tashkilotlar ro‘yxati</h3>
          <router-link to="/organizations/new" class="btn-primary">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            Yangi tashkilot qo'shish
          </router-link>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>INN</th>
              <th>To‘liq nom</th>
              <th>Qisqa nom</th>
              <th>Viloyat</th>
              <th>Hudud</th>
              <th>Amallar</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="org in filteredOrganizations" :key="org.id" @dblclick="startEdit(org)">
              <td>{{ org.id || '-' }}</td>
              <td>{{ org.inn || '-' }}</td>
              <td>{{ org.fullName || '-' }}</td>
              <td>{{ org.shortName || '-' }}</td>
              <td>{{ org.regionName || '-' }}</td>
              <td>{{ org.district || '-' }}</td>
              <td class="actions">
                <div class="action-dropdown-wrapper">
                  <button
                    type="button"
                    @click="toggleRow(org.id)"
                    :class="['icon-btn', { expanded: expandedOrgId === org.id }]"
                    title="Amallarni ko'rsatish"
                  >
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="6 9 12 15 18 9"></polyline>
                    </svg>
                  </button>
                  <div v-if="expandedOrgId === org.id" class="action-dropdown">
                    <button type="button" @click="startEdit(org)" class="dropdown-btn">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                        <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                    </svg>
                    Tahrirlash
                  </button>
                  <button type="button" @click="deleteRow(org.id)" class="dropdown-btn danger">
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
    </Transition>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import NavigationHistory from './NavigationHistory.vue';
import { getOrganizations, deleteOrganization, extractApiData, getField, getMe, normalizeUser } from '../api';

export default {
  setup() {
    const router = useRouter();
    const organizations = ref([]);
    const expandedOrgId = ref(null);
    const searchQuery = ref('');

    const filteredOrganizations = computed(() => {
      const q = searchQuery.value.toLowerCase();
      if (!q) return organizations.value;
      return organizations.value.filter(org => 
        (org.fullName || '').toLowerCase().includes(q) || 
        (org.shortName || '').toLowerCase().includes(q) ||
        (org.inn || '').toLowerCase().includes(q)
      );
    });

    const toggleRow = (id) => {
      expandedOrgId.value = expandedOrgId.value === id ? null : id;
    };

    const loadOrganizations = async () => {
      try {
        const response = await getOrganizations();
        organizations.value = extractApiData(response).map(org => ({
          id: getField(org, ['id', 'Id', 'organizationId', 'OrganizationId']),
          inn: getField(org, ['inn', 'Inn']) || '',
          fullName: getField(org, ['fullName', 'FullName']) || '',
          shortName: getField(org, ['shortName', 'ShortName']) || getField(org, ['fullName', 'FullName']) || '',
          regionId: getField(org, ['regionId', 'RegionId']) || 0,
          // Backenddan kelayotgan RegionName ni map qilish
          regionName: getField(org, ['regionName', 'RegionName', 'region', 'Region']) || '',
          district: getField(org, ['district', 'District']) || ''
        }));
      } catch (error) {
        console.error(error);
      }
    };
 
    const startEdit = (org) => {
      expandedOrgId.value = null;
      router.push(`/organizations/edit/${org.id}`);
    };

    const deleteRow = async (id) => {
      expandedOrgId.value = null;
      try {
        await deleteOrganization(id);
        await loadOrganizations();
      } catch (error) {
        console.error(error);
      }
    };

    onMounted(loadOrganizations);

    return {
      organizations,
      expandedOrgId,
      searchQuery,
      filteredOrganizations,
      toggleRow,
      startEdit,
      deleteRow
    };
  }
};
</script>

<style scoped>
.page-card {
  background: #111827;
  padding: 1.5rem;
  border-radius: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
}

.organizations-page.wide-card {
  max-width: none;
  width: 100%;
  margin: 0;
  box-sizing: border-box;
}

.section-header {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  margin-bottom: 1.5rem;
}

.section-header h2 { color: #f1f5f9; }
.section-header p { color: #94a3b8; }

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
  font-size: 0.95rem; /* Inherit o'rniga aniq qiymat */
  font-family: inherit;
  height: 38px; /* Chiqim hujjatlaridagi tugma balandligi */
  justify-content: center; /* Kontentni vertikal markazlash */
  font-weight: 600; /* Qalinroq matn */
}

.user-count {
  color: #94a3b8;
  font-weight: 600;
}

.entity-form {
  margin-bottom: 1.5rem;
  background: rgba(255, 255, 255, 0.02);
  padding: 1.5rem;
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
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
  display: block;
  font-weight: 600;
  color: #94a3b8;
}

input {
  width: 100%;
  padding: 0.75rem;
  margin-top: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.7rem;
  font-size: 0.95rem;
  box-sizing: border-box;
}

.entity-form > button[type='submit'],
.button-row button {
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
  margin-top: 1rem;
  flex-wrap: wrap;
}

.button-row button.danger {
  background: #dc2626;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
  font-size: 0.95rem;
}

.data-panel {
  background: #111827;
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  overflow: hidden;
}

.table-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 12px 16px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
}

.table-header h3 {
  margin: 0;
  color: #f1f5f9;
  font-size: 15px;
}

.btn-primary {
  background: #3b82f6;
  color: #fff;
  border-radius: 8px;
  padding: 7px 14px;
  font-size: 13px;
  display: flex;
  align-items: center;
  gap: 6px;
  text-decoration: none;
  font-weight: 600;
}

table {
  width: 100%;
  min-width: 0;
  border-collapse: collapse;
  table-layout: auto;
}

thead th {
  text-align: left;
  padding: 0.8rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  background-color: #0f172a;
  color: #475569;
  font-weight: 600;
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
  background-color: rgba(255, 255, 255, 0.03);
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
  background: #1e293b;
  padding: 0.4rem;
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.4);
  z-index: 10;
  white-space: nowrap;
}

.dropdown-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.3rem;
  margin: 0;
  border: 1px solid transparent;
  background: transparent;
  color: #f1f5f9;
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
  background: rgba(59, 130, 246, 0.15);
  border-color: rgba(59, 130, 246, 0.3);
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

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
