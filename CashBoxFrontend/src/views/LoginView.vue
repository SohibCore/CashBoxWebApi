  <template>
    <div class="login-form-wrap">
      <div class="form-header">
        <h2 class="form-title">Xush kelibsiz 👋</h2>
        <p class="form-subtitle">Hisobingizga kiring</p>
      </div>

      <form @submit.prevent="submit">
        <div class="fields-row">
          <div class="field">
            <label>Login</label>
            <div class="input-wrap">
              <span class="input-icon">
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
                  <circle cx="12" cy="7" r="4"/>
                </svg>
              </span>
              <input v-model="userName" type="text" autocomplete="username" placeholder="Loginni kiriting" required />
            </div>
          </div>
          <div class="field">
            <label>Parol</label>
            <div class="input-wrap password-wrap">
              <span class="input-icon">
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                  <rect x="3" y="11" width="18" height="11" rx="2"/>
                  <path d="M7 11V7a5 5 0 0 1 10 0v4"/>
                </svg>
              </span>
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                autocomplete="current-password"
                placeholder="Parolni kiriting"
                required
              />
              <button type="button" class="password-toggle" @click.prevent="showPassword = !showPassword">
                {{ showPassword ? 'Yashir' : "Ko'rsat" }}
              </button>
            </div>
          </div>
        </div>

        <div class="remember-row">
          <label class="checkbox-label">
            <input type="checkbox" />
            Eslab qolish
          </label>
          <span class="forgot-link">Parolni unutdingizmi?</span>
        </div>

        <button type="submit" class="submit-btn">Tizimga kirish</button>
        <p v-if="error" class="error-msg">{{ error }}</p>
      </form>

      <div class="divider">yoki</div>
      <p class="bottom-link">
        Akkauntingiz yo'qmi?
        <router-link to="/auth/register">Ro'yxatdan o'ting</router-link>
      </p>
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
          const data = extractApiData(response);
          const token = data?.token || data?.accessToken;

          if (token) {
            localStorage.setItem('token', token);
            let currentUser = {
              id: data?.id || data?.Id || data?.userId,
              userName: data?.userName || data?.UserName || userName.value,
              email: data?.email || data?.Email || ''
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
          console.error('Login error:', err);
          error.value = err.response?.data?.message || err.message || 'Login yoki parol noto‘g‘ri.';
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

  <style scoped>
.login-form-wrap { width: 100%; }

.form-header { margin-bottom: 22px; }
.form-title { font-size: 26px; font-weight: 600; color: var(--text-primary); margin-bottom: 5px; }
.form-subtitle { font-size: 13px; color: var(--text-secondary); }

.fields-row {
  display: grid; grid-template-columns: 1fr 1fr;
  gap: 14px; margin-bottom: 16px;
}

.field { display: flex; flex-direction: column; }
.field > label {
  font-size: 12px; color: var(--text-secondary);
  margin-bottom: 6px; font-weight: 500;
}

.input-wrap { position: relative; }
.input-wrap input {
  width: 100%;
  padding: 10px 12px 10px 38px;
  background: rgba(255, 255, 255, 0.05) !important;
  border: 1px solid rgba(255, 255, 255, 0.1) !important;
  border-radius: 9px; color: var(--text-primary);
  font-family: 'Outfit', sans-serif; font-size: 13.5px;
  outline: none;
  transition: border-color 0.2s, box-shadow 0.2s, background-color 0.2s;
  margin-top: 0;
}
.input-wrap input:hover { /* Hover holati uchun fon */
  background: rgba(255, 255, 255, 0.08) !important;
}
.input-wrap input:focus {
  border-color: var(--accent) !important;
  box-shadow: 0 0 0 3px var(--input-focus) !important;
  background: rgba(255, 255, 255, 0.12) !important;
}
.input-wrap input::placeholder { color: var(--text-muted); }
.input-icon {
  position: absolute; left: 11px; top: 50%; transform: translateY(-50%);
  color: var(--text-muted); pointer-events: none;
  display: flex; align-items: center;
}

.password-wrap input { padding-right: 72px; }
.password-toggle {
  position: absolute; right: 8px; top: 50%; transform: translateY(-50%);
  background: rgba(255,255,255,0.08);
  border: 1px solid var(--navy-border);
  border-radius: 6px; color: var(--text-secondary);
  font-size: 11px; font-family: 'Outfit', sans-serif;
  padding: 4px 8px; cursor: pointer;
  width: auto; margin: 0; transition: all 0.2s;
}
.password-toggle:hover { background: rgba(59,130,246,0.2); color: var(--accent-light); }

.remember-row {
  display: flex; justify-content: space-between; align-items: center;
  margin-bottom: 20px;
}
.checkbox-label {
  display: flex; align-items: center; gap: 7px;
  font-size: 13px; color: var(--text-secondary); cursor: pointer;
  font-weight: 400;
}
.checkbox-label input[type=checkbox] {
  width: 15px; height: 15px; accent-color: var(--accent); margin: 0;
}
.forgot-link {
  font-size: 13px; color: var(--accent-light); cursor: pointer;
}
.forgot-link:hover { text-decoration: underline; }

.submit-btn {
  width: 100%; padding: 13px;
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
  margin: 18px 0; color: var(--text-muted); font-size: 12px;
}
.divider::before, .divider::after {
  content: ''; flex: 1; height: 1px; background: var(--navy-border);
}

.bottom-link { text-align: center; font-size: 13px; color: var(--text-secondary); }
.bottom-link a { color: var(--accent-light); text-decoration: none; }
.bottom-link a:hover { text-decoration: underline; }

@media (max-width: 500px) {
  .fields-row { grid-template-columns: 1fr; }
}
</style>
