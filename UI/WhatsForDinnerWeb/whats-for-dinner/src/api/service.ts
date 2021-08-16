import axios from 'axios';
import { useStore } from 'vuex';

const store = useStore();
let token = ''

const service = axios.create({
    baseURL: "/api",
    timeout: 1 * 60 * 1000
});

//请求拦截
service.interceptors.request.use(
    config => {
        if (token) {
            // 判断是否存在token，如果存在的话，则每个http header都加上token
            config.headers.Authorization = `Bearer ${token}`; // 根据实际情况自行修改
        }
        return config;
    },
    err => {
        return Promise.reject(err);
    }
);

//响应拦截
service.interceptors.response.use(
    config => {
        const token: string = config.headers["access-token"];
        const rToken: string = config.headers["x-access-token"];

        if (token != undefined || token != null || token != '') {
            //解析jwt的payload
            const jwtStr = decodeURIComponent(escape(window.atob(token.split('.')[1])));
            const jwtInfo = JSON.parse(jwtStr);
            localStorage.setItem('userId', jwtInfo.userId);
            localStorage.setItem('userName', jwtInfo.userName);
            localStorage.setItem('token', token);
            localStorage.setItem('exp', jwtInfo.exp);
            // store.commit(ADD_TOKEN, { token: token, exp: 10 })
        }
        if (rToken != undefined || rToken != null || rToken != '') {
            // store.commit(ADD_RTOKEN, rToken)
            console.log(1 + "       " + rToken);
        }

        return config;
    },
    err => {
        return Promise.reject(err);
    }
);

export default service;