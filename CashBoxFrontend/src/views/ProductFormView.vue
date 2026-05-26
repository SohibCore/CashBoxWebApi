<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Mahsulotni tahrirlash' : 'Yangi mahsulot' }}</h2>
        <p>{{ isEdit ? 'Ma\'lumotlarni yangilang.' : 'Yangi mahsulot qo\'shing.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveProduct">
      <div class="form-grid">
        <label>
          Nomi
          <input v-model="form.name" type="text" placeholder="Mahsulot nomi" required />
        </label>
        <label>
          Kod
          <input v-model="form.code" type="text" placeholder="Mahsulot kodi" required />
        </label>
        <label>
          Tashkilot
          <select v-model.number="form.organizationId" required>
            <option value="" disabled>Tashkilotni tanlang</option>
            <option v-for="org in organizations" :key="org.id" :value="org.id">{{ org.shortName }}</option>
          </select>
        </label>
        <label>
          Yetkazilgan sana
          <input v-model="form.deliveredAt" type="datetime-local" required />
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
import { getProductById, createProduct, updateProduct, getOrganizations, extractApiData } from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const isEdit = computed(() => !!route.params.id);
    const saving = ref(false);
    const error = ref('');
    const successMessage = ref('');
    const organizations = ref([]);

    const form = ref({
      name: '',
      code: '',
      organizationId: '',
      deliveredAt: ''
    });

    const getField = (obj, keys) => {
      if (!obj) return null;
      for (const key of keys) {
        if (obj[key] !== undefined && obj[key] !== null) return obj[key];
      }
      return null;
    };

    const loadData = async () => {
      // Tashkilotlarni yuklash
      try {
        const orgRes = await getOrganizations();
        const orgData = extractApiData(orgRes);
        const raw = Array.isArray(orgData) ? orgData : [];
        organizations.value = raw.map(o => ({
          id: getField(o, ['id', 'Id', 'organizationId', 'OrganizationId']),
          shortName: getField(o, ['shortName', 'ShortName']) || getField(o, ['fullName', 'FullName']) || ''
        }));
      } catch (err) {
        console.error('Load organizations error:', err);
      }

      if (!isEdit.value) return;
      try {
        const response = await getProductById(route.params.id);
        const data = extractApiData(response);
        if (data) {
          // Format ISO date to datetime-local compatible string (YYYY-MM-DDTHH:mm)
          const dt = data.deliveredAt || data.DeliveredAt;
          const formattedDate = dt ? new Date(dt).toISOString().slice(0, 16) : '';
          
          form.value = {
            name: data.name || data.Name || '',
            code: data.code || data.Code || '',
            organizationId: data.organizationId || data.OrganizationId || '',
            deliveredAt: formattedDate
          };
        }
      } catch (err) {
        error.value = "Ma'lumotlarni yuklashda xatolik yuz berdi.";
        console.error(err);
      }
    };

    const saveProduct = async () => {
      saving.value = true;
      error.value = '';
      successMessage.value = '';
      try {
        const payload = { 
          ...form.value,
          deliveredAt: new Date(form.value.deliveredAt).toISOString()
        };
        
        if (isEdit.value) {
          payload.id = route.params.id;
          await updateProduct(route.params.id, payload);
          successMessage.value = "Mahsulot muvaffaqiyatli yangilandi!";
        } else {
          await createProduct(payload);
          successMessage.value = "Mahsulot muvaffaqiyatli qo'shildi!";
        }
        setTimeout(() => router.push('/products'), 1500);
      } catch (err) {
        error.value = err.response?.data?.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        saving.value = false;
      }
    };

    const cancel = () => router.back();

    onMounted(loadData);

    return { isEdit, form, organizations, saving, error, successMessage, saveProduct, cancel };
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
.button-row { display: flex; gap: 1rem; margin-top: 2rem; }
label { display: block; font-weight: 600; color: #1e293b; margin-bottom: 0.5rem; }
input {
  width: 100%; padding: 0.75rem; border: 1px solid #cbd5e1;
  border-radius: 0.5rem; font-size: 1rem; transition: border-color 0.2s;
}
input:focus { outline: none; border-color: #2563eb; }
button {
  background: #2563eb; color: white; padding: 0.85rem 1.5rem;
  border-radius: 0.75rem; border: none; font-weight: 600; cursor: pointer; flex: 1;
}
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