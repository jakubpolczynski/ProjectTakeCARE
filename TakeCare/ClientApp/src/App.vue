<template>
  <div class="d-flex flex-column min-vh-100">
    <TheHeader />
    <body class="flex-grow-1 container mt-3">
      <RouterView />
    </body>
    <TheFooter />
  </div>
</template>

<script setup lang="ts">
  import { RouterView } from "vue-router";
  import { store } from "@/store/store";
  import TheHeader from "@/components/TheHeader.vue";
  import TheFooter from "@/components/TheFooter.vue";
  import { onMounted } from "vue";

  onMounted(() => {
    const isAuthenticated = localStorage.getItem("authenticated") === "true";
    const user = localStorage.getItem("user");

    if (isAuthenticated) {
      store.dispatch("setAuth", true);
      if (user) {
        store.dispatch("setUser", user);
      }
    }
  });
</script>

<style scoped lang="scss"></style>
