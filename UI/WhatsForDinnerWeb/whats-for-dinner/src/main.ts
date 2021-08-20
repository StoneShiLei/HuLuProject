import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { store, key } from './models/store'
import '@vant/touch-emulator';
import { Dialog } from 'vant';

const app = createApp(App).use(Dialog).use(router).use(store, key);
//挂载全局
// app.config.globalProperties.$get = get;
// app.config.globalProperties.$post = post;

app.mount('#app');


//声明全局函数类型
declare module '@vue/runtime-core' {
    export interface ComponentCustomProperties {
        // $get: typeof get
        // $post: typeof post
    }
}