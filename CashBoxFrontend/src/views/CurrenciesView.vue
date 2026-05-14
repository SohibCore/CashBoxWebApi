<template>
  <div class="currencies-view">
    <h2>Currencies</h2>
    <div class="form-section">
      <form @submit.prevent="createNewCurrency" class="currency-form">
        <div class="form-group">
          <label for="name">Name:</label>
          <input v-model="newCurrency.name" type="text" id="name" required>
        </div>
        <div class="form-group">
          <label for="code">Code (numeric):</label>
          <input v-model.number="newCurrency.code" type="number" id="code" required>
        </div>
        <div class="form-group">
          <label for="symbol">Symbol:</label>
          <input v-model="newCurrency.symbol" type="text" id="symbol" required>
        </div>
        <button type="submit" class="btn btn-primary">Create Currency</button>
      </form>
    </div>
    <div class="table-section">
      <table class="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Code</th>
            <th>Symbol</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="currency in currencies" :key="currency.id">
            <td>{{ currency.id }}</td>
            <td>{{ currency.name }}</td>
            <td>{{ currency.code }}</td>
            <td>{{ currency.symbol }}</td>
            <td>
              <button @click="editCurrency(currency)" class="btn btn-edit">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                </svg>
              </button>
              <button @click="deleteCurrency(currency.id)" class="btn btn-delete">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                  <line x1="10" y1="11" x2="10" y2="17"></line>
                  <line x1="14" y1="11" x2="14" y2="17"></line>
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { getCurrencies, createCurrency, updateCurrency, deleteCurrency } from '../api.js';

export default {
  name: 'CurrenciesView',
  setup() {
    const currencies = ref([]);
    const newCurrency = ref({ name: '', code: '', symbol: '' });
    const editingCurrency = ref(null);

    const loadCurrencies = async () => {
      try {
        const response = await getCurrencies();
        const data = response.data;
        if (Array.isArray(data)) {
          currencies.value = data;
        } else if (data && Array.isArray(data.data)) {
          currencies.value = data.data;
        } else {
          currencies.value = [];
        }
      } catch (error) {
        console.error('Error loading currencies:', error);
        currencies.value = [];
      }
    };

    const createNewCurrency = async () => {
      // Validation
      if (!newCurrency.value.name?.trim()) {
        alert('Name is required');
        return;
      }
      if (!newCurrency.value.code?.trim()) {
        alert('Code is required');
        return;
      }
      if (!newCurrency.value.symbol?.trim()) {
        alert('Symbol is required');
        return;
      }

      try {
        const data = {
          name: newCurrency.value.name.trim(),
          code: newCurrency.value.code.trim(),
          symbol: newCurrency.value.symbol.trim()
        };
        console.log('Creating currency with data:', data);
        const response = await createCurrency(data);
        console.log('Create response:', response);
        newCurrency.value = { name: '', code: '', symbol: '' };
        await loadCurrencies();
        alert('Currency created successfully!');
      } catch (error) {
        console.error('Error creating currency:', error);
        console.error('Error response:', error.response?.data);
        console.error('Full error:', error);
        const errorMsg = error.response?.data?.message || 
                        error.response?.data?.error ||
                        error.message || 
                        'Failed to create currency';
        alert(`Error: ${errorMsg}`);
      }
    };

    const editCurrency = (currency) => {
      editingCurrency.value = { ...currency };
      // For simplicity, populate the form
      newCurrency.value = { name: currency.name, code: currency.code, symbol: currency.symbol };
    };

    const deleteCurrencyItem = async (id) => {
      try {
        await deleteCurrency(id);
        await loadCurrencies();
      } catch (error) {
        console.error('Error deleting currency:', error);
        alert('Failed to delete currency');
      }
    };

    onMounted(loadCurrencies);

    return {
      currencies,
      newCurrency,
      createNewCurrency,
      editCurrency,
      deleteCurrency: deleteCurrencyItem
    };
  }
};
</script>

<style scoped>
.currencies-view {
  padding: 20px;
}

.form-section {
  margin-bottom: 20px;
}

.currency-form {
  display: flex;
  gap: 10px;
  align-items: end;
}

.form-group {
  display: flex;
  flex-direction: column;
}

label {
  margin-bottom: 5px;
}

input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-edit {
  background-color: #ffc107;
  color: black;
  margin-right: 5px;
}

.btn-delete {
  background-color: #dc3545;
  color: white;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table th, .data-table td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

.data-table th {
  background-color: #f2f2f2;
}
</style>