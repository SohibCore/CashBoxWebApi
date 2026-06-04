<template>
  <div class="app-layout">
    <!-- TOP NAVBAR -->
    <nav class="navbar">
      <div class="hamburger" @click="sidebarOpen = !sidebarOpen">
        <span></span><span></span><span></span>
      </div>
      <span class="navbar-brand">CashBox — Boshqaruv Paneli</span>
    </nav>

    <div class="layout-body">
      <!-- SIDEBAR -->
      <aside class="sidebar" :class="{ collapsed: !sidebarOpen }">
        <div class="sidebar-head">
          <div class="sidebar-logo">CB</div>
          <div v-if="sidebarOpen">
            <div class="sidebar-title">CashBox</div>
            <div class="sidebar-sub">Dashboard</div>
          </div>
        </div>

        <div class="sec">
          <div class="sec-label" v-if="sidebarOpen">Asosiy</div>
          <router-link class="nav-item" to="/kirim-hujjatlari" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <polyline points="16 3 21 3 21 8"/><line x1="4" y1="20" x2="21" y2="3"/><polyline points="21 16 21 21 16 21"/><line x1="15" y1="15" x2="21" y2="21"/>
            </svg>
            <span v-if="sidebarOpen">Kirim hujjatlari</span>
          </router-link>
          <router-link class="nav-item" to="/chiqim-hujjatlari" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <polyline points="23 18 13.5 8.5 8.5 13.5 1 6"/>
              <polyline points="17 18 23 18 23 12"/>
            </svg>
            <span v-if="sidebarOpen">Chiqim hujjatlari</span>
          </router-link>
        </div>

        <div class="divider"></div>

        <div class="sec">
          <div class="sec-label" v-if="sidebarOpen">Katalog</div>
          <router-link class="nav-item" to="/mahsulotlar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z"/>
            </svg>
            <span v-if="sidebarOpen">Mahsulotlar</span>
          </router-link>
          <router-link class="nav-item" to="/tashkilotlar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <rect x="2" y="3" width="20" height="14" rx="2"/>
              <path d="M8 21h8M12 17v4"/>
            </svg>
            <span v-if="sidebarOpen">Tashkilotlar</span>
          </router-link>
          <router-link class="nav-item" to="/foydalanuvchilar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/>
            </svg>
            <span v-if="sidebarOpen">Foydalanuvchilar</span>
          </router-link>
          <router-link class="nav-item" to="/valyutalar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <circle cx="12" cy="12" r="10"/>
              <line x1="2" y1="12" x2="22" y2="12"/>
              <path d="M12 2a15.3 15.3 0 0 1 4 10 15.3 15.3 0 0 1-4 10 15.3 15.3 0 0 1-4-10 15.3 15.3 0 0 1 4-10z"/>
            </svg>
            <span v-if="sidebarOpen">Valyutalar</span>
          </router-link>
        </div>

        <div class="divider"></div>

        <div class="sec">
          <div class="sec-label" v-if="sidebarOpen">Joylashuv</div>
          <router-link class="nav-item" to="/viloyatlar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/>
              <circle cx="12" cy="10" r="3"/>
            </svg>
            <span v-if="sidebarOpen">Viloyatlar</span>
          </router-link>
          <router-link class="nav-item" to="/tumanlar" active-class="active">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
              <polyline points="9 22 9 12 15 12 15 22"/>
            </svg>
            <span v-if="sidebarOpen">Tumanlar</span>
          </router-link>
        </div>

        <div class="sidebar-bottom">
          <button class="logout-btn" @click="$emit('logout')">
            <svg class="nav-icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8">
              <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
              <polyline points="16 17 21 12 16 7"/>
              <line x1="21" y1="12" x2="9" y2="12"/>
            </svg>
            <span v-if="sidebarOpen">Chiqish</span>
          </button>
        </div>
      </aside>

      <!-- PAGE CONTENT -->
      <main class="main-content">
        <slot />
      </main>
    </div>
  </div>
</template>

<script>
export default {
  name: 'AppSidebar',
  emits: ['logout'],
  data() {
    return {
      sidebarOpen: true
    }
  }
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&family=Space+Grotesk:wght@400;500;600&display=swap');

* { box-sizing: border-box; }

:root {
  --navy: #0d1117;
  --sidebar-bg: #0a0f1e;
  --card-bg: #111827;
  --card-border: rgba(255,255,255,0.07);
  --accent: #3b82f6;
  --accent-light: #60a5fa;
  --text-primary: #f1f5f9;
  --text-secondary: #94a3b8;
  --text-muted: #475569;
  --sidebar-active: rgba(59,130,246,0.15);
  --sidebar-active-border: rgba(59,130,246,0.5);
}

.app-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background: #0d1117;
  font-family: 'Outfit', sans-serif;
  color: #f1f5f9;
}

