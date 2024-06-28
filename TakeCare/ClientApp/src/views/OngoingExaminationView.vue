<template>
  <div>
    <h1>Ongoing Examination</h1>
    <div v-if="errorMessage !== ''">
      <p class="text-danger">{{ errorMessage }}</p>
    </div>
    <div v-else-if="loadingEnded">
      <div class="border rounded mb-2 p-2">
        <h5>Visit details</h5>
        <div><strong>Id: </strong> {{ visit.id }}</div>
        <div><strong>Description: </strong> {{ visit.description }}</div>
        <div><strong>Slot: </strong> {{ formattedSlot(visit).date }} {{ formattedSlot(visit).time }}</div>
        <div><strong>Patient: </strong>{{ visit.patientFirstName }} {{ visit.patientLastName }}</div>
        <div><strong>Doctor: </strong>{{ visit.doctorFirstName }} {{ visit.doctorLastName }} {{ visit.doctorSpecialization }}</div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref, onMounted } from "vue";
  import { useRoute } from "vue-router";
  import { VisitDto } from "@/models/VisitDto";
  import { getVisit } from "@/api/visitsApi";
  import { AxiosError } from "axios";

  const route = useRoute();
  const visitId = ref<number>();
  const visit = ref<VisitDto>();
  const loadingEnded = ref(false);
  const errorMessage = ref("");
  onMounted(async () => {
    visitId.value = Number(route.params.visitId);
    if (Number.isNaN(visitId.value)) {
      errorMessage.value = "Invalid visit ID";
    } else {
      try {
        visit.value = (await getVisit(visitId.value)).data;
      } catch (error) {
        if (error instanceof AxiosError && error.response) {
          errorMessage.value = error.response.data;
        } else {
          errorMessage.value = "An unexpected error occurred";
        }
      }
      await getVisit(visitId.value).then((response) => {
        visit.value = response.data;
      });
    }
    loadingEnded.value = true;
  });

  const formattedSlot = (bookedVisit: VisitDto) => {
    if (bookedVisit.slot) {
      const [date, time] = bookedVisit.slot.split("T");
      const [hour, minute] = time.split(":");
      return {
        date,
        time: `${hour}:${minute}`,
      };
    }
    return { date: "", time: "" };
  };
</script>
