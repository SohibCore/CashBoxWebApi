<template>
  <div class="nav-history">
    <div class="history-content">
      <span class="history-item" @click="$router.back()">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
          <line x1="19" y1="12" x2="5" y2="12"></line>
          <polyline points="12 19 5 12 12 5"></polyline>
        </svg>
        Orqaga
      </span>
    </div>
  </div>
</template>

<script setup>
// Navigatsiya tarixi uchun sodda komponent
</script>

<style scoped>
.nav-history {
  padding: 10px 24px;
  background: #0d1117;
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}
.history-item {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  color: #94a3b8;
  cursor: pointer;
  font-size: 14px;
}
.history-item:hover { color: #f1f5f9; }
</style>