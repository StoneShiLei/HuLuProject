import { InjectionKey } from 'vue'
import { createStore, Store, useStore as baseUseStore } from 'vuex'
import persistedState from 'vuex-persistedstate'

// 声明自己的 store state
export interface State {
    userInfo: UserInfo | null
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

//添加userInfo
export const ADD_USERINFO = "ADD_USERINFO";

//创建store
export const store = createStore<State>({
    plugins: [persistedState()], //持久化到localstorage
    state() {
        return {
            userInfo: null
        }
    },
    mutations: {
        [ADD_USERINFO](state, userInfo: UserInfo) {
            state.userInfo = userInfo
        }
    }
})

//用户信息
export interface UserInfo {
    userId: string;
    userName: string;
    token: string;
    rToken: string;
    exp: string;
}