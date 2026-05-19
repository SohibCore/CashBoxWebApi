<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Valyutani tahrirlash' : 'Yangi valyuta yaratish' }}</h2>
        <p>{{ isEdit ? 'Valyuta ma\'lumotlarini o\'zgartiring.' : 'Yangi valyuta qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveCurrency">
      <div class="form-grid">
        <label>
          Valyuta kodi
          <input v-model="form.code" type="text" placeholder="Masalan: 001" required />
        </label>
        <label>
          Valyuta nomi (To'liq)
          <input v-model="form.name" type="text" placeholder="Masalan: O'zbek so'mi" required />
        </label>
        <label>
          Qisqa nomi / Simvoli
          <input v-model="form.symbol" type="text" placeholder="Masalan: SUM" required />
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
      name: '',
      code: '',
      symbol: ''
    });

    const isEdit = computed(() => !!route.params.id);

    // Ma'lumotlarni yuklash
    const loadCurrency = async () => {
      if (!isEdit.value) return;
      try {
        const response = await getCurrencyById(route.params.id); // getCurrencyById orqali ma'lumotni yuklash
        const item = response.data?.data || response.data;
        if (item) {
          // API dan kelgan maydonlarni bizning formaga xaritlaymiz
          form.value = { 
            id: item.id || item.Id,
            name: item.name || item.Name,
            code: item.code || item.Code,
            symbol: item.symbol || item.Symbol
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
      isSaving.value = true; // Saqlash holatini yoqish
      try {
        console.log('Saving currency with payload:', form.value); // Yuborilayotgan ma'lumotni log qilish
        if (isEdit.value) {
          // Tahrirlash (PUT request)
          await updateCurrency(form.value.id, form.value);
          successMessage.value = 'Valyuta muvaffaqiyatli yangilandi!';
        } else {
          // Yangi yaratish (POST request)
          await createCurrency(form.value);
          successMessage.value = 'Valyuta muvaffaqiyatli yaratildi!';
          form.value = { id: null, name: '', code: '', symbol: '' }; // Yaratilgandan so'ng formani tozalash
        }
        // Muvaffaqiyatli saqlangach, qisqa muddatdan so'ng ro'yxat sahifasiga qaytish
        setTimeout(() => {
          router.push('/currencies');
        }, 1500);
      } catch (err) {
        console.error('Error saving currency:', err); // To'liq xatolikni konsolga chiqarish
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
  /* Ko'k romka huddi boshqa modern formalardek */
  border: 1px solid #2563eb;
  box-shadow: 0 4px 15px rgba(37, 99, 235, 0.1);
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

.button-row {
  display: flex;
  gap: 1rem;
  margin-top: 2rem;
}

.btn-secondary {
  background: #e2e8f0;
  color: #475569;
  border: 1px solid #cbd5e1;
}

.btn-secondary:hover {
  background: #cbd5e1;
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