<template>
  <div v-if="isDataLoaded">
    <h1>Examinations</h1>
    <div class="mb-3 row">
      <div class="col-12 col-md-6">
        <label
          for="date"
          class="form-label"
        >
          Search by date
        </label>
        <input
          type="date"
          v-model="searchQuery.date"
          placeholder="Search by Date"
          class="form-control"
        />
      </div>
      <div class="col-12 col-md-6">
        <label
          for="date"
          class="form-label"
        >
          Search by time
        </label>
        <input
          type="time"
          v-model="searchQuery.time"
          placeholder="Search by time"
          class="form-control"
        />
      </div>
      <div class="col-12 col-md-6">
        <label
          for="specialization"
          class="form-label"
        >
          Search by specialization
        </label>
        <input
          type="text"
          v-model="searchQuery.specialization"
          placeholder="Search by specialization"
          class="form-control"
        />
      </div>
      <div class="col-12 col-md-6">
        <label
          for="name"
          class="form-label"
        >
          Search by doctor name
        </label>
        <input
          type="text"
          v-model="searchQuery.name"
          placeholder="Search by doctor name"
          class="form-control"
        />
      </div>
    </div>
    <div
      v-for="executedVisit in filteredExecutedVisits"
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
        <button class="btn btn-outline-secondary ms-2" type="button" @click="downloadPdf(executedVisit.examination.id)">
          Download saved PDF
        </button>
        <p v-if="downloadError" class="text-danger mt-2" role="alert">{{ downloadError }}</p>
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
        <div class="col-12">
          <h4 class="text-primary">Uploaded Images</h4>
          <div v-if="executedVisit.imagePreviews?.length > 0">
            <div
              v-for="(image, index) in executedVisit.imagePreviews"
              :key="index"
              class="mb-3"
            >
              <img
                :src="image"
                alt="Uploaded Image"
                class="rounded img-fluid"
              />
            </div>
          </div>
          <div v-else>
            <p>No images uploaded.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref, onMounted, computed } from "vue";
  import html2pdf from "html2pdf.js";
  import { useRoute } from "vue-router";

  import { getPatientExaminations, downloadExaminationPdf } from "@/api/examinationApi";
  import { getPatientExecutedVisits } from "@/api/visitsApi";
  import { ExaminationDto } from "@/models/ExaminationDto";
  import { VisitDto } from "@/models/VisitDto";

  const route = useRoute();
  const isDataLoaded = ref(false);
  const downloadError = ref("");
  const downloadPdf = async (id: number) => {
    downloadError.value = "";
    try { await downloadExaminationPdf(id); }
    catch { downloadError.value = "The saved PDF is unavailable. Older examinations may not have a stored PDF."; }
  };
  const examinations = ref<ExaminationDto[]>();
  const visits = ref<VisitDto[]>();
  const executedVisits = ref<ExecutedVisitDto[]>();

  interface ExecutedVisitDto {
    examination: ExaminationDto;
    visit: VisitDto;
    imagePreviews?: string[];
  }

  const isVisible = ref<boolean[]>([]);

  const searchQuery = ref({
    date: "",
    time: "",
    specialization: "",
    name: "",
  });

  const filteredExecutedVisits = computed(() => {
    return executedVisits.value.filter((executedVisit) => {
      const visitDate = formattedSlot(executedVisit.visit.slot).date;
      const visitTime = formattedSlot(executedVisit.visit.slot).time;
      const doctorSpecialization = executedVisit.visit.doctorSpecialization;
      const doctorName = `${executedVisit.visit.doctorFirstName} ${executedVisit.visit.doctorLastName}`;

      const matchesDate = !searchQuery.value.date || visitDate === searchQuery.value.date;
      const matchesTime = !searchQuery.value.time || visitTime === searchQuery.value.time;
      const matchesSpecialization = doctorSpecialization.toLowerCase().includes(searchQuery.value.specialization.toLowerCase());
      const matchesName = doctorName.toLowerCase().includes(searchQuery.value.name.toLocaleLowerCase());

      return matchesDate && matchesTime && matchesSpecialization && matchesName;
    });
  });

  onMounted(async () => {
    const routeDate = route.params.date as string | undefined;
    if (routeDate) {
      searchQuery.value.date = formattedSlot(routeDate).date;
      searchQuery.value.time = formattedSlot(routeDate).time;
    }

    examinations.value = (await getPatientExaminations(localStorage.getItem("email"))).data;
    // Older data can contain arbitrary client-supplied image paths; never fetch remote URLs.
    examinations.value = examinations.value.map((item) => ({
      ...item,
      images: (item.images || []).filter((image) =>
        image.startsWith("data:image/png;base64,") || image.startsWith("data:image/jpeg;base64,")),
    }));
    visits.value = (await getPatientExecutedVisits(localStorage.getItem("email"))).data;

    executedVisits.value = visits.value.flatMap((visit) => {
      const examination = examinations.value.find((item) => item.visitId === visit.id);
      return examination ? [{ examination, visit }] : [];
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
      filename: `examination-${executedVisit.examination.id}.pdf`,
      image: { type: "jpeg" as const, quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: "in", format: "letter", orientation: "portrait" as const },
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
