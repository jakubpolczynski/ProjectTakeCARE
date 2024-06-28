<template>
  <h1>TakeCARE</h1>
  <div
    v-for="(patient, index) in patients"
    :key="index"
    class="border rounded mb-2 p-2 d-flex justify-content-between"
  >
    <div>
      <div>{{ patient.firstName }} {{ patient.lastName }}</div>
      <div>{{ patient.phone }}</div>
    </div>
    <div>
      <button class="btn btn-info text-white mt-1">Show executions</button>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { getDoctorPatients } from "@/api/patientsApi";
  import { PatientDto } from "@/models/PatientDto";
  import { ref, onMounted } from "vue";

  const patients = ref<PatientDto[]>();

  onMounted(async () => {
    patients.value = (await getDoctorPatients(localStorage.getItem("email"))).data;
    console.log(patients.value[0]);
  });
</script>
<style scoped lang="scss"></style>
