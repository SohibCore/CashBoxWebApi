<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Foydalanuvchi tahrirlash</h2>
        <p>Foydalanuvchi ma'lumotlarini yangilash.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveUser">
      <div class="form-grid">
        <label>Login nomi<input v-model="user.userName" required /></label>
        <label>Parol<input type="password" v-model="user.password" /></label>
        <label>To‘liq ism<input v-model="user.fullName" required /></label>
        <label>Qisqa nom<input v-model="user.shortName" required /></label>
        <label>PINFL<input v-model="user.pinfl" required maxlength="14" /></label>
        <label>Telefon<input v-model="user.phoneNumber" required maxlength="9" /></label>
        <label>Manzil<input v-model="user.address" required /></label>
        <label>Tashkilot<select v-model.number="user.organizationId" required class="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500">
            <option value="0" disabled>Tanlang</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">{{ org.shortName }}</option>
          </select></label>
        <label>Tug‘ilgan sana<input type="text" v-model="user.dateOfBirth" placeholder="dd.MM.yyyy" required /></label>
        <label>Passport seriya<input v-model="user.passportSeries" required maxlength="9" /></label>        <label>Rol<select v-model="user.role" required>
            <option value="user">User</option>
            <option value="Admin">Admin</option>
          </select></label>      </div>
      <div class="button-row">
        <button type="submit">Saqlash</button>
        <button type="button" @click="cancel">Bekor qilish</button>
      </div>
    </form>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getUsers, updateUser, getOrganizations } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const user = ref({
      id: 0,
      userName: '',
      password: '',
      fullName: '',
      shortName: '',
      pinfl: '',
      phoneNumber: '',
      address: '',
      organizationId: 0,
      dateOfBirth: '',
      passportSeries: '',
      role: 'user'
    });
    const organizations = ref([]);

    const loadUser = async () => {
      try {
        const response = await getUsers();
        const result = response.data;
        const users = Array.isArray(result)
          ? result
          : Array.isArray(result?.data)
          ? result.data
          : Array.isArray(result?.items)
          ? result.items
          : Array.isArray(result?.value)
          ? result.value
          : [];
        const found = users.find(u => getField(u, ['id', 'Id', 'userId', 'userID']) == route.params.id);
        if (found) {
          user.value = {
            id: getField(found, ['id', 'Id', 'userId', 'userID']),
            userName: getField(found, ['userName', 'UserName', 'username', 'Username']) || '',
            email: getField(found, ['email', 'Email']) || '',
            fullName: getField(found, ['fullName', 'FullName']) || '',
            shortName: getField(found, ['shortName', 'ShortName']) || '',
            pinfl: getField(found, ['pinfl', 'Pinfl']) || '',
            phoneNumber: getField(found, ['phoneNumber', 'PhoneNumber']) || '',
            address: getField(found, ['address', 'Address']) || '',
            organizationId: getField(found, ['organizationId', 'OrganizationId']) || 0,
            dateOfBirth: parseDate(getField(found, ['dateOfBirth', 'DateOfBirth']) || ''),
            passportSeries: getField(found, ['passportSeries', 'PassportSeries']) || '',
            role: getField(found, ['role', 'Role']) || 'user',
            password: ''
          };
        }
      } catch (error) {
        console.error(error);
      }
    };

    const loadOrganizations = async () => {
      try {
        const response = await getOrganizations();
        const result = response.data;
        organizations.value = Array.isArray(result)
          ? result
          : Array.isArray(result?.data)
          ? result.data
          : Array.isArray(result?.items)
          ? result.items
          : Array.isArray(result?.value)
          ? result.value
          : [];
      } catch (error) {
        console.error(error);
      }
    };

    const saveUser = async () => {
      const payload = { ...user.value };
      delete payload.id;
      try {
        await updateUser(user.value.id, payload);
        router.push('/users');
      } catch (error) {
        console.error('Update user error:', error.response?.data || error.message);
        alert(
          error.response?.data?.errors
            ? Object.values(error.response.data.errors).flat().join(' ')
            : error.response?.data?.message || 'Foydalanuvchini yangilashda xatolik yuz berdi.'
        );
      }
    };

    const cancel = () => {
      router.push('/users');
    };

    const parseDate = (dateStr) => {
      if (!dateStr) return '';
      if (dateStr.includes('.')) {
        return dateStr;
      }
      const date = new Date(dateStr);
      if (isNaN(date.getTime())) return dateStr;
      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      return `${day}.${month}.${date.getFullYear()}`;
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

    onMounted(async () => {
      await loadOrganizations();
      await loadUser();
    });

    return {
      user,
      organizations,
      saveUser,
      cancel
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
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.entity-form {
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
  margin-bottom: 2rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1rem;
  margin-bottom: 1rem;
}

.form-grid label {
  display: flex;
  flex-direction: column;
  font-weight: 500;
}

.form-grid input,
.form-grid select {
  padding: 0.5rem;
  border: 1px solid #d1d5db;
  border-radius: 0.25rem;
  margin-top: 0.25rem;
}

.button-row {
  display: flex;
  gap: 1rem;
}

button {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 0.25rem;
  cursor: pointer;
  font-weight: 500;
}

button[type="submit"] {
  background: #3b82f6;
  color: white;
}

button.danger {
  background: #ef4444;
  color: white;
}

.data-panel {
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.5rem;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th,
td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #e5e7eb;
}

th {
  background: #f9fafb;
  font-weight: 600;
}

.actions {
  display: flex;
  gap: 0.5rem;
}
</style>