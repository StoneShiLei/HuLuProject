<template>
    <van-nav-bar title="今晚吃啥？这是一个问题！" :border="true" safe-area-inset-top>
        <template #right>
            <van-button
                size="normal"
                type="primary"
                native-type="button"
                style="position: absolute;right: 0px;"
                @click="handleRandomMenu"
            >吃！</van-button>
        </template>
    </van-nav-bar>

    <van-cell-group>
        <div v-for="item in models">
            <van-cell :title="item.typeName">
                <template #value>
                    <van-stepper
                        v-model.number="item.volume"
                        disable-input
                        integer
                        theme="round"
                        min="0"
                    />
                </template>
            </van-cell>
            <FieldCheckbox
                name="食材"
                placeholder="请选择食材"
                v-model:selectValue="item.foodIds"
                :rules="[{ required: true, message: '请选择食材' }]"
                :columns="foodList"
                label-width="100"
                :option="{ label: 'foodName', value: 'id' }"
                :is-search="true"
            />
        </div>
    </van-cell-group>
</template>
  
<script setup lang="ts">
import { onMounted, ref } from "vue";
import url from "../api/url";
import { useHttp } from "../api/http";
import { Notify } from "vant";
import FieldCheckbox from "../components/FieldCheckbox.vue";

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
                typeList.value.forEach((val) => {
                    models.value.push({
                        typeId: val.id,
                        typeName: val.typeName,
                        volume: 0,
                        foodIds: []
                    })
                })
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

interface MenuRandomInput {
    typeId?: string,
    typeName?: string,
    volume?: number,
    foodIds?: string[],
}
const models = ref<MenuRandomInput[]>([])


//随机出菜
function handleRandomMenu() {
    console.log(models.value)
}

</script>