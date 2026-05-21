<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Tumanni tahrirlash' : 'Yangi tuman yaratish' }}</h2>
        <p>{{ isEdit ? 'Tuman ma\'lumotlarini o\'zgartiring.' : 'Yangi tuman qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveDistrict">
      <div class="form-grid">
        <label>
          To'liq nomi (FullName)
          <input v-model="form.fullName" type="text" maxlength="500" required />
        </label>
        <label>
          Kod (Code)
          <input v-model="form.code" type="text" maxlength="9" required />
        </label>
        <label>
          Viloyat
          <select v-model.number="form.regionId" required>
            <option value="0" disabled>Viloyatni tanlang</option>
            <option v-for="reg in regions" :key="reg.id" :value="reg.id">{{ reg.fullName }}</option>
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
import { getDistrictById, createDistrict, updateDistrict, getRegions } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const regions = ref([]);
    const error = ref('');
    const successMessage = ref('');
    const isSaving = ref(false);

    const form = ref({
      id: null,
      fullName: '',
      code: '',
      regionId: 0
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

    const loadDistrict = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getDistrictById(route.params.id);
        const item = response.data?.data || response.data;
        if (item) {
          form.value = {
            id: item.id || item.Id,
            fullName: item.fullName || item.FullName,
            code: item.code || item.Code,
            regionId: item.regionId || item.RegionId
          };
        } else {
          error.value = 'Tuman topilmadi.';
        }
      } catch (err) {
        console.error('Error loading district:', err);
        error.value = err.response?.data?.message || err.message || 'Tuman ma\'lumotlarini yuklashda xatolik yuz berdi.';
      }
    };

    const saveDistrict = async () => {
      error.value = '';
      successMessage.value = '';
      isSaving.value = true;
      try {
        if (isEdit.value) {
          await updateDistrict(route.params.id, form.value);
          successMessage.value = 'Tuman muvaffaqiyatli yangilandi!';
        } else {
          await createDistrict(form.value);
          successMessage.value = 'Tuman muvaffaqiyatli yaratildi!';
        }
        setTimeout(() => router.push('/districts'), 1500);
      } catch (err) {
        console.error('Error saving district:', err);
        error.value = err.response?.data?.message || err.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        isSaving.value = false;
      }
    };

    const cancel = () => router.push('/districts');

    onMounted(async () => {
      await loadRegions();
      if (isEdit.value) await loadDistrict();
    });

    return { form, regions, isEdit, isSaving, error, successMessage, saveDistrict, cancel };
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
  margin-bottom: 1.5rem;
}
.form-grid { display: grid; grid-template-columns: 1fr; gap: 1.5rem; }
label { display: block; font-weight: 600; color: #1e293b; margin-bottom: 0.5rem; }
input {
  width: 100%; padding: 0.75rem; border: 1px solid #cbd5e1;
  border-radius: 0.5rem; font-size: 1rem; transition: border-color 0.2s;
}
input:focus { outline: none; border-color: #2563eb; }
.button-row { display: flex; gap: 1rem; margin-top: 2rem; }
.button-row button {
  border: none; background: #2563eb; color: white; padding: 0.85rem 1rem;
  border-radius: 0.75rem; cursor: pointer; font-weight: 500;
}
.btn-secondary { background: #e2e8f0 !important; color: #475569 !important; border: 1px solid #cbd5e1 !important; }
.error {
  color: #dc2626; margin-top: 1.5rem; padding: 0.75rem;
  background: #fef2f2; border-radius: 0.5rem; border: 1px solid #fee2e2;
}
.success {
  color: #16a34a; margin-top: 1.5rem; padding: 0.75rem;
  background: #f0fdf4; border: 1px solid #dcfce7;
}
</style>