<template>
  <div>
    <h1>Choose role</h1>
    <div class="row ps-2-5">
      <button
        @click="createPatient"
        class="btn btn-primary me-3 col-2"
        :class="[isPatientCreating ? 'disabled btn-secondary' : '']"
      >
        Patient
      </button>
      <button
        @click="createDoctor"
        class="btn btn-primary me-3 col-2"
        :class="[isDoctorCreating ? 'disabled btn-secondary' : '']"
      >
        Doctor
      </button>
      <button
        @click="createReceptionist"
        class="btn btn-primary me-3 col-2"
        :class="[isReceptionistCreating ? 'disabled btn-secondary' : '']"
      >
        Receptionist
      </button>
    </div>

    <!-- Doctor Form -->
    <div v-if="isDoctorCreating">
      <span>Create doctor account</span>
      <form
        class="form-group"
        @submit="createDoctorAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="doctor-first-name">First name</label>
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
            <label for="doctor-specialization">Medical specialization</label>
            <SearchableSelect
              v-model="selectedSpecialty"
              :options="specialties"
            />
            <span class="validation-error">{{ specializationError }}</span>
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
              type="email"
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

    <!-- Patient Form -->
    <div v-if="isPatientCreating">
      <span>Create patient account</span>
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
              type="email"
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

    <!-- Receptionist Form -->
    <div v-if="isReceptionistCreating">
      <span>Create receptionist account</span>
      <form
        class="form-group"
        @submit="createReceptionistAccount"
      >
        <div class="row mt-3">
          <div class="col-3">
            <label for="receptionist-name">First name</label>
            <input
              id="receptionist-name"
              v-model="receptionistFirstName"
              v-bind="receptionistFirstNameAttrs"
              class="form-control"
              type="text"
              placeholder="Name"
            />
            <span class="validation-error">{{ errors.receptionistFirstName }}</span>
          </div>
          <div class="col-3">
            <label for="receptionist-surname">Last name</label>
            <input
              id="receptionist-surname"
              v-model="receptionistLastName"
              v-bind="receptionistLastNameAttrs"
              class="form-control"
              type="text"
              placeholder="Surname"
            />
            <span class="validation-error">{{ errors.receptionistLastName }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="receptionist-email">Email</label>
            <input
              id="receptionist-email"
              v-model="receptionistEmail"
              v-bind="receptionistEmailAttrs"
              class="form-control"
              type="email"
              placeholder="Email"
            />
            <span class="validation-error">{{ errors.receptionistEmail }}</span>
          </div>
        </div>
        <div class="row mt-3">
          <div class="col-3">
            <label for="receptionist-password">Password</label>
            <input
              id="receptionist-password"
              v-model="receptionistPassword"
              v-bind="receptionistPasswordAttrs"
              class="form-control"
              type="password"
              placeholder="Password"
            />
            <span class="validation-error">{{ errors.receptionistPassword }}</span>
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
  import * as yup from "yup";
  import { useForm } from "vee-validate";
  import { useRouter } from "vue-router";
  import { AxiosError } from "axios";

  import { addDoctor, addPatient, addReceptionist } from "@/api/userApi";

  import { DoctorDto } from "@/models/DoctorDto";
  import { PatientDto } from "@/models/PatientDto";
  import { ReceptionistDto } from "@/models/ReceptionistDto";
  import { MedicalSpecialties } from "@/enums/MedicalSpecialties";

  import SearchableSelect from "@/components/SearchableSelect.vue";

  const router = useRouter();

  const isDoctorCreating = ref(false);
  const isPatientCreating = ref(false);
  const isReceptionistCreating = ref(false);
  const errorMessage = ref("");
  const specializationError = ref("");
  const selectedSpecialty = ref("");

  const patient = ref<PatientDto>({
    pesel: "",
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    city: "",
    street: "",
    postalCode: "",
    password: "",
    role: "Patient",
  });

  const receptionist = ref<ReceptionistDto>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    role: "Receptionist",
  });

  const doctor = ref<DoctorDto>({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    specialization: "",
    phone: "",
    role: "Doctor",
  });

  const specialties = ref(
    Object.values(MedicalSpecialties).map((specialty) => ({
      value: specialty,
      label: specialty,
    }))
  );

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

  const receptionistSchema = yup.object({
    receptionistFirstName: yup.string().max(32, "First name length exceeded").required("First name is required"),
    receptionistLastName: yup.string().max(64, "Last name length exceeded").required("Last name is required"),
    receptionistEmail: yup.string().email("Must be a valid email").required("Email is required"),
    receptionistPassword: yup
      .string()
      .matches(
        /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/,
        "Password must contain at least one lowercase letter, one uppercase letter, one digit, one special character, and be at least 8 characters long"
      )
      .required("Password is required"),
  });

  const validationSchema = computed(() => {
    if (isDoctorCreating.value) {
      return doctorSchema;
    } else if (isPatientCreating.value) {
      return patientSchema;
    } else if (isReceptionistCreating.value) {
      return receptionistSchema;
    }
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: validationSchema,
  });

  const [doctorFirstName, doctorFirstNameAttrs] = defineField("doctorFirstName");
  const [doctorLastName, doctorLastNameAttrs] = defineField("doctorLastName");
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

  const [receptionistFirstName, receptionistFirstNameAttrs] = defineField("receptionistFirstName");
  const [receptionistLastName, receptionistLastNameAttrs] = defineField("receptionistLastName");
  const [receptionistEmail, receptionistEmailAttrs] = defineField("receptionistEmail");
  const [receptionistPassword, receptionistPasswordAttrs] = defineField("receptionistPassword");

  const createDoctor = () => {
    errorMessage.value = "";
    isReceptionistCreating.value = false;
    isPatientCreating.value = false;
    isDoctorCreating.value = true;
    resetForm();
  };

  const createPatient = () => {
    errorMessage.value = "";
    isReceptionistCreating.value = false;
    isDoctorCreating.value = false;
    isPatientCreating.value = true;
    resetForm();
  };

  const createReceptionist = () => {
    errorMessage.value = "";
    isReceptionistCreating.value = true;
    isPatientCreating.value = false;
    isDoctorCreating.value = false;
    resetForm();
  };

  const createPatientAccount = handleSubmit(async () => {
    errorMessage.value = "";
    fillPatientDto();
    try {
      await addPatient(patient.value);
      await router.push("/login");
    } catch (error) {
      if (error instanceof AxiosError) {
        errorMessage.value = error.response.data;
      }
    }
  });

  const createDoctorAccount = handleSubmit(async () => {
    errorMessage.value = "";
    if (checkSpecialty()) {
      fillDoctorDto();
      try {
        await addDoctor(doctor.value);
        await router.push("/login");
      } catch (error) {
        if (error instanceof AxiosError) {
          errorMessage.value = error.response.data;
        }
      }
    }
  });

  const createReceptionistAccount = handleSubmit(async () => {
    errorMessage.value = "";
    fillReceptionistDto();
    try {
      await addReceptionist(receptionist.value);
      await router.push("/login");
    } catch (error) {
      if (error instanceof AxiosError) {
        errorMessage.value = error.response.data;
      }
    }
  });

  const fillReceptionistDto = () => {
    receptionist.value.firstName = receptionistFirstName.value;
    receptionist.value.lastName = receptionistLastName.value;
    receptionist.value.email = receptionistEmail.value;
    receptionist.value.password = receptionistPassword.value;
    receptionist.value.role = "Receptionist";
  };

  const fillPatientDto = () => {
    patient.value.firstName = patientFirstName.value;
    patient.value.lastName = patientLastName.value;
    patient.value.pesel = patientPesel.value;
    patient.value.city = patientCity.value;
    patient.value.street = patientStreet.value;
    patient.value.postalCode = patientPostalCode.value;
    patient.value.email = patientEmail.value;
    patient.value.phone = patientPhone.value;
    patient.value.password = patientPassword.value;
  };

  const fillDoctorDto = () => {
    doctor.value.firstName = doctorFirstName.value;
    doctor.value.lastName = doctorLastName.value;
    doctor.value.specialization = specialties.value.find((specialty) => specialty.value === selectedSpecialty.value)?.label;
    doctor.value.email = doctorEmail.value;
    doctor.value.phone = doctorPhone.value;
    doctor.value.password = doctorPassword.value;
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

  .request-ok {
    color: green;
    font-weight: bold;
  }
</style>
