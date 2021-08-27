<template>
    <van-nav-bar right-text="新增" @click-right="handleAddShow" :border="true" safe-area-inset-top>
        <template #title>
            <van-field
                v-model="searchVal"
                placeholder="搜索"
                input-align="center"
                style="width: 100%;"
            >
                <template #button>
                    <van-button size="small" plain type="primary" @click="handleClear">清空</van-button>
                </template>
            </van-field>
        </template>
    </van-nav-bar>

    <van-list
        v-model:loading="isLoading"
        :finished="isFinished"
        finished-text="没有更多了"
        @load="handleLoadEvevt"
    >
        <van-swipe-cell
            v-for="item in list"
            :key="item.id"
            :name="item.id"
            :title="item.foodName"
            :before-close="handleClose"
            ref="swipeCellRef"
        >
            <template #right>
                <van-button square type="primary" style="height: 100%;">修改</van-button>
            </template>
            <van-cell :title="item.foodName" :border="true" center />
            <template #left>
                <van-button square type="danger" style="height: 100%;">删除</van-button>
            </template>
        </van-swipe-cell>
    </van-list>

    <van-popup v-model:show="show" position="top" :style="{ height: '30%' }">
        <van-form validate-trigger="onBlur" ref="formRef" style="padding-top:50px;">
            <van-field
                v-model="model.foodName"
                name="名称"
                label="名称"
                placeholder="批量添加可用逗号分隔"
                :rules="[{ required: true, message: '请填写名称' }]"
                :border="true"
                error-message-align="right"
                input-align="right"
                center
            >
                <template #button>
                    <van-button
                        size="small"
                        type="primary"
                        native-type="button"
                        @click="handleAddOrUpdateClick"
                    >确定</van-button>
                </template>
            </van-field>
        </van-form>
    </van-popup>
</template>
  
<script setup lang="ts">
import url from "../api/url";
import { CallBack, useHttp } from "../api/http";
import { ref, watch } from "vue";
import { Dialog, FormInstance, Notify, SwipeCellInstance } from "vant";

const swipeCellRef = ref<SwipeCellInstance>();
const formRef = ref<FormInstance>();
const http = useHttp();

const show = ref(false);

const isLoading = ref(false);
const isFinished = ref(false);
const list = ref<FoodModel[]>([])

const model = ref<FoodModel>(
    {
        foodName: '',
    }
)

//搜索
const searchVal = ref('');
watch(searchVal, (newVal) => {
    handleLoadEvevt(newVal)
})
function handleClear() {
    searchVal.value = ''
}

//列表数据加载
function handleLoadEvevt(content = '') {
    isLoading.value = true;
    http
        .get<FoodModel[]>(url.FoodList, { text: content })
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                list.value = res.data;

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

//显示新增遮罩  初始化model
function handleAddShow() {
    model.value = {
        foodName: '',
    };
    show.value = true;
}

//滑动关闭
function handleClose(event: { name: string, position: 'left' | 'right' | 'cell' | 'outside' }) {
    switch (event.position) {
        case 'left':
            return new Promise((resolve) => {
                Dialog.confirm({
                    title: '确定删除吗？',
                })
                    .then(() => {
                        console.log(url.FoodRemove + ',' + event.name)
                        http
                            .post<boolean>(url.FoodRemove, { foodId: event.name })
                            .then((res) => {
                                if (res.statusCode == 200 && res.data) {
                                    Notify({ type: "success", message: "操作成功" });
                                    handleLoadEvevt()
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
                        return new Promise(resolve)
                    })
                    .catch(resolve);
            });
        case 'right':
            return new Promise((resolve) => {
                http
                    .get<FoodModel>(url.FoodInfo, { foodId: event.name })
                    .then((res) => {
                        if (res.statusCode == 200 && res.data) {
                            model.value = res.data
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
                show.value = true;
                return new Promise(resolve)
            })
        default:
            break;
    }
}

//提交新增或修改
function handleAddOrUpdateClick() {
    formRef.value?.validate()
        .then(() => {
            const nameArr = model.value.foodName.replaceAll('，', ',').split(',');
            const promiseList: Promise<CallBack<boolean>>[] = []
            nameArr.forEach((item) => {
                const tempModel = JSON.parse(JSON.stringify(model.value));
                tempModel.foodName = item;
                const promise = http.post<boolean>(url.FoodAddOrUpdate, tempModel);
                promiseList.push(promise)
            })

            Promise.all(promiseList).then((res) => {
                const resultFlag: { res: boolean, message?: string }[] = [];

                res.forEach((item) => {
                    if (item.statusCode == 200 && item.data) {

                    } else if (item.statusCode == 200 && !item.data) {
                        resultFlag.push({ res: false, message: item.extras.message });
                    } else {
                        resultFlag.push({ res: false });
                        console.error(res);
                    }
                })


                if (resultFlag.length) {
                    if (resultFlag[0].message) {
                        Notify({ type: "danger", message: resultFlag[0].message });
                    } else {
                        Notify({ type: "danger", message: "操作失败，系统异常" });
                    }
                }
                else {
                    Notify({ type: "success", message: "操作成功" });
                    show.value = false;
                    handleLoadEvevt()
                }
            })
                .catch((err) => {
                    console.error(err);
                });
        })
        .catch((err) => { console.log(err) })
}

</script>