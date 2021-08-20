import service from './service'

export const http = {
    get: get,
    post: post
}

export function useHttp() {
    return http;
}

function get<T>(url: string, params: any): Promise<CallBack<T>> {
    return new Promise((resolve, reject) => {
        service.get(url, { params: params }).then(res => {
            resolve(res.data);
        }).catch(err => {
            reject(err);
        })
    })
}

function post<T>(url: string, params: any): Promise<CallBack<T>> {
    return new Promise((resolve, reject) => {
        service.post(url, params).then(res => {
            resolve(res.data);
        }).catch(err => {
            reject(err);
        })
    })
}

export interface CallBack<T> {
    statusCode: number,
    succeeded: boolean,
    timestamp: number,
    data: T,
    extras: {
        message: string
    },
    errors: string
}