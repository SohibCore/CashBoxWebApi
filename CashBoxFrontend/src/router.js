import { createRouter, createWebHistory } from 'vue-router';
import AuthView from './views/AuthView.vue';
import LoginView from './views/LoginView.vue';
import RegisterView from './views/RegisterView.vue';
import UsersView from './views/UsersView.vue';
import OrganizationsView from './views/OrganizationsView.vue';
import EditUserView from './views/EditUserView.vue';
import CreateUserView from './views/CreateUserView.vue';
import ProfileView from './views/ProfileView.vue';
import CompleteProfileView from './views/CompleteProfileView.vue';
import RoleAssignmentView from './views/RoleAssignmentView.vue';
import CurrenciesView from './views/CurrenciesView.vue';
import RegionsView from './views/RegionsView.vue';
import DistrictsView from './views/DistrictsView.vue';
import CurrencyFormView from './views/CurrencyFormView.vue';
import OrganizationFormView from './views/OrganizationFormView.vue';
import RegionFormView from './views/RegionFormView.vue';
import DistrictFormView from './views/DistrictFormView.vue';

const routes = [
  {
    path: '/',
    redirect: () => (localStorage.getItem('token') ? '/users' : '/auth/login')
  },
  {
    path: '/auth',
    component: AuthView,
    redirect: '/auth/login',
    children: [
      { path: 'login', component: LoginView },
      { path: 'register', component: RegisterView }
    ]
  },
  { path: '/login', redirect: '/auth/login' },
  { path: '/register', redirect: '/auth/register' },
  { path: '/profile', component: ProfileView, meta: { requiresAuth: true } },
  { path: '/profile/fill', component: CompleteProfileView, meta: { requiresAuth: true } },
  { path: '/users', component: UsersView, meta: { requiresAuth: true } },
  { path: '/users/new', component: CreateUserView, meta: { requiresAuth: true } },
  { path: '/users/edit/:id', component: EditUserView, meta: { requiresAuth: true } },
  { path: '/user-role', component: RoleAssignmentView, meta: { requiresAuth: true } },
  { path: '/organizations', component: OrganizationsView, meta: { requiresAuth: true } },
  { path: '/organizations/new', component: OrganizationFormView, meta: { requiresAuth: true } },
  { path: '/organizations/edit/:id', component: OrganizationFormView, meta: { requiresAuth: true } },
  { path: '/currencies', component: CurrenciesView, meta: { requiresAuth: true } },
  { path: '/currencies/new', component: CurrencyFormView, meta: { requiresAuth: true } },
  { path: '/currencies/edit/:id', component: CurrencyFormView, meta: { requiresAuth: true } },
  { path: '/regions', component: RegionsView, meta: { requiresAuth: true } },
  { path: '/regions/new', component: RegionFormView, meta: { requiresAuth: true } },
  { path: '/regions/edit/:id', component: RegionFormView, meta: { requiresAuth: true } },
  { path: '/districts', component: DistrictsView, meta: { requiresAuth: true } },
  { path: '/districts/new', component: DistrictFormView, meta: { requiresAuth: true } },
  { path: '/districts/edit/:id', component: DistrictFormView, meta: { requiresAuth: true } }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token');
  if (to.meta.requiresAuth && !token) {
    return next('/auth/login');
  }
  if (to.path.startsWith('/auth') && token) {
    return next('/users');
  }
  next();
});

export default router;
