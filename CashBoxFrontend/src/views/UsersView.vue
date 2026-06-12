<template>
  <div class="page-card wide-card">
    <NavigationHistory />
    
    <div class="section-header">
      <div>
        <h2>Foydalanuvchilar</h2>
        <p>Yangi foydalanuvchi qo'shish va mavjudlarini boshqarish.</p>
      </div>
    </div>

    <div class="section-actions">
      <span class="user-count">{{ users.length }} ta foydalanuvchi</span>
    </div>

    <Transition name="fade" mode="out-in">
      <div class="data-panel" :key="users.length">
        <div class="table-header">
          <h3>Foydalanuvchi ro‘yxati</h3>
          <router-link to="/users/new" class="btn-primary">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3">
              <line x1="12" y1="5" x2="12" y2="19"></line>
              <line x1="5" y1="12" x2="19" y2="12"></line>
            </svg>
            Yangi foydalanuvchi qo'shish
          </router-link>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Login</th>
              <th>F.I.O.</th>
              <th>Email</th>
              <th>Qisqa nom</th>
              <th>PINFL</th>
              <th>Telefon</th>
              <th>Manzil</th>
              <th>Tug‘ilgan sana</th>
              <th>Passport seriya</th>
              <th>Tashkilot</th>
              <th>Amallar</th>
            </tr>
          </thead>
          <tbody>
            <template v-for="user in users" :key="user.id">
              <tr @dblclick="startEdit(user)" style="cursor: pointer;">
                <td>{{ user.id || '-' }}</td>
                <td>{{ user.userName || '-' }}</td>
                <td>{{ user.fullName || '-' }}</td>
                <td>{{ user.email || '-' }}</td>
                <td>{{ user.shortName || '-' }}</td>
                <td>{{ user.pinfl || '-' }}</td>
                <td>{{ user.phoneNumber || '-' }}</td>
                <td>{{ user.address || '-' }}</td>
                <td>{{ formatDate(user.dateOfBirth) || '-' }}</td>
                <td>{{ user.passportSeries || '-' }}</td>
                <td>{{ organizationName(user.organizationId) }}</td>
                <td class="actions">
                  <div class="action-dropdown-wrapper">
                    <button @click="toggleRow(user.id)" :class="['icon-btn', { expanded: expandedUserId === user.id }]" title="Amallarni ko'rsatish">
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                        <polyline points="6 9 12 15 18 9"></polyline>
                      </svg>
                    </button>
                    <div v-if="expandedUserId === user.id" class="action-dropdown">
                      <button @click="startEdit(user)" class="dropdown-btn">
                        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                          <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                          <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                        </svg>
                        Tahrirlash
                      </button>
                      <button @click="goToRoleAssign(user)" class="dropdown-btn">
                        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                          <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path>
                          <circle cx="9" cy="7" r="4"></circle>
                          <path d="M23 21v-2a4 4 0 0 0-3-3.87"></path>
                          <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
                        </svg>
                        Rol biriktirish
                      </button>
                      <button @click="deleteRow(user.id)" class="dropdown-btn danger">
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
            </template>
          </tbody>
        </table>
      </div>
    </Transition>

    <!-- Foydalanuvchi boshqaruv modali -->
    <div v-if="isModalOpen" class="modal-overlay" @click.self="closeModal">
      <div class="modal-content wide-modal">
        <div class="modal-header">
          <h3>{{ modalUser.fullName || modalUser.userName }} - Boshqarish</h3>
          <button class="close-btn" @click="closeModal">&times;</button>
        </div>
        
        <div class="modal-tabs">
          <button :class="['tab-item', { active: activeTab === 'edit' }]" @click="activeTab = 'edit'">
            Tahrirlash
          </button>
          <button :class="['tab-item', { active: activeTab === 'roles' }]" @click="activeTab = 'roles'">
            Rollar
          </button>
        </div>

        <div class="modal-body">
          <!-- Tahrirlash tabi -->
          <div v-if="activeTab === 'edit'" class="tab-pane">
            <form @submit.prevent="saveUserChanges" class="modal-form">
              <div class="form-grid">
                <label>Login nomi <input v-model="modalUser.userName" required /></label>
                <label>Email <input type="email" v-model="modalUser.email" required /></label>
                <label>To'liq ism <input v-model="modalUser.fullName" required /></label>
                <label>PINFL <input v-model="modalUser.pinfl" maxlength="14" /></label>
                <label>Telefon <input v-model="modalUser.phoneNumber" maxlength="9" /></label>
                <label>Passport <input v-model="modalUser.passportSeries" maxlength="9" /></label>
                <label>Tashkilot 
                  <select v-model.number="modalUser.organizationId">
                    <option v-for="org in organizations" :key="org.id" :value="org.id">{{ org.shortName }}</option>
                  </select>
                </label>
                <label>Tug'ilgan sana <input v-model="modalUser.dateOfBirth" placeholder="dd.MM.yyyy" /></label>
              </div>
              <div class="modal-footer">
                <button type="submit" class="save-btn" :disabled="isSaving">Saqlash</button>
              </div>
            </form>
          </div>

          <!-- Rollar tabi -->
          <div v-if="activeTab === 'roles'" class="tab-pane">
            <div class="role-management">
              <h4>Biriktirilgan rollar:</h4>
              <div class="role-chips">
                <span v-for="roleId in userRoleIds" :key="roleId" class="role-chip">
                  {{ getRoleName(roleId) }}
                  <button @click="removeRole(roleId)" class="remove-role">&times;</button>
                </span>
                <span v-if="userRoleIds.length === 0" class="empty-text">Rollari yo'q</span>
              </div>
              <hr />
              <h4>Mavjud rollar:</h4>
              <div class="available-roles">
                <button 
                  v-for="role in filteredAvailableRoles" 
                  :key="role.id" 
                  @click="addRole(role.id)"
                  class="add-role-btn"
                >
                  + {{ role.name }}
                </button>
              </div>
            </div>
          </div>
        </div>
        
        <div v-if="modalError" class="error-msg">{{ modalError }}</div>
        <div v-if="modalSuccess" class="success-msg">{{ modalSuccess }}</div>
      </div>
    </div>

  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import NavigationHistory from './NavigationHistory.vue';
