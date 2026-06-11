<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div class="header-centered">
        <h2>{{ isEdit ? 'Mahsulotni tahrirlash' : 'Yangi mahsulot yaratish' }}</h2>
        <p>{{ isEdit ? 'Mahsulot ma\'lumotlarini o\'zgartiring.' : 'Yangi mahsulot qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form v-if="!loading && form" class="entity-form" @submit.prevent="saveProduct">
      <div class="form-grid">
        <label>
          Nomi
          <input v-model="form.name" type="text" required />
        </label>
        <label>
          Kodi
          <input v-model="form.code" type="text" required />
        </label>
        <label>
          Tashkilot
          <select v-model="form.organizationId" required>
            <option :value="null" disabled>Tashkilotni tanlang</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">{{ org.fullName }}</option>
          </select>
        </label>
        <label>
          Yetkazilgan sana
          <input v-model="form.deliveredAt" type="date" />
        </label>
      </div>

      <div class="button-row">
        <button type="submit" class="btn-save" :disabled="isSaving">
          <span v-if="isSaving" class="spinner"></span>
          <span>{{ isSaving ? 'Saqlanmoqda...' : (isEdit ? 'Yangilash' : 'Saqlash') }}</span>
        </button>
        <button type="button" class="btn-cancel" @click="cancel">
          <span>Bekor qilish</span>
        </button>
      </div>
    </form>

    <div v-else-if="loading" class="loading-state">Yuklanmoqda...</div>
    <div v-else class="error-state">Ma'lumot topilmadi yoki yuklashda xato yuz berdi.</div>

    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getProductById, createProduct, updateProduct, getOrganizations, getField, extractApiData } from '../api'; // api.js faylidan kerakli funksiyalarni import qilamiz

const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.params.id);
const isSaving = ref(false);
const loading = ref(true);
const error = ref('');
const organizations = ref([]);

const form = ref({
  id: null,
  name: '',
  code: '',
  organizationId: null,
  deliveredAt: null,
});

const loadData = async () => {
  loading.value = true;
  try {
    // Tashkilotlar ro'yxatini yuklash
    const orgRes = await getOrganizations();
    organizations.value = extractApiData(orgRes).map(o => ({
      id: getField(o, ['id', 'Id']),
      fullName: getField(o, ['fullName', 'FullName']),
    }));

    if (isEdit.value) {
      const productId = route.params.id;
      const res = await getProductById(productId);
      const data = extractApiData(res);
      if (data) {
        form.value = {
          id: getField(data, ['id', 'Id']),
          name: getField(data, ['name', 'Name']),
          code: getField(data, ['code', 'Code']),
          organizationId: getField(data, ['organizationId', 'OrganizationId']),
          // Sanani to'g'ri formatlash
          deliveredAt: getField(data, ['deliveredAt', 'DeliveredAt']) ? new Date(getField(data, ['deliveredAt', 'DeliveredAt'])).toISOString().slice(0, 10) : null,
        };
      } else {
        error.value = 'Mahsulot topilmadi.';
      }
    }
  } catch (err) {
    console.error('Yuklashda xatolik:', err);
    error.value = 'Ma\'lumotlarni yuklashda xatolik yuz berdi.';
  } finally {
    loading.value = false;
  }
};

const saveProduct = async () => {
  isSaving.value = true;
  error.value = '';
  try {
    const payload = {
      name: form.value.name,
      code: form.value.code,
      organizationId: Number(form.value.organizationId),
      deliveredAt: form.value.deliveredAt ? new Date(form.value.deliveredAt).toISOString() : null,
    };

    if (isEdit.value) {
      await updateProduct(form.value.id, payload);
    } else {
      await createProduct(payload);
    }
    router.push('/products'); // Saqlangandan keyin mahsulotlar ro'yxatiga qaytish
  } catch (err) {
    console.error('Saqlashda xatolik:', err);
    error.value = 'Saqlashda xatolik yuz berdi: ' + (err.response?.data?.message || err.message);
  } finally {
    isSaving.value = false;
  }
};

const cancel = () => {
  router.push('/products'); // Bekor qilish tugmasi bosilganda mahsulotlar ro'yxatiga qaytish
};

onMounted(loadData);
</script>

<style scoped>
/* Ushbu stillar OutcomeDocumentFormView.vue faylidagi stillarga o'xshash bo'lishi mumkin */
.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; }
.wide-card { max-width: 1400px; margin: 0 auto; }
.section-header { margin-bottom: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.07); padding-bottom: 1rem; }
.header-centered { text-align: center; width: 100%; }
.entity-form { background: #111827; padding: 1.5rem; border-radius: 0.75rem; border: 1px solid rgba(255, 255, 255, 0.07); }
.form-grid { display: grid; grid-template-columns: 1fr; gap: 1.5rem; margin-bottom: 1rem; }
label { display: block; font-weight: 600; color: #94a3b8; margin-bottom: 0.5rem; font-size: 13px; }
input, select { width: 100%; padding: 0.75rem; background: rgba(255, 255, 255, 0.05); border: 1px solid rgba(255, 255, 255, 0.08); color: #f1f5f9; border-radius: 0.4rem; font-size: 13.5px; }
input:focus, select:focus { border-color: #3b82f6; outline: none; }
select option { background: #1e293b; color: #f1f5f9; }
.button-row { display: flex; gap: 1rem; margin-top: 3rem; padding-top: 1.5rem; border-top: 1px solid rgba(255, 255, 255, 0.07); }
.btn-save { background: linear-gradient(135deg, #3b82f6, #2563eb); color: white; padding: 0.8rem 2rem; border-radius: 0.6rem; border: none; font-weight: 600; cursor: pointer; display: flex; align-items: center; gap: 10px; }
.btn-save:hover:not(:disabled) { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37, 99, 235, 0.4); }
.btn-cancel { display: inline-flex; align-items: center; justify-content: center; gap: 0.6rem; background: rgba(255, 255, 255, 0.05); color: #94a3b8; border: 1px solid rgba(255, 255, 255, 0.1); padding: 0.8rem 2rem; border-radius: 0.6rem; cursor: pointer; font-size: 14px; font-weight: 600; }
.btn-cancel:hover { background: rgba(255, 255, 255, 0.1); color: #f1f5f9; border-color: rgba(255, 255, 255, 0.2); }
.error { color: #ef4444; margin-top: 1rem; font-size: 14px; }
.spinner { width: 16px; height: 16px; border: 2px solid rgba(255, 255, 255, 0.3); border-top-color: white; border-radius: 50%; animation: spin 0.8s linear infinite; }
.loading-state, .error-state { padding: 3rem; text-align: center; color: #64748b; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>