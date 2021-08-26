<template>
    <van-nav-bar title="今晚吃啥？这是一个问题！" :border="true" safe-area-inset-top>
        <template #right>
            <van-button
                size="normal"
                type="primary"
                native-type="button"
                style="position: absolute;right: 0px;"
                @click="handleRandomMenu"
                :loading="loading"
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

    <van-popup v-model:show="show" position="bottom" :style="{ height: '100%' }">
        <van-nav-bar title="吃这个！" :border="true" safe-area-inset-top>
            <template #left>
                <van-button
                    size="normal"
                    type="warning"
                    native-type="button"
                    style="position: absolute;left: 0px;min-width: 80px"
                    @click="() => show = false"
                >关闭</van-button>
            </template>
            <template #right>
                <van-button
                    size="normal"
                    type="primary"
                    native-type="button"
                    style="position: absolute;right: 0px;min-width: 80px"
                    @click="handleRandomMenu"
                    :loading="loading"
                >再想想</van-button>
            </template>
        </van-nav-bar>
        <van-cell-group>
            <van-cell v-for="item in results" :title="item.menuName">
                <template #value>{{ item.typeName }}</template>
                <template #label>{{ item.foods?.map((item) => { return item.foodName }).join(',') }}</template>
            </van-cell>
        </van-cell-group>
    </van-popup>
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
const show = ref(false);
const loading = ref(false)

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

const results = ref<MenuModel[]>([])

//随机出菜
function handleRandomMenu() {
    //如果都为0弹出警告提示
    const result = models.value.find((model) => { return !!model.volume })
    if (!result) {
        Notify({ type: 'warning', message: '至少选一种吃的吧', duration: 1000 });
        return;
    }

    handleLoadEvevt();
    show.value = true;
}

//列表数据加载
function handleLoadEvevt() {
    loading.value = true;
    http
        .post<MenuModel[]>(url.MenuRandom, models.value)
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                results.value = res.data
                loading.value = false;
            } else if (res.statusCode == 200 && !res.data) {
                Notify({ type: "danger", message: res.extras.message });
            } else {
                Notify({ type: "danger", message: "操作失败，系统异常" });
                console.error(res);
            }
        })
        .catch((err) => {
            Notify({ type: "danger", message: "操作失败，系统异常" });
            console.error(err);
        });
}

</script>