<template>
  <div class="container mx-auto p-6 max-w-4xl">
    <div class="bg-white rounded-lg shadow-xl p-8">
      <h2 class="text-3xl font-bold mb-8 text-gray-800">
        {{ isEdit ? 'Hujjatni tahrirlash' : 'Yangi Kirim Hujjati' }}
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
              <option v-for="s in suppliers" :key="s.id" :value="s.id">{{ s.code }}</option>
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
            <input 
              v-model.number="form.price" 
              @input="calculateTotal"
              type="number" 
              step="0.01"
              class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
              required
            />
          </div>

          <!-- Miqdor -->
          <div>
            <label class="block text-sm font-semibold mb-1">Miqdori</label>
            <input 
              v-model.number="form.quantity" 
              @input="calculateTotal"
              type="number" 
              step="0.01"
              class="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500 outline-none"
              required
            />
          </div>

          <!-- Jami Summa -->
          <div class="md:col-span-2">
            <label class="block text-sm font-semibold mb-1">Jami summa</label>
            <input 
              v-model.number="form.totalSum" 
              type="number" 
              readonly
              class="w-full px-4 py-2 border rounded-lg bg-gray-50 text-gray-600 outline-none font-bold"
            />
          </div>
        </div>

        <div class="flex gap-4 pt-6">
          <button 
            type="submit" 
            :disabled="saving"
            class="flex-1 bg-green-600 text-white py-3 rounded-lg font-bold hover:bg-green-700 disabled:opacity-50 transition"
          >
            {{ saving ? 'Saqlanmoqda...' : 'Saqlash' }}
          </button>
          <button 
            type="button" 
            @click="$router.push('/income-documents')"
            class="flex-1 bg-red-500 text-white py-3 rounded-lg font-bold hover:bg-red-600 transition"
          >
            Bekor qilish
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { 
  getIncomeDocumentById, createIncomeDocument, updateIncomeDocument, 
  getSuppliers, getProducts, extractApiData 
} from '../api';

export default {
  setup() {
    const route = useRoute();
    const router = useRouter();
    const isEdit = computed(() => !!route.params.id);
    const saving = ref(false);

    const suppliers = ref([]);
    const products = ref([]);

    const form = ref({
      dateStr: new Date().toLocaleDateString('ru-RU'),
      supplierId: '',
      productId: '',
      price: 0,
      quantity: 0,
      totalSum: 0
    });

    const calculateTotal = () => {
      const total = form.value.price * form.value.quantity;
      form.value.totalSum = Number(total.toFixed(2));
    };

    const loadData = async () => {
      suppliers.value = extractApiData(await getSuppliers());
      products.value = extractApiData(await getProducts());

      if (isEdit.value) {
        const res = await getIncomeDocumentById(route.params.id);
        const data = extractApiData(res);
        
        const getVal = (obj, keys) => {
          for (const k of keys) {
            if (obj[k] !== undefined && obj[k] !== null) return obj[k];
          }
          return null;
        };

        const dateValue = getVal(data, ['date', 'Date']);
        const date = dateValue ? new Date(dateValue) : new Date();

        form.value = {
          ...data,
          price: parseFloat(getVal(data, ['price', 'Price']) || 0),
          quantity: parseFloat(getVal(data, ['quantity', 'Quantity']) || 0),
          totalSum: parseFloat(getVal(data, ['totalSum', 'TotalSum']) || 0),
          dateStr: date.toLocaleDateString('ru-RU')
        };
      }
    };

    const saveDocument = async () => {
      saving.value = true;
      try {
        const [day, month, year] = form.value.dateStr.split('.');
        const isoDate = new Date(`${year}-${month}-${day}`).toISOString();
        
        const payload = { ...form.value, date: isoDate };
        delete payload.dateStr;

        if (isEdit.value) {
          await updateIncomeDocument(route.params.id, payload);
        } else {
          await createIncomeDocument(payload);
        }
        router.push('/income-documents');
      } catch (err) {
        alert('Saqlashda xatolik: ' + err.message);
      } finally {
        saving.value = false;
      }
    };

    onMounted(loadData);

    return { form, suppliers, products, isEdit, saving, calculateTotal, saveDocument };
  }
};
</script>