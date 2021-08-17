<template>
  <n-Button type="primary" @click="addCount">
    count is: {{ store.state.count }} test data :{{ msg }}
  </n-Button>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useStore } from "../models/store";
import { NButton } from "naive-ui";

let msg = ref("hello");

const store = useStore();
const addCount = () => {
  store.commit("add");
  msg.value = "1";
};
</script>
