import { InjectionKey } from 'vue'
import { createStore, Store, useStore as baseUseStore } from 'vuex'

// 声明自己的 store state
export interface State {
    count: number
}

// 为 `this.$store` 提供类型声明
export interface ComponentCustomProperties {
    $store: Store<State>
}

// 定义 injection key
export const key: InjectionKey<Store<State>> = Symbol()

//创建store
export const store = createStore<State>({
    state() {
        return {
            count: 0
        }
    },
    mutations: {
        add(state) {
            state.count++
        }
    }
})

// 定义自己的 `useStore` 组合式函数
export function useStore() {
    return baseUseStore(key)
}

