<template>
  <div class="page-card wide-card weather-page">
    <div class="section-header">
      <div>
        <h2>Ob-havo ma'lumotlari</h2>
      </div>
    </div>

    <div class="search-section">
      <div class="search-box">
        <svg class="search-icon" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <circle cx="11" cy="11" r="8"></circle><line x1="21" y1="21" x2="16.65" y2="16.65"></line>
        </svg>
        <input 
          v-model="cityName" 
          @keyup.enter="loadWeather"
          type="text" 
          placeholder="Shahar nomi (masalan: Toshkent)..." 
          class="search-input" 
        />
        <button @click="loadWeather" :disabled="loading" class="search-button">
          <span v-if="loading" class="spinner"></span>
          <span v-else>Qidirish</span>
        </button>
      </div>
    </div>

    <div v-if="error" class="error-banner">
      {{ error }}
    </div>

    <div v-if="loading && !weather" class="loading-state">
      <span class="spinner"></span> Ma'lumot yuklanmoqda...
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
import { getWeather, extractApiData } from '../api'; // api.js dagi funksiyalarni import qilish

const cityName = ref('Tashkent');
const weather = ref(null);
const loading = ref(false);
const error = ref('');

const loadWeather = async () => {
  console.log('loadWeather function called');
  if (!cityName.value.trim()) return;
  
  loading.value = true;
  error.value = '';
  console.log('Ob-havo uchun so\'rov yuborilmoqda:', cityName.value);
  try {
    const response = await getWeather(cityName.value);
    
    // Request qaysi URLga ketganini tekshirish
    console.log('Request URL:', response.config.url);
    console.log('Full Request URL:', response.config.baseURL + response.config.url);
    console.log('Backend javobi:', response.data);

    // extractApiData backenddan kelayotgan o'ralgan (wrapped) ma'lumotni ochib beradi
    weather.value = extractApiData(response);
  } catch (err) {
    console.error(err);
    // Backenddan kelgan aniq xatolik xabarini ko'rsatamiz
    const serverError = err.response?.data?.message || err.response?.data?.title || JSON.stringify(err.response?.data) || err.message;
    error.value = `Xatolik: ${serverError}. Shahar nomi to'g'riligini va internetni tekshiring.`;
    weather.value = null;
  } finally {
    loading.value = false;
  }
};

onMounted(loadWeather);
</script>

<style scoped>
.page-card { background: #0d1117; padding: 2rem; border-radius: 1rem; min-height: 400px; }
.weather-page { max-width: 800px; margin: 0 auto; }
.section-header { text-align: center; margin-bottom: 2rem; }
.search-section { margin-bottom: 2.5rem; display: flex; justify-content: center; }
.search-box {
  display: flex;
  align-items: center;
  background: #1e293b;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 1rem; /* Kattaroq border-radius */
  padding: 8px; /* Umumiy qidiruv qutisini kattaroq qilish */
  width: 100%;
  max-width: 550px; /* Umumiy qidiruv qutisini kengaytirish */
  transition: all 0.3s ease;
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}
.search-box:focus-within {
  border-color: #3b82f6;
  box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.25);
}
.search-icon {
  margin-left: 0.8rem; /* Ikonka va input orasidagi masofani sozlash */
  color: #64748b;
}
.search-input { 
  flex: 1;
  background: transparent;
  border: none;
  padding: 0.6rem 1rem; /* Input fieldning o'zini ixchamroq qilish */
  color: #f1f5f9;
  font-size: 0.9rem; /* Matn hajmini kichraytirish */
  outline: none;
}

.search-button {
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  color: white;
  border: none;
  padding: 0.6rem 1.5rem; /* Tugma paddingini sozlash */
  border-radius: 0.8rem; /* Tugma border-radiusini sozlash */
  cursor: pointer;
  font-weight: 600;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: 100px;
}

.search-button:hover:not(:disabled) {
  filter: brightness(1.1);
  box-shadow: 0 4px 12px rgba(37, 99, 235, 0.3);
}

.search-button:disabled { opacity: 0.6; cursor: not-allowed; }

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

.loading-state { text-align: center; padding: 3rem; color: #94a3b8; }

.spinner {
  display: inline-block;
  width: 18px; height: 18px; border: 2px solid rgba(255,255,255,0.3);
  border-top-color: #3b82f6; border-radius: 50%; animation: spin 0.8s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }
</style>