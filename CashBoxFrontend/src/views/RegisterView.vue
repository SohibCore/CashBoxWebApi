  <template>
    <div class="register-form-wrap">
      <div class="form-header">
        <h2 class="form-title">Hisob yaratish</h2>
        <p class="form-subtitle">Barcha maydonlarni to'ldiring</p>
      </div>

      <form @submit.prevent="submit">
        <div class="form-grid">

          <div class="field">
            <label><span class="req">*</span> Login</label>
            <div class="input-wrap">
            <span class="input-icon">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
                <circle cx="12" cy="7" r="4"/>
              </svg>
            </span>
              <input v-model="userName" type="text" maxlength="100" placeholder="Maksimal 100 belgi" required />
          </div>
              <small v-if="userNameError" class="field-error">{{ userNameError }}</small>
          </div>

          <div class="field">
            <label><span class="req">*</span> Email</label>
            <div class="input-wrap">
            <span class="input-icon">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/><polyline points="22,6 12,13 2,6"/>
              </svg>
            </span>
              <input v-model="email" type="email" required />
          </div>
              <small v-if="emailError" class="field-error">{{ emailError }}</small>
          </div>

          <div class="field">
            <label><span class="req">*</span> Parol</label>
            <div class="input-wrap">
            <span class="input-icon">
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <rect x="3" y="11" width="18" height="11" rx="2"/><path d="M7 11V7a5 5 0 0 1 10 0v4"/>
              </svg>
            </span>
              <input v-model="password" type="password" minlength="6" placeholder="Kamida 6 belgi" required />
          </div>
              <small v-if="passwordError" class="field-error">{{ passwordError }}</small>
          </div>

          <div class="field">
            <label><span class="req">*</span> To'liq ism</label>
            <div class="input-wrap">
              <input v-model="fullName" type="text" maxlength="300" required />
              <small v-if="fullNameError" class="field-error">{{ fullNameError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Qisqa ism</label>
            <div class="input-wrap">
              <input v-model="shortName" type="text" maxlength="200" required />
              <small v-if="shortNameError" class="field-error">{{ shortNameError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> PINFL</label>
            <div class="input-wrap">
              <input v-model="pinfl" type="text" maxlength="14" inputmode="numeric" placeholder="14 raqam" required />
              <small v-if="pinflError" class="field-error">{{ pinflError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Telefon raqam</label>
            <div class="input-wrap">
              <input v-model="phoneNumber" type="tel" maxlength="9" inputmode="numeric" placeholder="9 raqam" required />
              <small v-if="phoneNumberError" class="field-error">{{ phoneNumberError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Viloyat</label>
            <div class="input-wrap">
              <select v-model.number="selectedRegionId" @change="onRegionChange" required>
                <option value="0" disabled>Viloyatni tanlang</option>
                <option v-for="reg in regions" :key="reg.id" :value="reg.id">{{ reg.fullName }}</option>
              </select>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Tuman</label>
            <div class="input-wrap">
              <select v-model.number="selectedDistrictId" :disabled="!selectedRegionId" required>
                <option value="0" disabled>{{ selectedRegionId ? 'Tumanni tanlang' : 'Avval viloyat tanlang' }}</option>
                <option v-for="dist in districts" :key="dist.id" :value="dist.id">{{ dist.fullName }}</option>
              </select>
              <small v-if="addressError" class="field-error">{{ addressError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Tashkilot (INN)</label>
            <div class="input-wrap inn-wrap">
              <input
                v-model="innSearch"
                type="text"
                placeholder="INN kiriting..."
                @input="handleInnInput"
                inputmode="numeric"
                pattern="\d*"
              />
              <small v-if="innSearchError" class="field-error">{{ innSearchError }}</small>
              <div v-if="isLoadingOrganizations" class="dropdown-list loading-text">Yuklanmoqda...</div>
              <div v-else-if="orgResults.length > 0" class="dropdown-list">
                <div v-for="org in orgResults" :key="org.id" class="dropdown-item" @click="selectOrganization(org)">
                  {{ org.fullName }} ({{ org.inn }})
                </div>
              </div>
              <div v-else-if="innSearch.length >= 3 && !isLoadingOrganizations && orgResults.length === 0 && !selectedOrg" class="dropdown-list no-results">
                Natijalar topilmadi
              </div>
              <small v-if="selectedOrg" class="selected-org">✅ {{ selectedOrg.fullName }}</small>
              <small v-if="organizationIdError" class="field-error">{{ organizationIdError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Tug'ilgan sana</label>
            <div class="input-wrap">
              <input v-model="dateOfBirth" type="text" placeholder="dd.MM.yyyy" required />
              <small v-if="dateOfBirthError" class="field-error">{{ dateOfBirthError }}</small>
            </div>
          </div>

          <div class="field">
            <label><span class="req">*</span> Passport seriyasi</label>
            <div class="input-wrap">
              <input v-model="passportSeries" type="text" maxlength="9" placeholder="Masalan: AA123456" required />
              <small v-if="passportSeriesError" class="field-error">{{ passportSeriesError }}</small>
            </div>
          </div>

        </div>

        <button type="submit" class="submit-btn">Ro'yxatdan o'tish</button>
        <p v-if="error" class="error-msg">{{ error }}</p>
      </form>

      <div class="divider">yoki</div>
      <p class="bottom-link">
        Allaqachon hisobingiz bormi?
        <router-link to="/auth/login">Kirish</router-link>
      </p>
    </div>
  </template>

  <script>
  import { ref, onMounted } from 'vue';
  import { useRouter } from 'vue-router';
  import { register, getOrganizations, getRegions, getDistricts } from '../api';

  export default {
    setup() {
      const router = useRouter();
      const userName = ref('');
      const email = ref('');
      const password = ref('');
      const fullName = ref('');
      const shortName = ref('');
      const pinfl = ref('');
      const phoneNumber = ref('');
      const organizationId = ref(0);
      const dateOfBirth = ref('');
      const passportSeries = ref('');
      const error = ref('');

      const innSearch = ref('');
      const orgResults = ref([]);
      const selectedOrg = ref(null);
      const regions = ref([]);
      const districts = ref([]);
      const selectedRegionId = ref(0);
      const selectedDistrictId = ref(0);
      const isLoadingOrganizations = ref(false); // New ref for loading state
      let debounceTimer = null;

      // Validation error refs
      const userNameError = ref('');
      const emailError = ref('');
      const passwordError = ref('');
      const fullNameError = ref('');
      const shortNameError = ref('');
      const pinflError = ref('');
      const phoneNumberError = ref('');
      const addressError = ref('');
      const organizationIdError = ref('');
      const innSearchError = ref(''); // New ref for INN search validation
      const dateOfBirthError = ref('');
      const passportSeriesError = ref('');

      const validateUserName = () => {
        userNameError.value = '';
        const val = userName.value.trim();
        if (!val) return false;
        if (val.length > 100) {
          userNameError.value = 'Maksimal 100 belgi bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validateEmail = () => {
        emailError.value = '';
        const val = email.value.trim();
        if (!val) return false;
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(val)) {
          emailError.value = 'Email format noto\'g\'ri';
          return false;
        }
        return true;
      };

      const validatePassword = () => {
        passwordError.value = '';
        const val = password.value;
        if (!val) return false;
        if (val.length < 6) {
          passwordError.value = 'Kamida 6 belgi bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validateFullName = () => {
        fullNameError.value = '';
        const val = fullName.value.trim();
        if (!val) return false;
        if (val.length > 300) {
          fullNameError.value = 'Maksimal 300 belgi bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validateShortName = () => {
        shortNameError.value = '';
        const val = shortName.value.trim();
        if (!val) return false;
        if (val.length > 200) {
          shortNameError.value = 'Maksimal 200 belgi bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validatePinfl = () => {
        pinflError.value = '';
        const val = pinfl.value.trim();
        if (!val) return false;
        if (!/^\d{14}$/.test(val)) {
          pinflError.value = 'PINFL faqat 14 raqam bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validatePhoneNumber = () => {
        phoneNumberError.value = '';
        const val = phoneNumber.value.trim();
        if (!val) return false;
        if (!/^\d{9}$/.test(val)) {
          phoneNumberError.value = 'Telefon raqam faqat 9 raqam bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validateAddress = () => {
        addressError.value = '';
        if (selectedRegionId.value === 0 || selectedDistrictId.value === 0) {
          addressError.value = 'Viloyat va tuman tanlanishi shart';
          return false;
        }
        return true;
      };

      const validateOrganizationId = () => {
        organizationIdError.value = '';
        if (organizationId.value <= 0) {
          organizationIdError.value = 'Tashkilot tanlash majburiy';
          return false;
        }
        return true;
      };

      const validateInnSearch = () => {
        innSearchError.value = '';
        const val = innSearch.value.trim();
        if (selectedOrg.value) { // If an organization is selected, INN is valid
          return true;
        }
        if (!val) {
          innSearchError.value = 'INN kiritish majburiy';
          return false;
        }
        if (!/^\d+$/.test(val)) {
          innSearchError.value = 'INN faqat raqamlardan iborat bo\'lishi kerak';
          return false;
        }
        if (val.length < 3) {
          innSearchError.value = 'INN kamida 3 ta raqamdan iborat bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const handleInnInput = (event) => {
        // Allow only numeric input
        innSearch.value = event.target.value.replace(/\D/g, '');
        // Clear selected organization if input changes
        selectedOrg.value = null;
        organizationId.value = 0;
        searchOrganization();
      };

      const searchOrganization = () => {
        clearTimeout(debounceTimer);
        orgResults.value = []; // Clear previous results immediately

        const inn = innSearch.value.trim();

        if (!inn || inn.length < 3 || !/^\d+$/.test(inn)) {
          isLoadingOrganizations.value = false;
          innSearchError.value = inn && !/^\d+$/.test(inn) ? 'INN faqat raqamlardan iborat bo\'lishi kerak' : '';
          return;
        }

        isLoadingOrganizations.value = true;
        debounceTimer = setTimeout(async () => {
          try {
            const response = await getOrganizations({ inn: inn });
            const data = response.data?.data || response.data || [];
            orgResults.value = data.map(o => ({
              id: o.id || o.Id,
              fullName: o.fullName || o.FullName,
              inn: o.inn || o.Inn
            }));
          } catch (e) {
            console.error("Error searching organizations:", e);
            orgResults.value = [];
          } finally {
            isLoadingOrganizations.value = false;
          }
        }, 500); // Debounce time set to 500ms
      };

      const selectOrganization = (org) => {
        selectedOrg.value = org;
        organizationId.value = org.id;
        innSearch.value = org.inn;
        orgResults.value = [];
      };

      const loadRegions = async () => {
        try {
          const response = await getRegions();
          const data = response.data?.data || response.data || [];
          regions.value = data.map(r => ({
            id: r.id || r.Id,
            fullName: r.fullName || r.FullName
          }));
        } catch (e) {
          console.error(e);
        }
      };

      const onRegionChange = async () => {
        selectedDistrictId.value = 0;
        districts.value = [];
        if (!selectedRegionId.value) return;
        try {
          const response = await getDistricts(selectedRegionId.value);
          const data = response.data?.data || response.data || [];
          districts.value = data.map(d => ({
            id: d.id || d.Id,
            fullName: d.fullName || d.FullName
          }));
        } catch (e) {
          console.error(e);
        }
      };

      onMounted(loadRegions);

      const validateDateOfBirth = () => {
        dateOfBirthError.value = '';
        const val = dateOfBirth.value.trim();
        if (!val) return false;
        if (!/^\d{2}\.\d{2}\.\d{4}$/.test(val)) {
          dateOfBirthError.value = 'Format dd.MM.yyyy bo\'lishi kerak';
          return false;
        }
        const [day, month, year] = val.split('.');
        const d = parseInt(day);
        const m = parseInt(month);
        const y = parseInt(year);
        if (m < 1 || m > 12) {
          dateOfBirthError.value = 'Oy 01-12 oraliq\'ida bo\'lishi kerak';
          return false;
        }
        if (d < 1 || d > 31) {
          dateOfBirthError.value = 'Kun 01-31 oraliq\'ida bo\'lishi kerak';
          return false;
        }
        if (y < 1900 || y > new Date().getFullYear()) {
          dateOfBirthError.value = 'Yil 1900-hozirgi yil oraliq\'ida bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validatePassportSeries = () => {
        passportSeriesError.value = '';
        const val = passportSeries.value.trim().toUpperCase();
        if (!val) return false;
        if (val.length < 2 || val.length > 9) {
          passportSeriesError.value = 'Passport seriyasi 2-9 belgi bo\'lishi kerak';
          return false;
        }
        if (!/^[A-Z0-9]+$/.test(val)) {
          passportSeriesError.value = 'Faqat harf va raqam bo\'lishi kerak';
          return false;
        }
        return true;
      };

      const validateAll = () => {
        return (
          validateUserName() &&
          validateEmail() &&
          validatePassword() &&
          validateFullName() &&
          validateShortName() &&
          validatePinfl() &&
          validatePhoneNumber() &&
          validateAddress() &&
          validateInnSearch() && // Add INN search validation
          validateOrganizationId() &&
          validateDateOfBirth() &&
          validatePassportSeries()
        );
      };

      const formatDateToISO = (dateStr) => {
        if (!dateStr) return '';
        const [day, month, year] = dateStr.split('.');
        if (!day || !month || !year) return dateStr;
        return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
      };

      const submit = async () => {
        error.value = '';
        
        if (!validateAll()) {
          error.value = 'Iltimos barcha maydonlarni to\'g\'ri to\'ldiring';
          return;
        }

        try {
          const payload = {
            userName: userName.value.trim(),
            email: email.value.trim(),
            password: password.value,
            fullName: fullName.value.trim(),
            shortName: shortName.value.trim(),
            pinfl: pinfl.value.trim(),
            phoneNumber: phoneNumber.value.trim(),
            address: `${regions.value.find(r => r.id === selectedRegionId.value)?.fullName || ''}, ${districts.value.find(d => d.id === selectedDistrictId.value)?.fullName || ''}`,
            organizationId: organizationId.value, // Use the selected organizationId
            dateOfBirth: formatDateToISO(dateOfBirth.value),
            passportSeries: passportSeries.value.trim().toUpperCase()
          };
          console.log('Register payload:', payload);
          const response = await register(payload);
          console.log('Register response:', response);
          router.push('/auth/login');
        } catch (err) {
          console.error('Register error:', err);
          const errorMsg = err.response?.data?.message 
            || err.response?.data?.errors?.[0]
            || err.response?.statusText 
            || err.message 
            || 'Xato: Ro\'yxatdan o\'tishda muammo';
          error.value = errorMsg;
        }
      };

      return {
        userName,
        email,
        password,
        fullName,
        shortName,
        pinfl,
        phoneNumber,
        organizationId,
        dateOfBirth,
        passportSeries,
        error,
        innSearch,
        orgResults,
        selectedOrg,
        regions,
        isLoadingOrganizations, // Return new ref
        innSearchError, // Return new error ref
        districts,
        selectedRegionId,
        selectedDistrictId,
        userNameError,
        emailError,
        passwordError,
        fullNameError,
        shortNameError,
        pinflError,
        phoneNumberError,
        addressError,
        organizationIdError,
        validateInnSearch, // Return new validation function
        dateOfBirthError,
        passportSeriesError,
        submit,
        validateUserName,
        validateEmail,
        validatePassword,
        validateFullName,
        handleInnInput, // Return new input handler
        validateShortName,
        validatePinfl,
        validatePhoneNumber,
        validateAddress,
        validateOrganizationId,
        validateDateOfBirth,
        validatePassportSeries,
        searchOrganization,
        selectOrganization,
        onRegionChange
      };
    }
  };
  </script>

  <style scoped>
.register-form-wrap input,
.register-form-wrap select {
  background: rgba(255,255,255,0.05) !important;
  border: 1px solid rgba(255,255,255,0.1) !important;
  color: #f1f5f9 !important;
  -webkit-text-fill-color: #f1f5f9 !important;
}

.register-form-wrap input:hover,
.register-form-wrap select:hover {
  background: rgba(255,255,255,0.07) !important;
  border-color: rgba(255,255,255,0.18) !important;
}

.register-form-wrap input:focus,
.register-form-wrap select:focus {
  background: rgba(255,255,255,0.07) !important;
  border-color: #3b82f6 !important;
  box-shadow: 0 0 0 3px rgba(59,130,246,0.25) !important;
  outline: none !important;
  color: #f1f5f9 !important;
  -webkit-text-fill-color: #f1f5f9 !important;
}

/* Brauzer autocomplete oq background ni o'chirish */
.register-form-wrap input:-webkit-autofill,
.register-form-wrap input:-webkit-autofill:hover,
.register-form-wrap input:-webkit-autofill:focus {
  -webkit-box-shadow: 0 0 0 1000px rgba(22,29,47,0.98) inset !important;
  -webkit-text-fill-color: #f1f5f9 !important;
  border-color: #3b82f6 !important;
  transition: background-color 9999s ease-in-out 0s;
}

.register-form-wrap { width: 100%; }

.form-header { margin-bottom: 20px; }
.form-title { font-size: 26px; font-weight: 600; color: var(--text-primary); margin-bottom: 4px; }
.form-subtitle { font-size: 13px; color: var(--text-secondary); }

.form-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 12px;
  margin-bottom: 18px;
}
@media (max-width: 900px)  { .form-grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 500px)  { .form-grid { grid-template-columns: 1fr; } }

.field { display: flex; flex-direction: column; }
.field > label {
  font-size: 12px; color: var(--text-secondary);
  margin-bottom: 5px; font-weight: 500;
}
.req { color: #ef4444; margin-right: 2px; }

.input-wrap { position: relative; }

.input-wrap input,
.input-wrap select {
  width: 100%;
  padding: 9px 11px;
  background: var(--input-bg) !important;
  border: 1px solid rgba(255, 255, 255, 0.1) !important;
  border-radius: 9px; color: var(--text-primary);
  font-family: 'Outfit', sans-serif; font-size: 13px;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s, background-color 0.2s;
  appearance: none; margin-top: 0;
}

/* Faqat ikonka bor inputlar uchun padding */
.input-wrap:has(.input-icon) input {
  padding-left: 38px;
}

.input-icon {
  position: absolute; left: 11px; top: 50%; transform: translateY(-50%);
  color: var(--text-muted); pointer-events: none;
  display: flex; align-items: center; z-index: 1;
}
.input-wrap input:hover,
.input-wrap select:hover { /* Hover holati uchun fon */
  background: rgba(255,255,255,0.08) !important;
}
.input-wrap input::placeholder { color: var(--text-muted); }
.input-wrap select option { background: #1e293b; color: var(--text-primary); }
.input-wrap select:disabled { opacity: 0.4; cursor: not-allowed; }

.inn-wrap { position: relative; }

.dropdown-list {
  position: absolute; left: 0; right: 0;
  border: 1px solid var(--navy-border);
  border-radius: 8px; background: #1e293b;
  max-height: 140px; overflow-y: auto;
  z-index: 100; margin-top: 2px;
}
.dropdown-item {
  padding: 7px 11px; cursor: pointer;
  font-size: 12px; color: var(--text-secondary);
  transition: background 0.15s;
}
.dropdown-item:hover { background: rgba(59,130,246,0.12); color: var(--text-primary); }
.dropdown-list.loading-text,
.dropdown-list.no-results {
  padding: 8px 11px; color: var(--text-muted);
  text-align: center; font-size: 12px;
}

.field-error { color: #ef4444; font-size: 11px; margin-top: 3px; display: block; font-weight: 400; }
.selected-org { color: var(--green); font-size: 11px; margin-top: 3px; display: block; }

.submit-btn {
  width: 100%; padding: 12px;
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  border: none; border-radius: 10px; color: white;
  font-family: 'Outfit', sans-serif; font-size: 15px; font-weight: 600;
  cursor: pointer; transition: all 0.2s;
  box-shadow: 0 4px 20px rgba(59,130,246,0.35);
  margin-top: 0;
}
.submit-btn:hover { transform: translateY(-1px); box-shadow: 0 6px 28px rgba(59,130,246,0.55); }

.error-msg { color: #ef4444; font-size: 13px; margin-top: 10px; text-align: center; }

.divider {
  display: flex; align-items: center; gap: 12px;
  margin: 16px 0; color: var(--text-muted); font-size: 12px;
}
.divider::before, .divider::after {
  content: ''; flex: 1; height: 1px; background: var(--navy-border);
}

.bottom-link { text-align: center; font-size: 13px; color: var(--text-secondary); }
.bottom-link a { color: var(--accent-light); text-decoration: none; }
.bottom-link a:hover { text-decoration: underline; }
</style>
