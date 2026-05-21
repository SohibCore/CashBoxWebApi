<template>
  <div class="app-shell">
    <aside v-if="token" class="sidebar" :class="{ collapsed: collapsed }">
      <button class="menu-toggle" @click="collapsed = !collapsed" aria-label="Toggle navigation">
        <span></span>
        <span></span>
        <span></span>
      </button>

      <div class="sidebar-brand">
        <h2>CashBox</h2>
        <p>{{ role === 'Admin' ? 'Admin panel' : 'User panel' }}</p>
      </div>

      <div class="profile-card">
        <p class="profile-name">{{ userName || 'Profil' }}</p>
        <p class="profile-email" v-if="email">{{ email }}</p>
      </div>

      <nav class="sidebar-nav">
        <router-link to="/profile" class="nav-link">Profil</router-link>
        <!-- ✅ Faqat Admin ko'radi -->
        <template v-if="role === 'Admin'">
          <router-link to="/users" class="nav-link">Foydalanuvchilar</router-link>
          <router-link to="/user-role" class="nav-link">Foydalanuvchiga rol biriktirish</router-link>
          <router-link to="/organizations" class="nav-link">Tashkilotlar</router-link>
          <router-link to="/currencies" class="nav-link">Valyutalar</router-link>
          <router-link to="/regions" class="nav-link">Viloyatlar</router-link>
          <router-link to="/districts" class="nav-link">Tumanlar</router-link>
        </template>
      </nav>
      <button class="logout-btn" @click="logout">Chiqish</button>
    </aside>

    <div class="content-area" :class="{ 'with-sidebar': token }">
      <header class="topbar">
        <div>
          <h1>CashBox</h1>
        </div>
        <div v-if="token" class="topbar-actions">
          <span>Salom, {{ userName || 'Foydalanuvchi' }}</span>
          <button @click="logout">Chiqish</button>
        </div>
      </header>

      <main>
        <router-view />
      </main>
    </div>
  </div>
</template>

<script>
import { ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { jwtDecode } from 'jwt-decode';

export default {
  setup() {
    const router = useRouter();
    const token = ref(localStorage.getItem('token'));
    const userName = ref(localStorage.getItem('userName') || '');
    const email = ref(localStorage.getItem('email') || '');
    const collapsed = ref(false);

    // ✅ Token dan rol olish
    const getUserRole = () => {
      const t = localStorage.getItem('token');
      if (!t) return null;
      try {
        const decoded = jwtDecode(t);
        // Adjust this claim name if your JWT uses a different one for roles
        return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      } catch (e) {
        console.error("Error decoding token:", e);
        return null;
      }
    };

    const role = ref(getUserRole());

    router.afterEach(() => {
      token.value = localStorage.getItem('token');
      userName.value = localStorage.getItem('userName') || '';
      email.value = localStorage.getItem('email') || '';
      role.value = getUserRole(); // ✅ rol ham yangilansin
    });

    const logout = () => {
      localStorage.removeItem('token');
      localStorage.removeItem('userName');
      localStorage.removeItem('email');
      localStorage.removeItem('userId');
      router.push('/auth/login');
    };

    return {
      token,
      userName,
      email,
      collapsed,
      role, // Return the role
      logout,
    };
  }
};
</script>

<style>
body {
  margin: 0;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', sans-serif;
  background: #f4f6fb;
}

html {
  overflow-y: scroll;
}

.sidebar {
  width: 260px;
  background: #0f172a;
  color: #f8fafc;
  padding: 1.25rem;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  position: fixed;
  height: 100vh;
  overflow-y: auto;
  z-index: 100;
}

.sidebar.collapsed {
  width: 88px;
}

.app-shell {
  display: flex;
  min-height: 100vh;
}

.content-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  margin-left: 0;
  min-height: 100vh;
}

.content-area.with-sidebar {
  margin-left: 260px;
}

.content-area.collapsed-sidebar {
  margin-left: 88px;
}

.sidebar.collapsed ~ .content-area {
  margin-left: 88px;
}

.menu-toggle {
  border: none;
  background: transparent;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  gap: 6px;
  padding: 0;
  margin-bottom: 1rem;
  width: 24px;
  height: auto;
}

.menu-toggle span {
  display: block;
  width: 24px;
  height: 2px;
  background: #f8fafc;
}

.sidebar-brand {
  overflow: hidden;
}

.sidebar.collapsed .sidebar-brand h2,
.sidebar.collapsed .sidebar-brand p,
.sidebar.collapsed .profile-card,
.sidebar.collapsed .profile-name,
.sidebar.collapsed .profile-email,
.sidebar.collapsed .sidebar-nav,
.sidebar.collapsed .nav-link {
  overflow: hidden;
  width: 0;
  height: 0;
  opacity: 0;
}

.sidebar.collapsed .logout-btn {
  width: 100%;
}



.sidebar-brand h2,
.sidebar-brand p,
.profile-card p,
.sidebar-nav .nav-link,
.logout-btn {
  margin: 0;
}

.sidebar-brand h2 {
  font-size: 1.25rem;
}

.sidebar-brand p {
  color: #94a3b8;
  margin-top: 0.4rem;
  font-size: 0.9rem;
}

.profile-card {
  padding: 1rem;
  background: #111827;
  border-radius: 0.75rem;
}

.profile-name {
  font-weight: 700;
}

.profile-email {
  color: #94a3b8;
  margin-top: 0.25rem;
  font-size: 0.9rem;
}

.sidebar-nav {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.nav-link {
  color: #cbd5e1;
  text-decoration: none;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  transition: background 0.2s ease;
}

.nav-link.router-link-active {
  background: #2563eb;
  color: white;
}

.logout-btn {
  margin-top: auto;
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.85rem 1rem;
  border-radius: 0.75rem;
  cursor: pointer;
}

.content-area {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.topbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 1rem 1.5rem;
  background: #ffffff;
  border-bottom: 1px solid #e2e8f0;
}

.topbar h1 {
  margin: 0;
}

.subtitle {
  margin: 0.25rem 0 0;
  color: #64748b;
  font-size: 0.95rem;
}

.topbar-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.topbar-actions span {
  color: #0f172a;
  font-weight: 600;
}

.topbar-actions button {
  border: none;
  background: #2563eb;
  color: white;
  padding: 0.6rem 1rem;
  border-radius: 0.5rem;
  cursor: pointer;
}

main {
  padding: 1.5rem;
  flex: 1;
  overflow-y: auto;
  display: flex;
  flex-direction: column;
}

@media (max-width: 1024px) {
  .sidebar {
    width: 200px;
  }
  
  .content-area.with-sidebar {
    margin-left: 200px;
  }
  
  .sidebar.collapsed ~ .content-area {
    margin-left: 88px;
  }
}

@media (max-width: 768px) {
  .sidebar {
    width: 100%;
    height: auto;
    position: static;
    flex-direction: row;
    align-items: center;
    justify-content: space-between;
    gap: 1rem;
    flex-wrap: wrap;
  }

  .content-area {
    margin-left: 0;
    width: 100%;
  }
  
  .sidebar-brand {
    flex: 1;
  }
  
  .sidebar-nav {
    flex-direction: row;
    gap: 0.5rem;
  }
  
  .nav-link {
    padding: 0.5rem 0.75rem;
    font-size: 0.9rem;
  }
}
</style>
