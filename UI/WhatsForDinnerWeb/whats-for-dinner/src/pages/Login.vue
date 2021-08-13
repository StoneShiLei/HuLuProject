<template>
  <n-form :model="model" :rules="rules" ref="formRef">
    <n-form-item label="用户名" path="userName">
      <n-input v-model:value="model.userName" placeholder="请输入用户名" />
    </n-form-item>
    <n-form-item label="密码" path="password">
      <n-input
        v-model:value="model.password"
        placeholder="请输入密码"
        type="password"
      />
    </n-form-item>
    <n-button @click="handleLoginClick" attr-type="button">登陆</n-button>
  </n-form>
</template>

<script setup lang="ts">
import { getCurrentInstance, ref } from "vue";
import url from "../api/url";
import { useMessage, NForm, NFormItem, NInput, NButton } from "naive-ui";
import { CallBack } from "../api/http";

const com = getCurrentInstance();
const formRef = ref<InstanceType<typeof NForm>>();
const message = useMessage();

const model = ref({
  userName: null,
  password: null,
});

//登陆
function handleLoginClick(event: Event): void {
  formRef.value?.validate((errors) => {
    if (!errors) {
      com?.proxy?.$get<boolean>(url.UserLogin, model.value).then((res) => {
        if (res.statusCode == 200 && res.succeeded && res.data) {
          message.success("ok");
        } else {
          message.error("not ok");
        }
        console.log(res);
      });
    } else {
      message.error(errors[0][0].message);
    }
  });
}

const rules = {
  userName: {
    required: true,
    message: "请输入用户名",
    trigger: ["input", "blur"],
  },
  password: {
    required: true,
    message: "请输入密码",
    trigger: ["input", "blur"],
  },
};
</script>