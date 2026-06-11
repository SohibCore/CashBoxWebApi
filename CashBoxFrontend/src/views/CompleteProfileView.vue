<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Ma'lumotlarni to'liq to'ldiring</h2>
        <p>Ro'yxatdan o‘tganingizdan so‘ng qolgan profil ma’lumotlarini to‘ldiring.</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveProfile">
      <div class="form-grid">
        <label>To‘liq ism<input v-model="form.fullName" required /></label>
        <label>Qisqa nom<input v-model="form.shortName" required /></label>
        <label>PINFL<input v-model="form.pinfl" required maxlength="14" /></label>
        <label>Telefon<input v-model="form.phoneNumber" required maxlength="9" /></label>
        <label>Manzil<input v-model="form.address" required /></label>
        <label>Tashkilot
          <input
            v-model="organizationSearchTerm"
            type="text"
            placeholder="Tashkilotni qidirish..."
            class="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 mb-2"
          />
          <select v-model.number="form.organizationId" required>
            <option value="0" disabled>Tanlang</option>
            <option v-for="org in filteredOrganizations" :key="org.id" :value="org.id">{{ org.shortName }}</option>
          </select></label>
        <label>Tug‘ilgan sana<input type="text" v-model="form.dateOfBirth" placeholder="dd.MM.yyyy" required /></label>
        <label>Passport seriya<input v-model="form.passportSeries" required maxlength="9" /></label>
      </div>
      <div class="button-row">
        <button type="submit" :disabled="isSaving">
          {{ isSaving ? 'Saqlanmoqda...' : 'Saqlash' }}
        </button>
        <button type="button" class="danger" @click="cancel">Bekor qilish</button>
      </div>
      <p v-if="error" class="error">{{ error }}</p>
      <p v-if="success" class="success">{{ success }}</p>
    </form>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRouter } from 'vue-router';
import { getMe, getOrganizations, updateUser, normalizeUser, extractApiData } from '../api';

export default {
  setup() {
    const router = useRouter();
    const form = ref({
      id: null,
      fullName: '',
      shortName: '',
      pinfl: '',
      phoneNumber: '',
      address: '',
      organizationId: 0,
      dateOfBirth: '',
      passportSeries: ''
    });
    const organizations = ref([]);
    const organizationSearchTerm = ref('');
    const isSaving = ref(false);
    const error = ref('');
    const success = ref('');

    const normalizeDate = (value) => {
      if (!value) return '';
      if (value.includes('.')) return value;
      const date = new Date(value);
      if (isNaN(date.getTime())) return value;
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

    const normalize = (value) => {
      if (value === undefined || value === null) return null;
      return String(value).trim().toLowerCase();
    };

    const filteredOrganizations = computed(() => {
      if (!organizationSearchTerm.value) {
        return organizations.value;
      }
      const lowerCaseSearchTerm = organizationSearchTerm.value.toLowerCase();
      return organizations.value.filter(org =>
        org.shortName.toLowerCase().includes(lowerCaseSearchTerm) ||
        org.fullName.toLowerCase().includes(lowerCaseSearchTerm)
      );
    });
    const equals = (a, b) => {
      const left = normalize(a);
      const right = normalize(b);
      return left && right && left === right;
    };

    const loadProfile = async () => {
      try {
        const response = await getMe();
        const user = normalizeUser(extractApiData(response));
        if (user) {
          form.value = {
            id: user.id,
            fullName: user.fullName || '',
            shortName: user.shortName || '',
            pinfl: user.pinfl || '',
            phoneNumber: user.phoneNumber || '',
            address: user.address || '',
            organizationId: user.organizationId || 0,
            dateOfBirth: normalizeDate(user.dateOfBirth || ''),
            passportSeries: user.passportSeries || ''
          };
          localStorage.setItem('currentUser', JSON.stringify(user));
          if (user.id) localStorage.setItem('userId', user.id);
          if (user.userName) localStorage.setItem('userName', user.userName);
          if (user.email) localStorage.setItem('email', user.email);
          return;
        }
      } catch (err) {
        console.warn('GetMe failed on profile fill page, using cached user', err);
        const storedUser = JSON.parse(localStorage.getItem('currentUser') || 'null');
        if (storedUser) {
          const parsed = normalizeUser(storedUser) || storedUser;
          form.value = {
            id: parsed.id,
            fullName: parsed.fullName || '',
            shortName: parsed.shortName || '',
            pinfl: parsed.pinfl || '',
            phoneNumber: parsed.phoneNumber || '',
            address: parsed.address || '',
            organizationId: parsed.organizationId || 0,
            dateOfBirth: normalizeDate(parsed.dateOfBirth || ''),
            passportSeries: parsed.passportSeries || ''
          };
        }
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
      } catch (err) {
        console.error(err);
      }
    };

    const saveProfile = async () => {
      error.value = '';
      success.value = '';
      if (!form.value.id) {
        error.value = 'Foydalanuvchi aniqlanmadi.';
        return;
      }
      isSaving.value = true;
      try {
        const payload = { ...form.value };
        delete payload.id;
        await updateUser(form.value.id, payload);
        
        const existingUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
        const updatedUser = normalizeUser({
          id: form.value.id,
          fullName: form.value.fullName,
          shortName: form.value.shortName,
          pinfl: form.value.pinfl,
          phoneNumber: form.value.phoneNumber,
          address: form.value.address,
          organizationId: form.value.organizationId,
          dateOfBirth: form.value.dateOfBirth,
          passportSeries: form.value.passportSeries,
          userName: existingUser.userName || existingUser.UserName,
          email: existingUser.email || existingUser.Email,
          role: existingUser.role || existingUser.Role
        });
        localStorage.setItem('currentUser', JSON.stringify(updatedUser));
        if (updatedUser.id) {
          localStorage.setItem('userId', updatedUser.id);
        }
        if (updatedUser.userName) {
          localStorage.setItem('userName', updatedUser.userName);
        }
        if (updatedUser.email) {
          localStorage.setItem('email', updatedUser.email);
        }
        success.value = 'Maʼlumotlar saqlandi.';
        setTimeout(() => {
          router.push('/profile');
        }, 1500);
      } catch (err) {
        console.error('Save profile error:', err.response?.data || err.message);
        if (err.response?.data?.errors) {
          error.value = Object.values(err.response.data.errors).flat().join(' ');
        } else {
          error.value = 'Saqlashda xatolik yuz berdi.';
        }
      } finally {
        isSaving.value = false;
      }
    };

    const cancel = () => {
      router.push('/profile');
    };

    onMounted(async () => {
      await loadOrganizations();
      await loadProfile();
    });

    return {
      form,
      organizations,
      organizationSearchTerm,
      isSaving,
      error,
      success,
      saveProfile,
      cancel,
      filteredOrganizations
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
  max-width: 1000px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
}

.entity-form {
  background: #f8fafc;
  padding: 1.5rem;
  border-radius: 0.75rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1rem;
}

.form-grid label {
  display: flex;
  flex-direction: column;
  font-weight: 500;
}

.form-grid input,
.form-grid select {
  padding: 0.6rem;
  border: 1px solid #d1d5db;
  border-radius: 0.35rem;
  margin-top: 0.35rem;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

button {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.65rem;
  cursor: pointer;
}

button.danger {
  background: #ef4444;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
}

.success {
  margin-top: 1rem;
  color: #16a34a;
}
</style>