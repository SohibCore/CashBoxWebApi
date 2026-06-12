<template>
  <div v-if="tabs.length > 0" class="nav-history-bar">
    <div 
      v-for="(tab, index) in tabs" 
      :key="tab.fullPath" 
      class="nav-tab"
      :class="{ active: $route.fullPath === tab.fullPath }"
      @click="$router.push(tab.fullPath)"
    >
      <span class="tab-label">{{ tab.name }}</span>
      <button class="tab-close" @click.stop="removeTab(index)">
        <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="4">
          <line x1="18" y1="6" x2="6" y2="18"></line>
          <line x1="6" y1="6" x2="18" y2="18"></line>
        </svg>
      </button>
    </div>
  </div>
</template>

<script>
import { ref } from 'vue';
// Global state - komponentdan tashqarida bo'lgani uchun sahifa almashsa ham saqlanib qoladi
const tabs = ref([]);
const currentCategory = ref('');
export default { name: 'NavigationHistory' }
</script>

<script setup>
import { watch, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();

const routeLabels = {
  'users': '👥 Foydalanuvchilar',
  'organizations': '🏢 Tashkilotlar',
  'currencies': '💰 Valyutalar',
  'regions': '🗺️ Viloyatlar',
  'districts': '📍 Tumanlar',
  'income-documents': '📥 Kirim',
  'outcome-documents': '📤 Chiqim',
  'products': '📦 Mahsulotlar',
  'suppliers': '🚛 Ta\'minotchilar',
  'user-role': '🔐 Rollar',
  'profile': '👤 Profil'
};

const getTabName = (r) => {
  const path = r.path;
  if (path.endsWith('/new')) return 'Yangi qo\'shish';
  if (path.includes('/edit/')) return `Tahrirlash: #${r.params.id || ''}`;
  if (path.includes('/table-edit/')) return `Element tahrirlash`;
  
  const category = path.split('/')[1];
  return routeLabels[category] || category;
};

const updateTabs = (newRoute) => {
  // Agar argument berilmagan bo'lsa, joriy route'dan foydalanamiz
  const r = newRoute || route;
  if (!r.path) return;

  const pathParts = r.path.split('/');
  const category = pathParts[1];

  if (!category || category === 'auth') return;

  // Agar menyu (category) o'zgarsa, tarixni tozalash
  if (currentCategory.value && currentCategory.value !== category) {
    tabs.value = [];
  }
  currentCategory.value = category;

  const existingTab = tabs.value.find(t => t.fullPath === r.fullPath);
  if (!existingTab) {
    tabs.value.push({
      name: getTabName(r),
      fullPath: r.fullPath
    });
  }
};

watch(() => route.fullPath, updateTabs);
onMounted(updateTabs);

const removeTab = (index) => {
  const isCurrent = tabs.value[index].fullPath === route.fullPath;
  tabs.value.splice(index, 1);
  
  if (isCurrent) {
    if (tabs.value.length > 0) {
      router.push(tabs.value[tabs.value.length - 1].fullPath);
    } else {
      router.push(`/${currentCategory.value}`);
    }
  }
};
</script>

<style scoped>
.nav-history-bar {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 1rem;
  padding: 10px 0;
  overflow-x: auto;
  scrollbar-width: none;
  position: sticky;
  top: 0;
  z-index: 50;
  z-index: 100; /* Yuqoriroq z-index */
  background: #0d1117; /* Sahifa foni bilan bir xil */
  background: #0d1117; /* Sahifa foni bilan bir xil */
  border-bottom: 1px solid rgba(255, 255, 255, 0.05);
}
.nav-history-bar::-webkit-scrollbar { display: none; }

.nav-tab {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 6px 12px;
  background: #1e293b;
  background: #111827; /* Karta foni bilan bir xil */
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 8px;
  color: #cbd5e1;
  color: #94a3b8;
  font-size: 13px;
  cursor: pointer;
  transition: 0.2s;
  white-space: nowrap;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}
.nav-tab:hover { border-color: #3b82f6; color: #f1f5f9; }
.nav-tab.active { background: #2563eb; border-color: #3b82f6; color: white; }
.nav-tab:hover { border-color: #3b82f6; color: #f1f5f9; background: rgba(59, 130, 246, 0.1); }
.nav-tab.active { background: #2563eb; border-color: #3b82f6; color: white; font-weight: 600; }
.tab-close {
  border: none;
  background: transparent;
  color: #64748b;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2px;
  border-radius: 4px;
  cursor: pointer;
}
.tab-close:hover { background: rgba(239, 68, 68, 0.2); color: #ef4444; }
</style>