<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Foydalanuvchiga rol biriktirish</h2>
        <p>Foydalanuvchiga tegishli rolni shu yerda tanlang.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="assignUserRole">
      <div class="form-grid">
        <label>
          <span class="required-star">*</span> Foydalanuvchi
          <select v-model.number="roleAssignment.userId" required>
            <option value="0" disabled>Tanlang</option>
            <option v-for="user in users" :key="user.id" :value="user.id">
              {{ user.userName }} - {{ user.fullName }}
            </option>
          </select>
          <small v-if="users.length === 0" class="warning">Foydalanuvchi topilmadi</small>
        </label>

        <label>
          <span class="required-star">*</span> Rol
          <select v-model.number="roleAssignment.roleId" required>
            <option value="0" disabled>Tanlang</option>
            <option v-for="role in roles" :key="role.id" :value="role.id">
              {{ role.name }}
            </option>
          </select>
          <small v-if="roles.length === 0" class="warning">Rol topilmadi</small>
        </label>
      </div>

      <button type="submit">Rolni biriktirish</button>
      <p v-if="assignError" class="error">{{ assignError }}</p>
      <p v-if="assignSuccess" class="success">{{ assignSuccess }}</p>
    </form>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { getUsers, getRoles, assignUserRole as assignUserRoleAPI, getMe } from '../api';

export default {
  setup() {
    const users = ref([]);
    const roles = ref([]);
    const assignError = ref('');
    const assignSuccess = ref('');
    const roleAssignment = ref({
      userId: 0,
      roleId: 0
    });

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) {
          return obj[key];
        }
      }
      return null;
    };

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
        console.log('Raw users from API:', rawUsers);
        users.value = rawUsers.map((user) => ({
          id: getField(user, ['id', 'Id', 'userId', 'userID']),
          userName: getField(user, ['userName', 'UserName', 'username', 'Username']),
          fullName: getField(user, ['fullName', 'FullName'])
        }));

        // Fetch current user and add if not present
        try {
          const meResponse = await getMe();
          const currentUser = meResponse.data;
          const currentUserMapped = {
            id: getField(currentUser, ['id', 'Id', 'userId', 'userID']),
            userName: getField(currentUser, ['userName', 'UserName', 'username', 'Username']),
            fullName: getField(currentUser, ['fullName', 'FullName'])
          };
          const exists = users.value.some(u => u.id === currentUserMapped.id);
          if (!exists && currentUserMapped.id) {
            users.value.push(currentUserMapped);
          }
        } catch (meError) {
          console.error('Error fetching current user:', meError);
        }

        console.log('Loaded users count:', users.value.length);
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
        console.log('Raw roles from API:', rawRoles);
        roles.value = rawRoles.map((role) => ({
          id: getField(role, ['id', 'Id', 'roleId', 'RoleId']),
          name: getField(role, ['name', 'Name', 'roleName', 'RoleName', 'title', 'Title']) || ''
        }));
        console.log('Mapped roles:', roles.value);
      } catch (error) {
        console.error('Load roles error:', error);
      }
    };

    const assignUserRole = async () => {
      assignError.value = '';
      assignSuccess.value = '';
      if (!roleAssignment.value.userId) {
        assignError.value = 'Foydalanuvchi tanlang.';
        return;
      }
      if (!roleAssignment.value.roleId) {
        assignError.value = 'Rol tanlang.';
        return;
      }
      try {
        await assignUserRoleAPI({
          userId: roleAssignment.value.userId,
          roleId: roleAssignment.value.roleId
        });
        assignSuccess.value = 'Role muvaffaqiyatli biriktirildi.';
        roleAssignment.value.userId = 0;
        roleAssignment.value.roleId = 0;
      } catch (error) {
        console.error('Assign role error:', error.response?.data || error.message);
        assignError.value = error.response?.data?.message || 'Role biriktirishda xatolik yuz berdi.';
      }
    };

    onMounted(async () => {
      await loadUsers();
      await loadRoles();
    });

    return {
      users,
      roles,
      roleAssignment,
      assignError,
      assignSuccess,
      assignUserRole
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
  max-width: 1200px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 1rem;
}

label {
  display: block;
  font-weight: 600;
  color: #0f172a;
}

label small {
  display: block;
  margin-top: 0.35rem;
  font-weight: 400;
  font-size: 0.8rem;
}

label small.warning {
  color: #ea580c;
}

.required-star {
  color: #dc2626;
  margin-right: 0.25rem;
}

input,
select {
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
  margin-top: 1rem;
  width: 100%;
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
