import { createRouter, createWebHistory } from 'vue-router';
import AuthView from './AuthView.vue'; // Root Auth wrapper

const routes = [
  {
    path: '/',
    redirect: () => (localStorage.getItem('token') ? '/users' : '/auth/login'),
  },
  {
    path: '/auth',
    component: AuthView,
    redirect: '/auth/login',
    children: [
      { path: 'login', component: () => import('./LoginView.vue') },
      { path: 'register', component: () => import('./RegisterView.vue') },
    ],
  },
  { path: '/login', redirect: '/auth/login' },
  { path: '/register', redirect: '/auth/register' },
  { path: '/profile', component: () => import('./ProfileView.vue'), meta: { requiresAuth: true } },
  { path: '/profile/fill', component: () => import('./CompleteProfileView.vue'), meta: { requiresAuth: true } },
  { path: '/users', component: () => import('./UsersView.vue'), meta: { requiresAuth: true } },
  { path: '/users/new', component: () => import('./CreateUserView.vue'), meta: { requiresAuth: true } },
  { path: '/users/edit/:id', component: () => import('./EditUserView.vue'), meta: { requiresAuth: true } },
  { path: '/user-role', component: () => import('./RoleAssignmentView.vue'), meta: { requiresAuth: true } },
  { path: '/organizations', component: () => import('./OrganizationsView.vue'), meta: { requiresAuth: true } },
  { path: '/currencies', component: () => import('./CurrenciesView.vue'), meta: { requiresAuth: true } },
  { path: '/regions', component: () => import('./RegionsView.vue'), meta: { requiresAuth: true } },
  { path: '/districts', component: () => import('./DistrictsView.vue'), meta: { requiresAuth: true } },
  { path: '/income-documents', component: () => import('./IncomeDocumentListView.vue'), meta: { requiresAuth: true } },
  { path: '/income-documents/create', component: () => import('./IncomeDocumentFormView.vue'), meta: { requiresAuth: true } },
  { path: '/income-documents/:id/edit', component: () => import('./IncomeDocumentFormView.vue'), meta: { requiresAuth: true } },
  { path: '/income-documents/table-edit/:id', component: () => import('./IncomeDocumentTableEditView.vue'), meta: { requiresAuth: true } },
  { path: '/suppliers', component: () => import('./SuppliersView.vue'), meta: { requiresAuth: true } },
  { path: '/suppliers/new', component: () => import('./SupplierFormView.vue'), meta: { requiresAuth: true } },
  { path: '/suppliers/edit/:id', component: () => import('./SupplierFormView.vue'), meta: { requiresAuth: true } },
  { path: '/weather', component: () => import('./WeatherView.vue'), meta: { requiresAuth: true } },

  // --- Chiqim Hujjatlari uchun yangi yo'nalishlar ---
  { path: '/outcome-documents', component: () => import('./OutcomeDocumentView.vue'), meta: { requiresAuth: true } },
  { path: '/outcome-documents/new', component: () => import('./OutcomeDocumentFormView.vue'), meta: { requiresAuth: true } },
  { path: '/outcome-documents/edit/:id', component: () => import('./OutcomeDocumentFormView.vue'), meta: { requiresAuth: true } },
  { path: '/outcome-documents/table-edit/:id', component: () => import('./OutcomeDocumentTableEditView.vue'), meta: { requiresAuth: true } },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.meta.requiresAuth && !token) {
    next('/auth/login');
  } else if (to.path.startsWith('/auth') && token) {
    next('/users');
  } else {
    next();
  }
});

export default router;