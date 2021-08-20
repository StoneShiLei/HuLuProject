import { createRouter, createWebHashHistory } from 'vue-router'
import Index from './pages/Index.vue'
import Login from './pages/Login.vue'
import Type from './pages/Type.vue'
import Food from './pages/Food.vue'
import Menu from './pages/Menu.vue'
import NotFound from './pages/NotFound.vue'
import { checkUserAuth } from './util/util'

const routes = [
    { path: '/', redirect: '/login' },
    { path: '/login', component: Login },
    { path: '/index', component: Index, meta: { requiresAuth: true } },
    { path: '/cms/type', component: Type, meta: { requiresAuth: true } },
    { path: '/cms/food', component: Food, meta: { requiresAuth: true } },
    { path: '/cms/menu', component: Menu, meta: { requiresAuth: true } },
    { path: '/:pathMatch(.*)', component: NotFound },
]

const router = createRouter({
    routes: routes,
    history: createWebHashHistory()
})

//导航守卫  用户验证授权不通过则跳转到登录页
router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        checkUserAuth()
            .then((res) => {
                if (!res) {
                    next({
                        path: '/login'
                    })
                } else {
                    next();
                }
            })
            .catch((error) => {
                console.error(error)
            })
    }
    else if (to.matched.some(record => record.path === '/login')) {
        checkUserAuth()
            .then((res) => {
                if (res) {
                    next({
                        path: '/'
                    })
                } else {
                    next();
                }
            })
            .catch((error) => {
                console.error(error)
            })
    }
    else {
        next()
    }
});

export default router;