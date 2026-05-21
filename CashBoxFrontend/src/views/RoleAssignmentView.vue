<template>
  <div class="page-card wide-card role-assignment-page">
    <div class="section-header">
      <div>
        <h2>Foydalanuvchiga rol biriktirish</h2>
        <p>Foydalanuvchiga bir yoki bir nechta rol tanlang.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent>
      <div class="form-grid">
        <label class="form-field">
          <span class="field-label"><span class="required-star">*</span> Foydalanuvchi</span>
          <div class="field-with-icon">
            <span class="field-icon" aria-hidden="true">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
            </span>
            <select v-model.number="selectedUserId" required @change="onUserChange">
              <option value="0" disabled>Tanlang</option>
              <option v-for="user in users" :key="user.id" :value="user.id">
                {{ user.userName }} - {{ user.fullName }}
              </option>
            </select>
          </div>
          <small v-if="users.length === 0" class="warning">Foydalanuvchi topilmadi</small>
        </label>

        <label class="form-field">
          <span class="field-label">Email</span>
          <div class="field-with-icon">
            <span class="field-icon" aria-hidden="true">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z" />
                <polyline points="22,6 12,13 2,6" />
              </svg>
            </span>
            <input 
              type="email" 
              :value="userEmail" 
              disabled 
              style="background: #f1f5f9; cursor: not-allowed;" 
            />
          </div>
        </label>

        <label class="form-field">
          <span class="field-label"><span class="required-star">*</span> Rol</span>
          <div class="role-picker" ref="rolePickerRef">
            <button
              type="button"
              class="role-picker-trigger"
              :disabled="!selectedUserId || roles.length === 0"
              :aria-expanded="roleDropdownOpen"
              @click="toggleRoleDropdown"
            >
              <span class="field-icon role-picker-icon" aria-hidden="true">
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M12 2l3.09 6.26L22 9.27l-5 4.87L18.18 22 12 18.56 5.82 22 7 14.14l-5-4.87 6.91-1.01L12 2z" />
                </svg>
              </span>
              <span v-if="selectedRoleIds.length === 0" class="role-placeholder">
                {{ selectedUserId ? 'Rol tanlang' : 'Avval foydalanuvchini tanlang' }}
              </span>
              <span v-else class="role-chips">
                <span v-for="role in selectedRoles" :key="role.id" class="role-chip">
                  {{ role.name }}
                  <button
                    type="button"
                    class="role-chip-remove"
                    :aria-label="`${role.name} rolini olib tashlash`"
                    :disabled="roleActionLoading"
                    @click.stop="removeRole(role.id)"
                  >
                    ×
                  </button>
                </span>
              </span>
              <span class="role-chevron" :class="{ open: roleDropdownOpen }">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <polyline points="6 9 12 15 18 9" />
                </svg>
              </span>
            </button>

            <div v-if="roleDropdownOpen" class="role-dropdown">
              <p v-if="availableRoles.length === 0" class="role-dropdown-empty">Barcha rollar tanlangan</p>
              <button
                v-for="role in availableRoles"
                :key="role.id"
                type="button"
                class="role-dropdown-item"
                :disabled="roleActionLoading"
                @click="addRole(role)"
              >
                {{ role.name }}
              </button>
            </div>
          </div>
          <small v-if="roles.length === 0" class="warning">Rol topilmadi</small>
          <small v-else-if="loadingUserRoles" class="hint">Rollar yuklanmoqda...</small>
        </label>
      </div>

      <p v-if="assignError" class="error">{{ assignError }}</p>
      <p v-if="assignSuccess" class="success">{{ assignSuccess }}</p>
    </form>
  </div>
</template>

<script>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue';
import { useRoute } from 'vue-router';
import { getUsers, getRoles, assignUserRoles, removeUserRole, getUserRoles, getMe, extractApiData } from '../api';

