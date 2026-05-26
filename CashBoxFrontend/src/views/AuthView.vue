<template>
  <div class="auth-wrapper">
    <div class="auth-container">    
      <div class="auth-box">
        <h1 class="auth-logo">CashBox</h1>
        <div class="auth-tabs">
          <router-link to="/auth/login" class="auth-tab" active-class="active-tab">Kirish</router-link>
          <router-link to="/auth/register" class="auth-tab" active-class="active-tab">Ro'yxatdan o'tish</router-link>
        </div>
        <div class="auth-panel">
          <router-view />
        </div>
        <div class="auth-footer">
          <span class="footer-label">Aloqa uchun:</span>
          <div class="footer-links">
            <a href="https://t.me/bekmurodv_s" target="_blank" class="footer-link">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M22 2L11 13"></path><polygon points="22 2 15 22 11 13 2 9 22 2"></polygon></svg>
              Telegram
            </a>
            <a href="https://instagram.com/sohibjon_bekmurodov" target="_blank" class="footer-link">
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="2" y="2" width="20" height="20" rx="5" ry="5"></rect><path d="M16 11.37A4 4 0 1 1 12.63 8 4 4 0 0 1 16 11.37z"></path><line x1="17.5" y1="6.5" x2="17.51" y2="6.5"></line></svg>
              Instagram
            </a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  setup() {
    return {};
  }
};
</script>

<style>
html {
  scrollbar-width: none;
}

html::-webkit-scrollbar {
  display: none;
}

.auth-wrapper {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f4f6fb 0%, #e8edf5 100%);
  z-index: 1000;
  padding: 1rem;
  overflow: hidden;
}

.auth-container {
  width: calc(100% - 2rem);
  max-width: 1200px;
  padding: 0;
}

.auth-box {
  background: white;
  border-radius: 1.25rem;
  padding: 0;
  box-shadow: 0 20px 60px rgba(15, 23, 42, 0.15);
  width: 100%;
  box-sizing: border-box;
  display: flex;
  flex-direction: column;
  max-height: calc(100vh - 2rem);
  overflow: hidden;
}

.auth-logo {
  text-align: center;
  margin: 0;
  padding: 2rem 2rem 0 2rem;
  font-size: 2rem;
  color: #0f172a;
  flex-shrink: 0;
}

.auth-tabs {
  display: flex;
  gap: 0.5rem;
  margin: 1rem 2rem 0 2rem;
  justify-content: center;
  position: relative;
  z-index: 10;
  flex-shrink: 0;
}

.auth-tab {
  flex: 1;
  text-align: center;
  padding: 0.95rem 1rem;
  border-radius: 0.85rem;
  background: #e2e8f0;
  color: #0f172a;
  text-decoration: none;
  font-weight: 700;
  transition: all 0.3s ease;
  border: 2px solid transparent;
  cursor: pointer;
}

.auth-tab.active-tab {
  background: #2563eb;
  color: white;
}

.auth-panel {
  background: transparent;
  overflow-y: auto;
  overflow-x: hidden;
  flex: 1;
  padding: 1.5rem 2rem 2rem 2rem;
  scrollbar-width: thin;
  scrollbar-color: #cbd5e1 transparent;
}

.auth-panel::-webkit-scrollbar {
  width: 6px;
}

.auth-panel::-webkit-scrollbar-track {
  background: transparent;
}

.auth-panel::-webkit-scrollbar-thumb {
  background: #cbd5e1;
  border-radius: 3px;
}

.auth-panel::-webkit-scrollbar-thumb:hover {
  background: #94a3b8;
}

.auth-footer {
  border-top: 1px solid #0f172a;
  padding: 1rem 2rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 1rem;
  flex-shrink: 0;
  background: #ffffff;
}

.footer-label {
  font-size: 0.9rem;
  font-weight: 600;
  color: #64748b;
}

.footer-links {
  display: flex;
  gap: 1.5rem;
}

.footer-link {
  display: flex;
  align-items: center;
  gap: 0.4rem;
  font-size: 0.9rem;
  color: #0f172a;
  text-decoration: none;
  transition: color 0.2s ease;
}

.footer-link:hover {
  color: #2563eb;
}
</style>