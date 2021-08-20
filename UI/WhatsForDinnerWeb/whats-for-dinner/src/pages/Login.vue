<template>
  <van-form validate-trigger="onBlur" ref="formRef" style="margin-top: 40px;">
    <van-cell-group inset>
      <van-field
        v-model="model.userName"
        name="用户名"
        label="用户名"
        placeholder="用户名"
        :rules="[{ required: true, message: '请填写用户名' }]"
      ></van-field>
      <van-field
        v-model="model.password"
        type="password"
        name="密码"
        label="密码"
        placeholder="密码"
        :rules="[{ required: true, message: '请填写密码' }]"
      ></van-field>
    </van-cell-group>
    <div style="margin: 16px;">
      <van-button round block type="primary" native-type="button" @click="handleLoginClick">登陆</van-button>
    </div>
    <div style="margin: 16px;">
      <van-button round block type="success" native-type="button" @click="handleRegisterClick">注册</van-button>
    </div>
  </van-form>
</template>

<script setup lang="ts">
import { ref } from "vue";
import url from "../api/url";
import { useHttp } from "../api/http";
import { useRouter } from "vue-router";
import { FormInstance, Notify } from "vant";

const formRef = ref<FormInstance>();
const http = useHttp();
const router = useRouter();

const model = ref({
  userName: null,
  password: null,
});

//登陆
function handleLoginClick(event: Event): void {
  formRef.value?.validate()
    .then(() => {
      http
        .get<boolean>(url.UserLogin, model.value)
        .then((res) => {
          if (res.statusCode == 200 && res.data) {
            Notify({ type: "success", message: "登陆成功" });
            router.push({ path: "/index" });
          } else if (res.statusCode == 200 && !res.data) {
            Notify({ type: "danger", message: res.extras.message });
          } else {
            Notify({ type: "danger", message: "登陆失败，系统异常" });
            console.error(res);
          }
        })
        .catch((err) => {
          Notify({ type: "danger", message: "登陆失败，系统异常" });
          console.error(err);
        });
    })
    .catch((err) => { })
}

//注册
function handleRegisterClick(event: Event): void {
  formRef.value?.validate()
    .then(() => {
      http
        .post<boolean>(url.UserRegister, model.value)
        .then((res) => {
          if (res.statusCode == 200 && res.data) {
            Notify({ type: "success", message: "注册成功，请点击登陆" });
          } else if (res.statusCode == 200 && !res.data) {
            Notify("111")
            // Notify({ type: "danger", message: res.extras.message });
          } else {
            Notify({ type: "danger", message: "注册失败" });
            console.error(res);
          }
        })
        .catch((err) => {
          Notify({ type: "danger", message: "注册失败，系统异常" });
          console.error(err);
        });
    })
    .catch((err) => { })
}
</script>