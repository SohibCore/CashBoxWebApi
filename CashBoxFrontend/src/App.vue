<template>
  <div class="app-layout">
    <!-- Asosiy Top Header - Faqat login qilinganda ko'rinadi -->
    <header v-if="token" class="topbar">
      <button @click="isCollapsed = !isCollapsed" class="menu-toggle" aria-label="Toggle Menu">
        <span></span>
        <span></span>
        <span></span>
      </button>
      <div class="header-brand">CashBox — Boshqaruv Paneli</div>
    </header>

    <div class="app-shell">
      <!-- Sidebar (Chap menyu) -->
      <aside v-if="token" class="sidebar" :class="{ 'collapsed': isCollapsed }">
        <nav class="sidebar-nav">
          <!-- Asosiy operatsiyalar -->
          <div class="nav-section-label">Asosiy</div>
          <router-link to="/income-documents" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polyline points="16 3 21 3 21 8"/><line x1="4" y1="20" x2="21" y2="3"/><polyline points="21 16 21 21 16 21"/><line x1="15" y1="15" x2="21" y2="21"/></svg>
            <span>Kirim hujjatlari</span>
          </router-link>
          <router-link to="/outcome-documents" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><polyline points="23 18 13.5 8.5 8.5 13.5 1 6"/><polyline points="17 18 23 18 23 12"/></svg>
            <span>Chiqim hujjatlari</span>
          </router-link>

          <div class="nav-section-label">Katalog</div>
          <router-link to="/products" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/></svg>
            <span>Mahsulotlar</span>
          </router-link>
          <router-link to="/organizations" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="2" y="3" width="20" height="14" rx="2"/><path d="M8 21h8M12 17v4"/></svg>
            <span>Tashkilotlar</span>
          </router-link>
          <router-link to="/users" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
            <span>Foydalanuvchilar</span>
          </router-link>
          <router-link to="/currencies" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="10"/><line x1="2" y1="12" x2="22" y2="12"/><path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"/></svg>
            <span>Valyutalar</span>
          </router-link>

          <div class="nav-section-label">Joylashuv</div>
          <router-link to="/regions" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/></svg>
            <span>Viloyatlar</span>
          </router-link>
          <router-link to="/districts" class="nav-link">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>
            <span>Tumanlar</span>
          </router-link>
        </nav>
        
        <button @click="logout" class="logout-btn">
          <span class="icon">🚪</span> <span>Chiqish</span>
        </button>
      </aside>

      <!-- Asosiy kontent qismi -->
      <div class="content-area">
        <!-- Mehmonlar uchun header -->
        <header v-if="!token && !isAuthPage" class="unauth-header">
          <router-link to="/auth/login" class="unauth-link">Kirish</router-link>
          <router-link to="/auth/register" class="unauth-link">Ro'yxatdan o'tish</router-link>
        </header>

        <main class="router-content" :class="{ 'auth-mode': isAuthPage }">
          <router-view />
        </main>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch, computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();
const token = ref(localStorage.getItem('token'));
const isCollapsed = ref(false);

// Check if the current route is an authentication page
const isAuthPage = computed(() => route.path.startsWith('/auth/'));

// Sahifa o'zgarganda token borligini tekshirib turish (Login/Logout uchun)
watch(() => route.path, () => {
  token.value = localStorage.getItem('token');
});

const logout = () => {
  localStorage.removeItem('token');
  token.value = null;
  router.push('/auth/login'); // Redirect to login page after logout
};
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&family=Space+Grotesk:wght@400;500;600&display=swap');

