import { createRouter, createWebHistory } from 'vue-router';
import AppShell from '../AppShell.vue';
import AuthWrapper from '../AuthWrapper.vue';

// Auth Views
import LoginView from '../views/LoginView.vue';
import RegisterView from '../views/RegisterView.vue';

// Main App Views
import ProfileView from '../views/ProfileView.vue';
import ProfileFillView from '../views/ProfileFillView.vue';
import UsersView from '../views/UsersView.vue';
import UserFormView from '../views/UserFormView.vue';
import UserRoleAssignmentView from '../views/UserRoleAssignmentView.vue';
import OrganizationsView from '../views/OrganizationsView.vue';
import CurrenciesView from '../views/CurrenciesView.vue';
import RegionsView from '../views/RegionsView.vue';
import DistrictsView from '../views/DistrictsView.vue';
import IncomeDocumentListView from '../views/IncomeDocumentListView.vue';
import IncomeDocumentFormView from '../views/IncomeDocumentFormView.vue';
import IncomeDocumentTableEditView from '../views/IncomeDocumentTableEditView.vue'; // Assuming this exists for Income

// Outcome Document Views
import OutcomeDocumentView from '../views/OutcomeDocumentView.vue';
import OutcomeDocumentFormView from '../views/OutcomeDocumentFormView.vue';
import OutcomeDocumentTableEditView from '../views/OutcomeDocumentTableEditView.vue'; // New component

const routes = [
  {
    path: '/',
    redirect: () => (localStorage.getItem('token') ? '/users' : '/auth/login'),
  },
  {
    path: '/auth',
    component: AuthWrapper,
    redirect: '/auth/login',
    children: [
      { path: 'login', component: LoginView },
      { path: 'register', component: RegisterView },
    ],
  },
  { path: '/login', redirect: '/auth/login' },
  { path: '/register', redirect: '/auth/register' },
  { path: '/profile', component: ProfileView, meta: { requiresAuth: true } },
  { path: '/profile/fill', component: ProfileFillView, meta: { requiresAuth: true } },
  { path: '/users', component: UsersView, meta: { requiresAuth: true } },
  { path: '/users/new', component: UserFormView, meta: { requiresAuth: true } },
  { path: '/users/edit/:id', component: UserFormView, meta: { requiresAuth: true } },
  { path: '/user-role', component: UserRoleAssignmentView, meta: { requiresAuth: true } },
  { path: '/organizations', component: OrganizationsView, meta: { requiresAuth: true } },
  { path: '/currencies', component: CurrenciesView, meta: { requiresAuth: true } },
  { path: '/regions', component: RegionsView, meta: { requiresAuth: true } },
  { path: '/districts', component: DistrictsView, meta: { requiresAuth: true } },
  { path: '/income-documents', component: IncomeDocumentListView, meta: { requiresAuth: true } },
  { path: '/income-documents/create', component: IncomeDocumentFormView, meta: { requiresAuth: true } },
  { path: '/income-documents/:id/edit', component: IncomeDocumentFormView, meta: { requiresAuth: true } },
  { path: '/income-documents/table-edit/:id', component: IncomeDocumentTableEditView, meta: { requiresAuth: true } },

  // --- Chiqim Hujjatlari uchun yangi yo'nalishlar ---
  { path: '/outcome-documents', component: OutcomeDocumentView, meta: { requiresAuth: true } },
  { path: '/outcome-documents/new', component: OutcomeDocumentFormView, meta: { requiresAuth: true } },
  { path: '/outcome-documents/edit/:id', component: OutcomeDocumentFormView, meta: { requiresAuth: true } },
  { path: '/outcome-documents/table-edit/:id', component: OutcomeDocumentTableEditView, meta: { requiresAuth: true } },
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