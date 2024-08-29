<template>
  <div v-if="isDataLoaded">
    <h1>Examinations</h1>
    <div class="mb-3 row">
      <div class="col-12 col-md-4">
        <label
          for="name"
          class="form-label"
        >
          Search by Patient name
        </label>
        <input
          type="text"
          v-model="searchQuery.name"
          placeholder="Search by Patient Name"
          class="form-control mb-2"
        />
      </div>
      <div class="col-12 col-md-4">
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
      <div class="col-12 col-md-4">
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
      </div>
      <div
        v-if="isVisible[executedVisit.examination.id]"
        :id="'card-content-' + executedVisit.examination.id"
        class="row card-body"
      >
        <h4 class="card-title mb-3">
          Examination for {{ executedVisit.visit.patientFirstName }} {{ executedVisit.visit.patientLastName }} from {{ formattedSlot(executedVisit.visit.slot).date }}
          {{ formattedSlot(executedVisit.visit.slot).time }} executed by {{ executedVisit.visit.doctorFirstName }} {{ executedVisit.visit.doctorLastName }} ({{
            executedVisit.visit.doctorSpecialization
          }})
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
          <div v-if="executedVisit.examination.images?.length > 0">
            <div
              v-for="(image, index) in executedVisit.examination.images"
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
  import { ref, computed, onMounted } from "vue";
  import html2pdf from "html2pdf.js";
  import { useRoute } from "vue-router";

  import { getDoctorExaminations } from "@/api/examinationApi";
  import { ExaminationDto } from "@/models/ExaminationDto";
  import { VisitDto } from "@/models/VisitDto";
  import { getDoctorExecutedVisits } from "@/api/visitsApi";

  const route = useRoute();
  const isDataLoaded = ref(false);
  const examinations = ref<ExaminationDto[]>();
  const visits = ref<VisitDto[]>();
  const executedVisits = ref<ExecutedVisitDto[]>();

  interface ExecutedVisitDto {
    examination: ExaminationDto;
    visit: VisitDto;
  }

  const isVisible = ref<boolean[]>([]);

  const searchQuery = ref({
    name: "",
    date: "",
    time: "",
  });

  const filteredExecutedVisits = computed(() => {
    return executedVisits.value.filter((executedVisit) => {
      const patientName = `${executedVisit.visit.patientFirstName} ${executedVisit.visit.patientLastName}`;
      const visitDate = formattedSlot(executedVisit.visit.slot).date;
      const visitTime = formattedSlot(executedVisit.visit.slot).time;

      const matchesName = patientName.toLowerCase().includes(searchQuery.value.name.toLowerCase());
      const matchesDate = !searchQuery.value.date || visitDate === searchQuery.value.date;
      const matchesTime = !searchQuery.value.time || visitTime === searchQuery.value.time;

      return matchesName && matchesDate && matchesTime;
    });
  });

  onMounted(async () => {
    const routeDate = route.params.date as string | undefined;
    const routePatient = route.params.patient as string | undefined;
    if (routeDate && routeDate != " ") {
      searchQuery.value.date = formattedSlot(routeDate).date;
      searchQuery.value.time = formattedSlot(routeDate).time;
    }
    if (routePatient) {
      searchQuery.value.name = routePatient;
    }

    examinations.value = (await getDoctorExaminations(localStorage.getItem("email"))).data;
    console.log(examinations.value);
    visits.value = (await getDoctorExecutedVisits(localStorage.getItem("email"))).data;

    executedVisits.value = visits.value.map((visit) => {
      return {
        examination: examinations.value.find((examination) => examination.visitId === visit.id),
        visit,
      };
    });

    console.log(executedVisits.value);

    isVisible.value = new Array(executedVisits.value.length).fill(false);

    isDataLoaded.value = true;
  });

  function toggle(index: number) {
    isVisible.value[index] = !isVisible.value[index];
  }

  function printPDF(index: number, executedVisit: ExecutedVisitDto) {
    const element = document.getElementById("card-content-" + index);
    const opt = {
      margin: 0.5,
      filename: `Examination_${executedVisit.visit.patientEmail}_${formattedSlot(executedVisit.examination.date).date + "-" + formattedSlot(executedVisit.examination.date).time}.pdf`,
      image: { type: "jpeg", quality: 0.98 },
      html2canvas: { scale: 2 },
      jsPDF: { unit: "in", format: "letter", orientation: "portrait" },
      pagebreak: { mode: ["avoid-all"] }, // Avoid breaks inside elements
    };

    const pdfDoc = html2pdf().from(element).set(opt);

    // Check if there are any images to add on a new page
    if (executedVisit.examination.images?.length > 0) {
      pdfDoc
        .toPdf()
        .get("pdf")
        .then(function (pdf) {
          pdf.addPage();

          // Loop through each image and add them to the new page
          executedVisit.examination.images.forEach((imageSrc, index) => {
            const img = new Image();
            img.src = imageSrc;

            img.onload = function () {
              // Add the image to the new page at the correct position
              pdf.addImage(img, "JPEG", 15, index * 100 + 15, 180, 100); // Adjust x, y, width, height as needed

              // After the last image is added, save the PDF
              if (index === executedVisit.examination.images.length - 1) {
                pdfDoc.save();
              }
            };
          });
        });
    } else {
      // If no images, just save the PDF
      pdfDoc.save();
    }
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
