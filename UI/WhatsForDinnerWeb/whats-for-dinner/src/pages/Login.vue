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
    <n-button @click="handleRegisterClick" attr-type="button">注册</n-button>
    <n-button @click="handleLoginClick" attr-type="button">登陆</n-button>
  </n-form>
</template>

<script setup lang="ts">
import { getCurrentInstance, ref } from "vue";
import url from "../api/url";
import { useMessage, NForm, NFormItem, NInput, NButton } from "naive-ui";
import { useStore } from "vuex";

const com = getCurrentInstance();
// const store = useStore();
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
        if (res.statusCode == 200 && res.data) {
          message.success("登陆成功");
        } else {
          message.error("登陆失败");
          console.log(res);
        }
      });
    } else {
      message.error(errors[0][0].message);
    }
  });
}

//注册
function handleRegisterClick(event: Event): void {
  formRef.value?.validate((errors) => {
    if (!errors) {
      com?.proxy?.$post<boolean>(url.UserRegister, model.value).then((res) => {
        if (res.statusCode == 200 && res.data) {
          message.success("注册成功，请点击登陆");
        } else {
          message.error("注册失败");
          console.log(res);
        }
      });
    } else {
      message.error(errors[0][0].message);
    }
  });
}

//表单验证规则
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