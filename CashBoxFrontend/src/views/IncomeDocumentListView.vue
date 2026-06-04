<template>
  <div class="p-6">
    <div class="bg-white rounded-lg shadow p-6">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-2xl font-bold">Kirim Hujjatlari</h2>
        <button @click="$router.push('/income-documents/create')" class="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          + Yangi hujjat
        </button>
      </div>

      <!-- Asosiy Jadval -->
      <div class="overflow-x-auto">
        <table class="w-full text-left border-collapse">
          <thead>
            <tr class="bg-gray-100 border-b">
              <th class="p-3">ID</th>
              <th class="p-3">Sana</th>
              <th class="p-3">Ta'minotchi</th>
              <th class="p-3">Mahsulot</th>
              <th class="p-3">Miqdori</th>
              <th class="p-3">Narxi</th>
              <th class="p-3">Jami summa</th>
              <th class="p-3">Status</th>
              <th class="p-3">Amallar</th>
            </tr>
          </thead>
          <tbody>
            <tr 
              v-for="doc in documents" 
              :key="doc.id" 
              @dblclick="viewDetails(doc.id)"
              class="border-b hover:bg-blue-50 cursor-pointer transition"
              title="Batafsil ko'rish uchun ikki marta bosing"
            >
              <td class="p-3">#{{ doc.id }}</td>
              <td class="p-3">{{ formatDate(doc.docOn) }}</td>
              <td class="p-3">{{ doc.supplierName }}</td>
              <td class="p-3">{{ (doc as any).productName || '-' }}</td>
              <td class="p-3">{{ (doc as any).quantity || 0 }}</td>
              <td class="p-3">{{ formatCurrency((doc as any).price || 0) }}</td>
              <td class="p-3 font-semibold">{{ formatCurrency(doc.docSum) }}</td>
              <td class="p-3">
                <span :class="getStatusClass(doc.statusId)" class="px-2 py-1 rounded text-xs font-medium">
                  {{ doc.statusName }}
                </span>
              </td>
              <td class="p-3">
                <div class="flex gap-2">
                  <button v-if="doc.statusId !== 3" @click.stop="$router.push('/income-documents/' + doc.id + '/edit')" class="text-blue-600 hover:underline font-medium">Tahrirlash</button>
                  <button v-if="[1,2].includes(doc.statusId)" @click.stop="deleteDoc(doc.id)" class="text-red-600 hover:underline">O'chirish</button>
                  <button @click.stop="viewDetails(doc.id)" class="text-gray-600 hover:underline">Ko'rish</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Detal ko'rinishi (Modal) -->
    <div v-if="selectedDoc || detailLoading" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div v-if="detailLoading" class="bg-white p-8 rounded-lg shadow-xl">
        <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
        <p class="mt-4 text-gray-600">Ma'lumotlar yuklanmoqda...</p>
      </div>
      
      <div v-else class="bg-white rounded-lg shadow-xl w-full max-w-5xl max-h-[90vh] overflow-hidden flex flex-col">
        <div class="p-6 border-b flex justify-between items-center bg-gray-50">
          <div>
            <h3 class="text-xl font-bold text-gray-800">Hujjat ma'lumotlari #{{ selectedDoc.id }}</h3>
            <p class="text-sm text-gray-600">Ta'minotchi: <strong>{{ selectedDoc.supplierName }}</strong> | Sana: <strong class="text-blue-600">{{ formatDate(selectedDoc.docOn) }}</strong></p>
          </div>
          <button @click="closeModal" class="text-gray-400 hover:text-gray-600 text-3xl leading-none">&times;</button>
        </div>
        
        <div class="p-6 overflow-y-auto flex-1">
          <h4 class="font-bold mb-4 text-gray-700 uppercase text-sm tracking-wider">Mahsulotlar ro'yxati</h4>
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-gray-100 border-b text-xs font-bold text-gray-600 uppercase">
                <th class="p-2">Mahsulot</th>
                <th class="p-2 text-right w-32">Narxi</th>
                <th class="p-2 text-center w-24">Miqdori</th>
                <th class="p-2 text-right w-40">Jami</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="item in selectedDoc.tables" :key="item.id" class="border-b"> <!-- Bola table -->
                <td class="p-2 text-gray-700 font-medium">{{ item.productName || 'Mahsulot #' + item.productId }}</td>
                <td class="p-2 text-right">{{ formatCurrency(item.price) }}</td>
                <td class="p-2 text-center">{{ item.quantity }}</td>
                <td class="p-2 text-right font-bold text-gray-900">{{ formatCurrency(item.totalSum) }}</td>
              </tr>
            </tbody>
            <tfoot>
              <tr class="font-black bg-blue-50 text-xl border-t-2 border-blue-200">
                <td colspan="3" class="p-4 text-right text-gray-700">JAMI HUJJAT SUMMASI:</td>
                <td class="p-4 text-right text-blue-700">{{ formatCurrency(selectedDoc.docSum) }}</td>
              </tr>
            </tfoot>
          </table>
        </div>
        
        <div class="p-4 border-t bg-gray-50 flex justify-between items-center px-6">
          <div class="text-sm text-gray-500 italic">* Hujjat holati: {{ selectedDoc.statusName }}</div>
          <div class="flex gap-3">
            <button @click="closeModal" class="bg-gray-200 text-gray-700 px-6 py-2 rounded-lg font-semibold hover:bg-gray-300 transition">Yopish</button>
            <button v-if="selectedDoc.statusId !== 3" @click="handleAccept(selectedDoc.id)" class="bg-green-600 text-white px-6 py-2 rounded-lg font-semibold hover:bg-green-700 transition">Tasdiqlash</button>
            <button v-if="selectedDoc.statusId === 3" @click="handleNotAccept(selectedDoc.id)" class="bg-orange-500 text-white px-6 py-2 rounded-lg font-semibold hover:bg-orange-600 transition">Qaytarish (Not Accept)</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, type Ref } from 'vue';