/* Global styles for the overall application layout */
.app-layout {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

body { margin: 0; font-family: 'Outfit', sans-serif; background: #0d1117; color: #f1f5f9; height: 100vh; overflow: hidden; }
.topbar { height: 46px; background: #3b82f6; display: flex; align-items: center; padding: 0 16px; gap: 12px; }
.header-brand { font-family: 'Space Grotesk', sans-serif; font-weight: 600; font-size: 14px; color: white; }
.app-shell { display: flex; flex: 1; background: #0d1117; overflow: hidden; }
.sidebar { width: 220px; background: #0a0f1e; border-right: 1px solid rgba(255,255,255,0.07); padding: 0; transition: all 0.3s ease; }
.sidebar.collapsed { width: 64px; }
.nav-section-label { font-size: 10px; text-transform: uppercase; letter-spacing: 1px; color: #475569; padding: 16px 16px 8px; }
.nav-link { display: flex; align-items: center; gap: 10px; padding: 10px 16px; color: #94a3b8; text-decoration: none; font-size: 13px; transition: 0.2s; }
.nav-link:hover { background: rgba(255,255,255,0.04); color: #f1f5f9; }
.nav-link.router-link-active { background: rgba(59,130,246,0.15); color: #60a5fa; border-left: 3px solid #3b82f6; }
.nav-icon { width: 16px; height: 16px; flex-shrink: 0; }
.sidebar.collapsed .nav-section-label, .sidebar.collapsed span { display: none; }

.content-area { flex: 1; display: flex; flex-direction: column; background: #0d1117; }

.sidebar-header {
  text-align: center;
  margin-bottom: 30px;
}

.sidebar.collapsed .sidebar-header {
  justify-content: center;
}

.sidebar-brand {
  font-size: 22px;
  margin: 0;
  color: #3b82f6; /* Bright blue accent for the brand */
}

.header-title {
  font-weight: 700;
  font-size: 1.1rem;
  color: #1e293b;
}

.menu-toggle {
  background: #3b82f6;
  border: none;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  gap: 4px;
  padding: 10px;
  border-radius: 8px;
}

.menu-toggle span {
  display: block;
  width: 22px;
  height: 2px;
  background-color: white;
}

.sidebar-nav {
  flex-grow: 1; /* Allows navigation to take up available space */
}

.nav-link {
  display: flex;
  align-items: center;
  padding: 10px 15px;
  margin-bottom: 5px;
  color: #cbd5e1;
  text-decoration: none;
  border-radius: 8px;
  transition: background-color 0.2s, color 0.2s;
  white-space: nowrap;
}

.nav-link .icon {
  font-size: 1.4rem;
  margin-right: 12px;
  display: inline-block;
  width: 24px;
  text-align: center;
}

.nav-link:hover, .nav-link.router-link-active {
  background-color: #334155;
  color: #ffffff;
}

.nav-divider {
  border-top: 1px solid #475569;
  margin: 20px 0;
}

.logout-btn {
  background-color: #ef4444;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-size: 16px;
  font-weight: bold;
  transition: background-color 0.2s;
  margin-top: auto;
}

.logout-btn:hover {
  background-color: #dc2626;
}

/* Asosiy kontent qismi */
.content-area {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow-y: auto;
  background-color: #0d1117;
}

.router-content {
  padding: 24px;
  flex: 1;
  background: #0d1117;
  min-height: 100%;
}

.router-content.auth-mode {
  padding: 0;
}

.unauth-header {
  display: flex;
  justify-content: flex-end;
  padding: 15px 30px;
  background-color: #0a0f1e;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  gap: 20px;
}

.unauth-link {
  color: #3b82f6; /* Blue link color */
  text-decoration: none;
  font-weight: 600;
  padding: 8px 15px;
  border-radius: 6px;
  transition: background-color 0.2s;
}

.unauth-link:hover {
  background-color: #eff6ff; /* Light blue background on hover */
}

/* Basic styles for router-view content (from previous context, ensure they are still here) */
.p-6 { padding: 1.5rem; }
.max-w-5xl { max-width: 80rem; }
.mx-auto { margin-left: auto; margin-right: auto; }
.bg-white { background-color: #fff; }
.rounded-lg { border-radius: 0.5rem; }
.shadow-lg { box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05); }
.p-8 { padding: 2rem; }
.text-2xl { font-size: 1.5rem; }
.font-bold { font-weight: 700; }
.mb-6 { margin-bottom: 1.5rem; }
.text-gray-800 { color: #1f2937; }
.grid { display: grid; }
.grid-cols-1 { grid-template-columns: repeat(1, minmax(0, 1fr)); }
.md\:grid-cols-2 { /* For medium screens and up */ }
.gap-6 { gap: 1.5rem; }
.mb-8 { margin-bottom: 2rem; }
.block { display: block; }
.text-sm { font-size: 0.875rem; }
.font-semibold { font-weight: 600; }
.mb-1 { margin-bottom: 0.25rem; }
.text-gray-600 { color: #4b5563; }
.w-full { width: 100%; }
.px-4 { padding-left: 1rem; padding-right: 1rem; }
.py-2 { padding-top: 0.5rem; padding-bottom: 0.5rem; }
.border { border-width: 1px; }
.rounded { border-radius: 0.25rem; }
.shadow-sm { box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05); }
.focus\:ring-2:focus { --tw-ring-offset-shadow: var(--tw-ring-inset) 0 0 0 var(--tw-ring-offset-width) var(--tw-ring-offset-color); --tw-ring-shadow: var(--tw-ring-inset) 0 0 0 calc(2px + var(--tw-ring-offset-width)) var(--tw-ring-color); box-shadow: var(--tw-ring-offset-shadow), var(--tw-ring-shadow), var(--tw-shadow, 0 0 #0000); }
.focus\:ring-blue-500:focus { --tw-ring-color: #3b82f6; }
.outline-none:focus { outline: 2px solid transparent; outline-offset: 2px; }
.bg-white { background-color: #fff; }
.mb-6 { margin-bottom: 1.5rem; }
.flex { display: flex; }
.justify-between { justify-content: space-between; }
.items-center { align-items: center; }
.mb-4 { margin-bottom: 1rem; }
.text-lg { font-size: 1.125rem; }
.bg-green-600 { background-color: #059669; }
.text-white { color: #fff; }
.px-3 { padding-left: 0.75rem; padding-right: 0.75rem; }
.py-1 { padding-top: 0.25rem; padding-bottom: 0.25rem; }
.text-sm { font-size: 0.875rem; }
.hover\:bg-green-700:hover { background-color: #047857; }
.w-full { width: 100%; }
.border-collapse { border-collapse: collapse; }
.bg-gray-100 { background-color: #f3f4f6; }
.border-b { border-bottom-width: 1px; }
.p-2 { padding: 0.5rem; }
.text-left { text-align: left; }
.w-1\/3 { width: 33.333333%; }
.w-24 { width: 6rem; }
.w-16 { width: 4rem; }
.hover\:bg-gray-50:hover { background-color: #f9fafb; }
.p-1 { padding: 0.25rem; }
.text-red-500 { color: #ef4444; }
.hover\:text-red-700:hover { color: #b91c1c; }
.flex-col { flex-direction: column; }
.items-end { align-items: flex-end; }
.border-t { border-top-width: 1px; }
.pt-4 { padding-top: 1rem; }
.text-3xl { font-size: 1.875rem; }
.text-blue-700 { color: #1d4ed8; }
.gap-4 { gap: 1rem; }
.mt-8 { margin-top: 2rem; }
.flex-1 { flex: 1 1 0%; }
.bg-blue-600 { background-color: #2563eb; }
.py-3 { padding-top: 0.75rem; padding-bottom: 0.75rem; }
.rounded-lg { border-radius: 0.5rem; }
.hover\:bg-blue-700:hover { background-color: #1d4ed8; }
.transition { transition-property: color, background-color, border-color, text-decoration-color, fill, stroke, opacity, box-shadow, transform, filter, backdrop-filter; transition-timing-function: cubic-bezier(0.4, 0, 0.2, 1); transition-duration: 150ms; }
.disabled\:opacity-50:disabled { opacity: 0.5; }
.bg-gray-500 { background-color: #6b7280; }
.px-8 { padding-left: 2rem; padding-right: 2rem; }
.hover\:bg-gray-600:hover { background-color: #4b5563; }
</style>