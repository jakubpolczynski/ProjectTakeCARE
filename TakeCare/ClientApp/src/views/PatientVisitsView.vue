<template>
  <h1>Visits</h1>
  <div class="row">
    <div class="col-5">
      <CreateVisit :selectedDate="selectedDate"></CreateVisit>
    </div>
    <div class="col">
      <Calendar @dayClick="handleDayClick"></Calendar>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref } from "vue";

  import Calendar from "@/components/Calendar.vue";
  import CreateVisit from "@/components/CreateVisit.vue";

  const selectedDate = ref<Date | null>(null);

  function handleDayClick(date: Date) {
    console.log("Data odebrana: ", date);
    selectedDate.value = date;
  }
</script>
<style scoped lang="scss"></style>
