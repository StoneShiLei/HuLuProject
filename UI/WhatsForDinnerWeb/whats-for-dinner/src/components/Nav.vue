<template>
  <n-menu mode="horizontal" :options="menuOptions" />
</template>

<script setup lang="ts">
import { h } from "vue";
import { NIcon, NMenu, darkTheme, NConfigProvider } from "naive-ui";
import { Book as BookIcon } from "@vicons/ionicons5";
import { DefineComponent } from "vue";

function renderIcon(icon: DefineComponent<{}, {}, any>) {
  return () => h(NIcon, null, { default: () => h(icon) });
}

const menuOptions = [
  {
    label: () =>
      h(
        "a",
        {
          href: "https://baike.baidu.com/item/%E4%B8%94%E5%90%AC%E9%A3%8E%E5%90%9F",
          target: "_blank",
          rel: "noopenner noreferrer",
        },
        "主页"
      ),
    key: "hear-the-wind-sing",
    icon: renderIcon(BookIcon),
  },
  {
    label: "寻羊冒险记",
    key: "a-wild-sheep-chase",
    icon: renderIcon(BookIcon),
    disabled: true,
  },
  {
    label: "1973年的弹珠玩具",
    key: "pinball-1973",
    icon: renderIcon(BookIcon),
    children: [
      {
        label: "鼠",
        key: "rat",
      },
    ],
  },
];
</script>