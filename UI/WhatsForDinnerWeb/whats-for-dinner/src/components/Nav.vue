<template>
  <van-tabbar safe-area-inset-bottom route>
    <van-tabbar-item replace to="/index" icon="home-o">主页</van-tabbar-item>
    <van-tabbar-item replace to="/cms/type" icon="records">分类</van-tabbar-item>
    <van-tabbar-item replace to="/cms/food" icon="records">食材</van-tabbar-item>
    <van-tabbar-item replace to="/cms/menu" icon="records">菜谱</van-tabbar-item>
  </van-tabbar>
</template>

<script setup lang="ts">
import { defineComponent } from "@vue/runtime-core";

</script>
