<template>
  <div class="auth-page">
    <div class="bg-grid"></div>
    <div class="bg-glow"></div>
    <div class="bg-glow2"></div>
    <div class="auth-wrapper">
      <!-- CHAP PANEL: branding -->
      <div class="left-panel">
        <div>
          <div class="logo-badge">
            <div class="logo-icon">CB</div>
            <span class="logo-name">CashBox</span>
          </div>
          <p class="tagline">
            Moliyaviy boshqaruvni<br>
            <span>oson va tez</span><br>
            amalga oshiring
          </p>
        </div>
        <div class="stats-grid">
          <div class="stat-tile"><div class="lbl">Foydalanuvchilar</div><div class="val blue">4</div></div>
          <div class="stat-tile"><div class="lbl">Tashkilotlar</div><div class="val green">3</div></div>
          <div class="stat-tile"><div class="lbl">Valyuta</div><div class="val blue">UZS</div></div>
          <div class="stat-tile">
            <div class="lbl">Status</div>
            <div class="val green"><span class="status-dot"></span> Faol</div>
          </div>
        </div>
        <div class="footer-links">
          <a class="footer-link" href="https://t.me/bekmurodv_s" target="_blank">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 0C5.373 0 0 5.373 0 12s5.373 12 12 12 12-5.373 12-12S18.627 0 12 0zm5.894 8.221l-1.97 9.28c-.145.658-.537.818-1.084.508l-3-2.21-1.447 1.394c-.16.16-.295.295-.605.295l.213-3.053 5.56-5.023c.242-.213-.054-.333-.373-.12L7.19 13.9l-2.95-.924c-.64-.203-.658-.64.136-.953l11.57-4.461c.537-.194 1.006.131.948.659z"/>
            </svg>
            Telegram
          </a>
          <a class="footer-link" href="https://instagram.com/sohibjon_bekmurodov" target="_blank">
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="2" y="2" width="20" height="20" rx="5"/>
              <path d="M16 11.37A4 4 0 1 1 12.63 8 4 4 0 0 1 16 11.37z"/>
              <line x1="17.5" y1="6.5" x2="17.51" y2="6.5" stroke-linecap="round"/>
            </svg>
            Instagram
          </a>
        </div>
      </div>

      <!-- O'NG PANEL: router-view -->
      <div class="right-panel">
        <div class="tabs">
          <router-link to="/auth/login" class="tab-btn" active-class="active">Kirish</router-link>
          <router-link to="/auth/register" class="tab-btn" active-class="active">Ro'yxatdan o'tish</router-link>
        </div>
        <router-view />
      </div>
    </div>
  </div>
</template>

<script>
export default {
  setup() { return {}; }
};
</script>

<style>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@300;400;500;600;700&family=Space+Grotesk:wght@400;500;600&display=swap');

:root {
  --navy: #0a0f1e;
  --navy-card: #161d2f;
  --navy-border: rgba(255,255,255,0.08);
  --accent: #3b82f6;
  --accent-light: #60a5fa;
  --green: #10b981;
  --text-primary: #f1f5f9;
  --text-secondary: #94a3b8;
  --text-muted: #64748b;
  --input-bg: rgba(255,255,255,0.05);
  --input-border: rgba(255,255,255,0.08);
  --input-focus: rgba(59,130,246,0.4);
}

/* Global Input Overrides to enforce Dark Theme */
input, select, textarea {
  background: rgba(255, 255, 255, 0.05) !important;
  border: 1px solid var(--input-border) !important;
  color: #f1f5f9 !important;
  border-radius: 9px !important;
  font-family: 'Outfit', sans-serif !important;
  outline: none !important;
}

input:focus, select:focus {
  outline: none !important;
  border-color: #3b82f6 !important;
  background: rgba(255, 255, 255, 0.07) !important;
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.4) !important;
}

input::placeholder {
  color: #64748b !important;
}

