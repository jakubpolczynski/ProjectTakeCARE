<template>
  <div class="d-flex flex-column min-vh-100">
    <header>
      <TheHeader />
    </header>
    <body class="flex-grow-1 container mt-3">
      <RouterView />
    </body>
    <footer>
      <TheFooter />
    </footer>
  </div>
</template>

<script setup lang="ts">
  import { RouterView } from "vue-router";
  import TheHeader from "@/components/main/TheHeader.vue";
  import TheFooter from "@/components/main/TheFooter.vue";
</script>
<style scoped lang="scss"></style>
