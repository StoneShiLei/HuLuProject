import { createRouter, createWebHistory } from 'vue-router'
import App from './App.vue'
import Index from './pages/Index.vue'


const routes = [
    { path: '/', redirect: '/index' },
    { path: '/index', component: Index },
]

export default createRouter({
    history: createWebHistory(),
    routes: routes
})