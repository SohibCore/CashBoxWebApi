<template>
  <div class="p-6 max-w-5xl mx-auto">
    <div class="bg-white rounded-lg shadow-lg p-8">
      <h2 class="text-2xl font-bold mb-6 text-gray-800">
        {{ isEdit ? 'Hujjatni tahrirlash' : 'Yangi Kirim Hujjati' }}
      </h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
        <div>
          <label class="block text-sm font-semibold mb-1 text-gray-600">Sana (DD.MM.YYYY)</label>
          <input v-model="form.dateStr" type="text" class="w-full px-4 py-2 border rounded shadow-sm focus:ring-2 focus:ring-blue-500 outline-none" placeholder="01.01.2024" />
        </div>
        <div>
          <label class="block text-sm font-semibold mb-1 text-gray-600">Ta'minotchi</label>
          <select v-model.number="form.supplierId" class="w-full px-4 py-2 border rounded shadow-sm bg-white outline-none">
            <option value="" disabled>Tanlang</option>
            <option v-for="s in suppliers" :key="s.id" :value="s.id">{{ s.name || s.code || s.inn }}</option>
          </select>
        </div>
      </div>

      <div class="mb-6">
        <div class="flex justify-between items-center mb-4">
          <h3 class="text-lg font-bold">Mahsulotlar</h3>
          <button @click="addRow" type="button" class="bg-green-600 text-white px-3 py-1 rounded text-sm hover:bg-green-700">
            + Qator qo'shish
          </button>
        </div>

        <table class="w-full border-collapse">
          <thead>
            <tr class="bg-gray-100 border-b text-sm">
              <th class="p-2 text-left w-1/3">Mahsulot</th>
              <th class="p-2 text-left">Narxi</th>
              <th class="p-2 text-left w-24">Miqdori</th>
              <th class="p-2 text-left">Jami</th>
              <th class="p-2 text-center w-16"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in form.tables" :key="index" class="border-b hover:bg-gray-50">
              <td class="p-2">
                <select v-model.number="item.productId" class="w-full p-1 border rounded bg-white" required>
                  <option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option>
                </select>
              </td>
              <td class="p-2">
                <input v-model.number="item.price" @input="updateRow(item)" type="number" class="w-full p-1 border rounded" />
              </td>
              <td class="p-2">
                <input v-model.number="item.quantity" @input="updateRow(item)" type="number" class="w-full p-1 border rounded" />
              </td>
              <td class="p-2 font-semibold">
                {{ (item.price * item.quantity).toLocaleString() }}
              </td>
              <td class="p-2 text-center">
                <button @click="removeRow(index)" class="text-red-500 hover:text-red-700 font-bold">&times;</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="flex flex-col items-end border-t pt-4">
        <div class="text-gray-600">Jami hujjat summasi:</div>
        <div class="text-3xl font-bold text-blue-700">{{ totalDocSum.toLocaleString() }}</div>
      </div>

      <div class="flex gap-4 mt-8">
        <button @click="save" :disabled="saving" class="flex-1 bg-blue-600 text-white py-3 rounded-lg font-bold hover:bg-blue-700 transition disabled:opacity-50">
          {{ saving ? 'Saqlanmoqda...' : 'Hujjatni saqlash' }}
        </button>
        <button @click="$router.push('/income-documents')" class="bg-gray-500 text-white px-8 py-3 rounded-lg font-bold hover:bg-gray-600">
          Bekor qilish
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { incomeDocumentService } from './incomeDocumentService';

const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.params.id);
const saving = ref(false);
const suppliers = ref([]);
const products = ref([]);

const form = ref({
  dateStr: new Date().toLocaleDateString('ru-RU'),
  supplierId: '',
  docSum: 0,
  tables: []
});

const totalDocSum = computed(() => {
  return form.value.tables.reduce((acc, curr) => acc + (curr.price * curr.quantity), 0);
});

const addRow = () => {
  form.value.tables.push({ id: 0, productId: '', price: 0, quantity: 1, totalSum: 0 });
};

const removeRow = (index) => {
  form.value.tables.splice(index, 1);
};

const updateRow = (item) => {
  item.totalSum = item.price * item.quantity;
};

const loadMetaData = async () => {
  try {
    const [sRes, pRes] = await Promise.all([incomeDocumentService.getSuppliers(), incomeDocumentService.getProducts()]);
    suppliers.value = Array.isArray(sRes.data) ? sRes.data : (sRes.data?.data || []);
    products.value = Array.isArray(pRes.data) ? pRes.data : (pRes.data?.data || []);

    if (isEdit.value) {
      const res = await incomeDocumentService.getById(route.params.id as string);
      const data = res.data;
      form.value = { 
        ...data, 
        dateStr: new Date(data.docOn).toLocaleDateString('ru-RU'),
        // Backend IncomeDocumentTableDto dan IncomeDocumentTableDlDto ga o'tkazish
        tables: data.tables.map(t => ({ ...t }))
      };
    } else {
      addRow(); // Yangi bo'lsa bitta qator bo'sh tursin
    }
  } catch (err) { console.error(err); }
};

const save = async () => {
  if (!form.value.supplierId || form.value.tables.length === 0) {
    alert('Iltimos barcha maydonlarni to\'ldiring');
    return;
  }
  
  saving.value = true;
  try {
    const [d, m, y] = form.value.dateStr.split('.');
    // Backend kutayotgan Update yoki Create DTO strukturasi
    const payload = {
      id: form.value.id || 0,
      docOn: new Date(`${y}-${m}-${d}T12:00:00Z`).toISOString(),
      supplierId: form.value.supplierId,
      docSum: totalDocSum.value, 
      tables: form.value.tables.map(t => ({
        id: t.id || 0,
        productId: t.productId,
        price: t.price,
        quantity: t.quantity,
        totalSum: t.price * t.quantity
      }))
    };

    if (isEdit.value) await incomeDocumentService.update(payload);
    else await incomeDocumentService.create(payload);
    
    router.push('/income-documents');
  } catch (err) { alert('Xatolik yuz berdi'); }
  finally { saving.value = false; }
};

onMounted(loadMetaData);
</script>