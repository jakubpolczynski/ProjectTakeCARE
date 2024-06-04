<template>
  <h1>Visits</h1>
  <Calendar
    @dayClick="handleDayClick"
    :bookedVisits="patientVisits"
    :selectedDate="selectedDate"
  ></Calendar>
  <VisitDetails
    :bookedVisits="patientVisits"
    :selectedDate="selectedDate"
    @visitCancelled="handleVisitChange"
  ></VisitDetails>
</template>
<script setup lang="ts">
  import { ref, onMounted } from "vue";

  import { getDoctorVisits } from "@/api/visitsApi";

  import Calendar from "@/components/Calendar.vue";
  import VisitDetails from "@/components/VisitDetails.vue";

  import { VisitDto } from "@/models/VisitDto";

  const patientVisits = ref<VisitDto[]>();
  const selectedDate = ref<string | null>(null);

  onMounted(async () => {
    patientVisits.value = (await getDoctorVisits(localStorage.getItem("email"))).data;
    selectedDate.value = new Date().toISOString().split("T")[0];
  });

  function handleDayClick(date: string) {
    selectedDate.value = date;
  }

  const handleVisitChange = async () => {
    patientVisits.value = (await getDoctorVisits(localStorage.getItem("email"))).data;
  };
</script>
<style scoped lang="scss"></style>
