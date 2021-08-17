<template>
  <n-config-provider :locale="zhCN" :date-locale="dateZhCN">
    <n-message-provider>
      <Nav>
        <router-view></router-view>
      </Nav>
    </n-message-provider>
  </n-config-provider>
</template>

<script setup lang="ts">
import { NConfigProvider, zhCN, dateZhCN, NMessageProvider } from "naive-ui";
import Nav from "./components/Nav.vue";
</script>
