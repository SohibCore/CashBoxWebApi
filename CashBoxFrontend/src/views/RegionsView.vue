<template>
  <div class="regions-view">
    <h2>Regions</h2>
    <div class="form-section">
      <form @submit.prevent="createNewRegion" class="region-form">
        <div class="form-group">
          <label for="name">Name:</label>
          <input v-model="newRegion.name" type="text" id="name" required>
        </div>
        <button type="submit" class="btn btn-primary">Create Region</button>
      </form>
    </div>
    <div class="table-section">
      <table class="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="region in regions" :key="region.id">
            <td>{{ region.id }}</td>
            <td>{{ region.name }}</td>
            <td>
              <button @click="editRegion(region)" class="btn btn-edit">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                </svg>
              </button>
              <button @click="deleteRegion(region.id)" class="btn btn-delete">
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
import { getRegions, createRegion, updateRegion, deleteRegion } from '../api.js';

export default {
  name: 'RegionsView',
  setup() {
    const regions = ref([]);
    const newRegion = ref({ name: '' });
    const editingRegion = ref(null);

    const loadRegions = async () => {
      try {
        const response = await getRegions();
        const data = response.data;
        if (Array.isArray(data)) {
          regions.value = data;
        } else if (data && Array.isArray(data.data)) {
          regions.value = data.data;
        } else {
          regions.value = [];
        }
      } catch (error) {
        console.error('Error loading regions:', error);
        regions.value = [];
      }
    };

    const createNewRegion = async () => {
      try {
        await createRegion(newRegion.value);
        newRegion.value = { name: '' };
        await loadRegions();
      } catch (error) {
        console.error('Error creating region:', error);
        alert('Failed to create region');
      }
    };

    const editRegion = (region) => {
      editingRegion.value = { ...region };
      newRegion.value = { name: region.name };
    };

    const deleteRegionItem = async (id) => {
      try {
        await deleteRegion(id);
        await loadRegions();
      } catch (error) {
        console.error('Error deleting region:', error);
        alert('Failed to delete region');
      }
    };

    onMounted(loadRegions);

    return {
      regions,
      newRegion,
      createNewRegion,
      editRegion,
      deleteRegion: deleteRegionItem
    };
  }
};
</script>

<style scoped>
.regions-view {
  padding: 20px;
}

.form-section {
  margin-bottom: 20px;
}

.region-form {
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