/* Brauzer autofill (sariq/oq) fonini dark theme uchun to'g'rilash */
input:-webkit-autofill,
input:-webkit-autofill:hover, 
input:-webkit-autofill:focus {
  -webkit-text-fill-color: #f1f5f9 !important; /* var(--text-primary) ning solid qiymati */
  -webkit-box-shadow: 0 0 0px 1000px #1a2335 inset !important; /* O'ta to'q solid rang (shaffof emas) */
  transition: background-color 5000s ease-in-out 0s;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.auth-page {
  font-family: 'Outfit', sans-serif;
  background: var(--navy);
  min-height: 100vh;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative;
  overflow: hidden;
}

.bg-grid {
  position: fixed; inset: 0; z-index: 0;
  background-image:
    linear-gradient(rgba(59,130,246,0.03) 1px, transparent 1px),
    linear-gradient(90deg, rgba(59,130,246,0.03) 1px, transparent 1px);
  background-size: 40px 40px;
  pointer-events: none;
}
.bg-glow {
  position: fixed; top: -200px; right: -200px;
  width: 600px; height: 600px;
  background: radial-gradient(circle, rgba(59,130,246,0.12) 0%, transparent 70%);
  z-index: 0; pointer-events: none;
  animation: authPulse 6s ease-in-out infinite;
}
.bg-glow2 {
  position: fixed; bottom: -200px; left: -100px;
  width: 500px; height: 500px;
  background: radial-gradient(circle, rgba(16,185,129,0.08) 0%, transparent 70%);
  z-index: 0; pointer-events: none;
  animation: authPulse 8s ease-in-out infinite reverse;
}
@keyframes authPulse { 0%,100%{opacity:1} 50%{opacity:0.6} }
@keyframes authSlideUp { from{opacity:0;transform:translateY(20px)} to{opacity:1;transform:translateY(0)} }

.auth-wrapper {
  position: relative; margin: auto; z-index: 1;
  display: grid;
  grid-template-columns: 300px 1fr;
  width: 1100px;
  max-width: 96vw;
  max-height: calc(100vh - 2rem);
  border-radius: 24px;
  overflow: hidden;
  border: 1px solid var(--navy-border);
  box-shadow: 0 32px 80px rgba(0,0,0,0.6);
  animation: authSlideUp 0.5s ease both;
}

/* CHAP PANEL */
.left-panel {
  background: linear-gradient(145deg, #0f172a 0%, #1a2540 100%);
  padding: 44px 36px;
  min-height: 480px;
  display: flex; flex-direction: column;
  justify-content: space-between;
  border-right: 1px solid var(--navy-border);
  position: relative; overflow: hidden;
}
.left-panel::before {
  content: '';
  position: absolute; top: 0; left: 0; right: 0; bottom: 0;
  background: radial-gradient(ellipse at 20% 20%, rgba(59,130,246,0.1) 0%, transparent 60%);
  pointer-events: none;
}

.logo-badge {
  display: inline-flex; align-items: center; gap: 10px;
  background: rgba(59,130,246,0.1);
  border: 1px solid rgba(59,130,246,0.25);
  border-radius: 12px; padding: 7px 14px;
  margin-bottom: 24px; width: fit-content;
  position: relative; z-index: 1;
}
.logo-icon {
  width: 30px; height: 30px;
  background: linear-gradient(135deg, #3b82f6, #2563eb);
  border-radius: 8px;
  display: flex; align-items: center; justify-content: center;
  font-family: 'Space Grotesk', sans-serif;
  font-weight: 700; font-size: 12px; color: white;
  box-shadow: 0 0 14px rgba(59,130,246,0.5);
}
.logo-name {
  font-family: 'Space Grotesk', sans-serif;
  font-weight: 600; font-size: 15px; color: var(--text-primary);
}
.tagline {
  font-size: 20px; font-weight: 600; line-height: 1.4;
  color: var(--text-primary);
  position: relative; z-index: 1; margin-bottom: 24px;
}
.tagline span { color: var(--accent-light); }

.stats-grid {
  display: grid; grid-template-columns: 1fr 1fr;
  gap: 8px; position: relative; z-index: 1;
}
.stat-tile {
  background: rgba(255,255,255,0.04);
  border: 1px solid var(--navy-border);
  border-radius: 10px; padding: 10px 12px;
}
.stat-tile .lbl {
  font-size: 10px; color: var(--text-muted);
  text-transform: uppercase; letter-spacing: 0.7px; margin-bottom: 4px;
}
.stat-tile .val {
  font-size: 17px; font-weight: 600;
  font-family: 'Space Grotesk', sans-serif;
}
.stat-tile .val.blue { color: var(--accent-light); }
.stat-tile .val.green {
  color: var(--green); font-size: 13px;
  display: flex; align-items: center; gap: 5px;
}
.status-dot {
  width: 7px; height: 7px;
  background: var(--green); border-radius: 50%;
  box-shadow: 0 0 7px var(--green);
  display: inline-block;
}
.footer-links {
  display: flex; gap: 12px;
  position: relative; z-index: 1; flex-wrap: wrap;
}
.footer-link {
  display: flex; align-items: center; gap: 5px;
  color: var(--text-muted); font-size: 12px;
  text-decoration: none; transition: color 0.2s;
}
.footer-link:hover { color: var(--accent-light); }

/* O'NG PANEL */
.right-panel {
  background: var(--navy-card);
  padding: 44px 52px;
  display: flex; flex-direction: column;
  overflow-y: auto;
  scrollbar-width: thin;
  scrollbar-color: rgba(255,255,255,0.1) transparent;
}
.right-panel::-webkit-scrollbar { width: 5px; }
.right-panel::-webkit-scrollbar-thumb {
  background: rgba(255,255,255,0.1); border-radius: 3px;
}

/* TABS */
.tabs {
  display: flex;
  background: rgba(255,255,255,0.04);
  border: 1px solid var(--navy-border);
  border-radius: 12px; padding: 4px;
  margin-bottom: 32px; flex-shrink: 0;
}
.tab-btn {
  flex: 1; padding: 9px;
  border: none; border-radius: 9px;
  background: transparent; color: var(--text-secondary);
  font-family: 'Outfit', sans-serif; font-size: 14px; font-weight: 500;
  cursor: pointer; transition: all 0.2s;
  text-decoration: none; text-align: center;
  border: 1px solid transparent;
}
.tab-btn.active,
.tab-btn.router-link-active {
  background: rgba(59,130,246,0.18);
  color: var(--accent-light);
  border-color: rgba(59,130,246,0.35);
}

@media (max-width: 720px) {
  .auth-wrapper { grid-template-columns: 1fr; }
  .left-panel { display: none; }
  .right-panel { padding: 28px 20px; }
}
</style>