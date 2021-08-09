<template>
  <a-Button type="primary" @click="addCount">
    count is: {{ store.state.count }}
  </a-Button>
</template>

<script lang="ts">
import { defineComponent } from "vue";
import { useStore } from "../models/store";

export default defineComponent({
  name: "Index",
  setup: () => {
    const store = useStore();
    const addCount = () => {
      store.commit("add");
    };
    return { store, addCount };
  },
});
</script>