import { useRouter } from 'vue-router';
import { incomeDocumentService } from './incomeDocumentService';
import { type IncomeDocumentListDto } from '../types/incomeDocument';

const router = useRouter();
const documents: Ref<IncomeDocumentListDto[]> = ref([]);
const selectedDoc = ref(null);
const detailLoading = ref(false);

const fetchList = async () => {
  try {
    const res = await incomeDocumentService.getList();
    // Backend ma'lumotni res.data.data yoki res.data ichida qaytarishini tekshiramiz
    const data = (res.data as any).data || res.data;
    documents.value = Array.isArray(data) ? data : [];
  } catch (err) { console.error('Xatolik:', err); }
};

const viewDetails = async (id) => {
  detailLoading.value = true;
  selectedDoc.value = null;
  try {
    const res = await incomeDocumentService.getById(id);
    selectedDoc.value = res.data;
  } catch (err) { console.error('Yuklashda xatolik:', err); }
  finally { detailLoading.value = false; }
};

const closeModal = () => {
  selectedDoc.value = null;
  detailLoading.value = false;
};

const handleAccept = async (id) => {
  if (!confirm('Ushbu hujjatni tasdiqlaysizmi?')) return;
  try {
    await incomeDocumentService.accept(id);
    closeModal();
    fetchList();
  } catch (err) { alert('Tasdiqlashda xatolik'); }
};

const handleNotAccept = async (id) => {
  if (!confirm('Ushbu hujjatni "Tasdiqlanmagan" holatiga qaytarmoqchimisiz?')) return;
  try {
    await incomeDocumentService.notAccept(id);
    closeModal();
    fetchList();
  } catch (err) { alert('Amalni bajarishda xatolik'); }
};

const deleteDoc = async (id) => {
  if (!confirm('Hujjatni o\'chirmoqchimisiz?')) return;
  try {
    await incomeDocumentService.delete(id);
    fetchList();
  } catch (err) { alert('O\'chirishda xatolik'); }
};

const formatDate = (dateStr) => {
  if (!dateStr) return '';
  const date = new Date(dateStr);
  return date.toLocaleDateString('ru-RU'); // DD.MM.YYYY
};

const formatCurrency = (val) => {
  return new Intl.NumberFormat('ru-RU').format(val || 0);
};

const getStatusClass = (id) => {
  switch (id) {
    case 1: return 'bg-blue-900/30 text-blue-400 border border-blue-500/30';     // CREATED
    case 2: return 'bg-green-900/30 text-green-400 border border-green-500/30';   // ACCEPT
    case 3: return 'bg-red-900/30 text-red-400 border border-red-500/30';         // NOT_ACCEPT
    case 4: return 'bg-purple-900/30 text-purple-400 border border-purple-500/30'; // MODIFIED
    case 5: return 'bg-gray-900/30 text-gray-400 border border-gray-500/30';      // DELETE
    default: return 'bg-gray-800 text-gray-400';
  }
};

onMounted(fetchList);
</script>

<style scoped>
.cursor-pointer {
  user-select: none;
}
table th {
  font-weight: 600;
  color: #374151;
}
</style>