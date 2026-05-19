<template>
  <div class="page-card">
    <h2>Kirish</h2>
    <form @submit.prevent="submit">
      <label>
        Login
        <input v-model="userName" type="text" autocomplete="username" required />
      </label>
      <label>
        Parol
        <div class="password-wrap">
          <input
            v-model="password"
            :type="showPassword ? 'text' : 'password'"
            autocomplete="current-password"
            required
          />
          <button
            type="button"
            class="password-toggle"
            :aria-pressed="showPassword"
            :aria-label="showPassword ? 'Parolni yashirish' : 'Parolni ko‘rsatish'"
            @click.prevent="showPassword = !showPassword"
          >
            {{ showPassword ? 'Yashirish' : 'Ko‘rsatish' }}
          </button>
        </div>
      </label>
      <button type="submit">Kirish</button>
      <p v-if="error" class="error">{{ error }}</p>
    </form>
    <p class="signup-link">Akauntingiz yo'qmi? <router-link to="/auth/register">Ro'yxatdan o'ting</router-link></p>
  </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { login, getMe, normalizeUser, extractApiData } from '../api';

export default {
  setup() {
    const router = useRouter();
    const userName = ref('');
    const password = ref('');
    const showPassword = ref(false);
    const error = ref('');

    const submit = async () => {
      error.value = '';
      try {
        const response = await login({ userName: userName.value, password: password.value });
        const payload = response.data?.data || response.data || {};
        const token = payload.token || payload?.accessToken;
        if (token) {
          localStorage.setItem('token', token);
          let currentUser = {
            id: payload.id || payload?.Id || payload?.userId || payload?.userID,
            userName: payload.userName || payload?.UserName || userName.value,
            email: payload.email || payload?.Email || ''
          };
          try {
            const meResponse = await getMe();
            const meData = normalizeUser(extractApiData(meResponse));
            if (meData) {
              currentUser = {
                ...currentUser,
                ...meData
              };
            }
          } catch (meError) {
            console.warn('GetMe failed after login', meError);
          }
          localStorage.setItem('userName', currentUser.userName || userName.value);
          localStorage.setItem('email', currentUser.email || '');
          if (currentUser.id) {
            localStorage.setItem('userId', currentUser.id);
          }
          localStorage.setItem('currentUser', JSON.stringify(currentUser));
          router.push('/users');
        } else {
          throw new Error('Token topilmadi');
        }
      } catch (err) {
        console.error(err);
        error.value = 'Login yoki parol noto‘g‘ri.';
      }
    };

    return {
      userName,
      password,
      showPassword,
      error,
      submit
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
  box-shadow: none;
}

.page-card h2 {
  margin-top: 0;
  margin-bottom: 1.5rem;
  text-align: center;
  font-size: 1.75rem;
}

label {
  display: block;
  margin-bottom: 1.25rem;
  font-weight: 600;
  color: #0f172a;
}

.password-wrap {
  position: relative;
  margin-top: 0.5rem;
}

.password-wrap input {
  width: 100%;
  padding: 0.85rem 6.5rem 0.85rem 0.85rem;
  margin-top: 0;
  border: 1px solid #cbd5e1;
  border-radius: 0.65rem;
  font-size: 1rem;
  box-sizing: border-box;
  background: #f8fafc;
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

.password-wrap input:focus {
  background: white;
}

.password-toggle {
  position: absolute;
  right: 0.4rem;
  top: 50%;
  transform: translateY(-50%);
  width: auto;
  margin: 0;
  padding: 0.45rem 0.65rem;
  font-size: 0.82rem;
  font-weight: 600;
  background: #e2e8f0;
  color: #0f172a;
  border: none;
  border-radius: 0.45rem;
  cursor: pointer;
}

.password-toggle:hover {
  background: #cbd5e1;
}

form > button[type='submit'] {
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

form > button[type='submit']:hover {
  background: #1d4ed8;
}

.error {
  margin-top: 1rem;
  color: #dc2626;
}

.signup-link {
  text-align: center;
  margin-top: 1.5rem;
  font-size: 0.95rem;
}

.signup-link a {
  color: #2563eb;
  text-decoration: none;
  font-weight: 600;
}

.signup-link a:hover {
  text-decoration: underline;
}
</style>
