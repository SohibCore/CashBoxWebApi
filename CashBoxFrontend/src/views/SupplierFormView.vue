<template>
  <div class="page-card wide-card supplier-page-container">
    <div class="supplier-create">
      <div class="section-header">
        <div class="header-centered">
          <h2>{{ isEdit ? 'Ta\'minotchini tahrirlash' : 'Yangi Ta\'minlovchi' }}</h2>
          <p>{{ isEdit ? 'Ta\'minotchi ma\'lumotlarini o\'zgartiring.' : 'Yangi ta\'minotchi qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
        </div>
      </div>

      <form class="entity-form" @submit.prevent="saveSupplier">
        <div class="form-grid">
          <label>
            Kod

          <input
            v-model="form.code"
            type="text"
            class="form-input"
            placeholder="SUP-001"
            required
          />
          </label>

          <label>
            INN
          <div class="inn-wrapper">
            <input
              v-model="form.inn"
              type="text"
              class="form-input"
              placeholder="123456789"
              maxlength="10"
              required
              @input="onInnInput"
            />
            <span v-if="innLoading" class="inn-spinner"></span>
            <span v-if="innError" class="inn-error-icon">✕</span>
            <span v-if="orgInfo && !innLoading" class="inn-success-icon">✓</span>
          </div>
          <small v-if="innError" class="error-text">{{ innError }}</small>
          </label>
        </div>

        <!-- Organization ma'lumotlari — readonly -->
        <transition name="fade">
          <div v-if="orgInfo" class="org-info-block">
            <div class="org-info-header">
              <span class="org-badge">Tashkilot ma'lumotlari</span>
            </div>

            <div class="org-fields">
              <div class="org-field">
                <span class="org-field-label">To'liq nomi</span>
                <span class="org-field-value">{{ orgInfo.fullName }}</span>
              </div>
              <div class="org-field">
                <span class="org-field-label">Qisqa nomi</span>
                <span class="org-field-value">{{ orgInfo.shortName }}</span>
              </div>
              <div class="org-field">
                <span class="org-field-label">Viloyat</span>
                <span class="org-field-value">{{ orgInfo.regionName || orgInfo.regionId || '-' }}</span>
              </div>
            </div>
          </div>
        </transition>

        <div class="button-row">
          <button type="button" class="btn-cancel" @click="cancel">
            <span>Bekor qilish</span>
          </button>
          <button
            type="submit"
            class="btn-save"
            :disabled="!canSubmit || isSaving"
          >
            <span v-if="isSaving" class="spinner"></span>
            {{ isSaving ? 'Saqlanmoqda...' : (isEdit ? 'Yangilash' : 'Saqlash') }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { getSupplierById, createSupplier, updateSupplier, getField, extractApiData, api } from '../api';

const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.params.id);
const isSaving = ref(false);
const loading = ref(true);
const error = ref('');

const form = ref({
  id: null,
  code: '',
  inn: '',
});

const orgInfo = ref(null);
const innLoading = ref(false);
const innError = ref('');
let innTimer = null;

const canSubmit = computed(() => {
  return form.value.code.trim() && form.value.inn.trim() && orgInfo.value && !innError.value;
});

const loadData = async () => {
  loading.value = true;
  try {
    if (isEdit.value) {
      const supplierId = route.params.id;
      const res = await getSupplierById(supplierId);
      const data = extractApiData(res);
      if (data) {
        form.value = {
          id: getField(data, ['id', 'Id']),
          code: getField(data, ['code', 'Code']),
          inn: getField(data, ['inn', 'Inn']),
        };
        // Edit rejimida tashkilot ma'lumotlarini ham darhol yuklaymiz
        if (form.value.inn) fetchOrg();
      } else {
        error.value = 'Ta\'minotchi topilmadi.';
      }
    }
  } catch (err) {
    console.error('Yuklashda xatolik:', err);
    error.value = 'Ma\'lumotlarni yuklashda xatolik yuz berdi.';
  } finally {
    loading.value = false;
  }
};

const onInnInput = () => {
  orgInfo.value = null;
  innError.value = '';
  clearTimeout(innTimer);

  if (form.value.inn.length < 9) return;

  // 600ms debounce
  innTimer = setTimeout(() => fetchOrg(), 600);
};

const fetchOrg = async () => {
  if (!form.value.inn) return;
  innLoading.value = true;
  innError.value = '';
  try {
    const res = await api.get(`/api/Organization/by-inn/${form.value.inn}`);
    const data = res.data?.data || res.data;
    if (data) {
      orgInfo.value = {
        fullName: getField(data, ['fullName', 'FullName']),
        shortName: getField(data, ['shortName', 'ShortName']),
        regionName: getField(data, ['regionName', 'RegionName', 'region', 'Region']),
        regionId: getField(data, ['regionId', 'RegionId'])
      };
    }
  } catch (e) {
    innError.value = 'Bu INN bo\'yicha tashkilot topilmadi';
    orgInfo.value = null;
  } finally {
    innLoading.value = false;
  }
};

const saveSupplier = async () => {
  isSaving.value = true;
  error.value = '';
  try {
    const payload = {
      code: form.value.code,
      inn: form.value.inn,
    };

    if (isEdit.value) {
      await updateSupplier(form.value.id, payload);
    } else {
      await createSupplier(payload);
    }
    router.push('/suppliers'); // Saqlangandan keyin ta'minotchilar ro'yxatiga qaytish
  } catch (err) {
    console.error('Saqlashda xatolik:', err);
    error.value = 'Saqlashda xatolik yuz berdi: ' + (err.response?.data?.message || err.message);
  } finally {
    isSaving.value = false;
  }
};

const cancel = () => {
  router.push('/suppliers'); // Bekor qilish tugmasi bosilganda ta'minotchilar ro'yxatiga qaytish
};

onMounted(loadData);
</script>

<style scoped>
.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; }
.wide-card { max-width: 1400px; margin: 0 auto; }
.section-header { margin-bottom: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.07); padding-bottom: 1rem; }
.header-centered { text-align: center; width: 100%; }
.supplier-create { display: flex; justify-content: center; padding: 0 16px; }
.entity-form {
  background: #111827;
  padding: 1.5rem;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  width: 100%;
  max-width: 520px;
}
.form-grid { display: grid; grid-template-columns: 1fr; gap: 1.5rem; margin-bottom: 1rem; }
label { display: block; font-weight: 600; color: #94a3b8; margin-bottom: 0.5rem; font-size: 13px; }
input, select {
  width: 100%;
  padding: 0.75rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.08);
  color: #f1f5f9;
  border-radius: 0.4rem;
  font-size: 13.5px;
}
input:focus, select:focus { border-color: #3b82f6; outline: none; }

.inn-wrapper { position: relative; display: flex; align-items: center; }
.inn-wrapper input { padding-right: 36px; }
.inn-spinner, .inn-success-icon, .inn-error-icon {
  position: absolute;
  right: 10px;
}
.inn-spinner {
  width: 16px; height: 16px; border: 2px solid rgba(255,255,255,0.3);
  border-top-color: #3b82f6; border-radius: 50%; animation: spin 0.8s linear infinite;
}
.inn-success-icon { color: #3fb950; font-size: 16px; font-weight: 700; }
.inn-error-icon { color: #f85149; font-size: 14px; font-weight: 700; }
.error-text { font-size: 12px; color: #f85149; margin-top: 0.5rem; }

.org-info-block {
  background: rgba(35, 134, 54, 0.1); /* Greenish background */
  border: 1px solid #238636;
  border-radius: 0.75rem;
  padding: 1rem;
  margin-top: 1rem;
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}
.org-info-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 0.5rem; }
.org-badge {
  font-size: 0.7rem;
  font-weight: 600;
  color: #3fb950;
  background: rgba(63, 185, 80, 0.15);
  border: 1px solid #238636;
  border-radius: 1rem;
  padding: 0.25rem 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}
.org-fields { display: flex; flex-direction: column; gap: 0.5rem; }
.org-field { display: flex; justify-content: space-between; align-items: center; }
.org-field-label { font-size: 0.8rem; color: #8b949e; }
.org-field-value { font-size: 0.85rem; color: #e6edf3; font-weight: 500; text-align: right; }

.button-row { display: flex; gap: 1rem; margin-top: 3rem; padding-top: 1.5rem; border-top: 1px solid rgba(255, 255, 255, 0.07); }
.btn-save {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  color: white;
  padding: 0.8rem 2rem;
  border-radius: 0.6rem;
  border: none;
  font-weight: 600;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 10px;
  transition: all 0.2s ease;
}
.btn-save:hover:not(:disabled) { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37, 99, 235, 0.4); }
.btn-save:disabled { opacity: 0.6; cursor: not-allowed; }
.btn-cancel {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 0.6rem;
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  border: 1px solid rgba(255, 255, 255, 0.1);
  padding: 0.8rem 2rem;
  border-radius: 0.6rem;
  cursor: pointer;
  font-size: 14px;
  font-weight: 600;
  transition: all 0.2s ease;
}
.btn-cancel:hover { background: rgba(255, 255, 255, 0.1); color: #f1f5f9; border-color: rgba(255, 255, 255, 0.2); }
.spinner { width: 16px; height: 16px; border: 2px solid rgba(255, 255, 255, 0.3); border-top-color: white; border-radius: 50%; animation: spin 0.8s linear infinite; }
.error { color: #ef4444; margin-top: 1rem; font-size: 14px; text-align: center; }
.fade-enter-active, .fade-leave-active { transition: opacity 0.25s, transform 0.25s; }
.fade-enter-from, .fade-leave-to { opacity: 0; transform: translateY(-6px); }
@keyframes spin { to { transform: rotate(360deg); } }
</style>