<template>
  <div v-if="isDataLoaded">
    <h1>Receptionist visits</h1>
  </div>
</template>
<script setup lang="ts">
  import { onMounted, ref } from "vue";
  const isDataLoaded = ref(false);
  onMounted(() => {
    isDataLoaded.value = true;
  });
</script>
