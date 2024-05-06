<template>
  <li :id="text">
    <router-link
      v-if="link"
      class="nav-link px-2 text-white text-decoration-none"
      :to="link"
    >
      {{ text }}
    </router-link>
  </li>
</template>
<script setup lang="ts">
  import { RouterLink } from "vue-router";

  defineProps({
    link: String,
    text: String,
  });
</script>
<style scoped lang="scss"></style>