export default {
  setup() {
    const route = useRoute();
    const users = ref([]);
    const roles = ref([]);
    const selectedUserId = ref(Number(route.query.userId) || 0);
    const userEmail = ref(route.query.email || '');
    const selectedRoleIds = ref([]);
    const roleDropdownOpen = ref(false);
    const rolePickerRef = ref(null);
    const assignError = ref('');
    const assignSuccess = ref('');
    const loadingUserRoles = ref(false);
    const roleActionLoading = ref(false);

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) {
          return obj[key];
        }
      }
      return null;
    };

    const selectedRoles = computed(() =>
      selectedRoleIds.value
        .map((id) => roles.value.find((r) => r.id === id))
        .filter(Boolean)
    );

    const availableRoles = computed(() =>
      roles.value.filter((role) => !selectedRoleIds.value.includes(role.id))
    );

    const loadUsers = async () => {
      try {
        const response = await getUsers();
        const result = response.data;
        const rawUsers = Array.isArray(result)
          ? result
          : Array.isArray(result?.data)
          ? result.data
          : Array.isArray(result?.items)
          ? result.items
          : Array.isArray(result?.value)
          ? result.value
          : [];
        users.value = rawUsers.map((user) => ({
          id: getField(user, ['id', 'Id', 'userId', 'userID']),
          userName: getField(user, ['userName', 'UserName', 'username', 'Username']),
          fullName: getField(user, ['fullName', 'FullName']),
          email: getField(user, ['email', 'Email'])
        }));

        try {
          const meResponse = await getMe();
          const currentUser = extractApiData(meResponse);
          const currentUserMapped = {
            id: getField(currentUser, ['id', 'Id', 'userId', 'userID']),
            userName: getField(currentUser, ['userName', 'UserName', 'username', 'Username']),
            fullName: getField(currentUser, ['fullName', 'FullName']),
            email: getField(currentUser, ['email', 'Email'])
          };
          const exists = users.value.some((u) => u.id === currentUserMapped.id);
          if (!exists && currentUserMapped.id) {
            users.value.push(currentUserMapped);
          }
        } catch (meError) {
          console.error('Error fetching current user:', meError);
        }
      } catch (error) {
        console.error('Load users error:', error);
      }
    };

    const loadRoles = async () => {
      try {
        const response = await getRoles();
        const result = response.data;
        const rawRoles = Array.isArray(result)
          ? result
          : Array.isArray(result?.data)
          ? result.data
          : Array.isArray(result?.items)
          ? result.items
          : Array.isArray(result?.value)
          ? result.value
          : [];
        roles.value = rawRoles.map((role) => ({
          id: getField(role, ['id', 'Id', 'roleId', 'RoleId']),
          name: getField(role, ['name', 'Name', 'roleName', 'RoleName', 'title', 'Title']) || ''
        }));
      } catch (error) {
        console.error('Load roles error:', error);
      }
    };

    const loadUserRoles = async (userId) => {
      if (!userId) {
        selectedRoleIds.value = [];
        return;
      }
      loadingUserRoles.value = true;
      assignError.value = '';
      try {
        const response = await getUserRoles(userId);
        const data = extractApiData(response);
        const rawList = Array.isArray(data) ? data : [];
        const roleIds = rawList
          .map((item) => getField(item, ['roleId', 'RoleId']))
          .filter((id) => id != null);
        selectedRoleIds.value = [...new Set(roleIds)];
      } catch (error) {
        console.error('Load user roles error:', error);
        assignError.value = 'Foydalanuvchi rollarini yuklashda xatolik yuz berdi.';
        selectedRoleIds.value = [];
      } finally {
        loadingUserRoles.value = false;
      }
    };

    const onUserChange = async () => {
      assignSuccess.value = '';
      assignError.value = '';
      roleDropdownOpen.value = false;
      const found = users.value.find(u => u.id === selectedUserId.value);
      if (found) userEmail.value = found.email || '';
      await loadUserRoles(selectedUserId.value);
    };

    const toggleRoleDropdown = () => {
      if (!selectedUserId.value || roles.value.length === 0) return;
      roleDropdownOpen.value = !roleDropdownOpen.value;
    };

    const closeRoleDropdown = () => {
      roleDropdownOpen.value = false;
    };

    const handleClickOutside = (event) => {
      if (rolePickerRef.value && !rolePickerRef.value.contains(event.target)) {
        closeRoleDropdown();
      }
    };

    const addRole = async (role) => {
      if (!selectedUserId.value || selectedRoleIds.value.includes(role.id)) return;
      assignError.value = '';
      assignSuccess.value = '';
      roleActionLoading.value = true;
      try {
        await assignUserRoles(selectedUserId.value, [role.id]);
        selectedRoleIds.value = [...selectedRoleIds.value, role.id];
        assignSuccess.value = `"${role.name}" roli biriktirildi.`;
        if (availableRoles.value.length === 0) {
          closeRoleDropdown();
        }
      } catch (error) {
        console.error('Assign role error:', error.response?.data || error.message);
        assignError.value = error.response?.data?.message || error.response?.data || 'Rol biriktirishda xatolik yuz berdi.';
      } finally {
        roleActionLoading.value = false;
      }
    };

    const removeRole = async (roleId) => {
      if (!selectedUserId.value) return;
      const role = roles.value.find((r) => r.id === roleId);
      assignError.value = '';
      assignSuccess.value = '';
      roleActionLoading.value = true;
      try {
        await removeUserRole(selectedUserId.value, roleId);
        selectedRoleIds.value = selectedRoleIds.value.filter((id) => id !== roleId);
        assignSuccess.value = role ? `"${role.name}" roli olib tashlandi.` : 'Rol olib tashlandi.';
      } catch (error) {
        console.error('Remove role error:', error.response?.data || error.message);
        assignError.value = error.response?.data?.message || error.response?.data || 'Rolni olib tashlashda xatolik yuz berdi.';
      } finally {
        roleActionLoading.value = false;
      }
    };

    onMounted(async () => {
      document.addEventListener('click', handleClickOutside);
      await loadUsers();
      await loadRoles();
      if (selectedUserId.value) {
        await loadUserRoles(selectedUserId.value);
        if (!userEmail.value) {
          const found = users.value.find(u => u.id === selectedUserId.value);
          if (found) userEmail.value = found.email || '';
        }
      }
    });

    onBeforeUnmount(() => {
      document.removeEventListener('click', handleClickOutside);
    });

    return {
      users,
      roles,
      selectedUserId,
      userEmail,
      selectedRoleIds,
      selectedRoles,
      availableRoles,
      roleDropdownOpen,
      rolePickerRef,
      assignError,
      assignSuccess,
      loadingUserRoles,
      roleActionLoading,
      onUserChange,
      toggleRoleDropdown,
      addRole,
      removeRole
    };
  }
};
</script>

