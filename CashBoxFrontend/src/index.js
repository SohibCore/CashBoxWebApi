import { createRouter, createWebHistory } from 'vue-router';
import OutcomeDocumentView from '@/views/OutcomeDocumentView.vue';

const routes = [
  {
    path: '/outcome-documents',
    name: 'OutcomeDocuments',
    component: OutcomeDocumentView
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;