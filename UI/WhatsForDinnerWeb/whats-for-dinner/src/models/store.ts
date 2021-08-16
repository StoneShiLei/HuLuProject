import { InjectionKey } from 'vue'
import { createStore, Store, useStore as baseUseStore } from 'vuex'
import persistedState from 'vuex-persistedstate'

// 声明自己的 store state
export interface State {
    userName: string
    userId: string
    tokenInfo: { token: string, exp: number }
    rToken: string
}

// 为 `this.$store` 提供类型声明
export interface ComponentCustomProperties {
    $store: Store<State>
}

// 定义 injection key
export const key: InjectionKey<Store<State>> = Symbol()

// 定义自己的 `useStore` 组合式函数
export function useStore() {
    return baseUseStore(key)
}

export const ADD_USERINFO = 'ADD_USERINFO'; //添加用户信息
export const ADD_TOKENINFO = 'ADD_TOKENINFO'; //添加token
export const ADD_RTOKEN = 'ADD_RTOKEN';  //添加刷新token
export const REMOVE_USERAUTH = 'REMOVE_USERAUTH';  //移除全部用户信息

//创建store
export const store = createStore<State>({
    plugins: [persistedState()], //持久化到localstorage
    state() {
        return {
            userName: '',
            userId: '',
            tokenInfo: { token: '', exp: 0 },
            rToken: '',
        }
    },
    mutations: {
        [ADD_USERINFO](state, userInfo: { userId: string, userName: string }) {
            state.userId = userInfo.userId;
            state.userName = userInfo.userName;
        },
        [ADD_TOKENINFO](state, tokenInfo: { token: string, exp: number }) {
            state.tokenInfo.token = tokenInfo.token;
            state.tokenInfo.exp = tokenInfo.exp;
        },
        [ADD_RTOKEN](state, rToken: string) {
            state.rToken = rToken
        },
        [REMOVE_USERAUTH](state) {
            state.userName = ''
            state.userId = ''
            state.tokenInfo = { token: '', exp: 0 }
            state.rToken = ''
        }
    }
})