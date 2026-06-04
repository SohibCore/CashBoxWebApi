<template>
  <div class="container mx-auto p-6 max-w-4xl">
    <div class="bg-white rounded-lg shadow-xl p-8">
      <h2 class="text-3xl font-bold mb-8 text-gray-800">
        {{ isEdit ? 'Hujjatni tahrirlash' : 'Yangi Chiqim Hujjati' }}
      </h2>

      <form @submit.prevent="saveDocument" class="space-y-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <!-- Sana -->
          <div>
            <label class="block text-sm font-semibold mb-1">Sana</label>
            <input 
              v-model="form.dateStr" 
              type="text" 
              placeholder="31.12.2023"
              class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
              required
            />
          </div>

          <!-- Ta'minotchi -->
          <div>
            <label class="block text-sm font-semibold mb-1">Ta'minotchi</label>
            <select 
              v-model.number="form.supplierId" 
              class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
              required
            >
              <option value="" disabled>Ta'minotchini tanlang</option>
              <option v-for="s in suppliers" :key="s.id" :value="s.id">{{ s.code || s.inn }}</option>
            </select>
          </div>

          <!-- Mahsulot -->
          <div>
            <label class="block text-sm font-semibold mb-1">Mahsulot</label>
            <select 
              v-model.number="form.productId" 
              class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none bg-white"
              required
            >
              <option value="" disabled>Mahsulotni tanlang</option>
              <option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option>
            </select>
          </div>

          <!-- Narx -->
          <div>
            <label class="block text-sm font-semibold mb-1">Narxi</label>
            <input v-model.number="form.price" @input="calculateTotal" type="number" step="0.01" class="w-full px-4 py-2 border rounded-lg" required />
          </div>

          <!-- Miqdor -->
          <div>
            <label class="block text-sm font-semibold mb-1">Miqdori</label>
            <input v-model.number="form.quantity" @input="calculateTotal" type="number" step="0.01" class="w-full px-4 py-2 border rounded-lg" required />
          </div>

          <!-- Jami Summa -->
          <div>
            <label class="block text-sm font-semibold mb-1">Jami summa</label>
            <input v-model.number="form.totalSum" type="number" readonly class="w-full px-4 py-2 border rounded-lg bg-gray-50 font-bold" />
          </div>

          <!-- Status — faqat edit rejimida -->
          <div v-if="isEdit" class="md:col-span-2">
            <label class="block text-sm font-semibold mb-1">Status</label>
            <select v-model.number="form.status" class="w-full px-4 py-2 border rounded-lg bg-white focus:ring-2 focus:ring-blue-500 outline-none" required>
              <option :value="0">⏳ To'lanmagan</option>
              <option :value="1">✅ To'langan</option>
            </select>
          </div>
        </div>

        <div class="flex gap-4 pt-6">
          <button type="submit" :disabled="saving" class="flex-1 bg-green-600 text-white py-3 rounded-lg font-bold hover:bg-green-700 disabled:opacity-50 transition">
            {{ saving ? 'Saqlanmoqda...' : 'Saqlash' }}
          </button>
          <button type="button" @click="$router.push('/outcome-documents')" class="flex-1 bg-red-500 text-white py-3 rounded-lg font-bold hover:bg-red-600 transition">
            Bekor qilish
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';

const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.params.id);
const saving = ref(false);
const suppliers = ref([]);
const products = ref([]);
const api = axios.create({ baseURL: 'http://localhost:5107' });
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

const form = ref({
  dateStr: new Date().toLocaleDateString('ru-RU'),
  supplierId: '',
  productId: '',
  price: 0,
  quantity: 0,
  totalSum: 0,
  status: 0
});

const calculateTotal = () => { form.value.totalSum = Number((form.value.price * form.value.quantity).toFixed(2)); };

const loadData = async () => {
  try {
    const [sRes, pRes] = await Promise.all([api.get('/api/Supplier/GetList'), api.get('/api/Product/GetList')]);
    suppliers.value = sRes.data?.data || sRes.data || [];
    products.value = pRes.data?.data || pRes.data || [];

    if (isEdit.value) {
      const res = await api.get(`/api/OutcomeDocument/Get/${route.params.id}`);
      const data = res.data;
      if (data) {
        const date = data.date ? new Date(data.date) : new Date();
        form.value = { ...data, dateStr: date.toLocaleDateString('ru-RU') };
      }
    }
  } catch (err) { console.error('Load error:', err); }
};

const saveDocument = async () => {
  saving.value = true;
  try {
    const [day, month, year] = form.value.dateStr.split('.');
    const isoDate = new Date(`${year}-${month}-${day}`).toISOString();
    const payload = { ...form.value, date: isoDate };
    delete payload.dateStr;

    if (isEdit.value) { await api.put(`/api/OutcomeDocument/Update?id=${route.params.id}`, payload); }
    else { await api.post('/api/OutcomeDocument/Create', payload); }
    router.push('/outcome-documents');
  } catch (err) { alert('Saqlashda xatolik: ' + (err.response?.data?.message || err.message)); }
  finally { saving.value = false; }
};

onMounted(loadData);
</script>