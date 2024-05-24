<template>
  <h1>Visits</h1>
  <div class="row">
    <div class="col-5">
      <CreateVisit></CreateVisit>
    </div>
    <div class="col">
      <Calendar></Calendar>
    </div>
  </div>
</template>
<script setup lang="ts">
  import Calendar from "@/components/Calendar.vue";
  import CreateVisit from "@/components/CreateVisit.vue";
</script>
<style scoped lang="scss"></style>