/* NAVBAR */
.navbar {
  height: 46px;
  background: #3b82f6;
  display: flex;
  align-items: center;
  padding: 0 16px;
  gap: 12px;
  flex-shrink: 0;
  position: sticky;
  top: 0;
  z-index: 100;
}
.hamburger {
  display: flex;
  flex-direction: column;
  gap: 4px;
  cursor: pointer;
  padding: 4px;
}
.hamburger span {
  display: block;
  width: 18px;
  height: 2px;
  background: white;
  border-radius: 2px;
  transition: all 0.2s;
}
.navbar-brand {
  font-family: 'Space Grotesk', sans-serif;
  font-weight: 600;
  font-size: 14px;
  color: white;
}

/* LAYOUT */
.layout-body {
  display: flex;
  flex: 1;
  overflow: hidden;
  height: calc(100vh - 46px);
}

/* SIDEBAR */
.sidebar {
  width: 220px;
  background: #0a0f1e;
  border-right: 1px solid rgba(255,255,255,0.07);
  display: flex;
  flex-direction: column;
  flex-shrink: 0;
  overflow-y: auto;
  transition: width 0.25s ease;
}
.sidebar.collapsed {
  width: 56px;
}
.sidebar-head {
  padding: 16px;
  border-bottom: 1px solid rgba(255,255,255,0.07);
  display: flex;
  align-items: center;
  gap: 10px;
  flex-shrink: 0;
}
.sidebar-logo {
  width: 30px;
  height: 30px;
  background: linear-gradient(135deg, #3b82f6, #1d4ed8);
  border-radius: 8px;
  display: flex; align-items: center; justify-content: center;
  font-family: 'Space Grotesk', sans-serif;
  font-weight: 700; font-size: 12px; color: white;
  box-shadow: 0 0 10px rgba(59,130,246,0.35);
  flex-shrink: 0;
}
.sidebar-title {
  font-family: 'Space Grotesk', sans-serif;
  font-weight: 600;
  font-size: 14px;
  color: #f1f5f9;
  white-space: nowrap;
}
.sidebar-sub {
  font-size: 11px;
  color: #475569;
}
.sec {
  padding: 14px 10px 6px;
}
.sec-label {
  font-size: 10px;
  text-transform: uppercase;
  letter-spacing: 1px;
  color: #475569;
  padding: 0 8px;
  margin-bottom: 6px;
  white-space: nowrap;
}
.nav-item {
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 9px 10px;
  border-radius: 9px;
  color: #94a3b8;
  cursor: pointer;
  font-size: 13px;
  position: relative;
  border: 1px solid transparent;
  margin-bottom: 2px;
  text-decoration: none;
  transition: all 0.15s;
  white-space: nowrap;
  overflow: hidden;
}
.nav-item:hover {
  background: rgba(255,255,255,0.04);
  color: #f1f5f9;
}
.nav-item.active {
  background: rgba(59,130,246,0.15);
  border-color: rgba(59,130,246,0.5);
  color: #60a5fa;
}
.nav-item.active::before {
  content: '';
  position: absolute;
  left: -1px;
  top: 20%;
  bottom: 20%;
  width: 3px;
  background: #3b82f6;
  border-radius: 0 3px 3px 0;
}
.nav-icon {
  width: 16px;
  height: 16px;
  opacity: 0.85;
  flex-shrink: 0;
}
.divider {
  height: 1px;
  background: rgba(255,255,255,0.07);
  margin: 4px 10px;
}
.sidebar-bottom {
  margin-top: auto;
  padding: 10px;
  border-top: 1px solid rgba(255,255,255,0.07);
}
.logout-btn {
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 9px 10px;
  border-radius: 9px;
  background: rgba(239,68,68,0.1);
  border: 1px solid rgba(239,68,68,0.25);
  color: #ef4444;
  cursor: pointer;
  font-size: 13px;
  font-family: 'Outfit', sans-serif;
  width: 100%;
  transition: all 0.15s;
  white-space: nowrap;
  overflow: hidden;
}
.logout-btn:hover {
  background: rgba(239,68,68,0.2);
}

/* MAIN */
.main-content {
  flex: 1;
  overflow-y: auto;
  padding: 24px 28px;
  background: #0d1117;
}
</style>