<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div class="header-centered">
        <h2>Mahsulot qatorini tahrirlash</h2>
        <p>Hujjat tarkibidagi mahsulot miqdori va narxini o'zgartiring.</p>
      </div>
    </div>

    <form v-if="row" class="entity-form" @submit.prevent="saveItem">
      <div class="form-grid">
        <label>
          Mahsulot
          <select v-model="row.productId" disabled>
            <option v-for="p in products" :key="p.id || p.Id" :value="p.id || p.Id">{{ p.name }}</option>
          </select>
        </label>
        
        <div class="input-group">
          <label>
            Miqdor
            <input v-model.number="row.quantity" type="number" step="any" min="0.01" required />
          </label>
          <label>
            Narx
            <input v-model.number="row.price" type="number" step="any" min="0" required />
          </label>
        </div>

        <div class="row-total-display">
          Jami qator summasi: <strong>{{ formatSum(row.quantity * row.price) }}</strong>
        </div>
      </div>

      <div class="button-row">
        <button type="submit" class="btn-save" :disabled="isSaving">
          <span v-if="isSaving" class="spinner"></span>
          <span>{{ isSaving ? 'Saqlanmoqda...' : 'Yangilash' }}</span>
        </button>
        <button type="button" class="btn-cancel" @click="cancel">
          Bekor qilish
        </button>
      </div>
    </form>

    <div v-else-if="loading" class="loading-state">Yuklanmoqda...</div>
    <div v-else class="error-state">Ma'lumot topilmadi yoki yuklashda xato yuz berdi.</div>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { outcomeDocumentService } from './outcomeDocumentService';

const route = useRoute();
const router = useRouter();
const isSaving = ref(false);
const loading = ref(true);
const error = ref('');
const products = ref([]);
const row = ref(null);

const formatSum = (val) => new Intl.NumberFormat('uz-UZ').format(val || 0) + ' UZS';

const loadData = async () => {
  loading.value = true;
  try {
    const tableId = Number(route.params.id);
    const docId = route.query.docId;

    const [pRes, rRes] = await Promise.all([
      outcomeDocumentService.getProducts(),
      outcomeDocumentService.getById(docId)
    ]);
    
    products.value = pRes.data?.data || pRes.data?.value || pRes.data || [];
    
    const doc = rRes.data?.data || rRes.data?.value || rRes.data;
    const tables = doc?.tables || doc?.Tables || [];
    const found = tables.find(t => (t.id || t.Id) === tableId);

    if (found) {
      row.value = {
        id: found.id || found.Id,
        productId: found.productId || found.ProductId,
        quantity: found.quantity || found.Quantity,
        price: found.price || found.Price
      };
    }
  } catch (err) {
    console.error('Yuklashda xatolik:', err);
    error.value = 'Ma\'lumotlarni yuklashda xatolik yuz berdi.';
  } finally {
    loading.value = false;
  }
};

const saveItem = async () => {
  isSaving.value = true;
  error.value = '';
  try {
    const docId = route.query.docId;
    const docRes = await outcomeDocumentService.getById(docId);
    const doc = docRes.data?.data || docRes.data?.value || docRes.data;
    const tables = doc?.tables || doc?.Tables || [];

    // Qatorlarni yangilaymiz
    const updatedTables = tables.map(t => {
      const tId = Number(t.id || t.Id);
      if (tId === Number(row.value.id)) {
        return {
          Id: row.value.id,
          ProductId: Number(row.value.productId),
          Price: parseFloat(row.value.price),
          Quantity: parseFloat(row.value.quantity),
          TotalSum: parseFloat(row.value.price) * parseFloat(row.value.quantity)
        };
      }
      return {
        Id: tId,
        ProductId: t.productId || t.ProductId,
        Price: t.price || t.Price,
        Quantity: t.quantity || t.Quantity,
        TotalSum: t.totalSum || t.TotalSum
      };
    });

    // Butun hujjatni yangilash uchun yuboramiz
    await outcomeDocumentService.update({
      Id: Number(docId),
      SupplierId: doc.supplierId || doc.SupplierId,
      DocOn: doc.docOn || doc.DocOn,
      Tables: updatedTables
    });

    router.back();
  } catch (err) {
    console.error('Saqlashda xatolik:', err);
    error.value = 'Saqlashda xatolik yuz berdi: ' + (err.response?.data?.message || err.message);
  } finally {
    isSaving.value = false;
  }
};

const cancel = () => router.back();

onMounted(loadData);
</script>

<style scoped>
/* OutcomeDocumentFormView.vue dagi stillarni bu yerga ham qo'shishingiz mumkin */
.page-card { background: #0d1117; padding: 1.5rem; border-radius: 1rem; }
m .wide-card { max-width: 1400px; margin: 0 auto; }
.section-header { margin-bottom: 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.07); padding-bottom: 1rem; }
.header-centered { text-align: center; width: 100%; }

.entity-form { background: #111827; padding: 1.5rem; border-radius: 0.75rem; border: 1px solid rgba(255, 255, 255, 0.07); }
.form-grid { display: grid; grid-template-columns: repeat(2, 1fr); gap: 1.5rem; margin-bottom: 1rem; }
.input-group { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }

label { display: block; font-weight: 600; color: #94a3b8; margin-bottom: 0.5rem; font-size: 13px; }
input, select { width: 100%; padding: 0.75rem; background: rgba(255, 255, 255, 0.05); border: 1px solid rgba(255, 255, 255, 0.08); color: #f1f5f9; border-radius: 0.4rem; font-size: 13.5px; }
input:focus, select:focus { border-color: #3b82f6; outline: none; }
select:disabled { opacity: 0.7; cursor: not-allowed; }

select option { background: #1e293b; color: #f1f5f9; }
.button-row { display: flex; gap: 1rem; margin-top: 3rem; padding-top: 1.5rem; border-top: 1px solid rgba(255, 255, 255, 0.07); }
.btn-save { background: linear-gradient(135deg, #3b82f6, #2563eb); color: white; padding: 0.8rem 2rem; border-radius: 0.6rem; border: none; font-weight: 600; cursor: pointer; display: flex; align-items: center; gap: 10px; }
.btn-save:hover:not(:disabled) { transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37, 99, 235, 0.4); }
.btn-cancel { display: inline-flex; align-items: center; justify-content: center; gap: 0.6rem; background: rgba(255, 255, 255, 0.05); color: #94a3b8; border: 1px solid rgba(255, 255, 255, 0.1); padding: 0.8rem 2rem; border-radius: 0.6rem; cursor: pointer; font-size: 14px; font-weight: 600; }
.btn-cancel:hover { background: rgba(255, 255, 255, 0.1); color: #f1f5f9; border-color: rgba(255, 255, 255, 0.2); }
.error { color: #ef4444; margin-top: 1rem; font-size: 14px; }

.row-total-display { margin: 1.5rem 0; font-size: 1.1rem; color: #94a3b8; grid-column: span 2; }
.row-total-display strong { color: #3b82f6; }
.spinner { width: 16px; height: 16px; border: 2px solid rgba(255, 255, 255, 0.3); border-top-color: white; border-radius: 50%; animation: spin 0.8s linear infinite; }
.loading-state, .error-state { padding: 3rem; text-align: center; color: #64748b; }
@keyframes spin { to { transform: rotate(360deg); } }
</style>