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
        <button type="submit" class="btn-save" :disabled="isSaving">
          <svg v-if="!isSaving" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"></polyline>
          </svg>
          <span v-else class="spinner"></span>
          <span>{{ isSaving ? 'Saqlanmoqda...' : (isEdit ? 'Yangilash' : 'Saqlash') }}</span>
        </button>
        <button type="button" class="btn-cancel" @click="cancel">
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="19" y1="12" x2="5" y2="12"></line>
            <polyline points="12 19 5 12 12 5"></polyline>
          </svg>
          <span>Bekor qilish</span>
        </button>
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
.page-card {
  background: #0d1117;
  padding: 1.5rem;
  border-radius: 1rem;
}

.wide-card { max-width: 800px; margin: 0 auto; }

.section-header {
  margin-bottom: 2rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  padding-bottom: 1rem;
}

.section-header h2 { color: #f1f5f9; }
.section-header p { color: #94a3b8; }

.entity-form {
  background: #111827;
  padding: 2rem;
  border-radius: 1rem;
  border: 1px solid #2563eb;
  box-shadow: 0 4px 15px rgba(37, 99, 235, 0.05);
  margin-bottom: 1.5rem;
}

.form-grid { display: grid; grid-template-columns: 1fr; gap: 1.5rem; }

label {
  display: block;
  font-weight: 600;
  color: #94a3b8;
  margin-bottom: 0.5rem;
}

input, select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s;
}

input:focus, select:focus {
  outline: none;
  border-color: #2563eb;
  background: #1e293b !important;
}

select option {
  background: #1e293b;
  color: #f1f5f9;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 3rem;
  padding-top: 1.5rem;
  border-top: 1px solid rgba(255, 255, 255, 0.07);
}

.btn-save, .btn-cancel {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.6rem;
  padding: 0.8rem 2rem;
  border-radius: 0.6rem;
  font-size: 14px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.2s cubic-bezier(0.4, 0, 0.2, 1);
  border: none;
}

.btn-save {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  color: #ffffff;
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.25);
}

.btn-save:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(37, 99, 235, 0.4);
}

.btn-cancel {
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.btn-cancel:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #f1f5f9;
  border-color: rgba(255, 255, 255, 0.2);
}

.spinner {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: #ffffff;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin { to { transform: rotate(360deg); } }
</style>