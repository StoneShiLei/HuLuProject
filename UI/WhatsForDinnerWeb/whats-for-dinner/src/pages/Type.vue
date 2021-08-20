<template>
    <van-nav-bar
        title="分类"
        right-text="新增"
        @click-right="handleNavBarClick"
        :border="true"
        safe-area-inset-top
    />

    <van-list
        v-model:loading="isLoading"
        :finished="isFinished"
        finished-text="没有更多了"
        @load="handleLoadEvevt"
    >
        <van-swipe-cell v-for="item in list" :key="item.id" :title="item.typeName">
            <template #right>
                <van-button square type="primary">修改</van-button>
            </template>
            <van-cell :border="true" :title="item.typeName" :value="`${item.menuCount}个菜谱`" />
            <template #left>
                <van-button square type="danger">删除</van-button>
            </template>
        </van-swipe-cell>
    </van-list>
</template>
  
<script setup lang="ts">
import url from "../api/url";
import { useHttp } from "../api/http";
import { ref } from "vue";
import { Notify } from "vant";

const http = useHttp();

const isLoading = ref(false);
const isFinished = ref(false);
const list = ref<TypeModel[]>([])

function handleLoadEvevt() {
    http
        .get<TypeModel[]>(url.TypeList, {})
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                list.value = res.data;
                console.log(res.data)
                isLoading.value = false;
                isFinished.value = true;
            } else if (res.statusCode == 200 && !res.data) {
                Notify({ type: "danger", message: res.extras.message });
            } else {
                Notify({ type: "danger", message: "接口异常" });
                console.error(res);
            }
        })
        .catch((err) => {
            Notify({ type: "danger", message: "接口异常" });
            console.error(err);
        });
}

function handleNavBarClick() {

}

</script>