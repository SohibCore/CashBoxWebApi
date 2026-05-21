<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Viloyatni tahrirlash' : 'Yangi viloyat yaratish' }}</h2>
        <p>{{ isEdit ? 'Viloyat ma\'lumotlarini o\'zgartiring.' : 'Yangi viloyat qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveRegion">
      <div class="form-grid">
        <label>
          To'liq nomi (FullName)
          <input v-model="form.fullName" type="text" maxlength="500" required />
        </label>
        <label>
          Qisqa nomi (ShortName)
          <input v-model="form.shortName" type="text" maxlength="300" required />
        </label>
        <label>
          Kod (Code)
          <input v-model="form.code" type="text" maxlength="9" required />
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
import { getRegionById, createRegion, updateRegion } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const error = ref('');
    const successMessage = ref('');
    const isSaving = ref(false);

    const form = ref({
      id: null,
      fullName: '',
      shortName: '',
      code: ''
    });

    const isEdit = computed(() => !!route.params.id);

    const loadRegion = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getRegionById(route.params.id);
        const item = response.data?.data || response.data;
        if (item) {
          form.value = {
            id: item.id || item.Id,
            fullName: item.fullName || item.FullName,
            shortName: item.shortName || item.ShortName,
            code: item.code || item.Code
          };
        } else {
          error.value = 'Viloyat topilmadi.';
        }
      } catch (err) {
        console.error('Error loading region:', err);
        error.value = err.response?.data?.message || err.message || 'Viloyat ma\'lumotlarini yuklashda xatolik yuz berdi.';
      }
    };

    const saveRegion = async () => {
      error.value = '';
      successMessage.value = '';
      isSaving.value = true;
      try {
        if (isEdit.value) {
          await updateRegion(route.params.id, form.value);
          successMessage.value = 'Viloyat muvaffaqiyatli yangilandi!';
        } else {
          await createRegion(form.value);
          successMessage.value = 'Viloyat muvaffaqiyatli yaratildi!';
        }
        setTimeout(() => router.push('/regions'), 1500);
      } catch (err) {
        console.error('Error saving region:', err);
        error.value = err.response?.data?.message || err.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        isSaving.value = false;
      }
    };

    const cancel = () => router.push('/regions');

    onMounted(loadRegion);

    return { form, isEdit, isSaving, error, successMessage, saveRegion, cancel };
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