import { 
  getUsers, 
  deleteUser, 
  getOrganizations, 
  updateUser, 
  getRoles, 
  getUserRoles, 
  assignUserRoles, 
  removeUserRole,
  extractApiData,
  getField,
  getMe,
  normalizeUser
} from '../api';

export default {
  setup() {
    const router = useRouter();
    const users = ref([]);
    const organizations = ref([]);
    const expandedUserId = ref(null);

    // Modal holati
    const isModalOpen = ref(false);
    const activeTab = ref('edit'); // 'edit' yoki 'roles'
    const modalUser = ref({});
    const allRoles = ref([]);
    const userRoleIds = ref([]);
    const isSaving = ref(false);
    const modalError = ref('');
    const modalSuccess = ref('');

    const toggleRow = (id) => {
      expandedUserId.value = expandedUserId.value === id ? null : id;
    };

    const loadUsers = async () => {
      try {
        const response = await getUsers();
        const result = extractApiData(response);
        const rawUsers = Array.isArray(result) ? result : [];

        users.value = rawUsers.map((user) => ({
          id: getField(user, ['id', 'Id', 'userId', 'userID']),
          userName: getField(user, ['userName', 'UserName', 'username', 'Username']),
          email: getField(user, ['email', 'Email']),
          fullName: getField(user, ['fullName', 'FullName']),
          shortName: getField(user, ['shortName', 'ShortName']),
          pinfl: getField(user, ['pinfl', 'Pinfl']),
          phoneNumber: getField(user, ['phoneNumber', 'PhoneNumber']),
          address: getField(user, ['address', 'Address']),
          dateOfBirth: getField(user, ['dateOfBirth', 'DateOfBirth']),
          passportSeries: getField(user, ['passportSeries', 'PassportSeries']),
          organizationId: getField(user, ['organizationId', 'OrganizationId']),
          role: getField(user, ['role', 'Role']) || 'user'
        }));
      } catch (error) {
        console.error('Load users error:', error);
      }
    };

    const loadOrganizations = async () => {
      try {
        const response = await getOrganizations();
        const result = extractApiData(response);
        const rawOrgs = Array.isArray(result) ? result : [];

        organizations.value = rawOrgs.map((org) => ({
          id: getField(org, ['id', 'Id', 'organizationId', 'OrganizationId']),
          shortName: getField(org, ['shortName', 'ShortName']) || getField(org, ['fullName', 'FullName']) || '',
          fullName: getField(org, ['fullName', 'FullName']) || '',
          inn: getField(org, ['inn', 'Inn']) || '',
          regionId: getField(org, ['regionId', 'RegionId']) || 0,
          district: getField(org, ['district', 'District']) || ''
        }));
      } catch (error) {
        console.error('Load organizations error:', error);
      }
    };

    const openModal = async (user, tab = 'edit') => {
      modalError.value = '';
      modalSuccess.value = '';
      activeTab.value = tab;
      modalUser.value = { ...user, dateOfBirth: formatDate(user.dateOfBirth) };
      isModalOpen.value = true;
      expandedUserId.value = null;

      // Rollarni yuklash
      if (allRoles.value.length === 0) {
        try {
          const resp = await getRoles();
          const data = resp.data?.data || resp.data || [];
          allRoles.value = data.map(r => ({
            id: getField(r, ['id', 'Id', 'roleId', 'RoleId']),
            name: getField(r, ['name', 'Name', 'roleName', 'RoleName', 'title', 'Title'])
          }));
        } catch (e) { console.error(e); }
      }

      // Tanlangan user rollarini yuklash
      try {
        const resp = await getUserRoles(user.id);
        const data = resp.data?.data || resp.data || [];
        userRoleIds.value = data.map(r => getField(r, ['roleId', 'RoleId'])).filter(id => id != null);
      } catch (e) { console.error(e); }
    };

    const closeModal = () => {
      isModalOpen.value = false;
    };

    const startEdit = (user) => {
      openModal(user, 'edit');
    };

    const goToRoleAssign = (user) => {
      router.push(`/user-role?userId=${user.id}&email=${encodeURIComponent(user.email || '')}`);
      expandedUserId.value = null;
    };

    const saveUserChanges = async () => {
      isSaving.value = true;
      modalError.value = '';
      const userId = modalUser.value.id;
      const payload = { ...modalUser.value };
      delete payload.id;

      try {
        await updateUser(userId, payload);
        modalSuccess.value = 'Muvaffaqiyatli saqlandi';
        await loadUsers();
        setTimeout(() => { closeModal(); }, 1000);
      } catch (e) {
        modalError.value = e.response?.data?.message || 'Xatolik yuz berdi';
      } finally {
        isSaving.value = false;
      }
    };

    const addRole = async (roleId) => {
      try {
        await assignUserRoles(modalUser.value.id, [roleId]);
        if (!userRoleIds.value.includes(roleId)) {
          userRoleIds.value.push(roleId);
        }
      } catch (e) {
        modalError.value = "Rol biriktirishda xatolik";
      }
    };

    const removeRole = async (roleId) => {
      try {
        await removeUserRole(modalUser.value.id, roleId);
        userRoleIds.value = userRoleIds.value.filter(id => id !== roleId);
      } catch (e) {
        modalError.value = "Rolni o'chirishda xatolik";
      }
    };

    const getRoleName = (id) => {
      const role = allRoles.value.find(r => r.id === id);
      return role ? role.name : 'Noma\'lum';
    };

    const filteredAvailableRoles = computed(() => {
      return allRoles.value.filter(role => !userRoleIds.value.includes(role.id));
    });

    const deleteRow = async (id) => {
      try {
        await deleteUser(id);
        loadUsers();
      } catch (error) {
        console.error(error);
      }
    };

    const organizationName = (id) => {
      const org = organizations.value.find((item) => item.id === id);
      return org ? org.shortName : '-';
    };

    const formatDate = (dateStr) => {
      if (!dateStr) return '';
      // Agar dd.MM.yyyy formatida bo'lsa, uni parse qilish
      if (dateStr.includes('.')) {
        const parts = dateStr.split('.');
        if (parts.length === 3) {
          return dateStr; // Allaqachon dd.MM.yyyy
        }
      }
      // Aks holda, Date object dan formatlash
      const date = new Date(dateStr);
      if (isNaN(date.getTime())) return dateStr; // Agar parse bo'lmasa, asl qiymatni qaytarish
      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const year = date.getFullYear();
      return `${day}.${month}.${year}`;
    };

    onMounted(async () => {
      await loadOrganizations();
      await loadUsers();
    });

    return {
      users,
      organizations,
      expandedUserId,
      startEdit,
      deleteRow,
      toggleRow,
      organizationName,
      formatDate,
      goToRoleAssign,
      isModalOpen,
      activeTab,
      modalUser,
      closeModal,
      saveUserChanges,
      userRoleIds,
      addRole,
      removeRole,
      getRoleName,
      filteredAvailableRoles,
      isSaving,
      modalError,
      modalSuccess,
      NavigationHistory
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

.wide-card {
  max-width: none !important;
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

.entity-form {
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
  font-size: 0.95rem; /* Inherit o'rniga aniq qiymat */
  font-family: inherit;
  height: 38px; /* Chiqim hujjatlaridagi tugma balandligi */
  justify-content: center; /* Kontentni vertikal markazlash */
  font-weight: 600; /* Qalinroq matn */
}

.toggle-create:hover {
  background: #1d4ed8;
  color: white;
}

.user-count {
  color: #94a3b8;
  font-weight: 600;
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
  display: flex; /* Flexbox xususiyatini saqlab qolamiz */
  gap: 0.25rem; /* Bo'shliqni kamaytiramiz */
  background: #1e293b;
  padding: 0.25rem; /* Paddingni kamaytiramiz */
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
  border: 1px solid transparent;
  background: transparent;
  color: #f1f5f9;
  padding: 0.3rem 0.5rem; /* Paddingni kamaytiramiz */
  font-size: 0.72rem; /* Font hajmini kichiklashtiramiz */
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

.icon-btn.expanded svg {
  transform: rotate(180deg);
}

.expanded-actions {
  padding: 0.75rem 1rem;
  display: flex;
  justify-content: flex-end;
  background: rgba(255, 255, 255, 0.02);
}

.action-card {
  display: inline-flex;
  gap: 0.75rem;
  flex-wrap: wrap;
  align-items: center;
  background: #1e293b;
  padding: 0.5rem 0.75rem;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.3);
}

.small-action {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
  border: 1px solid transparent;
  background: #ffffff;
  color: #0f172a;
  padding: 0.4rem 0.7rem;
  font-size: 0.82rem;
  border-radius: 0.55rem;
  cursor: pointer;
  transition: background 0.2s ease, border-color 0.2s ease;
}

.small-action svg {
  width: 14px;
  height: 14px;
}

.small-action:hover {
  background: #eff6ff;
  border-color: #cbd5e1;
}

.small-action.danger {
  color: #dc2626;
}

.small-action.danger:hover {
  background: #fee2e2;
}

.section-card {
  background: #111827;
  border-radius: 1rem;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
}

label {
  display: block;
  font-weight: 600;
  color: #94a3b8;
}

input,
select {
  width: 100%;
  padding: 0.75rem;
  margin-top: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.7rem;
  font-size: 0.95rem;
  outline: none;
}

button {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  cursor: pointer;
}

.success {
  margin-top: 1rem;
  color: #16a34a;
  font-size: 0.95rem;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

.data-panel {
  width: 100%; /* Ensure it takes full width */
  overflow-x: visible; /* Allow content to overflow if necessary, but text will wrap */
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
  min-width: 0; /* Allow table to shrink if content wraps */
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

tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.03);
  cursor: pointer;
}

tbody td {
  padding: 0.9rem 0.8rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  color: #94a3b8;
  word-break: break-word; /* Matnni o'rashga ruxsat berish */
  font-size: 13.5px;
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

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background: #111827;
  border-radius: 1rem;
  padding: 2rem;
  max-width: 800px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  position: relative;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.5);
  border: 1px solid rgba(255, 255, 255, 0.07);
}

.modal-header {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1.5rem;
}

.close-btn {
  background: none;
  border: none;
  font-size: 2rem;
  cursor: pointer;
  color: #64748b;
}

.modal-tabs {
  display: flex;
  gap: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  margin-bottom: 1.5rem;
}

.tab-item {
  padding: 0.75rem 1.5rem;
  background: none;
  border: none;
  color: #64748b;
  font-weight: 600;
  cursor: pointer;
  border-bottom: 3px solid transparent;
}

.tab-item.active {
  color: #2563eb;
  border-bottom-color: #2563eb;
}

.modal-footer {
  margin-top: 2rem;
  display: flex;
  justify-content: flex-end;
}

.save-btn {
  background: #2563eb;
  padding: 0.8rem 2rem;
  font-weight: 600;
}

/* Role management inside modal */
.role-chips {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin: 1rem 0;
}

.role-chip {
  background: #dbeafe;
  color: #1e40af;
  padding: 0.4rem 0.8rem;
  border-radius: 2rem;
  font-size: 0.85rem;
  display: flex;
  align-items: center;
  gap: 0.4rem;
}

.remove-role {
  background: none;
  border: none;
  color: #ef4444;
  font-weight: bold;
  cursor: pointer;
}

.available-roles {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5rem;
  margin-top: 1rem;
}

.add-role-btn {
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  border: 1px solid rgba(255, 255, 255, 0.1);
  font-size: 0.8rem;
  padding: 0.4rem 0.8rem;
  border-radius: 0.5rem;
  cursor: pointer;
}

.error-msg { color: #dc2626; margin-top: 1rem; }
.success-msg { color: #16a34a; margin-top: 1rem; }

</style>
