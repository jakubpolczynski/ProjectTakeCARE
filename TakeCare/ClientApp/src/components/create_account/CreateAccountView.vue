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
        @submit="createDoctorAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-first-name">first name</label>
            <input
              id="doctor-first-name"
              v-model="doctorFirstName"
              v-bind="doctorFirstNameAttrs"
              class="form-control"
              type="text"
              placeholder="Name"
            />
            <span class="validation-error">{{ errors.doctorFirstName }}</span>
          </div>
          <div class="col-3">
            <label for="doctor-last-name">Last name</label>
            <input
              id="doctor-last-name"
              v-model="doctorLastName"
              v-bind="doctorLastNameAttrs"
              class="form-control col-3"
              type="text"
              placeholder="Surname"
            />
            <span class="validation-error">{{ errors.doctorLastName }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-6">
            <label for="doctor-title">Title</label>
            <input
              id="doctor-title"
              v-model="doctorTitle"
              v-bind="doctorTitleAttrs"
              class="form-control"
              type="text"
              placeholder="Title"
            />
            <span class="validation-error">{{ errors.doctorTitle }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-email">Email</label>
            <input
              id="doctor-email"
              v-model="doctorEmail"
              v-bind="doctorEmailAttrs"
              class="form-control"
              type="text"
              placeholder="Email"
            />
            <span class="validation-error">{{ errors.doctorEmail }}</span>
          </div>
          <div class="col-3">
            <label for="doctor-phone">Phone</label>
            <input
              id="doctor-phone"
              v-model="doctorPhone"
              v-bind="doctorPhoneAttrs"
              class="form-control"
              type="text"
              placeholder="Phone"
            />
            <span class="validation-error">{{ errors.doctorPhone }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-password">Password</label>
            <input
              id="doctor-password"
              v-model="doctorPassword"
              v-bind="doctorPasswordAttrs"
              class="form-control"
              type="password"
              placeholder="Password"
            />
            <span class="validation-error">{{ errors.doctorPassword }}</span>
          </div>
        </div>
        <button class="btn btn-success mt-3">Create</button>
      </form>
    </div>
    <div v-if="isPatientCreating">
      <form
        class="form-group"
        @submit="createPatientAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-name">First name</label>
            <input
              id="patient-name"
              v-model="patientFirstName"
              v-bind="patientFirstNameAttrs"
              class="form-control"
              type="text"
              placeholder="Name"
            />
            <span class="validation-error">{{ errors.patientFirstName }}</span>
          </div>
          <div class="col-3">
            <label for="patient-surname">Last name</label>
            <input
              id="patient-surname"
              v-model="patientLastName"
              v-bind="patientLastNameAttrs"
              class="form-control"
              type="text"
              placeholder="Surname"
            />
            <span class="validation-error">{{ errors.patientLastName }}</span>
          </div>
          <div class="col-3">
            <label for="patient-pesel">Pesel</label>
            <input
              id="patient-pesel"
              v-model="patientPesel"
              v-bind="patientPeselAttrs"
              class="form-control"
              type="text"
              placeholder="Pesel"
            />
            <span class="validation-error">{{ errors.patientPesel }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-city">City</label>
            <input
              id="patient-city"
              v-model="patientCity"
              v-bind="patientCityAttrs"
              class="form-control"
              type="text"
              placeholder="City"
            />
            <span class="validation-error">{{ errors.patientCity }}</span>
          </div>
          <div class="col-3">
            <label for="patient-street">Street</label>
            <input
              id="patient-street"
              v-model="patientStreet"
              v-bind="patientStreetAttrs"
              class="form-control"
              type="text"
              placeholder="Street"
            />
            <span class="validation-error">{{ errors.patientStreet }}</span>
          </div>
          <div class="col-3">
            <label for="patient-postal-code">Postal Code</label>
            <input
              id="patient-postal-code"
              v-model="patientPostalCode"
              v-bind="patientPostalCodeAttrs"
              class="form-control"
              type="text"
              placeholder="Postal Code"
            />
            <span class="validation-error">{{ errors.patientPostalCode }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-email">Email</label>
            <input
              id="patient-email"
              v-model="patientEmail"
              v-bind="patientEmailAttrs"
              class="form-control"
              type="text"
              placeholder="Email"
            />
            <span class="validation-error">{{ errors.patientEmail }}</span>
          </div>
          <div class="col-3">
            <label for="patient-phone">Phone</label>
            <input
              id="patient-phone"
              v-model="patientPhone"
              v-bind="patientPhoneAttrs"
              class="form-control"
              type="text"
              placeholder="Phone"
            />
            <span class="validation-error">{{ errors.patientPhone }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="patient-password">Password</label>
            <input
              id="patient-password"
              v-model="patientPassword"
              v-bind="patientPasswordAttrs"
              class="form-control"
              type="password"
              placeholder="Password"
            />
            <span class="validation-error">{{ errors.patientPassword }}</span>
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
    <div class="request-error">{{ errorMessage }}</div>
  </div>
</template>

<script setup lang="ts">
  import { ref, computed } from "vue";
  import { addDoctor, addPatient } from "@/api/userApi";
  import { DoctorDto } from "@/models/DoctorDto";
  import { PatientDto } from "@/models/PatientDto";
  import * as yup from "yup";
  import { useForm } from "vee-validate";

  const isDoctorCreating = ref(false);
  const isPatientCreating = ref(false);
  const errorMessage = ref("");

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
    doctorEmail: yup.string().email("Must be a valid email").required("Email is required"),
    doctorPassword: yup
      .string()
      .matches(
        /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
        "Password must contain at least one lowercase letter, one uppercase letter, one digit, one special character, and be at least 8 characters long"
      )
      .required("Password is required"),
    doctorFirstName: yup.string().max(32, "First name length exceeded").required("First name is required"),
    doctorLastName: yup.string().max(64, "Last name length exceeded").required("Last name is required"),
    doctorTitle: yup.string().max(128, "Title length exceeded").required("Title is required"),
    doctorPhone: yup
      .string()
      .matches(/^[0-9]+$/, "Phone number must be numeric")
      .required("Phone is required"),
  });

  const patientSchema = yup.object({
    patientPesel: yup.string().min(11, "PESEL must be exactly 11 characters").max(11, "PESEL must be exactly 11 characters").required("Pesel is required"),
    patientFirstName: yup.string().max(32, "First name length exceeded").required("First name is required"),
    patientLastName: yup.string().max(64, "Last name length exceeded").required("Last name is required"),
    patientEmail: yup.string().email("Must be a valid email").required("Email is required"),
    patientPhone: yup
      .string()
      .matches(/^[0-9]+$/, "Phone number must be numeric")
      .required("Phone is required"),
    patientCity: yup.string().max(64, "City length exceeded").required("City is required"),
    patientStreet: yup.string().max(64, "Street length exceeded").required("Street is required"),
    patientPostalCode: yup
      .string()
      .matches(/^\d{2}-\d{3}$/, "Postal code must match the format xx-xxx where 'x' is a digit")
      .required("Postal code is required"),
    patientPassword: yup
      .string()
      .matches(
        /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
        "Password must contain at least one lowercase letter, one uppercase letter, one digit, one special character, and be at least 8 characters long"
      )
      .required("Password is required"),
  });

  const validationSchema = computed(() => {
    return isDoctorCreating.value ? doctorSchema : patientSchema;
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: validationSchema,
  });

  const [doctorFirstName, doctorFirstNameAttrs] = defineField("doctorFirstName");
  const [doctorLastName, doctorLastNameAttrs] = defineField("doctorLastName");
  const [doctorTitle, doctorTitleAttrs] = defineField("doctorTitle");
  const [doctorEmail, doctorEmailAttrs] = defineField("doctorEmail");
  const [doctorPhone, doctorPhoneAttrs] = defineField("doctorPhone");
  const [doctorPassword, doctorPasswordAttrs] = defineField("doctorPassword");

  const [patientFirstName, patientFirstNameAttrs] = defineField("patientFirstName");
  const [patientLastName, patientLastNameAttrs] = defineField("patientLastName");
  const [patientPesel, patientPeselAttrs] = defineField("patientPesel");
  const [patientCity, patientCityAttrs] = defineField("patientCity");
  const [patientStreet, patientStreetAttrs] = defineField("patientStreet");
  const [patientPostalCode, patientPostalCodeAttrs] = defineField("patientPostalCode");
  const [patientEmail, patientEmailAttrs] = defineField("patientEmail");
  const [patientPhone, patientPhoneAttrs] = defineField("patientPhone");
  const [patientPassword, patientPasswordAttrs] = defineField("patientPassword");

  const createDoctor = () => {
    errorMessage.value = "";
    isPatientCreating.value = false;
    isDoctorCreating.value = true;
    resetForm();
  };

  const createPatient = () => {
    errorMessage.value = "";
    isDoctorCreating.value = false;
    isPatientCreating.value = true;
    resetForm();
  };

  const createPatientAccount = handleSubmit(async (values) => {
    errorMessage.value = "";
    fillPatientDto();
    await addPatient(patient.value).catch((error) => {
      errorMessage.value = error.response.data;
      console.log(error.response.data);
    });
  });

  const createDoctorAccount = handleSubmit(async (values) => {
    errorMessage.value = "";
    fillDoctorDto();
    await addDoctor(doctor.value).catch((error) => {
      errorMessage.value = error.response.data;
      console.log(error.response.data);
    });
  });

  const fillPatientDto = () => {
    patient.value.FirstName = patientFirstName.value;
    patient.value.LastName = patientLastName.value;
    patient.value.Pesel = patientPesel.value;
    patient.value.City = patientCity.value;
    patient.value.Street = patientStreet.value;
    patient.value.PostalCode = patientPostalCode.value;
    patient.value.Email = patientEmail.value;
    patient.value.Phone = patientPhone.value;
    patient.value.Password = patientPassword.value;
  };

  const fillDoctorDto = () => {
    doctor.value.FirstName = doctorFirstName.value;
    doctor.value.LastName = doctorLastName.value;
    doctor.value.Title = doctorTitle.value;
    doctor.value.Email = doctorEmail.value;
    doctor.value.Phone = doctorPhone.value;
    doctor.value.Password = doctorPassword.value;
  };
</script>
<style scoped>
  .ps-2-5 {
    padding-left: 0.8rem;
  }

  .validation-error {
    color: red;
    font-size: small;
  }

  .request-error {
    color: red;
    font-weight: bold;
  }
</style>
