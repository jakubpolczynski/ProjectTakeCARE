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
      <form class="form-group">
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-name">Name</label>
            <input
              id="doctor-name"
              class="form-control"
              type="text"
              placeholder="Name"
            />
          </div>
          <div class="col-3">
            <label for="doctor-surname">Surname</label>
            <input
              id="doctor-surname"
              class="form-control col-3"
              type="text"
              placeholder="Surname"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-6">
            <label for="doctor-role">Role</label>
            <input
              id="doctor-role"
              class="form-control"
              type="text"
              placeholder="Role"
            />
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-6">
            <label for="doctor-title">Title</label>
            <input
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
              class="form-control"
              type="text"
              placeholder="Email"
            />
          </div>
          <div class="col-3">
            <label for="doctor-phone">Phone</label>
            <input
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
      <form class="form-group">
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-naem">Name</label>
            <input
              class="form-control"
              type="text"
              placeholder="Name"
            />
          </div>
          <div class="col-3">
            <label for="patient-surname">Surname</label>
            <input
              class="form-control"
              type="text"
              placeholder="Surname"
            />
          </div>
          <div class="col-3">
            <label for="patient-pesel">Pesel</label>
            <input
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
              class="form-control"
              type="text"
              placeholder="City"
            />
          </div>
          <div class="col-3">
            <label for="patient-street">Street</label>
            <input
              class="form-control"
              type="text"
              placeholder="Street"
            />
          </div>
          <div class="col-3">
            <label for="patient-postal-code">Postal Code</label>
            <input
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
              class="form-control"
              type="text"
              placeholder="Email"
            />
          </div>
          <div class="col-3">
            <label for="patient-phone">Phone</label>
            <input
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
  import { ref } from "vue";
  import { DoctorDto } from "@/models/DoctorDto";
  import { PatientDto } from "@/models/PatientDto";

  const isDoctorCreating = ref(false);
  const isPatientCreating = ref(false);
  const patient = ref<PatientDto>();
  const doctor = ref<DoctorDto>();

  const createDoctor = () => {
    isPatientCreating.value = false;
    isDoctorCreating.value = true;
  };

  const createPatient = () => {
    isDoctorCreating.value = false;
    isPatientCreating.value = true;
  };
</script>
<style scoped>
  .ps-2-5 {
    padding-left: 0.8rem;
  }
</style>
