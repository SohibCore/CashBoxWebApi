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
          <input v-model="address" type="text" maxlength="200" required />
          <small v-if="addressError" class="error">{{ addressError }}</small>
        </label>
        <label>
          <span class="required-star">*</span> Tashkilot ID
          <input v-model="organizationId" type="number" min="1" required />
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
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { register } from '../api';

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
    const address = ref('');
    const organizationId = ref('');
    const dateOfBirth = ref('');
    const passportSeries = ref('');
    const error = ref('');

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
      const val = address.value.trim();
      if (!val) return false;
      if (val.length < 1 || val.length > 200) {
        addressError.value = '1-200 belgi oraliq\'ida bo\'lishi kerak';
        return false;
      }
      return true;
    };

    const validateOrganizationId = () => {
      organizationIdError.value = '';
      const val = parseInt(organizationId.value) || 0;
      if (val <= 0) {
        organizationIdError.value = 'Tashkilot tanlash majburiy';
        return false;
      }
      return true;
    };

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
          address: address.value.trim(),
          organizationId: parseInt(organizationId.value) || 0,
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
      address,
      organizationId,
      dateOfBirth,
      passportSeries,
      error,
      userNameError,
      emailError,
      passwordError,
      fullNameError,
      shortNameError,
      pinflError,
      phoneNumberError,
      addressError,
      organizationIdError,
      dateOfBirthError,
      passportSeriesError,
      submit,
      validateUserName,
      validateEmail,
      validatePassword,
      validateFullName,
      validateShortName,
      validatePinfl,
      validatePhoneNumber,
      validateAddress,
      validateOrganizationId,
      validateDateOfBirth,
      validatePassportSeries
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
