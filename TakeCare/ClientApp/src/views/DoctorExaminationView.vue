<template>
  <div v-if="isDataLoaded">
    <h1>Examinations</h1>
    <div
      v-for="executedVisit in executedVisits"
      :key="executedVisit.examination.id"
      class="card mb-3"
    >
      <div class="card-header">
        <h4
          v-if="!isVisible[executedVisit.examination.id]"
          class="card-title"
        >
          Examination for {{ executedVisit.visit.patientFirstName }} {{ executedVisit.visit.patientLastName }} from {{ formattedSlot(executedVisit.visit.slot).date }}
          {{ formattedSlot(executedVisit.visit.slot).time }}
        </h4>
        <button
          class="btn btn-primary me-2"
          @click="toggle(executedVisit.examination.id)"
        >
          {{ isVisible[executedVisit.examination.id] ? "Hide Details" : "Show Details" }}
        </button>
        <button
          class="btn btn-outline-primary"
          :class="{ disabled: !isVisible[executedVisit.examination.id] }"
          @click="printPDF(executedVisit.examination.id, executedVisit)"
        >
          Print PDF
        </button>
      </div>
      <div
        v-if="isVisible[executedVisit.examination.id]"
        :id="'card-content-' + executedVisit.examination.id"
        class="row card-body"
      >
        <h4 class="card-title mb-3">
          Examination for {{ executedVisit.visit.patientFirstName }} {{ executedVisit.visit.patientLastName }} from {{ formattedSlot(executedVisit.visit.slot).date }}
          {{ formattedSlot(executedVisit.visit.slot).time }}
        </h4>
        <hr />
        <div class="col-12 col-md-6">
          <h4 class="text-primary">Doctor data</h4>
          <p><strong>Doctor: </strong>{{ executedVisit.visit.doctorFirstName }} {{ executedVisit.visit.doctorLastName }} ({{ executedVisit.visit.doctorSpecialization }})</p>
          <p><strong>Doctor email: </strong>{{ executedVisit.visit.doctorEmail }}</p>
          <hr />
        </div>
        <div class="col-12 col-md-6">
          <h4 class="text-primary">Patient data</h4>
          <p><strong>Patient: </strong>{{ executedVisit.visit.patientFirstName }} {{ executedVisit.visit.patientLastName }}</p>
          <p><strong>Patient email: </strong>{{ executedVisit.visit.patientEmail }}</p>
          <hr />
        </div>
        <div class="col-12 col-md-6">
          <h4 class="text-primary">Examination details</h4>
          <p><strong>Data: </strong>{{ formattedSlot(executedVisit.examination.date).date }} {{ formattedSlot(executedVisit.examination.date).time }}</p>
          <p><strong>Name: </strong>{{ executedVisit.examination.name }}</p>
          <p><strong>Type: </strong>{{ executedVisit.examination.type }}</p>
          <p><strong>Description: </strong>{{ executedVisit.examination.description }}</p>
          <p><strong>Result: </strong>{{ executedVisit.examination.result }}</p>
          <hr class="d-block d-md-none" />
        </div>
        <div class="col-12 col-md-6">
          <h4 class="text-primary">Visit details</h4>
          <p><strong>Reason: </strong>{{ executedVisit.visit.reason }}</p>
          <p><strong>Date: </strong>{{ formattedSlot(executedVisit.visit.slot).date }} {{ formattedSlot(executedVisit.visit.slot).time }}</p>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref, onMounted } from "vue";
  import html2pdf from "html2pdf.js";

  import { getDoctorExaminations } from "@/api/examinationApi";
  import { ExaminationDto } from "@/models/ExaminationDto";
  import { VisitDto } from "@/models/VisitDto";
  import { getDoctorExecutedVisits } from "@/api/visitsApi";

  const isDataLoaded = ref(false);
  const examinations = ref<ExaminationDto[]>();
  const visits = ref<VisitDto[]>();
  const executedVisits = ref<ExecutedVisitDto[]>();

  interface ExecutedVisitDto {
    examination: ExaminationDto;
    visit: VisitDto;
  }

  const isVisible = ref<boolean[]>([]);

  onMounted(async () => {
    examinations.value = (await getDoctorExaminations(localStorage.getItem("email"))).data;
    visits.value = (await getDoctorExecutedVisits(localStorage.getItem("email"))).data;

    executedVisits.value = visits.value.map((visit) => {
      return {
        examination: examinations.value.find((examination) => examination.visitId === visit.id),
        visit,
      };
    });

    isVisible.value = new Array(executedVisits.value.length).fill(false);

    isDataLoaded.value = true;
  });

  function toggle(index: number) {
    isVisible.value[index] = !isVisible.value[index];
  }

  function printPDF(index: number, executedVisit: ExecutedVisitDto) {
    const element = document.getElementById("card-content-" + index);
    const opt = {
      margin: 1,
      filename: `Examination_${executedVisit.visit.patientEmail}_${formattedSlot(executedVisit.examination.date).date + "-" + formattedSlot(executedVisit.examination.date).time}.pdf`,
      image: { type: "jpeg", quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: "in", format: "letter", orientation: "portrait" },
    };
    html2pdf().from(element).set(opt).save();
  }

  const formattedSlot = (date: string) => {
    if (date) {
      const [formatedDate, formatedTime] = date.split("T");
      const [hour, minute] = formatedTime.split(":");
      return {
        date: formatedDate,
        time: `${hour}:${minute}`,
      };
    }
    return { date: "", time: "" };
  };
</script>
