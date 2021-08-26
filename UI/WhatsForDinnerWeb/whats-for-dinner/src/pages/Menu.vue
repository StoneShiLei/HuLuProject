<template>
    <van-nav-bar
        title="菜谱"
        right-text="新增"
        @click-right="handleAddShow"
        :border="true"
        safe-area-inset-top
    />

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
            :title="item.typeName"
            :before-close="handleClose"
            ref="swipeCellRef"
        >
            <template #right>
                <van-button square type="primary" style="height: 100%;">修改</van-button>
            </template>
            <van-cell :title="item.menuName" :value="item.typeName" :border="true" center>
                <template #right-icon>
                    <van-switch
                        :model-value="item.isEnabled"
                        @update:model-value="handleEnabledSwitch(!item.isEnabled, item.id)"
                        :name="item.id"
                        size="24px"
                        style="margin-left:15px;"
                    />
                </template>
                <template #label>{{ item.foodNames }}</template>
            </van-cell>
            <template #left>
                <van-button square type="danger" style="height: 100%;">删除</van-button>
            </template>
        </van-swipe-cell>
    </van-list>

    <van-popup v-model:show="show" position="top" :style="{ height: '50%' }">
        <van-form validate-trigger="onBlur" ref="formRef" style="padding-top:50px;">
            <van-field
                v-model="model.menuName"
                name="名称"
                label="名称"
                placeholder="名称"
                :rules="[{ required: true, message: '请填写名称' }]"
                :border="true"
                error-message-align="right"
                input-align="right"
            />

            <FieldSelectPicker
                label="类型"
                name="类型"
                placeholder="请选择类型"
                v-model:selectValue="model.typeId"
                :rules="[{ required: true, message: '请选择类型' }]"
                :columns="typeList"
                :option="{ label: 'typeName', value: 'id' }"
            />

            <FieldCheckbox
                label="食材"
                name="食材"
                placeholder="请选择食材"
                v-model:selectValue="model.foodIds"
                :rules="[{ required: true, message: '请选择食材' }]"
                :columns="foodList"
                label-width="100"
                :option="{ label: 'foodName', value: 'id' }"
                :is-search="true"
            />
            <van-button
                size="normal"
                type="primary"
                native-type="button"
                @click="handleAddOrUpdateClick"
                style="position: absolute;right: 30px;bottom: 20px;"
            >确定</van-button>
        </van-form>
    </van-popup>
</template>
  
<script setup lang="ts">
import url from "../api/url";
import { useHttp } from "../api/http";
import { onMounted, ref } from "vue";
import { Dialog, FormInstance, Notify, SwipeCellInstance } from "vant";
import FieldCheckbox from "../components/FieldCheckbox.vue";
import FieldSelectPicker from "../components/FieldSelectPicker.vue";

const swipeCellRef = ref<SwipeCellInstance>();
const formRef = ref<FormInstance>();
const http = useHttp();

const show = ref(false);

const isLoading = ref(false);
const isFinished = ref(false);

const list = ref<MenuModel[]>([])
const foodList = ref<FoodModel[]>([])
const typeList = ref<TypeModel[]>([])

let model = ref<MenuModel>(
    {
        menuName: '',
        typeId: '',
        foodIds: []
    }
)

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

//列表数据加载
function handleLoadEvevt() {
    http
        .get<MenuModel[]>(url.MenuList, {})
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
        menuName: '',
        typeId: '',
        foodIds: []
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
                        http
                            .post<boolean>(url.MenuRemove, { menuId: event.name })
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
                    .get<MenuModel>(url.MenuInfo, { menuId: event.name })
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
            console.log(model.value)
            http
                .post<boolean>(url.MenuAddOrUpdate, model.value)
                .then((res) => {
                    if (res.statusCode == 200 && res.data) {
                        Notify({ type: "success", message: "操作成功" });
                        show.value = false;
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
        })
        .catch((err) => { console.log(err) })
}

//切换开启状态
function handleEnabledSwitch(newValue: boolean, menuId: string | undefined) {
    const id = menuId as string
    http
        .get<MenuModel>(url.MenuEnable, { state: newValue, menuId: id })
        .then((res) => {
            if (res.statusCode == 200 && res.data) {
                const model = list.value.find(m => m.id === id)
                if (model != undefined) {
                    model.isEnabled = newValue
                }
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