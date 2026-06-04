<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div>
        <h2>{{ isEdit ? 'Kirim hujjatini tahrirlash' : 'Yangi kirim hujjati yaratish' }}</h2>
        <p>{{ isEdit ? 'Kirim hujjati ma\'lumotlarini o\'zgartiring.' : 'Yangi kirim hujjati qo\'shish uchun ma\'lumotlarni kiriting.' }}</p>
      </div>
    </div>

    <form class="entity-form" @submit.prevent="saveDocument">
      <div class="form-header-grid">
        <label>
          Ta'minotchi
          <select v-model="form.supplierId" required>
            <option :value="null" disabled>Ta'minotchini tanlang</option>
            <option v-for="supplier in suppliers" :key="supplier.id || supplier.Id" :value="supplier.id || supplier.Id">
              {{ supplier.fullName || supplier.code || supplier.name }}
            </option>
          </select>
        </label>
        <label>
          Sana
          <input v-model="form.date" type="date" required />
        </label>
        <label>
          Status
          <select v-model.number="form.status" :disabled="!isEdit" required>
            <option :value="1">Yaratildi</option>
            <option :value="2">Tasdiqlandi</option>
            <option :value="3">Rad etildi</option>
            <option :value="4">O'zgartirildi</option>
          </select>
        </label>
      </div>

      <div class="items-section">
        <h3>Mahsulotlar tarkibi</h3>
        <table class="items-table">
          <thead>
            <tr>
              <th>Mahsulot</th>
              <th style="width: 120px;">Miqdor</th>
              <th style="width: 150px;">Narx</th>
              <th style="width: 150px;">Jami</th>
              <th style="width: 40px;"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(item, index) in form.tables" :key="index">
              <td>
                <select v-model="item.productId" required @change="updateItemName(item)">
                  <option :value="null" disabled>Tanlang</option>
                  <option v-for="p in products" :key="p.id || p.Id" :value="p.id || p.Id">{{ p.name }}</option>
                </select>
              </td>
              <td><input v-model.number="item.quantity" type="number" step="any" min="0.01" required /></td>
              <td><input v-model.number="item.price" type="number" step="any" min="0" required /></td>
              <td class="item-total">{{ formatSum(item.quantity * item.price) }}</td>
              <td>
                <button type="button" class="btn-remove" @click="removeItem(index)">&times;</button>
              </td>
            </tr>
          </tbody>
        </table>
        <button type="button" class="btn-add-item" @click="addItem">+ Mahsulot qo'shish</button>
      </div>

      <div class="form-footer">
        <div class="overall-total">
          Jami summa: <span>{{ formatSum(calculateTotalSum) }}</span>
        </div>
      </div>

      <div class="button-row">
        <button type="submit" class="btn-save" :disabled="isSaving || form.tables.length === 0">
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
import { incomeDocumentService } from './incomeDocumentService';

export default {
  name: 'IncomeDocumentFormView',
  setup() {
    const route = useRoute();
    const router = useRouter();
    const error = ref('');
    const successMessage = ref('');
    const isSaving = ref(false);
    const suppliers = ref([]);
    const products = ref([]);

    const form = ref({
      id: null,
      supplierId: null,
      date: new Date().toISOString().slice(0, 10),
      status: 1, // CREATED = 1
      tables: [] // Array of items
    });

    const isEdit = computed(() => !!route.params.id);

    const loadMetaData = async () => {
      try {
        const [sRes, pRes] = await Promise.all([
          incomeDocumentService.getSuppliers(),
          incomeDocumentService.getProducts()
        ]);
        // Robust check for array data in different potential response formats
        const extract = (res) => res.data?.data || res.data?.items || (Array.isArray(res.data) ? res.data : []);
        suppliers.value = extract(sRes);
        products.value = extract(pRes);
      } catch (err) {
        console.error('Metadata yuklashda xato:', err);
      }
    };

    const loadDocument = async () => {
      if (!isEdit.value) return;
      try {
        const response = await incomeDocumentService.getById(route.params.id);
        const data = response.data?.data || response.data;
        if (data) {
          form.value = {
            id: data.id ?? data.Id ?? data.ID,
            supplierId: data.supplierId ?? data.SupplierId ?? data.SupplierID,
            // Sanani yuklashda barcha variantlarni tekshiramiz
            date: (data.docOn || data.DocOn || data.date || '').slice(0, 10),
            status: data.statusId ?? data.StatusId ?? data.status ?? 1,
            // Bola table (tables) elementlarini IDsi bilan birga yuklaymiz
            tables: (data.tables || data.Tables || []).map(t => ({
              id: t.id ?? t.Id ?? t.ID ?? 0,
              productId: t.productId ?? t.ProductId ?? t.ProductID,
              quantity: t.quantity ?? t.Quantity ?? 0,
              price: t.price ?? t.Price ?? 0
            }))
          };
        } else {
          error.value = 'Hujjat topilmadi.';
        }
      } catch (err) {
        console.error('Error loading document:', err);
        error.value = err.response?.data?.message || err.message || 'Hujjat ma\'lumotlarini yuklashda xatolik yuz berdi.';
      }
    };

    const addItem = () => {
      form.value.tables.push({
        productId: null,
        quantity: 1,
        price: 0
      });
    };

    const removeItem = (index) => {
      form.value.tables.splice(index, 1);
    };

    const calculateTotalSum = computed(() => {
      return form.value.tables.reduce((sum, item) => {
        const q = parseFloat(item.quantity) || 0;
        const p = parseFloat(item.price) || 0;
        return sum + (q * p);
      }, 0);
    });

    const formatSum = (val) => new Intl.NumberFormat('uz-UZ').format(val || 0) + ' UZS';

    const updateItemName = (item) => {
      const p = products.value.find(x => x.id === item.productId);
      if (p) item.productName = p.name;
    };

    const saveDocument = async () => {
      error.value = '';
      successMessage.value = '';
      isSaving.value = true;
      try {
        const payload = {
          Id: isEdit.value ? (form.value.id || 0) : 0,
          SupplierId: parseInt(form.value.supplierId),
          DocOn: new Date(form.value.date + 'T00:00:00Z').toISOString(), // Sanani UTC ISO formatida yuborish
          StatusId: isEdit.value ? parseInt(form.value.status) : 1, // Yangi hujjat uchun har doim 1 (Yaratildi)
          DocSum: calculateTotalSum.value, // Ota jami summasi = bola jadvallar yig'indisi
          tables: form.value.tables.map(t => ({
            Id: isEdit.value ? (t.id || 0) : 0,
            ProductId: parseInt(t.productId),
            Price: parseFloat(t.price),
            Quantity: parseFloat(t.quantity),
            TotalSum: parseFloat(t.price) * parseFloat(t.quantity) // Bola table qator summasi
          }))
        };

        if (isEdit.value) {
          await incomeDocumentService.update(payload);
          successMessage.value = 'Hujjat muvaffaqiyatli yangilandi!';
        } else {
          await incomeDocumentService.create(payload);
          successMessage.value = 'Hujjat muvaffaqiyatli yaratildi!';
        }
        
        setTimeout(() => {
          router.push('/income-documents');
        }, 1500);
      } catch (err) {
        console.error('Error saving document:', err);
        error.value = err.response?.data?.message || err.response?.data?.title || err.message || 'Saqlashda xatolik yuz berdi.';
      } finally {
        isSaving.value = false;
      }
    };

    const cancel = () => router.push('/income-documents');

    onMounted(async () => {
      await loadMetaData();
      await loadDocument();
      if (form.value.tables.length === 0 && !isEdit.value) {
        addItem();
      }
    });

    return {
      form,
      isEdit,
      calculateTotalSum,
      addItem,
      removeItem,
      formatSum,
      updateItemName,
      successMessage,
      isSaving,
      error,
      suppliers,
      products,
      saveDocument,
      cancel
    };
  }
};
</script>