<style scoped>
.role-assignment-page.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.role-assignment-page.wide-card {
  max-width: 1200px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 1rem;
  align-items: start;
}

@media (max-width: 720px) {
  .form-grid {
    grid-template-columns: 1fr;
  }
}

.form-field {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin: 0;
  font-weight: 600;
  color: #0f172a;
}

.field-label {
  display: block;
  line-height: 1.35;
}

.form-field small {
  display: block;
  margin: 0;
  font-weight: 400;
  font-size: 0.8rem;
}

.form-field small.warning {
  color: #ea580c;
}

.form-field small.hint {
  color: #64748b;
}

.required-star {
  color: #dc2626;
  margin-right: 0.25rem;
}

.field-with-icon,
.role-picker {
  position: relative;
  margin: 0;
}

.field-icon {
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 2rem;
  height: 2rem;
  border-radius: 0.5rem;
  background: #eff6ff;
  color: #2563eb;
}

.field-with-icon .field-icon {
  position: absolute;
  left: 0.65rem;
  top: 50%;
  transform: translateY(-50%);
  z-index: 1;
  pointer-events: none;
}

.field-with-icon select, .field-with-icon input {
  width: 100%;
  min-height: 3rem;
  padding: 0.75rem 2.25rem 0.75rem 3.15rem;
  margin: 0;
  border: 1px solid #cbd5e1;
  border-radius: 0.7rem;
  font-size: 0.95rem;
  font-weight: 400;
  background: #fff;
  box-sizing: border-box;
  appearance: none;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='16' height='16' viewBox='0 0 24 24' fill='none' stroke='%2364748b' stroke-width='2'%3E%3Cpolyline points='6 9 12 15 18 9'/%3E%3C/svg%3E");
  background-repeat: no-repeat;
  background-position: right 0.65rem center;
}

