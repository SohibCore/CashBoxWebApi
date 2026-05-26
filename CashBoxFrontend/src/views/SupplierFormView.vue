<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Ta\'minotchini tahrirlash' : 'Yangi ta\'minotchi' }}</h2>
        <p>{{ isEdit ? 'Ma\'lumotlarni yangilang.' : 'Yangi ta\'minotchi qo\'shing.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveSupplier">
      <div class="form-grid">
        <label>
          INN
          <input v-model="form.inn" type="text" placeholder="INN kiriting" required />
        </label>
        <label>
          Code
          <input v-model="form.code" type="text" placeholder="Kod kiriting" required />
        </label>
      </div>

      <div class="button-row">
        <button type="submit" :disabled="saving">
          {{ saving ? 'Saqlanmoqda...' : (isEdit ? 'Yangilash' : 'Saqlash') }}
        </button>
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
import { getSupplierById, createSupplier, updateSupplier, extractApiData } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const isEdit = computed(() => !!route.params.id);
    const saving = ref(false);
    const error = ref('');
    const successMessage = ref('');

    const form = ref({
      inn: '',
      code: ''
    });

    const loadSupplier = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getSupplierById(route.params.id);
        const data = extractApiData(response);
        if (data) {
          form.value = {
            inn: data.inn || data.Inn || '',
            code: data.code || data.Code || ''
          };
        }
      } catch (err) {
        error.value = "Ma'lumotlarni yuklashda xatolik yuz berdi.";
        console.error(err);
      }
    };

    const saveSupplier = async () => {
      saving.value = true;
      error.value = '';
      successMessage.value = '';
      try {
        const payload = { ...form.value };
        if (isEdit.value) {
          payload.id = route.params.id; // Backend uchun ID ni payloadga qo'shamiz
          await updateSupplier(route.params.id, payload);
          successMessage.value = "Ta'minotchi muvaffaqiyatli yangilandi!";
        } else {
          await createSupplier(payload);
          successMessage.value = "Ta'minotchi muvaffaqiyatli qo'shildi!";
        }
        setTimeout(() => router.push('/suppliers'), 1500);
      } catch (err) {
        error.value = err.response?.data?.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        saving.value = false;
      }
    };

    const cancel = () => router.back();

    onMounted(loadSupplier);

    return { isEdit, form, saving, error, successMessage, saveSupplier, cancel };
  }
};
</script>

<style scoped>
.page-card {
  background: white;
  padding: 1.5rem;
  border-radius: 1rem;
  box-shadow: 0 18px 50px rgba(15, 23, 42, 0.08);
}

.wide-card {
  max-width: 800px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 2rem;
  border-bottom: 1px solid #f1f5f9;
  padding-bottom: 1rem;
}

.entity-form {
  background: #f8fafc;
  padding: 2rem;
  border-radius: 1rem;
  border: 1px solid #2563eb;
  box-shadow: 0 4px 15px rgba(37, 99, 235, 0.1);
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

label {
  display: block;
  font-weight: 600;
  color: #1e293b;
  margin-bottom: 0.5rem;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s;
}

input:focus {
  outline: none;
  border-color: #2563eb;
}

button {
  background: #2563eb;
  color: white;
  padding: 0.85rem 1.5rem;
  border-radius: 0.75rem;
  border: none;
  font-weight: 600;
  cursor: pointer;
  flex: 1;
}

.btn-secondary {
  background: #e2e8f0;
  color: #475569;
  border: 1px solid #cbd5e1;
}

.error {
  color: #dc2626;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: #fef2f2;
  border-radius: 0.5rem;
  border: 1px solid #fee2e2;
}
.success {
  color: #16a34a;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: #f0fdf4;
  border: 1px solid #dcfce7;
}
</style>