<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Tashkilotni tahrirlash' : 'Yangi tashkilot yaratish' }}</h2>
        <p>{{ isEdit ? 'Tashkilot ma\'lumotlarini o\'zgartiring.' : 'Yangi tashkilot qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveOrganization">
      <div class="form-grid">
        <label>
          INN
          <input v-model="form.inn" type="text" maxlength="9" required />
        </label>
        <label>
          To‘liq nom
          <input v-model="form.fullName" type="text" required />
        </label>
        <label>
          Qisqa nom
          <input v-model="form.shortName" type="text" required />
        </label>
        <label>
          Viloyat
          <select v-model.number="form.regionId" required @change="handleRegionChange">
            <option value="0" disabled>Viloyatni tanlang</option>
            <option v-for="region in regions" :key="region.id" :value="region.id">{{ region.fullName }}</option>
          </select>
        </label>
        <label>
          Hudud (Tuman)
          <select v-model="form.district" required :disabled="!form.regionId || districts.length === 0">
            <option value="" disabled>{{ districts.length === 0 ? 'Avval viloyatni tanlang' : 'Tumanni tanlang' }}</option>
            <option v-for="district in districts" :key="district.id" :value="district.fullName">
              {{ district.fullName }}
            </option>
          </select>
        </label>
      </div>
      <div class="button-row">
        <button type="submit" :disabled="isSaving">{{ isSaving ? 'Saqlanmoqda...' : (isEdit ? 'Yangilash' : 'Saqlash') }}</button>
        <button type="button" class="btn-secondary" @click="cancel">Bekor qilish</button>
      </div>
      <p v-if="error" class="error">{{ error }}</p>
      <p v-if="successMessage" class="success">{{ successMessage }}</p>
    </form>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getOrganizationById, createOrganization, updateOrganization, getRegions, getDistricts } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const regions = ref([]);
    const districts = ref([]);
    const error = ref('');
    const successMessage = ref('');
    const isSaving = ref(false);

    const form = ref({
      id: null,
      inn: '',
      fullName: '',
      shortName: '',
      regionId: 0,
      district: ''
    });

    const isEdit = computed(() => !!route.params.id);

    const loadRegions = async () => {
      try {
        const response = await getRegions();
        const data = response.data?.data || response.data || [];
        regions.value = data.map(r => ({
          id: r.id || r.Id,
          fullName: r.fullName || r.FullName
        }));
      } catch (err) {
        console.error('Error loading regions:', err);
      }
    };

    const loadDistricts = async (regionId) => {
      if (!regionId) {
        districts.value = [];
        return;
      }
      try {
        const response = await getDistricts(regionId);
        const data = response.data?.data || response.data || [];
        districts.value = data.map(d => ({
          id: d.id || d.Id,
          fullName: d.fullName || d.FullName
        }));
      } catch (err) {
        console.error('Error loading districts:', err);
      }
    };

    const handleRegionChange = async () => {
      // Viloyat o'zgarganda tumanni tozalaymiz
      form.value.district = '';
      if (form.value.regionId) {
        await loadDistricts(form.value.regionId);
      }
    };

    const loadOrganization = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getOrganizationById(route.params.id);
        const item = response.data?.data || response.data;
        if (item) {
          form.value = {
            id: item.id || item.Id,
            inn: item.inn || item.Inn,
            fullName: item.fullName || item.FullName,
            shortName: item.shortName || item.ShortName,
            regionId: item.regionId || item.RegionId,
            district: item.district || item.District
          };
          // Tahrirlashda tumanlar ro'yxatini yuklab qo'yish kerak
          if (form.value.regionId) {
            await loadDistricts(form.value.regionId);
          }
        }
      } catch (err) {
        error.value = 'Ma\'lumotlarni yuklashda xatolik yuz berdi.';
      }
    };

    const saveOrganization = async () => {
      error.value = '';
      successMessage.value = '';
      isSaving.value = true;
      try {
        if (isEdit.value) {
          await updateOrganization(route.params.id, form.value);
          successMessage.value = 'Tashkilot muvaffaqiyatli yangilandi!';
        } else {
          await createOrganization(form.value);
          successMessage.value = 'Tashkilot muvaffaqiyatli yaratildi!';
        }
        setTimeout(() => router.push('/organizations'), 1500);
      } catch (err) {
        error.value = err.response?.data?.message || err.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        isSaving.value = false;
      }
    };

    const cancel = () => router.push('/organizations');

    onMounted(async () => {
      await loadRegions();
      if (isEdit.value) await loadOrganization();
    });

    return { 
      form, 
      regions, 
      districts, 
      isEdit, 
      isSaving, 
      error, 
      successMessage, 
      saveOrganization, 
      cancel,
      handleRegionChange
    };
  }
};
</script>

<style scoped>
.page-card { background: white; padding: 1.5rem; border-radius: 1rem; box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08); }
.wide-card { max-width: 800px; margin: 0 auto; }
.section-header { margin-bottom: 2rem; border-bottom: 1px solid #f1f5f9; padding-bottom: 1rem; }
.entity-form {
  background: #f8fafc; padding: 2rem; border-radius: 1rem;
  border: 1px solid #2563eb; box-shadow: 0 4px 15px rgba(37, 99, 235, 0.1);
}
.form-grid { display: grid; grid-template-columns: 1fr; gap: 1.5rem; }
label { display: block; font-weight: 600; color: #1e293b; margin-bottom: 0.5rem; }
input {
  width: 100%; padding: 0.75rem; border: 1px solid #cbd5e1;
  border-radius: 0.5rem; font-size: 1rem; transition: border-color 0.2s;
}
input:focus { outline: none; border-color: #2563eb; }
.button-row { display: flex; gap: 1rem; margin-top: 2rem; }
.btn-secondary { background: #e2e8f0; color: #475569; border: 1px solid #cbd5e1; }
.error {
  color: #dc2626; margin-top: 1.5rem; padding: 0.75rem;
  background: #fef2f2; border-radius: 0.5rem; border: 1px solid #fee2e2;
}
.success {
  color: #16a34a; margin-top: 1.5rem; padding: 0.75rem;
  background: #f0fdf4; border: 1px solid #dcfce7;
}
</style>