<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div class="header-centered">
        <h2>{{ isEdit ? 'Tashkilotni tahrirlash' : 'Yangi tashkilot yaratish' }}</h2>
        <p>{{ isEdit ? 'Tashkilot ma\'lumotlarini o\'zgartiring.' : 'Yangi tashkilot qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form v-if="!loading && form" class="entity-form" @submit.prevent="saveOrganization">
      <div class="form-grid">
        <label>
          INN
          <input v-model="form.inn" type="text" required maxlength="9" placeholder="9 raqamli INN" />
        </label>
        <label>
          To'liq nomi
          <input v-model="form.fullName" type="text" required placeholder="Tashkilot to'liq nomi" />
        </label>
        <label>
          Qisqa nomi
          <input v-model="form.shortName" type="text" required placeholder="Tashkilot qisqa nomi" />
        </label>
        <label>
          Viloyat
          <select v-model="form.regionId" @change="onRegionChange" required>
            <option :value="null" disabled>Viloyatni tanlang</option>
            <option v-for="reg in regions" :key="reg.id" :value="reg.id">
              {{ reg.fullName }}
            </option>
          </select>
        </label>
        <label>
          Hudud (Tuman)
          <select v-model="form.district" :disabled="!form.regionId" required>
            <option value="" disabled>{{ form.regionId ? 'Tumanni tanlang' : 'Avval viloyat tanlang' }}</option>
            <option v-for="dist in districts" :key="dist.id" :value="dist.fullName">{{ dist.fullName }}</option>
          </select>
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
import { getOrganizationById, createOrganization, updateOrganization, getField, extractApiData, getRegions, getDistricts } from '../api';

const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.params.id);
const isSaving = ref(false);
const loading = ref(true);
const error = ref('');
const regions = ref([]);
const districts = ref([]);

const form = ref({
  id: null,
  inn: '',
  fullName: '',
  shortName: '',
  regionId: null,
  district: '',
});

const fetchDistricts = async (regId) => {
  if (!regId) {
    districts.value = [];
    return;
  }
  try {
    const res = await getDistricts(regId);
    districts.value = extractApiData(res).map(d => ({
      id: getField(d, ['id', 'Id']),
      fullName: getField(d, ['fullName', 'FullName']),
    }));
  } catch (err) {
    console.error('Tumanlarni yuklashda xatolik:', err);
  }
};

const onRegionChange = () => {
  form.value.district = '';
  fetchDistricts(form.value.regionId);
};

const loadData = async () => {
  loading.value = true;
  try {
    const regRes = await getRegions();
    regions.value = extractApiData(regRes).map(r => ({
      id: getField(r, ['id', 'Id']),
      fullName: getField(r, ['fullName', 'FullName']),
    }));

    if (!isEdit.value) return;

    const id = route.params.id;
    const res = await getOrganizationById(id);
    const data = extractApiData(res);
    if (data) {
      form.value = {
        id: getField(data, ['id', 'Id']),
        inn: getField(data, ['inn', 'Inn']),
        fullName: getField(data, ['fullName', 'FullName']),
        shortName: getField(data, ['shortName', 'ShortName']),
        regionId: getField(data, ['regionId', 'RegionId']),
        district: getField(data, ['district', 'District']),
      };

      if (form.value.regionId) {
        await fetchDistricts(form.value.regionId);
      }
    } else {
      error.value = 'Tashkilot topilmadi.';
    }
  } catch (err) {
    console.error('Yuklashda xatolik:', err);
    error.value = 'Ma\'lumotlarni yuklashda xatolik yuz berdi.';
  } finally {
    loading.value = false;
  }
};

const saveOrganization = async () => {
  isSaving.value = true;
  error.value = '';
  try {
    const payload = {
      inn: form.value.inn,
      fullName: form.value.fullName,
      shortName: form.value.shortName,
      regionId: Number(form.value.regionId),
      district: form.value.district,
    };

    if (isEdit.value) {
      await updateOrganization(form.value.id, payload);
    } else {
      await createOrganization(payload);
    }
    router.push('/organizations');
  } catch (err) {
    console.error('Saqlashda xatolik:', err);
    error.value = 'Saqlashda xatolik yuz berdi: ' + (err.response?.data?.message || err.message);
  } finally {
    isSaving.value = false;
  }
};

const cancel = () => {
  router.push('/organizations');
};

onMounted(loadData);
</script>

<style scoped>
.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; }
.wide-card { max-width: 1400px; margin: 0 auto; }
.section-header { margin-bottom: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.07); padding-bottom: 1rem; }
.header-centered { text-align: center; width: 100%; }
.entity-form { background: #111827; padding: 1.5rem; border-radius: 0.75rem; border: 1px solid rgba(255, 255, 255, 0.07); }
.form-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1.5rem; margin-bottom: 1rem; }
label { display: block; font-weight: 600; color: #94a3b8; margin-bottom: 0.5rem; font-size: 13px; }
input, select { width: 100%; padding: 0.75rem; background: rgba(255, 255, 255, 0.05); border: 1px solid rgba(255, 255, 255, 0.08); color: #f1f5f9; border-radius: 0.4rem; font-size: 13.5px; }
input:focus, select:focus { border-color: #3b82f6; outline: none; background: #1e293b !important; }
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