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
        <van-picker
            v-bind="$attrs"
            :columns="columnsData"
            show-toolbar
            @cancel="cancel"
            @confirm="onConfirm"
            @change="change"
            :value-key="props.option.label"
        />
    </van-popup>
</template>
  
<script setup lang="ts">
import { computed } from "@vue/reactivity";
import { ref, watch } from "vue";

//定义props和默认值
interface Props {
    selectValue: string | number,
    columns?: any[],
    option?: { label: string, value: string }
}
const props = withDefaults(defineProps<Props>(), {
    selectValue: '',
    columns: () => [] as any[],
    option: undefined
})

//定义emits
const emit = defineEmits(['confirm', 'change', 'cancel', 'update:selectValue'])

//定义ref变量
const show = ref(false);
const columnsData = ref<any[]>(props.columns);
const resultValue = ref<string | number>(props.selectValue);


//计算属性
const resultLabel = computed({
    get: () => {
        const res = props.columns.filter(item => {
            const data = props.option ? item[props.option.value] : item
            return data === resultValue.value
        })
        let label = ''
        if (res.length) {
            label = props.option ? res[0][props.option.label] : res[0]
        }
        return label;
    },
    set: (val) => {

    }
})

//methods
function onConfirm(value: any, index: number) {
    const data = props.option ? value[props.option.value] : value
    resultValue.value = data;
    show.value = !show.value
    emit('confirm', value, index, resultValue.value)
}

function change(value: any, index: number) {
    emit('change', value, index, resultValue.value)
}

function cancel(value: any, index: number) {
    show.value = !show.value
    emit('cancel', value, index, resultValue.value)
}

function showPopu(disabled: any) {
    columnsData.value = props.columns;
    resultValue.value = props.selectValue;
    if (disabled !== undefined && disabled !== false) {
        return;
    } else {
        show.value = !show.value
    }
}

//watch
watch(() => props.selectValue, (newVal) => {
    resultValue.value = newVal;
}, { immediate: true })
watch(resultValue, (newVal) => {
    columnsData.value = props.columns;
    emit('update:selectValue', newVal);
});

</script>