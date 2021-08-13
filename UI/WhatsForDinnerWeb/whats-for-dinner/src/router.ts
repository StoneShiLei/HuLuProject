import { createRouter, createWebHistory } from 'vue-router'
import Index from './pages/Index.vue'
import Login from './pages/Login.vue'


const routes = [
    { path: '/', redirect: '/index' },
    { path: '/index', component: Index },
    { path: '/login', component: Login },
    { path: '/register', component: Index },
    { path: '/cms', component: Index },
]

export default createRouter({
    history: createWebHistory(),
    routes: routes
})