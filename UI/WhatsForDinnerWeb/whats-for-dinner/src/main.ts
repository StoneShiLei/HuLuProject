import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { store, key } from './models/store'
import { get, post } from './api/http'

const app = createApp(App).use(router).use(store, key);

//挂载全局
app.config.globalProperties.$get = get;
app.config.globalProperties.$post = post;

app.mount('#app');


//声明类型
declare module '@vue/runtime-core' {
    export interface ComponentCustomProperties {
        $get: typeof get
        $post: typeof post
    }
}