.role-picker-trigger {
  position: relative;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  width: 100%;
  min-height: 3rem;
  margin: 0;
  padding: 0.5rem 2.25rem 0.5rem 0.65rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.7rem;
  background: #fff;
  cursor: pointer;
  text-align: left;
  font: inherit;
  font-weight: 400;
  color: #0f172a;
}

.role-picker-trigger:hover:not(:disabled) {
  border-color: #94a3b8;
}

.role-picker-trigger:disabled {
  background: #f1f5f9;
  cursor: not-allowed;
  opacity: 0.85;
}

.role-placeholder {
  flex: 1;
  color: #94a3b8;
  font-weight: 400;
  font-size: 0.95rem;
}

.role-chips {
  flex: 1;
  display: flex;
  flex-wrap: wrap;
  gap: 0.35rem;
  align-items: center;
  min-width: 0;
}

.role-chip {
  display: inline-flex;
  align-items: center;
  gap: 0.2rem;
  max-width: 100%;
  padding: 0.2rem 0.35rem 0.2rem 0.55rem;
  background: #dbeafe;
  color: #1e40af;
  border-radius: 999px;
  font-size: 0.82rem;
  font-weight: 600;
  line-height: 1.3;
}

.role-chip-remove {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 1.25rem;
  height: 1.25rem;
  margin: 0;
  padding: 0;
  border: none;
  border-radius: 50%;
  background: transparent;
  color: #1e40af;
  font-size: 1.1rem;
  line-height: 1;
  cursor: pointer;
}

.role-chip-remove:hover:not(:disabled) {
  background: #bfdbfe;
  color: #1e3a8a;
}

.role-chip-remove:disabled {
  cursor: wait;
  opacity: 0.5;
}

.role-chevron {
  position: absolute;
  right: 0.65rem;
  top: 50%;
  transform: translateY(-50%);
  color: #64748b;
  pointer-events: none;
}

.role-chevron.open svg {
  transform: rotate(180deg);
}

.role-dropdown {
  position: absolute;
  z-index: 20;
  left: 0;
  right: 0;
  top: calc(100% + 0.35rem);
  max-height: 220px;
  overflow-y: auto;
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 0.65rem;
  box-shadow: 0 8px 24px rgba(15, 23, 42, 0.12);
}

.role-dropdown-empty {
  margin: 0;
  padding: 0.75rem 1rem;
  color: #64748b;
  font-size: 0.88rem;
  font-weight: 400;
}

.role-dropdown-item {
  display: block;
  width: 100%;
  margin: 0;
  padding: 0.65rem 1rem;
  border: none;
  border-bottom: 1px solid #f1f5f9;
  border-radius: 0;
  background: #fff;
  color: #0f172a;
  font-size: 0.92rem;
  font-weight: 400;
  text-align: left;
  cursor: pointer;
}

.role-dropdown-item:last-child {
  border-bottom: none;
}

.role-dropdown-item:hover:not(:disabled) {
  background: #eff6ff;
}

.role-dropdown-item:disabled {
  opacity: 0.6;
  cursor: wait;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
  font-size: 0.95rem;
}

.success {
  margin-top: 1rem;
  color: #16a34a;
  font-size: 0.95rem;
}
</style>
