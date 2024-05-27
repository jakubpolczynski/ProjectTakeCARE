<template>
  <div class="w-100">
    <form
      class="form-group"
      @submit="findAvailableDate"
    >
      <div class="mt-2">
        <label for="doctor"> Doctor </label>
        <input
          id="doctor"
          type="text"
          class="form-control"
          v-model="doctor"
          v-bind="doctorAttrs"
          placeholder="Doctor"
        />
        <span class="validation-error">{{ errors.doctor }}</span>
      </div>
      <div class="mt-2">
        <label for="date"> Date </label>
        <input
          id="date"
          type="date"
          class="form-control"
          v-model="date"
          v-bind="dateAttrs"
        />
        <span class="validation-error">{{ errors.date }}</span>
      </div>
      <div class="mt-2">
        <label for="doctor-specialization">Medical specialization</label>
        <SearchableSelect
          v-model="selectedSpecialty"
          :options="specialties"
        />
        <span class="validation-error">{{ specializationError }}</span>
      </div>
      <div class="mt-2">
        <button
          type="submit"
          class="btn btn-primary w-100"
        >
          Find available date
        </button>
      </div>
    </form>
    <div class="request-error">{{ errorMessage }}</div>
    <div v-if="availableDates">
      <div v-for="doctor in availableDates.doctors">
        <span class="text-center d-flex mt-2">{{ doctor.firstName }} {{ doctor.lastName }} ({{ doctor.specialization }})</span>
        <div
          v-for="slot in doctor.availableSlots"
          class="btn btn-info m-1 mt-2"
        >
          {{ new Date(slot).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }) }}
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref, watch } from "vue";
  import * as yup from "yup";
  import { useForm } from "vee-validate";
  import { AxiosError } from "axios";

  import { FindDateDto } from "@/models/FindDateDto";
  import { AvailableDateDto } from "@/models/AvailableDateDto";
  import SearchableSelect from "@/components/SearchableSelect.vue";
  import { MedicalSpecialties } from "@/enums/MedicalSpecialties";
  import { findAvailableVisits } from "@/api/visitsApi";

  const props = defineProps<{ selectedDate: Date | null }>();

  const errorMessage = ref("");
  const specializationError = ref("");
  const selectedSpecialty = ref("");
  const availableDates = ref<AvailableDateDto>();

  const findDate = ref<FindDateDto>({
    firstName: "",
    lastName: "",
    specialization: "",
    date: "",
  });

  const specialties = ref(
    Object.values(MedicalSpecialties).map((specialty) => ({
      value: specialty,
      label: specialty,
    }))
  );

  const findDateSchema = yup.object({
    doctor: yup.string(),
    date: yup.date().required(),
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: findDateSchema,
  });

  const [doctor, doctorAttrs] = defineField("doctor");
  const [date, dateAttrs] = defineField("date");

  watch(
    () => props.selectedDate,
    (newDate) => {
      if (newDate) {
        date.value = newDate;
      }
    }
  );

  const findAvailableDate = handleSubmit(async () => {
    errorMessage.value = "";
    if (checkSpecialty()) {
      fillFindDateDto();
      try {
        const response = await findAvailableVisits(findDate.value);
        availableDates.value = response.data;
      } catch (error) {
        if (error instanceof AxiosError && error.response) {
          errorMessage.value = error.response.data;
        } else {
          errorMessage.value = "An unexpected error occurred";
        }
      }
    }
  });

  const fillFindDateDto = () => {
    if (doctor.value && doctor.value != "") {
      findDate.value.firstName = doctor.value.split(" ")[0];
      findDate.value.lastName = doctor.value.split(" ")[1];
    } else {
      findDate.value.firstName = "";
      findDate.value.lastName = "";
    }
    findDate.value.specialization = selectedSpecialty.value;
    findDate.value.date = new Date(date.value).toISOString();
  };

  const checkSpecialty = () => {
    if (!specialties.value.find((specialty) => specialty.value === selectedSpecialty.value)) {
      specializationError.value = "Invalid specialty";
      return false;
    } else {
      specializationError.value = "";
      return true;
    }
  };
</script>
<style scoped>
  .validation-error {
    color: red;
    font-size: small;
  }

  .request-error {
    color: red;
    font-weight: bold;
  }

  .available-slot {
    border: 1px solid black;
    padding: 5px;
    margin-top: 5px;
  }
</style>
