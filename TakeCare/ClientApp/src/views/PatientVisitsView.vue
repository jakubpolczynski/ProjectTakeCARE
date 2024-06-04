<template>
  <h1>Visits</h1>
  <div class="row">
    <div class="col-5">
      <CreateVisit
        :selectedDate="selectedDate >= new Date().toISOString().split('T')[0] ? selectedDate : null"
        @visitBooked="handleVisitChange"
      ></CreateVisit>
    </div>
    <div class="col">
      <Calendar
        @dayClick="handleDayClick"
        :bookedVisits="patientVisits"
      ></Calendar>
      <VisitDetails
        :bookedVisits="patientVisits"
        :selectedDate="selectedDate"
        @visitCancelled="handleVisitChange"
      ></VisitDetails>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref, onMounted } from "vue";

  import { getPatientVisits } from "@/api/visitsApi";

  import Calendar from "@/components/Calendar.vue";
  import CreateVisit from "@/components/CreateVisit.vue";
  import VisitDetails from "@/components/VisitDetails.vue";

  import { VisitDto } from "@/models/VisitDto";

  const patientVisits = ref<VisitDto[]>();
  const selectedDate = ref<string | null>(null);

  onMounted(async () => {
    patientVisits.value = (await getPatientVisits(localStorage.getItem("email"))).data;
    selectedDate.value = new Date().toISOString().split("T")[0];
  });

  function handleDayClick(date: string) {
    selectedDate.value = date;
  }

  const handleVisitChange = async () => {
    patientVisits.value = (await getPatientVisits(localStorage.getItem("email"))).data;
  };
</script>
<style scoped lang="scss"></style>
