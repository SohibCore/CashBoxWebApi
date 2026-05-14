<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Profil</h2>
        <p>Foydalanuvchi ma'lumotlari va to'liq profilni ko'rish.</p>
      </div>
    </div>

    <div v-if="user" class="profile-grid">
      <div class="profile-item"><strong>Login:</strong> {{ user.userName }}</div>
      <div class="profile-item"><strong>Email:</strong> {{ user.email || '-' }}</div>
      <div class="profile-item"><strong>To'liq ism:</strong> {{ user.fullName || '-' }}</div>
      <div class="profile-item"><strong>Qisqa nom:</strong> {{ user.shortName || '-' }}</div>
      <div class="profile-item"><strong>PINFL:</strong> {{ user.pinfl || '-' }}</div>
      <div class="profile-item"><strong>Telefon:</strong> {{ user.phoneNumber || '-' }}</div>
      <div class="profile-item"><strong>Manzil:</strong> {{ user.address || '-' }}</div>
      <div class="profile-item"><strong>Tug'ilgan sana:</strong> {{ formatDisplayDate(user.dateOfBirth) || '-' }}</div>
      <div class="profile-item"><strong>Passport seriya:</strong> {{ user.passportSeries || '-' }}</div>
      <div class="profile-item"><strong>Tashkilot:</strong> {{ organizationName(user.organizationId) }}</div>
    </div>

    <div v-if="!user" class="empty-state">
      <p>Profil ma'lumotlari yuklanmoqda yoki mavjud emas. Iltimos, sahifani yangilang.</p>
    </div>

    <div class="button-row">
      <button @click="goToFill">Ma'lumotlarni to'liq to'ldirish</button>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { getMe, getOrganizations, normalizeUser, extractApiData } from '../api';

export default {
  setup() {
    const router = useRouter();
    const user = ref(null);
    const organizations = ref([]);

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

    const equals = (a, b) => {
      const left = normalize(a);
      const right = normalize(b);
      return left && right && left === right;
    };

    const loadUser = async () => {
      try {
        const response = await getMe();
        const currentUser = normalizeUser(extractApiData(response));
        if (currentUser) {
          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          if (currentUser.id) {
            localStorage.setItem('userId', currentUser.id);
          }
          if (currentUser.userName) {
            localStorage.setItem('userName', currentUser.userName);
          }
          if (currentUser.email) {
            localStorage.setItem('email', currentUser.email);
          }
          user.value = currentUser;
          return;
        }
      } catch (error) {
        console.warn('GetMe failed on profile load, using cached user', error);
      }

      const storedUser = JSON.parse(localStorage.getItem('currentUser') || 'null');
      if (storedUser) {
        user.value = normalizeUser(storedUser) || storedUser;
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

    const formatDisplayDate = (value) => {
      if (!value) return '';
      if (value.includes('.')) return value;
      const date = new Date(value);
      if (isNaN(date.getTime())) return value;
      const day = String(date.getDate()).padStart(2, '0');
      const month = String(date.getMonth() + 1).padStart(2, '0');
      return `${day}.${month}.${date.getFullYear()}`;
    };

    const organizationName = (id) => {
      const org = organizations.value.find(item => {
        const orgId = getField(item, ['id', 'Id']);
        return Number(orgId) === Number(id);
      });
      return org ? getField(org, ['shortName', 'ShortName']) || '-' : '-';
    };

    const goToFill = () => {
      router.push('/profile/fill');
    };

    onMounted(async () => {
      await loadOrganizations();
      await loadUser();
    });

    return {
      user,
      formatDisplayDate,
      organizationName,
      goToFill
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

.profile-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
  margin-bottom: 1.5rem;
}

.profile-item {
  background: #f8fafc;
  padding: 1rem;
  border-radius: 0.75rem;
  border: 1px solid #e2e8f0;
}

.empty-state {
  padding: 1.5rem;
  background: #f8fafc;
  border-radius: 0.75rem;
  border: 1px dashed #cbd5e1;
}

.button-row {
  display: flex;
  justify-content: flex-start;
}

button {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.9rem 1.25rem;
  border-radius: 0.75rem;
  cursor: pointer;
}
</style>