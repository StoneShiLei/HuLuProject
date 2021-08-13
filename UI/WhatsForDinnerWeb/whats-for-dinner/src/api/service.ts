import axios from 'axios';

const service = axios.create({
    baseURL: "/api",
    timeout: 1 * 60 * 1000
});

let token = ''

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

export default service;