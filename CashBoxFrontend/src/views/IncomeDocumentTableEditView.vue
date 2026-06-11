<template>
  <div class="page-card wide-card">
    <div class="section-header">
      <div class="header-centered">
        <h2>Mahsulot qatorini tahrirlash</h2>
        <p>Hujjat tarkibidagi mahsulot miqdori va narxini o'zgartiring.</p>
      </div>
    </div>

    <form v-if="row" class="entity-form" @submit.prevent="saveRow">
      <div class="form-grid">
        <label>
          Mahsulot
          <select v-model="row.productId" disabled>
            <option v-for="p in products" :key="p.id || p.Id" :value="p.id || p.Id">
              {{ p.name }}
            </option>
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
        <button type="button" class="btn-cancel" @click="router.back()">
          Bekor qilish
        </button>
      </div>
    </form>
    
    <div v-else-if="loading" class="loading-state">Yuklanmoqda...</div>
    <div v-else class="error-state">Ma'lumot topilmadi yoki yuklashda xato yuz berdi.</div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { incomeDocumentService } from './incomeDocumentService';

const route = useRoute();
const router = useRouter();
const row = ref(null);
const products = ref([]);
const loading = ref(true);
const isSaving = ref(false);

const formatSum = (val) => new Intl.NumberFormat('uz-UZ').format(val || 0) + ' UZS';

const loadData = async () => {
  loading.value = true;
  try {
    const tableId = Number(route.params.id);
    const docId = route.query.docId;

    const [pRes, rRes] = await Promise.all([
      incomeDocumentService.getProducts(),
      incomeDocumentService.getById(docId)
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
    console.error('Error loading row data:', err);
  } finally {
    loading.value = false;
  }
};

const saveRow = async () => {
  isSaving.value = true;
  try {
    const docId = route.query.docId;
    const docRes = await incomeDocumentService.getById(docId);
    const doc = docRes.data?.data || docRes.data?.value || docRes.data;
    const tables = doc?.tables || doc?.Tables || [];

    const updatedTables = tables.map(t => {
      const tId = t.id || t.Id;
      if (tId === row.value.id) {
        return {
          Id: row.value.id,
          ProductId: parseInt(row.value.productId),
          Price: parseFloat(row.value.price),
          Quantity: parseFloat(row.value.quantity),
          TotalSum: parseFloat(row.value.price) * parseFloat(row.value.quantity)
        };
      }
      return {
        Id: t.id || t.Id,
        ProductId: t.productId || t.ProductId,
        Price: t.price || t.Price,
        Quantity: t.quantity || t.Quantity,
        TotalSum: t.totalSum || t.TotalSum
      };
    });

    await incomeDocumentService.update({
      Id: Number(docId),
      SupplierId: doc.supplierId || doc.SupplierId,
      DocOn: doc.docOn || doc.DocOn,
      Tables: updatedTables
    });

    router.back();
  } catch (err) {
    console.error(err);
    alert('Saqlashda xatolik yuz berdi');
  } finally {
    isSaving.value = false;
  }
};

onMounted(loadData);
</script>

<style scoped>
.page-card {
  background: #0d1117;
  padding: 1.5rem;
  border-radius: 1rem;
  min-height: 100%;
}

.wide-card {
  max-width: 1200px; /* Kenglikni oshirdik */
  margin: 0 auto;
}

.section-header {
  margin-bottom: 1.5rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  padding-bottom: 1rem;
}

.header-centered {
  text-align: center;
  width: 100%;
}

.entity-form {
  background: #111827;
  padding: 2rem;
  border-radius: 0.75rem;
  border: 1px solid rgba(255, 255, 255, 0.07);
}

label {
  display: block;
  font-weight: 600;
  color: #94a3b8;
  margin-bottom: 0.8rem;
  font-size: 14px;
}

input, select {
  width: 100%;
  padding: 0.85rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #f1f5f9;
  border-radius: 0.5rem;
  outline: none;
}

input:focus {
  border-color: #3b82f6;
}

.btn-save {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  color: white;
  border: none;
  padding: 0.8rem 2rem;
  border-radius: 0.6rem;
  font-weight: 600;
  cursor: pointer;
}

.btn-cancel {
  background: rgba(255, 255, 255, 0.05);
  color: #94a3b8;
  border: 1px solid rgba(255, 255, 255, 0.1);
  padding: 0.8rem 2rem;
  border-radius: 0.6rem;
  cursor: pointer;
}

.row-total-display { margin: 1.5rem 0; font-size: 1.1rem; color: #94a3b8; }
.row-total-display strong { color: #3b82f6; }
.input-group { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
.loading-state, .error-state { padding: 3rem; text-align: center; color: #64748b; }
</style>