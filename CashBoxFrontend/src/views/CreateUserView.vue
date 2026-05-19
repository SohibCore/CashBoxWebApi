<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Yangi foydalanuvchi</h2>
        <p>Yangi foydalanuvchi ma'lumotlarini kiriting.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="createNewUser">
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
      <div class="button-row">
        <button type="submit">Saqlash</button>
        <button type="button" class="btn-secondary" @click="cancel">Bekor qilish</button>
      </div>
      <p v-if="createError" class="error">{{ createError }}</p>
    </form>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { createUser, getOrganizations } from '../api';

export default {
  setup() {
    const router = useRouter();
    const organizations = ref([]);
    const createError = ref('');
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

    const createNewUser = async () => {
      createError.value = '';
      const payload = { ...newUser.value };
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
        router.push('/users');
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

    const cancel = () => {
      router.push('/users');
    };

    onMounted(async () => {
      await loadOrganizations();
    });

    return {
      organizations,
      newUser,
      createError,
      createNewUser,
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
  flex-wrap: wrap;
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

.btn-secondary {
  background: #e5e7eb;
  color: #111827;
}

.btn-secondary:hover {
  background: #d1d5db;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
  font-size: 0.95rem;
}
</style>
