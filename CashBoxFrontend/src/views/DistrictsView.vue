<template>
  <div class="districts-page">
    <div class="page-card wide-card">
      <div class="section-header">
        <div>
          <h2>Tumanlar</h2>
          <p>Tumanlarni boshqarish va yangilarini qo'shish.</p>
        </div>
      </div>

      <div class="section-actions">
        <span class="user-count">{{ districts.length }} ta tuman</span>
      </div>

      <div class="data-panel">
        <div class="table-header">
          <h3>Tumanlar ro'yxati</h3>
          <router-link to="/districts/new" class="btn-primary">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            Yangi tuman qo'shish
          </router-link>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Nomi</th>
              <th>Kodi</th>
              <th>Viloyat</th>
              <th class="actions-head">Amallar</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="district in districts" :key="district.id" @dblclick="startEdit(district.id)">
              <td>{{ district.id }}</td>
              <td style="font-weight: 500; color: #f1f5f9;">{{ district.fullName }}</td>
              <td>{{ district.code }}</td>
              <td>{{ regionName(district.regionId) }}</td>
              <td class="actions">
                <div class="action-dropdown-wrapper">
                  <button @click="toggleRow(district.id)" :class="['icon-btn', { expanded: expandedDistrictId === district.id }]" title="Amallarni ko'rsatish">
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                      <polyline points="6 9 12 15 18 9"></polyline>
                    </svg>
                  </button>
                  <div v-if="expandedDistrictId === district.id" class="action-dropdown">
                    <button @click="startEdit(district.id)" class="dropdown-btn">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                        <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                      </svg>
                      Tahrirlash
                    </button>
                    <button @click="deleteDistrictRow(district.id)" class="dropdown-btn danger">
                      <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
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
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getDistricts, getRegions, deleteDistrict, extractApiData, getField } from '../api';

export default {
  name: 'DistrictsView',
  setup() {
    const router = useRouter();
    const districts = ref([]);
    const regions = ref([]);
    const expandedDistrictId = ref(null);

    const loadData = async () => {
      try {
        const [distResp, regResp] = await Promise.all([
          getDistricts(),
          getRegions()
        ]);
        
        const distResult = extractApiData(distResp);
        districts.value = (Array.isArray(distResult) ? distResult : []).map(item => ({
          id: getField(item, ['id', 'Id']),
          fullName: getField(item, ['fullName', 'FullName', 'name', 'Name']),
          code: getField(item, ['code', 'Code']),
          regionId: getField(item, ['regionId', 'RegionId'])
        }));

        const regResult = extractApiData(regResp);
        regions.value = (Array.isArray(regResult) ? regResult : []).map(item => ({
          id: getField(item, ['id', 'Id']),
          fullName: getField(item, ['fullName', 'FullName', 'name', 'Name'])
        }));
      } catch (error) {
        console.error('Error loading data:', error);
      }
    };

    const regionName = (id) => {
      const reg = regions.value.find(r => r.id === id);
      return reg ? reg.fullName : id || '-';
    };

    const startEdit = (id) => {
      router.push(`/districts/edit/${id}`);
      expandedDistrictId.value = null;
    };

    const deleteDistrictRow = async (id) => {
      if (confirm('Rostdan ham ushbu tumanni o\'chirmoqchimisiz?')) {
        try {
          await deleteDistrict(id);
          await loadData();
        } catch (error) {
          console.error('Error deleting district:', error);
        }
      }
    };

    const toggleRow = (id) => {
      expandedDistrictId.value = expandedDistrictId.value === id ? null : id;
    };

    onMounted(loadData);

    return { districts, expandedDistrictId, startEdit, deleteDistrictRow, toggleRow, regionName };
  }
};
</script>

<style scoped>
.districts-page { background: #0d1117 !important; min-height: 100%; width: 100%; box-sizing: border-box; }
.page-card { background: #111827; padding: 24px; border-radius: 12px; border: 1px solid rgba(255, 255, 255, 0.07); width: 100%; box-sizing: border-box; }
.wide-card { max-width: none; margin: 0; }
.section-header { margin-bottom: 1.5rem; }
.section-header h2 { color: #f1f5f9; margin: 0; }
.section-header p { color: #94a3b8; margin: 4px 0 0; }
.section-actions { display: flex; flex-wrap: wrap; justify-content: space-between; gap: 1rem; align-items: center; margin-bottom: 1rem; }
.user-count { color: #94a3b8; font-weight: 600; }
.data-panel { background: #111827; border-radius: 0.5rem; border: 1px solid rgba(255, 255, 255, 0.07); overflow: hidden; }
.table-header { display: flex; align-items: center; justify-content: space-between; padding: 12px 16px; border-bottom: 1px solid rgba(255, 255, 255, 0.07); }
.table-header h3 { margin: 0; color: #f1f5f9; font-size: 15px; }
.btn-primary { background: #3b82f6; color: #fff; border: none; border-radius: 8px; padding: 7px 14px; font-size: 13px; display: flex; align-items: center; gap: 6px; cursor: pointer; text-decoration: none; font-weight: 600; transition: background 0.2s; }
.btn-primary:hover { background: #2563eb; }
th { text-align: left; padding: 0.75rem; background-color: #0f172a; color: #475569; font-size: 11px; text-transform: uppercase; letter-spacing: 0.6px; border-bottom: 1px solid rgba(255, 255, 255, 0.07); font-weight: 600; }
td { padding: 0.75rem; border-bottom: 1px solid rgba(255, 255, 255, 0.07); color: #94a3b8; vertical-align: middle; font-size: 13.5px; }
tbody tr:hover { background-color: rgba(255, 255, 255, 0.03); cursor: pointer; }
.action-dropdown-wrapper { position: relative; display: inline-block; }
.action-dropdown { position: absolute; top: 100%; right: 0; margin-top: 0.3rem; background: #1e293b; border: 1px solid rgba(255, 255, 255, 0.1); border-radius: 0.5rem; box-shadow: 0 8px 24px rgba(0, 0, 0, 0.4); z-index: 100; display: flex; gap: 0.25rem; padding: 0.25rem; white-space: nowrap; }
.dropdown-btn { padding: 0.4rem 0.6rem; font-size: 0.75rem; display: flex; align-items: center; gap: 0.4rem; background: transparent; border: 1px solid transparent; color: #f1f5f9; border-radius: 0.35rem; cursor: pointer; }
.dropdown-btn:hover { background: rgba(59, 130, 246, 0.15); border-color: rgba(59, 130, 246, 0.3); }
.dropdown-btn.danger { color: #ef4444; }
.dropdown-btn.danger:hover { background: rgba(239, 68, 68, 0.1); border-color: rgba(239, 68, 68, 0.2); }
.icon-btn { border: none; background: #2563eb; color: white; padding: 0.5rem; border-radius: 0.4rem; cursor: pointer; display: flex; align-items: center; justify-content: center; transition: background 0.2s ease; }
.icon-btn.expanded { background: #1d4ed8; }
.icon-btn:hover { background: #1d4ed8; }
</style>