<template>
  <router-link
    v-if="link"
    class="px-2 text-white text-decoration-none nav-link"
    :to="link"
  >
    {{ text }}
  </router-link>
</template>
<script setup lang="ts">
  import { RouterLink } from "vue-router";

  defineProps({
    link: String,
    text: String,
  });
</script>
<style scoped>
  .nav-link:hover {
    background-color: rgba(255, 255, 255, 0.05);
  }
</style>
