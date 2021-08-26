<template>
    <van-nav-bar
        title="分类"
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
            <van-cell :title="item.typeName" :value="`${item.menuCount}个菜谱`" :border="true" center>
                <template #label>{{ item.menus?.map((item) => { return item.menuName }).join(',') }}</template>
            </van-cell>
            <template #left>
                <van-button square type="danger" style="height: 100%;">删除</van-button>
            </template>
        </van-swipe-cell>
    </van-list>

    <van-popup v-model:show="show" position="top" :style="{ height: '20%' }">
        <van-form validate-trigger="onBlur" ref="formRef" style="padding-top:50px;">
            <van-field
                v-model="model.typeName"
                name="名称"
                label="名称"
                placeholder="名称"
                :rules="[{ required: true, message: '请填写名称' }]"
                :border="true"
                error-message-align="right"
                input-align="right"
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
import { useHttp } from "../api/http";
import { ref } from "vue";
import { Dialog, FormInstance, Notify, SwipeCellInstance } from "vant";

const swipeCellRef = ref<SwipeCellInstance>();
const formRef = ref<FormInstance>();
const http = useHttp();

const show = ref(false);

const isLoading = ref(false);
const isFinished = ref(false);
const list = ref<TypeModel[]>([])

let model = ref<TypeModel>(
    {
        typeName: '',
    }
)

//列表数据加载
function handleLoadEvevt() {
    http
        .get<TypeModel[]>(url.TypeList, {})
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
        typeName: '',
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
                            .post<boolean>(url.TypeRemove, { typeId: event.name })
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
                    .get<TypeModel>(url.TypeInfo, { typeId: event.name })
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
                .post<boolean>(url.TypeAddOrUpdate, model.value)
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

</script>