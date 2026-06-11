<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>Profil</h2>
        <p>Tizimga kirgan foydalanuvchining shaxsiy ma'lumotlari.</p>
      </div>
    </div>

    <div v-if="user" class="profile-grid">
      <div class="profile-item"><span class="label">Login:</span> <span class="value">{{ user.userName }}</span></div>
      <div class="profile-item"><span class="label">Email:</span> <span class="value">{{ user.email || '-' }}</span></div>
      <div class="profile-item"><span class="label">To'liq ism:</span> <span class="value">{{ user.fullName || '-' }}</span></div>
      <div class="profile-item"><span class="label">Qisqa nom:</span> <span class="value">{{ user.shortName || '-' }}</span></div>
      <div class="profile-item"><span class="label">PINFL:</span> <span class="value">{{ user.pinfl || '-' }}</span></div>
      <div class="profile-item"><span class="label">Telefon:</span> <span class="value">+998 {{ user.phoneNumber || '-' }}</span></div>
      <div class="profile-item"><span class="label">Manzil:</span> <span class="value">{{ user.address || '-' }}</span></div>
      <div class="profile-item"><span class="label">Tug'ilgan sana:</span> <span class="value">{{ formatDisplayDate(user.dateOfBirth) || '-' }}</span></div>
      <div class="profile-item"><span class="label">Passport:</span> <span class="value">{{ user.passportSeries || '-' }}</span></div>
      <div class="profile-item"><span class="label">Tashkilot:</span> <span class="value">{{ organizationName(user.organizationId) }}</span></div>
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

<style scoped>
.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; }
.wide-card { max-width: 1400px; margin: 0 auto; }
.section-header { margin-bottom: 2rem; border-bottom: 1px solid rgba(255,255,255,0.07); padding-bottom: 1rem; }
.section-header h2 { color: #f1f5f9; margin: 0; }
.section-header p { color: #94a3b8; margin: 5px 0 0; }

.profile-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1.5rem; margin-bottom: 2rem; }
@media (max-width: 768px) { .profile-grid { grid-template-columns: 1fr; } }

.profile-item { 
  background: #111827; 
  padding: 1.25rem; 
  border-radius: 0.75rem; 
  border: 1px solid rgba(255, 255, 255, 0.07);
  display: flex;
  flex-direction: column;
  gap: 5px;
}
.label { font-size: 12px; text-transform: uppercase; color: #64748b; font-weight: 600; letter-spacing: 0.5px; }
.value { color: #f1f5f9; font-size: 15px; font-weight: 500; }

.empty-state { padding: 3rem; text-align: center; color: #64748b; background: #111827; border-radius: 0.75rem; border: 1px dashed rgba(255,255,255,0.1); }
.button-row { display: flex; justify-content: flex-start; margin-top: 1rem; }

button { 
  border: none; background: #2563eb; color: white; padding: 0.9rem 2rem; border-radius: 0.75rem; cursor: pointer; 
  font-weight: 600; transition: all 0.2s;
}
button:hover { background: #1d4ed8; transform: translateY(-1px); }
</style>