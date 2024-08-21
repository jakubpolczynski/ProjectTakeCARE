<template>
  <div v-if="isDataLoaded">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">PDF preview</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="card mb-3">
            <div
              :id="'card-content-' + props.executedVisit.examination.id"
              class="row card-body"
            >
              <h4 class="card-title mb-3">
                Examination for {{ props.executedVisit.visit.patientFirstName }} {{ props.executedVisit.visit.patientLastName }} from
                {{ formattedSlot(props.executedVisit.visit.slot).date }}
                {{ formattedSlot(props.executedVisit.visit.slot).time }}
              </h4>
              <hr />
              <div class="col-12 col-md-6">
                <h4 class="text-primary">Doctor data</h4>
                <p>
                  <strong>Doctor: </strong>{{ props.executedVisit.visit.doctorFirstName }} {{ props.executedVisit.visit.doctorLastName }} ({{
                    props.executedVisit.visit.doctorSpecialization
                  }})
                </p>
                <p><strong>Doctor email: </strong>{{ props.executedVisit.visit.doctorEmail }}</p>
                <hr />
              </div>
              <div class="col-12 col-md-6">
                <h4 class="text-primary">Patient data</h4>
                <p><strong>Patient: </strong>{{ props.executedVisit.visit.patientFirstName }} {{ props.executedVisit.visit.patientLastName }}</p>
                <p><strong>Patient email: </strong>{{ props.executedVisit.visit.patientEmail }}</p>
                <hr />
              </div>
              <div class="col-12 col-md-6">
                <h4 class="text-primary">Examination details</h4>
                <p><strong>Data: </strong>{{ formattedSlot(props.executedVisit.examination.date).date }} {{ formattedSlot(props.executedVisit.examination.date).time }}</p>
                <p><strong>Name: </strong>{{ props.executedVisit.examination.name }}</p>
                <p><strong>Type: </strong>{{ props.executedVisit.examination.type }}</p>
                <p><strong>Description: </strong>{{ props.executedVisit.examination.description }}</p>
                <p><strong>Result: </strong>{{ props.executedVisit.examination.result }}</p>
                <hr class="d-block d-md-none" />
              </div>
              <div class="col-12 col-md-6">
                <h4 class="text-primary">Visit details</h4>
                <p><strong>Reason: </strong>{{ props.executedVisit.visit.reason }}</p>
                <p><strong>Date: </strong>{{ formattedSlot(props.executedVisit.visit.slot).date }} {{ formattedSlot(props.executedVisit.visit.slot).time }}</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ExaminationDto } from "@/models/ExaminationDto";
  import { VisitDto } from "@/models/VisitDto";
  import { onMounted, ref } from "vue";

  const props = defineProps<{ executedVisit: ExecutedVisitDto }>();

  interface ExecutedVisitDto {
    examination: ExaminationDto;
    visit: VisitDto;
  }

  const isDataLoaded = ref(false);

  onMounted(() => {
    isDataLoaded.value = true;
  });

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
