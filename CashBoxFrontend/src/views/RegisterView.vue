  <template>
    <div class="page-card">
      <h2>Ro'yxatdan o'tish</h2>
      <form @submit.prevent="submit">
        <div class="form-grid">
          <label>
            <span class="required-star">*</span> Login
            <input v-model="userName" type="text" maxlength="100" placeholder="Maksimal 100 belgi" required />
            <small v-if="userNameError" class="error">{{ userNameError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Email
            <input v-model="email" type="email" required />
            <small v-if="emailError" class="error">{{ emailError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Parol
            <input v-model="password" type="password" minlength="6" placeholder="Kamida 6 belgi" required />
            <small v-if="passwordError" class="error">{{ passwordError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> To'liq ism
            <input v-model="fullName" type="text" maxlength="300" required />
            <small v-if="fullNameError" class="error">{{ fullNameError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Qisqa ism
            <input v-model="shortName" type="text" maxlength="200" required />
            <small v-if="shortNameError" class="error">{{ shortNameError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> PINFL
            <input v-model="pinfl" type="text" maxlength="14" inputmode="numeric" placeholder="14 raqam" required />
            <small v-if="pinflError" class="error">{{ pinflError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Telefon raqam
            <input v-model="phoneNumber" type="tel" maxlength="9" inputmode="numeric" placeholder="9 raqam" required />
            <small v-if="phoneNumberError" class="error">{{ phoneNumberError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Manzil
            <select v-model.number="selectedRegionId" @change="onRegionChange" required>
              <option value="0" disabled>Viloyatni tanlang</option>
              <option v-for="reg in regions" :key="reg.id" :value="reg.id">
                {{ reg.fullName }}
              </option>
            </select>
          </label>
          <label>
            <span class="required-star">*</span> Tuman
            <select v-model.number="selectedDistrictId" :disabled="!selectedRegionId" required>
              <option value="0" disabled>
                {{ selectedRegionId ? 'Tumanni tanlang' : 'Avval viloyat tanlang' }}
              </option>
              <option v-for="dist in districts" :key="dist.id" :value="dist.id">
                {{ dist.fullName }}
              </option>
            </select>
            <small v-if="addressError" class="error">{{ addressError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Tashkilot (INN orqali qidiring)
            <input
              v-model="innSearch"
              type="text"
              placeholder="INN kiriting..."
              @input="handleInnInput"
              inputmode="numeric"
              pattern="\d*"
            />
            <small v-if="innSearchError" class="error">{{ innSearchError }}</small>
            <div v-if="isLoadingOrganizations" class="dropdown-list loading-indicator">
              Yuklanmoqda...
            </div>
            <div v-else-if="orgResults.length > 0" class="dropdown-list">
              <div v-for="org in orgResults" :key="org.id" class="dropdown-item" @click="selectOrganization(org)">
                {{ org.fullName }} ({{ org.inn }})
              </div>
            </div>
            <div v-else-if="innSearch.length >= 3 && !isLoadingOrganizations && orgResults.length === 0 && !selectedOrg" class="dropdown-list no-results">
              Natijalar topilmadi
            </div>
            <small v-if="selectedOrg">✅ {{ selectedOrg.fullName }}</small>
            <small v-if="organizationIdError" class="error">{{ organizationIdError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Tug'ilgan sana
            <input v-model="dateOfBirth" type="text" placeholder="dd.MM.yyyy" required />
            <small v-if="dateOfBirthError" class="error">{{ dateOfBirthError }}</small>
          </label>
          <label>
            <span class="required-star">*</span> Passport seriyasi
            <input v-model="passportSeries" type="text" maxlength="9" placeholder="Masalan: AA123456" required />
            <small v-if="passportSeriesError" class="error">{{ passportSeriesError }}</small>
          </label>
        </div>
        <button type="submit">Ro'yxatdan o'tish</button>
        <p v-if="error" class="error">{{ error }}</p>
      </form>
      <p class="login-link">Allaqachon akauntingiz bormi? <router-link to="/auth/login">Kirib olish</router-link></p>
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

  <style>
  .page-card {
    width: min(1200px, 100%);
    margin: 0;
    padding: 2rem;
    background: transparent;
    border-radius: 1rem;
    box-shadow: none;
  }

  .page-card h2 {
    margin-top: 0;
    margin-bottom: 1.5rem;
    text-align: center;
    font-size: 1.75rem;
  }

  .form-grid {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 1rem;
  }

  @media (max-width: 1040px) {
    .form-grid {
      grid-template-columns: repeat(2, minmax(0, 1fr));
    }
  }

  @media (max-width: 720px) {
    .page-card {
      width: 100%;
      padding: 1.5rem;
      margin: 1rem auto;
    }

    .form-grid {
      grid-template-columns: 1fr;
    }
  }

  label {
    display: block;
    margin-bottom: 1.25rem;
    font-weight: 600;
    color: #0f172a;
  }

  label small {
    display: block;
    margin-top: 0.35rem;
    font-size: 0.8rem;
    font-weight: 400;
  }

  label small.error {
    color: #dc2626;
    margin-top: 0.35rem;
  }

  .required-star {
    color: #dc2626;
    margin-right: 0.25rem;
  }

  input {
    width: 100%;
    padding: 0.85rem;
    margin-top: 0.5rem;
    border: 1px solid #cbd5e1;
    border-radius: 0.65rem;
    font-size: 1rem;
    box-sizing: border-box;
    background: #f8fafc;
  }

  .dropdown-list {
    border: 1px solid #cbd5e1;
    border-radius: 0.5rem;
    background: white;
    max-height: 150px;
    overflow-y: auto;
    z-index: 10;
    position: relative;
  }

  .dropdown-list.loading-indicator,
  .dropdown-list.no-results {
    padding: 0.6rem 0.85rem;
    color: #64748b;
    text-align: center;
    z-index: 10;
    position: relative;
  }

  .dropdown-item {
    padding: 0.6rem 0.85rem;
    cursor: pointer;
    font-size: 0.9rem;
  }

  .dropdown-item:hover {
    background: #eff6ff;
  }

  select {
    width: 100%;
    padding: 0.85rem;
    margin-top: 0.5rem;
    border: 1px solid #cbd5e1;
    border-radius: 0.65rem;
    font-size: 1rem;
    box-sizing: border-box;
    background: #f8fafc;
  }

  select:disabled {
    opacity: 0.5;
    cursor: not-allowed;
  }

  input:focus {
    outline: none;
    border-color: #2563eb;
    background: white;
  }

  button {
    width: 100%;
    padding: 0.95rem;
    border: none;
    border-radius: 0.75rem;
    background: #2563eb;
    color: white;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    margin-top: 1.5rem;
    transition: background 0.3s ease;
  }

  button:hover {
    background: #1d4ed8;
  }

  .error {
    margin-top: 1rem;
    color: #dc2626;
    font-size: 0.95rem;
  }

  .login-link {
    text-align: center;
    margin-top: 1.5rem;
    font-size: 0.95rem;
  }

  .login-link a {
    color: #2563eb;
    text-decoration: none;
    font-weight: 600;
  }

  .login-link a:hover {
    text-decoration: underline;
  }
  </style>
