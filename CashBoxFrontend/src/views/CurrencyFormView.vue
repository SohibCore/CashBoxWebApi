<template>
  <div class="page-card wide-card">
    <NavigationHistory />
    
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Valyutani tahrirlash' : 'Yangi valyuta yaratish' }}</h2>
        <p>{{ isEdit ? 'Valyuta ma\'lumotlarini o\'zgartiring.' : 'Yangi valyuta qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveCurrency">
      <div class="form-grid">
        <label>
          Valyuta kodi (Code)
          <input v-model="form.code" type="text" placeholder="Masalan: USD" required />
        </label>
        <label>
          Valyuta nomi (To'liq nomi)
          <input v-model="form.fullName" type="text" placeholder="Masalan: Amerika Qo'shma Shtatlari Dollari" required />
        </label>
        <label>
          Qisqa nomi / Simvoli (ShortName)
          <input v-model="form.shortName" type="text" placeholder="Masalan: $" required />
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
import NavigationHistory from './NavigationHistory.vue';
import { getCurrencyById, createCurrency, updateCurrency } from '../api'; // getCurrencies olib tashlandi, getCurrencyById qo'shildi

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const error = ref('');
    const successMessage = ref('');
    const isSaving = ref(false);

    const form = ref({
      id: null,
      code: '',
      fullName: '',
      shortName: ''
    });

    const isEdit = computed(() => !!route.params.id);

    // Ma'lumotlarni yuklash
    const loadCurrency = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getCurrencyById(route.params.id);
        const item = response.data?.data || response.data;
        if (item) {
          form.value = { 
            id: item.id || item.Id, // ID ni saqlab qolish muhim
            code: item.code || item.Code,
            fullName: item.fullName || item.FullName,
            shortName: item.shortName || item.ShortName
          };
        } else {
          error.value = 'Valyuta topilmadi.';
        }
      } catch (err) {
        console.error('Error loading currency:', err); // Xatolikni konsolga chiqarish
        error.value = err.response?.data?.message || err.message || 'Valyuta ma\'lumotlarini yuklashda xatolik yuz berdi.';
      }
    };

    const saveCurrency = async () => {
      error.value = '';
      successMessage.value = '';
      isSaving.value = true;
      try {
        if (isEdit.value) {
          await updateCurrency(route.params.id, form.value);
          successMessage.value = 'Valyuta muvaffaqiyatli yangilandi!';
        } else {
          // Yangi yaratish - POST API bilan bog'lanish
          await createCurrency(form.value);
          successMessage.value = 'Valyuta muvaffaqiyatli yaratildi!';
          form.value = { id: null, code: '', fullName: '', shortName: '' }; // Formani tozalash
        }
        
        setTimeout(() => {
          router.push('/currencies');
        }, 1500);
      } catch (err) {
        console.error('Error saving currency:', err);
        error.value = err.response?.data?.message || err.response?.data?.title || err.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        isSaving.value = false; // Saqlash holatini o'chirish
      }
    };

    const cancel = () => router.push('/currencies');

    onMounted(loadCurrency);

    return {
      form,
      isEdit,
      successMessage,
      isSaving,
      saveCurrency,
      cancel,
      error
    };
  }
};
</script>

<style scoped>
.page-card {
  background: #111827;
  padding: 1.5rem;
  border-radius: 1rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
}

.wide-card {
  max-width: 800px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 2rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  padding-bottom: 1rem;
}

.section-header h2 { color: #f1f5f9; }
.section-header p { color: #94a3b8; }

.entity-form {
  background: rgba(255, 255, 255, 0.02);
  padding: 2rem;
  border-radius: 1rem;
  /* Ko'k romka huddi boshqa modern formalardek */
  border: 1px solid #2563eb;
  box-shadow: 0 4px 15px rgba(37, 99, 235, 0.05);
  margin-bottom: 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 1.5rem;
}

label {
  display: block;
  font-weight: 600;
  color: #94a3b8;
  margin-bottom: 0.5rem;
}

input {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.5rem;
  font-size: 1rem;
  transition: border-color 0.2s;
}

input:focus {
  outline: none;
  border-color: #2563eb;
  background: #1e293b !important;
}

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

.button-row button[type="submit"] {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  color: white;
  font-weight: 600;
}

.btn-secondary {
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: all 0.2s ease;
}

.btn-secondary:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #f1f5f9;
  border-color: rgba(255, 255, 255, 0.2);
}

.error {
  color: #ef4444;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: rgba(220, 38, 38, 0.1);
  border-radius: 0.5rem;
  border: 1px solid rgba(220, 38, 38, 0.2);
}

.success {
  color: #10b981;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: rgba(16, 185, 129, 0.1);
  border-radius: 0.5rem;
  border: 1px solid rgba(16, 185, 129, 0.2);
}
</style>