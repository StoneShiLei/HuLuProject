<template>
    <van-field
        v-model="resultLabel"
        v-bind="$attrs"
        readonly
        :is-link="$attrs.disabled === undefined"
        error-message-align="right"
        input-align="right"
        @click="showPopu($attrs.disabled)"
    />

    <van-popup v-model:show="show" position="bottom" safe-area-inset-bottom>
        <van-row justify="space-between">
            <van-col span="5">
                <van-button type="warning" @click="cancel" size="normal" style="width: 100%;">取消</van-button>
            </van-col>
            <van-col span="8">
                <van-field
                    v-model="searchVal"
                    placeholder="搜索"
                    v-if="isSearch"
                    input-align="center"
                    style="width: 100%;"
                />
            </van-col>
            <van-col span="5">
                <van-button type="success" @click="onConfirm" size="normal" style="width: 100%;">确认</van-button>
            </van-col>
        </van-row>
        <div class="checkbox-con" style="max-height:264px;overflow-y:auto">
            <van-checkbox-group v-model="checkboxValue" @change="change" ref="checkboxGroup">
                <van-cell-group>
                    <van-cell
                        v-for="(item, index) in columnsData"
                        clickable
                        :key="item[option.value]"
                        :title="item[option.label]"
                        @click="toggle(index)"
                    >
                        <template #right-icon>
                            <van-checkbox
                                :name="item[option.value]"
                                :ref="(el) => checkboxRefs[index] = el"
                            />
                        </template>
                    </van-cell>
                </van-cell-group>
            </van-checkbox-group>
        </div>
    </van-popup>
</template>
  
<script setup lang="ts">
import { computed } from "@vue/reactivity";
import { CheckboxInstance } from "vant";
import { ref, watch } from "vue";

//定义props和默认值
interface Props {
    selectValue: string[],
    columns?: any[],
    option?: { label: string, value: string },
    isSearch?: boolean
}
const props = withDefaults(defineProps<Props>(), {
    selectValue: () => [] as string[],
    columns: () => [] as any[],
    option: () => { return { label: 'label', value: 'value' } },
    isSearch: false
})

//定义emits
const emit = defineEmits(['confirm', 'change', 'cancel', 'update:selectValue'])

//定义ref变量
const show = ref(false);
const searchVal = ref('');
const columnsData = ref<any[]>(JSON.parse(JSON.stringify(props.columns)));
const resultValue = ref<string[]>(JSON.parse(JSON.stringify(props.selectValue)));
const checkboxValue = ref<string[]>(JSON.parse(JSON.stringify(props.selectValue)))

//复选框组件ref
const checkboxRefs = ref<any[]>([]);

//计算属性
const resultLabel = computed({
    get: () => {
        const res = props.columns.filter(item => {
            return resultValue.value.indexOf(item[props.option.value]) > -1
        })
        const resLabel = res.map(item => {
            return item[props.option.label]
        })
        return resLabel.join(',')
    },
    set: (val) => {

    }
})

//methods
function getData(val: string[]) {
    const res = columnsData.value.filter(item => {
        return val.indexOf(item[props.option.value]) > -1
    })
    return res
}

function onConfirm() {
    resultValue.value = checkboxValue.value
    show.value = !show.value
    emit('confirm', resultValue.value, getData(resultValue.value))
}

function change(val: string) {
    emit('change', val, getData(resultValue.value))
}

function cancel() {
    show.value = !show.value
    emit('cancel', resultValue.value)
}

function toggle(index: number) {
    (checkboxRefs.value[index] as CheckboxInstance).toggle()
}

function showPopu(disabled: any) {
    columnsData.value = JSON.parse(JSON.stringify(props.columns));
    checkboxValue.value = JSON.parse(JSON.stringify(props.selectValue));
    resultValue.value = JSON.parse(JSON.stringify(props.selectValue));
    if (disabled !== undefined && disabled !== false) {
        return;
    } else {
        show.value = !show.value
    }
}

//watch
watch(searchVal, (newVal) => {
    if (newVal) {
        columnsData.value = props.columns.filter(item => {
            return item[props.option.label].indexOf(newVal) > -1
        })
    } else {
        columnsData.value = JSON.parse(JSON.stringify(props.columns))
    }
})

watch(() => props.selectValue, (newVal) => resultValue.value = newVal, { immediate: true });
watch(resultValue, (newVal) => {
    searchVal.value = '';
    columnsData.value = JSON.parse(JSON.stringify(props.columns));
    emit('update:selectValue', newVal);
});

</script>