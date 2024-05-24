<template>
  <div class="w-100">
    <form
      class="form-group"
      @submit="findAvilableDate"
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
        <label for="doctor-title">Medical specialization</label>
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
          Find avilable date
        </button>
      </div>
    </form>
  </div>
</template>
<script setup lang="ts">
  import { ref } from "vue";
  import * as yup from "yup";
  import { useForm } from "vee-validate";

  import { FindDateDto } from "@/models/FindDateDto";
  import SearchableSelect from "@/components/SearchableSelect.vue";
  import { MedicalSpecialties } from "@/enums/MedicalSpecialties";

  const errorMessage = ref("");
  const specializationError = ref("");
  const selectedSpecialty = ref("");

  const findDate = ref<FindDateDto>({
    firstName: "",
    lastName: "",
    date: new Date(),
  });

  const specialties = ref(
    Object.values(MedicalSpecialties).map((specialty) => ({
      value: specialty,
      label: specialty,
    }))
  );

  const findDateSchema = yup.object({
    doctor: yup.string().required(),
    date: yup.date().required(),
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: findDateSchema,
  });

  const [doctor, doctorAttrs] = defineField("doctor");
  const [date, dateAttrs] = defineField("date");

  const findAvilableDate = handleSubmit(async () => {
    errorMessage.value = "";
    if (checkSpecialty()) {
      fillFindDateDto();
    }
  });

  const fillFindDateDto = () => {
    findDate.value.firstName = doctor.value.split(" ")[0];
    findDate.value.lastName = doctor.value.split(" ")[1];
    findDate.value.date = date.value;
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
</style>
