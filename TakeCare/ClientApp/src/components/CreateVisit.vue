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
          type="button"
          class="btn btn-info text-light m-1 mt-2"
          data-bs-toggle="modal"
          data-bs-target="#modal"
          @click="bookVisit(doctor, new Date(slot))"
        >
          {{ new Date(slot).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }) }}
        </div>
      </div>
    </div>
    <div
      class="modal fade"
      id="modal"
      tabindex="-1"
      aria-labelledby="modalLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5
              class="modal-title"
              id="visitModalLabel"
            >
              Book Visit
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            <p><strong>Doctor:</strong> {{ selectedDoctor.firstName }} {{ selectedDoctor.lastName }} ({{ selectedDoctor.specialization }})</p>
            <p>
              <strong>Contact:</strong>
              <br />
              <span>Email: {{ selectedDoctor.email }}</span>
              <br />
              <span>Phone: {{ selectedDoctor.phone }}</span>
            </p>
            <p
              v-if="formattedSlot"
              class="mt-2"
            >
              <strong>Slot:</strong> {{ formattedSlot.date }}
              {{ formattedSlot.time }}
            </p>
            <p><strong>Patient Email:</strong> {{ selectedDate.patientEmail }}</p>
            <div class="mb-3">
              <label
                for="visitReason"
                class="form-label"
                >Reason for Visit</label
              >
              <textarea
                class="form-control"
                id="visitReason"
                v-model="visitReason"
                rows="3"
              ></textarea>
              <div class="validation-error">{{ visitReasonError }}</div>
            </div>
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-secondary"
              data-bs-dismiss="modal"
            >
              Close
            </button>
            <button
              type="button"
              class="btn btn-primary"
              @click="confirmBooking()"
              :disabled="checkReason()"
              data-bs-dismiss="modal"
            >
              Confirm Booking
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, watch, computed } from "vue";
  import * as yup from "yup";
  import { useForm } from "vee-validate";
  import { AxiosError } from "axios";

  import { FindDateDto } from "@/models/FindDateDto";
  import { AvailableDateDto } from "@/models/AvailableDateDto";
  import SearchableSelect from "@/components/SearchableSelect.vue";
  import { MedicalSpecialties } from "@/enums/MedicalSpecialties";
  import { findAvailableVisits } from "@/api/visitsApi";
  import { VisitDto } from "@/models/VisitDto";
  import { AvailableDateDoctorDto } from "@/models/AvailableDateDoctorDto";

  const props = defineProps<{ selectedDate: string | null }>();

  const errorMessage = ref("");
  const specializationError = ref("");
  const selectedSpecialty = ref("");
  const availableDates = ref<AvailableDateDto | null>(null);
  const modalVisible = ref<boolean>();
  const selectedDoctor = ref<AvailableDateDoctorDto>({
    email: "",
    firstName: "",
    lastName: "",
    specialization: "",
    phone: "",
    availableSlots: [],
  });
  const selectedDate = ref<VisitDto>({
    doctorEmail: "",
    slot: "",
    patientEmail: "",
    description: "",
  });

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

  const visitReason = ref("");
  const visitReasonError = ref("");

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

  const formattedSlot = computed(() => {
    if (selectedDate.value.slot) {
      const [date, time] = selectedDate.value.slot.split("T");
      const [hour, minute] = time.split(":");
      return {
        date,
        time: `${hour}:${minute}`,
      };
    }
    return null;
  });

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

  const bookVisit = (doctor: AvailableDateDoctorDto, date: Date) => {
    fillVisitDto(doctor, date);
    fillSelectedDoctorDto(doctor);
    showModal();
  };

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

  const fillVisitDto = (doctor: AvailableDateDoctorDto, date: Date) => {
    const adjustedDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
    selectedDate.value.doctorEmail = doctor.email;
    selectedDate.value.slot = adjustedDate.toISOString();
    selectedDate.value.patientEmail = localStorage.getItem("email") || "";
  };

  const fillSelectedDoctorDto = (doctor: AvailableDateDoctorDto) => {
    selectedDoctor.value.email = doctor.email;
    selectedDoctor.value.firstName = doctor.firstName;
    selectedDoctor.value.lastName = doctor.lastName;
    selectedDoctor.value.phone = doctor.phone;
    selectedDoctor.value.specialization = doctor.specialization;
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

  const checkReason = () => {
    if (!visitReason.value) {
      visitReasonError.value = "Reason for visit is required";
      return true;
    } else {
      visitReasonError.value = "";
      return false;
    }
  };

  const showModal = () => {
    modalVisible.value = true;
    console.log("Booking start: ", selectedDate.value);
  };

  const confirmBooking = () => {
    console.log("Booking confirmed: ", selectedDate.value);
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
