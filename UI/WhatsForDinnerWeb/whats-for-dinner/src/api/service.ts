import axios from 'axios';
import { ADD_RTOKEN, ADD_USERINFO, REMOVE_USERAUTH, store } from '../models/store';
import { ADD_TOKENINFO } from '../models/store';

const service = axios.create({
    baseURL: "/api",
    timeout: 1 * 60 * 1000
});

//请求拦截
service.interceptors.request.use(
    config => {
        if (store.state.tokenInfo.token) {
            // 判断是否存在token，如果存在的话，则每个http header都加上token
            config.headers.Authorization = `Bearer ${store.state.tokenInfo.token}`;

            //如果token过期  则带上刷新token
            const now = Date.parse(new Date().toString()) / 1000;
            if (now > store.state.tokenInfo.exp) {
                config.headers['X-Authorization'] = `Bearer ${store.state.rToken}`
            }
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
        if (!config.data?.succeeded) return Promise.reject(config);

        const token: string = config.headers["access-token"];
        const rToken: string = config.headers["x-access-token"];

        if (token) {
            //解析jwt的payload
            const jwtStr = decodeURIComponent(escape(window.atob(token.split('.')[1])));
            const jwtInfo = JSON.parse(jwtStr);

            store.commit(ADD_TOKENINFO, { token: token, exp: jwtInfo.exp as number });
            store.commit(ADD_USERINFO, { userId: jwtInfo.userId, userName: jwtInfo.userName });
        }
        if (rToken) {
            store.commit(ADD_RTOKEN, rToken);
        }

        return config;
    },
    err => {
        if (!err.response.data) {
            return Promise.reject(err.response.data);
        } else if (err.response.data.statusCode == 401) {
            store.commit(REMOVE_USERAUTH)
        } else if (err.response.data.statusCode == 500) {
            return Promise.reject(err.response.data)
        } else if (!err.response.data.succeeded) {
            return Promise.reject(err.response.data);
        } else {
            return Promise.reject(err.response.data);
        }
    }
);

export default service;