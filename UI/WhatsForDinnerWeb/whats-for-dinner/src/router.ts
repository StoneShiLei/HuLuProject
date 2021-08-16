import { createRouter, createWebHashHistory, createWebHistory } from 'vue-router'
import Index from './pages/Index.vue'
import Login from './pages/Login.vue'
import { store } from './models/store'
import { checkUserAuth } from './util/util'

const routes = [
    { path: '/', redirect: '/index' },
    { path: '/login', component: Login },
    { path: '/register', component: Index },
    { path: '/index', component: Index, meta: { requiresAuth: true } },
    { path: '/cms', component: Index, meta: { requiresAuth: true } },
    { path: '/:pathMatch(.*)', redirect: '/' },
]

const router = createRouter({
    routes: routes,
    history: createWebHashHistory()
})

//导航守卫  用户验证授权不通过则跳转到登录页
router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!checkUserAuth()) {
            next({
                path: '/login',
                query: { redirect: to.fullPath }
            })
        } else {
            next();
        }
    }
    else {
        next()
    }
});

export default router;