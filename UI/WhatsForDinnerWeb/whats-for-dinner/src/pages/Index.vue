<template>
    <van-nav-bar title="今晚吃啥？这是一个问题！" :border="true" safe-area-inset-top />
</template>
  
<script setup lang="ts">
import { onMounted, ref } from "vue";
import url from "../api/url";
import { useHttp } from "../api/http";
import { Notify } from "vant";

const http = useHttp();

const foodList = ref<FoodModel[]>([])
const typeList = ref<TypeModel[]>([])

onMounted(() => {
    http
        .get<FoodModel[]>(url.FoodList, {})
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                foodList.value = res.data;
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
    http
        .get<TypeModel[]>(url.TypeList, {})
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                typeList.value = res.data;
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
})

const model = ref({})
model.value.a

</script>