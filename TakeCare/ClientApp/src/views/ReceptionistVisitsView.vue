<template>
  <div v-if="isDataLoaded">
    <h1>Receptionist visits</h1>
    <div class="row">
      <div class="col-5">
        <CreateVisit :selectedDate="selectedDate >= new Date().toISOString().split('T')[0] ? selectedDate : null"></CreateVisit>
      </div>
      <div class="col">
        <Calendar @dayClick="handleDayClick"></Calendar>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { onMounted, ref } from "vue";

  import Calendar from "@/components/Calendar.vue";
  import CreateVisit from "@/components/CreateVisit.vue";

  const isDataLoaded = ref(false);
  const selectedDate = ref<string | null>(null);

  onMounted(async () => {
    selectedDate.value = new Date().toISOString().split("T")[0];
    isDataLoaded.value = true;
  });

  function handleDayClick(date: string) {
    selectedDate.value = date;
  }
</script>