<style scoped>
.page-card {
  background: #0d1117;
  padding: 1.5rem;
  border-radius: 1rem;
}

.wide-card {
  max-width: 800px;
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  padding-bottom: 1rem;
}

.entity-form {
  background: #111827;
  padding: 1.5rem;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
  margin-bottom: 1.5rem;
}

.form-header-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.items-section {
  margin-top: 2rem;
}

.items-section h3 {
  font-size: 16px;
  color: #f1f5f9;
  margin-bottom: 1rem;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 1rem;
}

.items-table th {
  text-align: left;
  font-size: 11px;
  color: #475569;
  text-transform: uppercase;
  padding: 8px;
  background-color: #0f172a;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
}

.items-table td {
  padding: 10px 8px;
  vertical-align: middle;
  border-bottom: 1px solid rgba(255, 255, 255, 0.03);
}

.items-table tbody tr:hover {
  background-color: rgba(255, 255, 255, 0.04) !important;
  transition: background-color 0.2s ease;
}

.item-total {
  color: #94a3b8;
  font-size: 14px;
  font-weight: 600;
}

.btn-add-item {
  background: rgba(59, 130, 246, 0.1);
  color: #3b82f6;
  border: 1px dashed #3b82f6;
  padding: 8px 16px;
  border-radius: 6px;
  cursor: pointer;
  font-weight: 600;
}

.btn-remove {
  background: rgba(239, 68, 68, 0.1);
  border: 1px solid rgba(239, 68, 68, 0.2);
  color: #ef4444;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 16px;
  transition: all 0.2s ease;
  margin: 0 auto;
}

.btn-remove:hover {
  background: #ef4444;
  color: white;
  transform: rotate(90deg);
}

label {
  display: block;
  font-weight: 600;
  color: #94a3b8;
  margin-bottom: 0.5rem;
}

input, select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.08);
  background: rgba(255, 255, 255, 0.05);
  color: #f1f5f9;
  border-radius: 0.4rem;
  font-size: 13.5px;
  transition: border-color 0.2s;
}

input:focus, select:focus {
  outline: none;
  border-color: #2563eb;
  background: #1e293b !important; /* Global !important stilni bekor qilish */
}

select option {
  background: #1e293b;
  color: #f1f5f9;
}

.form-footer {
  margin-top: 2rem;
  padding-top: 1rem;
  border-top: 1px solid rgba(255, 255, 255, 0.07);
  text-align: right;
}

.overall-total {
  font-size: 18px;
  color: #94a3b8;
}

.overall-total span {
  color: #f1f5f9;
  font-weight: 700;
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
  background: linear-gradient(135deg, #60a5fa, #2563eb);
}

.btn-save:active:not(:disabled) {
  transform: translateY(0);
}

.btn-save:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  filter: grayscale(0.5);
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

@keyframes spin {
  to { transform: rotate(360deg); }
}

.error {
  color: #dc2626;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: rgba(220, 38, 38, 0.1);
  border-radius: 0.5rem;
  border: 1px solid rgba(220, 38, 38, 0.2);
}

.success {
  color: #16a34a;
  margin-top: 1.5rem;
  padding: 0.75rem;
  background: rgba(22, 163, 74, 0.1);
  border: 1px solid rgba(22, 163, 74, 0.2);
}
</style>