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
  import TheHeader from "@/components/TheHeader.vue";
  import TheFooter from "@/components/TheFooter.vue";
</script>
<style scoped lang="scss"></style>
