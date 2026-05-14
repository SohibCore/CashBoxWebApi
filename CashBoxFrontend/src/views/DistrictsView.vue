<template>
  <div class="districts-view">
    <h2>Districts</h2>
    <div class="form-section">
      <form @submit.prevent="createNewDistrict" class="district-form">
        <div class="form-group">
          <label for="name">Name:</label>
          <input v-model="newDistrict.name" type="text" id="name" required>
        </div>
        <div class="form-group">
          <label for="regionId">Region ID:</label>
          <input v-model="newDistrict.regionId" type="number" id="regionId" required>
        </div>
        <button type="submit" class="btn btn-primary">Create District</button>
      </form>
    </div>
    <div class="table-section">
      <table class="data-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Region ID</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="district in districts" :key="district.id">
            <td>{{ district.id }}</td>
            <td>{{ district.name }}</td>
            <td>{{ district.regionId }}</td>
            <td>
              <button @click="editDistrict(district)" class="btn btn-edit">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                </svg>
              </button>
              <button @click="deleteDistrict(district.id)" class="btn btn-delete">
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
import { getDistricts, createDistrict, updateDistrict, deleteDistrict } from '../api.js';

export default {
  name: 'DistrictsView',
  setup() {
    const districts = ref([]);
    const newDistrict = ref({ name: '', regionId: '' });
    const editingDistrict = ref(null);

    const loadDistricts = async () => {
      try {
        const response = await getDistricts();
        const data = response.data;
        if (Array.isArray(data)) {
          districts.value = data;
        } else if (data && Array.isArray(data.data)) {
          districts.value = data.data;
        } else {
          districts.value = [];
        }
      } catch (error) {
        console.error('Error loading districts:', error);
        districts.value = [];
      }
    };

    const createNewDistrict = async () => {
      try {
        await createDistrict(newDistrict.value);
        newDistrict.value = { name: '', regionId: '' };
        await loadDistricts();
      } catch (error) {
        console.error('Error creating district:', error);
        alert('Failed to create district');
      }
    };

    const editDistrict = (district) => {
      editingDistrict.value = { ...district };
      newDistrict.value = { name: district.name, regionId: district.regionId };
    };

    const deleteDistrictItem = async (id) => {
      try {
        await deleteDistrict(id);
        await loadDistricts();
      } catch (error) {
        console.error('Error deleting district:', error);
        alert('Failed to delete district');
      }
    };

    onMounted(loadDistricts);

    return {
      districts,
      newDistrict,
      createNewDistrict,
      editDistrict,
      deleteDistrict: deleteDistrictItem
    };
  }
};
</script>

<style scoped>
.districts-view {
  padding: 20px;
}

.form-section {
  margin-bottom: 20px;
}

.district-form {
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