<template>
  <div class="page-card wide-card weather-page">
    <div class="section-header">
      <div>
        <h2>Ob-havo ma'lumotlari</h2>
        <p>Shahar nomini kiriting va joriy ob-havo holatini bilib oling.</p>
      </div>
    </div>

    <div class="search-section">
      <div class="search-input-wrapper">
        <input 
          v-model="cityName" 
          @keyup.enter="loadWeather"
          type="text" 
          placeholder="Shahar nomi (masalan: Tashkent)..." 
          class="search-input" 
        />
        <button @click="loadWeather" :disabled="loading" class="btn-primary">
          <svg v-if="!loading" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line>
          </svg>
          <span v-else class="spinner"></span>
          Qidirish
        </button>
      </div>
    </div>

    <div v-if="error" class="error-banner">
      {{ error }}
    </div>

    <div v-if="weather" class="weather-content">
      <div class="weather-main-card">
        <div class="location">
          <span class="icon">📍</span> {{ weather.name }}, {{ weather.sys?.country }}
        </div>
        
        <div class="temp-display">
          <div class="main-temp">{{ Math.round(weather.main?.temp) }}°C</div>
          <div class="weather-icon-box">
            <img 
              v-if="weather.weather?.[0]" 
              :src="`https://openweathermap.org/img/wn/${weather.weather[0].icon}@2x.png`" 
              :alt="weather.weather[0].description"
            />
            <p class="description">{{ weather.weather?.[0]?.description }}</p>
          </div>
        </div>

        <div class="weather-grid">
          <div class="grid-item">
            <span class="label">Hissiyot</span>
            <span class="value">{{ Math.round(weather.main?.feels_like) }}°C</span>
          </div>
          <div class="grid-item">
            <span class="label">Namlik</span>
            <span class="value">{{ weather.main?.humidity }}%</span>
          </div>
          <div class="grid-item">
            <span class="label">Shamol</span>
            <span class="value">{{ weather.wind?.speed }} m/s</span>
          </div>
          <div class="grid-item">
            <span class="label">Bosim</span>
            <span class="value">{{ weather.main?.pressure }} hPa</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { getWeather } from '../api'; // api.js dagi funksiyani import qilish

const cityName = ref('Tashkent');
const weather = ref(null);
const loading = ref(false);
const error = ref('');

const loadWeather = async () => {
  if (!cityName.value.trim()) return;
  
  loading.value = true;
  error.value = '';
  try {
    const response = await getWeather(cityName.value);
    // Backend to'g'ridan-to'g'ri WeatherResponseDto qaytaradi
    weather.value = response.data?.data || response.data;
  } catch (err) {
    console.error(err);
    error.value = "Ob-havo ma'lumotlarini yuklab bo'lmadi. Shahar nomi to'g'riligini tekshiring.";
    weather.value = null;
  } finally {
    loading.value = false;
  }
};

onMounted(loadWeather);
</script>

<style scoped>
.weather-page { max-width: 800px; margin: 0 auto; }
.search-section { margin-bottom: 2rem; display: flex; justify-content: center; }
.search-input-wrapper { display: flex; gap: 10px; width: 100%; max-width: 500px; }
.search-input { flex: 1; }

.weather-main-card {
  background: #1e293b;
  border-radius: 1.5rem;
  padding: 2.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  text-align: center;
}

.location { font-size: 1.5rem; font-weight: 600; color: #f1f5f9; margin-bottom: 1.5rem; }
.temp-display { display: flex; align-items: center; justify-content: center; gap: 2rem; margin-bottom: 2rem; }
.main-temp { font-size: 5rem; font-weight: 800; color: #3b82f6; }
.description { color: #94a3b8; text-transform: capitalize; font-size: 1.1rem; }

.weather-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1rem;
  border-top: 1px solid rgba(255, 255, 255, 0.07);
  padding-top: 2rem;
}

.grid-item { display: flex; flex-direction: column; gap: 5px; }
.grid-item .label { font-size: 0.75rem; color: #64748b; text-transform: uppercase; }
.grid-item .value { font-size: 1.1rem; font-weight: 600; color: #f1f5f9; }

.error-banner {
  background: rgba(220, 38, 38, 0.1);
  color: #ef4444;
  padding: 1rem;
  border-radius: 0.75rem;
  margin-bottom: 1.5rem;
  text-align: center;
}

.spinner {
  width: 14px; height: 14px; border: 2px solid rgba(255,255,255,0.3);
  border-top-color: white; border-radius: 50%; animation: spin 0.8s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }
</style>