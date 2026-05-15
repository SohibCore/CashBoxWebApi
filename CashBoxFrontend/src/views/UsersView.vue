<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Foydalanuvchilar</h2>
        <p>Yangi foydalanuvchi qo'shish va mavjudlarini boshqarish.</p>
      </div>
    </div>

    <div class="section-actions">
      <button type="button" class="toggle-create" @click="toggleCreateForm">
        <span>+</span> Yangi foydalanuvchi
      </button>
      <span class="user-count">{{ users.length }} ta foydalanuvchi</span>
    </div>

    <form v-if="showCreateForm" class="entity-form" @submit.prevent="createNewUser">
      <h3>Yangi foydalanuvchi</h3>
      <div class="form-grid">
        <label>Login nomi<input v-model="newUser.userName" required /></label>
        <label>Email<input type="email" v-model="newUser.email" required /></label>
        <label>Parol<input type="password" v-model="newUser.password" required /></label>
        <label>To‘liq ism<input v-model="newUser.fullName" required /></label>
        <label>Qisqa nom<input v-model="newUser.shortName" required /></label>
        <label>PINFL<input v-model="newUser.pinfl" required maxlength="14" /></label>
        <label>Telefon<input v-model="newUser.phoneNumber" required maxlength="9" /></label>
        <label>Manzil<input v-model="newUser.address" required /></label>
        <label>Tashkilot<select v-model.number="newUser.organizationId" required>
            <option value="0" disabled>Tanlang</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">{{ org.shortName }}</option>
          </select></label>
        <label>Tug‘ilgan sana<input type="text" v-model="newUser.dateOfBirth" placeholder="dd.MM.yyyy" required /></label>
        <label>Passport seriya<input v-model="newUser.passportSeries" required maxlength="9" /></label>
      </div>
      <button type="submit">Saqlash</button>
      <p v-if="createError" class="error">{{ createError }}</p>
    </form>

    <div class="data-panel">
      <h3>Foydalanuvchi ro‘yxati</h3>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Login</th>
            <th>F.I.O.</th>
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
            <tr>
              <td>{{ user.id || '-' }}</td>
              <td>{{ user.userName || '-' }}</td>
              <td>{{ user.fullName || '-' }}</td>
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

  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getUsers, createUser, updateUser, deleteUser, getOrganizations } from '../api';

export default {
  setup() {
    const router = useRouter();
    const users = ref([]);
    const organizations = ref([]);
    const createError = ref('');
    const showCreateForm = ref(false);
    const expandedUserId = ref(null);
    const newUser = ref({
      userName: '',
      email: '',
      password: '',
      fullName: '',
      shortName: '',
      pinfl: '',
      phoneNumber: '',
      address: '',
      organizationId: 0,
      dateOfBirth: '',
      passportSeries: ''
    });

    const toggleCreateForm = () => {
      showCreateForm.value = !showCreateForm.value;
    };

    const toggleRow = (id) => {
      expandedUserId.value = expandedUserId.value === id ? null : id;
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
        console.log('Organizations response:', response.data);
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
          shortName: getField(org, ['shortName', 'ShortName']) || getField(org, ['fullName', 'FullName']) || '',
          fullName: getField(org, ['fullName', 'FullName']) || '',
          inn: getField(org, ['inn', 'Inn']) || '',
          regionId: getField(org, ['regionId', 'RegionId']) || 0,
          district: getField(org, ['district', 'District']) || ''
        }));
        console.log('Loaded organizations:', organizations.value);
      } catch (error) {
        console.error('Load organizations error:', error);
      }
    };


    const createNewUser = async () => {
      createError.value = '';
      const payload = { ...newUser.value };
      console.log('Payload being sent:', payload);
      if (payload.organizationId === 0) {
        createError.value = 'Tashkilot tanlang.';
        return;
      }
      if (!payload.dateOfBirth || payload.dateOfBirth.length !== 10 || !payload.dateOfBirth.includes('.')) {
        createError.value = 'Tug‘ilgan sanani dd.MM.yyyy formatida kiriting.';
        return;
      }
      try {
        await createUser(payload);
        loadUsers();
        Object.assign(newUser.value, {
          userName: '', email: '', password: '', fullName: '', shortName: '', pinfl: '', phoneNumber: '', address: '', organizationId: 0, dateOfBirth: '', passportSeries: ''
        });
      } catch (error) {
        console.error('Create user error:', error.response?.data || error.message);
        if (error.response?.data?.errors) {
          const messages = Object.values(error.response.data.errors).flat().join(' ');
          createError.value = messages;
        } else {
          createError.value = error.response?.data?.message || error.response?.statusText || 'Foydalanuvchini yaratishda xatolik yuz berdi.';
        }
      }
    };

    const startEdit = (user) => {
      router.push(`/users/edit/${user.id}`);
    };

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
      newUser,
      createError,
      showCreateForm,
      expandedUserId,
      createNewUser,
      startEdit,
      deleteRow,
      toggleCreateForm,
      toggleRow,
      organizationName,
      formatDate
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
}

.user-count {
  color: #475569;
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

.icon-btn.expanded svg {
  transform: rotate(180deg);
}

.expanded-row td {
  background: #eff6ff;
}

.expanded-actions {
  padding: 0.75rem 1rem;
  display: flex;
  justify-content: flex-end;
  background: #f8fafc;
}

.action-card {
  display: inline-flex;
  gap: 0.75rem;
  flex-wrap: wrap;
  align-items: center;
  background: white;
  padding: 0.5rem 0.75rem;
  border-radius: 0.75rem;
  border: 1px solid #e2e8f0;
  box-shadow: 0 3px 8px rgba(15, 23, 42, 0.08);
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
  background: white;
  border-radius: 1rem;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

label {
  display: block;
  font-weight: 600;
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
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
}

table {
  width: 100%;
  min-width: 1200px;
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
