<template>
  <div>
    <h1>Choose role</h1>
    <div class="row ps-2-5">
      <button
        @click="createPatient"
        class="btn btn-primary me-3 col-2"
      >
        Patient
      </button>
      <button
        @click="createDoctor"
        class="btn btn-primary col-2"
      >
        Doctor
      </button>
    </div>
    <div v-if="isDoctorCreating">
      <form
        class="form-group"
        @submit.prevent="createDoctorAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-name">Name</label>
            <input
              id="doctor-name"
              v-model="doctor.FirstName"
              class="form-control"
              type="text"
              placeholder="Name"
            />
          </div>
          <div class="col-3">
            <label for="doctor-surname">Surname</label>
            <input
              id="doctor-surname"
              v-model="doctor.LastName"
              class="form-control col-3"
              type="text"
              placeholder="Surname"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-6">
            <label for="doctor-title">Title</label>
            <input
              id="doctor-title"
              v-model="doctor.Title"
              class="form-control"
              type="text"
              placeholder="Title"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-email">Email</label>
            <input
              id="doctor-email"
              v-model="doctor.Email"
              class="form-control"
              type="text"
              placeholder="Email"
            />
          </div>
          <div class="col-3">
            <label for="doctor-phone">Phone</label>
            <input
              id="doctor-phone"
              v-model="doctor.Phone"
              class="form-control"
              type="text"
              placeholder="Phone"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-password">Password</label>
            <input
              id="doctor-password"
              v-model="doctor.Password"
              class="form-control"
              type="password"
              placeholder="Password"
            />
          </div>
        </div>
        <button
          type="submit"
          class="btn btn-success mt-3"
        >
          Create
        </button>
      </form>
    </div>
    <div v-if="isPatientCreating">
      <form
        class="form-group"
        @submit.prevent="createPatientAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-name">Name</label>
            <input
              id="patient-name"
              v-model="patient.FirstName"
              class="form-control"
              type="text"
              placeholder="Name"
            />
          </div>
          <div class="col-3">
            <label for="patient-surname">Surname</label>
            <input
              id="patient-surname"
              v-model="patient.LastName"
              class="form-control"
              type="text"
              placeholder="Surname"
            />
          </div>
          <div class="col-3">
            <label for="patient-pesel">Pesel</label>
            <input
              id="patient-pesel"
              v-model="patient.Pesel"
              class="form-control"
              type="text"
              placeholder="Pesel"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-city">City</label>
            <input
              id="patient-city"
              v-model="patient.City"
              class="form-control"
              type="text"
              placeholder="City"
            />
          </div>
          <div class="col-3">
            <label for="patient-street">Street</label>
            <input
              id="patient-street"
              v-model="patient.Street"
              class="form-control"
              type="text"
              placeholder="Street"
            />
          </div>
          <div class="col-3">
            <label for="patient-postal-code">Postal Code</label>
            <input
              id="patient-postal-code"
              v-model="patient.PostalCode"
              class="form-control"
              type="text"
              placeholder="Postal Code"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-email">Email</label>
            <input
              id="patient-email"
              v-model="patient.Email"
              class="form-control"
              type="text"
              placeholder="Email"
            />
          </div>
          <div class="col-3">
            <label for="patient-phone">Phone</label>
            <input
              id="patient-phone"
              v-model="patient.Phone"
              class="form-control"
              type="text"
              placeholder="Phone"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-password">Password</label>
            <input
              id="patient-password"
              v-model="patient.Password"
              class="form-control"
              type="password"
              placeholder="Password"
            />
          </div>
        </div>
        <button
          type="submit"
          class="btn btn-success mt-3"
        >
          Create
        </button>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, watch } from "vue";
  import { addDoctor, addPatient } from "@/api/userApi";
  import { DoctorDto } from "@/models/DoctorDto";
  import { PatientDto } from "@/models/PatientDto";
  import * as yup from "yup";
  import { useForm } from "vee-validate";

  const isDoctorCreating = ref(false);
  const isPatientCreating = ref(false);

  const patient = ref<PatientDto>({
    Pesel: "",
    FirstName: "",
    LastName: "",
    Email: "",
    Phone: "",
    City: "",
    Street: "",
    PostalCode: "",
    Password: "",
    Role: "Patient",
  });

  const doctor = ref<DoctorDto>({
    Email: "",
    Password: "",
    FirstName: "",
    LastName: "",
    Title: "",
    Phone: "",
    Role: "Doctor",
  });

  const doctorSchema = yup.object({
    Email: yup.string().email("Must be a valid email").required("Email is required"),
    Password: yup.string().min(8, "Password must be at least 8 characters").required("Password is required"),
    FirstName: yup.string().required("First name is required"),
    LastName: yup.string().required("Last name is required"),
    Title: yup.string().required("Title is required"),
    Phone: yup
      .string()
      .matches(/^[0-9]+$/, "Phone number must be numeric")
      .required("Phone is required"),
  });

  const patientSchema = yup.object({
    Pesel: yup
      .string()
      .matches(/^[0-9]+$/, "Phone number must be numeric")
      .min(11, "PESEL must be exactly 11 characters")
      .max(11, "PESEL must be exactly 11 characters")
      .required("Pesel is required"),
    FirstName: yup.string().required("First name is required"),
    LastName: yup.string().required("Last name is required"),
    Email: yup.string().email("Must be a valid email").required("Email is required"),
    Phone: yup
      .string()
      .matches(/^[0-9]+$/, "Phone number must be numeric")
      .required("Phone is required"),
    City: yup.string().required("City is required"),
    Street: yup.string().required("Street is required"),
    PostalCode: yup.string().required("Postal code is required"),
    Password: yup.string().min(8, "Password must be at least 8 characters").required("Password is required"),
  });

  const { handleSubmit, validate } = useForm({
    validationSchema: isDoctorCreating.value ? doctorSchema : patientSchema,
  });

  const createDoctor = () => {
    isPatientCreating.value = false;
    isDoctorCreating.value = true;
  };

  const createPatient = () => {
    isDoctorCreating.value = false;
    isPatientCreating.value = true;
  };

  const createPatientAccount = () => {
    addPatient(patient.value);
  };

  const createDoctorAccount = () => {
    addDoctor(doctor.value);
  };
</script>
<style scoped>
  .ps-2-5 {
    padding-left: 0.8rem;
  }
</style>
