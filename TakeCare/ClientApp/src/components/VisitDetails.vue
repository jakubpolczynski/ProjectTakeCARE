<template>
  <div>
    <h5>Visit details</h5>
    <div
      v-if="filteredVisits.length > 0"
      class="scrollable-container"
    >
      <div
        v-for="bookedVisit in filteredVisits"
        :key="bookedVisit.slot"
      >
        <div
          id="visit-list"
          class="list-group mt-1"
        >
          <div
            class="list-group-item"
            aria-current="true"
          >
            <div class="d-flex w-100 justify-content-between">
              <h5 class="mb-1">Doctor: {{ bookedVisit.doctorFirstName }} {{ bookedVisit.doctorLastName }} ({{ bookedVisit.doctorSpecialization }})</h5>
              <small>{{ formattedSlot(bookedVisit).date }} {{ formattedSlot(bookedVisit).time }}</small>
            </div>
            <div class="d-flex w-100">
              <p class="mb-1 mt-2 flex-grow-1">Reason: {{ bookedVisit.description }}</p>
              <button
                v-if="actualRole === 'Doctor' && bookedVisit.isVisitExecuted === false"
                class="btn btn-success me-2"
                :class="{
                  disabled: formattedSlot(bookedVisit).date < new Date().toISOString().split('T')[0],
                }"
                @click="startExamination(bookedVisit)"
              >
                Start examination
              </button>
              <button
                v-if="bookedVisit.isVisitExecuted === true"
                class="btn btn-success me-2"
                @click="showExamination(bookedVisit)"
              >
                Show examination
              </button>
              <button
                class="btn btn-danger"
                :class="{
                  disabled: formattedSlot(bookedVisit).date < new Date().toISOString().split('T')[0],
                }"
                @click="cancelVisit(bookedVisit)"
              >
                Cancel visit
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else>No visits found for this day</div>
    <div class="request-error">{{ errorMessage }}</div>
  </div>
</template>

<script setup lang="ts">
  import { computed, defineProps, onMounted, ref } from "vue";
  import { VisitDto } from "@/models/VisitDto";
  import { deleteBookedVisit } from "@/api/visitsApi";
  import { AxiosError } from "axios";
  import { useRouter } from "vue-router";

  const router = useRouter();

  const emit = defineEmits(["visitCancelled"]);
  const props = defineProps<{ bookedVisits?: VisitDto[]; selectedDate?: string | null }>();

  const actualRole = ref("");
  onMounted(() => {
    actualRole.value = localStorage.getItem("user");
  });

  const errorMessage = ref("");

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

  const filteredVisits = computed(() => {
    if (!props.selectedDate) return [];
    return props.bookedVisits.filter((visit) => visit.slot.split("T")[0] === props.selectedDate);
  });

  const cancelVisit = async (bookedVisit: VisitDto) => {
    console.log("Cancel visit", bookedVisit);
    try {
      await deleteBookedVisit(bookedVisit);
      emit("visitCancelled");
    } catch (error) {
      if (error instanceof AxiosError && error.response) {
        errorMessage.value = error.response.data;
      } else {
        errorMessage.value = "An unexpected error occurred";
      }
    }
  };

  const startExamination = async (bookedVisit: VisitDto) => {
    await router.push({ name: "ongoingexamination", params: { visitId: bookedVisit.id.toString() } });
  };

  const showExamination = (bookedVisit: VisitDto) => {
    console.log("Show examination", bookedVisit);
  };
</script>
<style scoped>
  .scrollable-container {
    overflow-y: auto;
    max-height: 300px;
  }

  .request-error {
    color: red;
    font-weight: bold;
  }
</style>
