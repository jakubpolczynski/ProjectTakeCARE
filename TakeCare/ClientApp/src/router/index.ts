import { createRouter, createWebHistory } from "vue-router";
import { store } from "@/store/store";
import { getUser } from "@/api/loginApi";
import HomeView from "@/views/HomeView.vue";
import FormsView from "@/views/FormsView.vue";
import CreateAccountView from "@/views/CreateAccountView.vue";
import LoginView from "@/views/LoginView.vue";
import PatientsView from "@/views/PatientsView.vue";
import DoctorVisitsView from "@/views/DoctorVisitsView.vue";
import PatientVisitsView from "@/views/PatientVisitsView.vue";
import PatientExaminationView from "@/views/PatientExaminationView.vue";
import OngoingExaminationView from "@/views/OngoingExaminationView.vue";
import DoctorExaminationView from "@/views/DoctorExaminationView.vue";
import ReceptionistVisitsView from "@/views/ReceptionistVisitsView.vue";

const router = createRouter({
  history: createWebHistory(),
  linkActiveClass: "active",
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
      meta: { requiresAuth: true },
    },
    {
      path: "/login",
      name: "login",
      component: LoginView,
    },
    {
      path: "/createaccount",
      name: "createaccount",
      component: CreateAccountView,
    },
    {
      path: "/doctorvisits",
      name: "doctorvisits",
      component: DoctorVisitsView,
      meta: { requiresAuth: true, requiresDoctor: true },
    },
    {
      path: "/forms",
      name: "forms",
      component: FormsView,
      meta: { requiresAuth: true, requiresDoctor: true },
    },
    {
      path: "/patients",
      name: "patients",
      component: PatientsView,
      meta: { requiresAuth: true, requiresDoctor: true },
    },
    {
      path: "/patientvisits",
      name: "patientvisits",
      component: PatientVisitsView,
      meta: { requiresAuth: true, requiresPatient: true },
    },
    {
      path: "/ongoingexamination/:visitId",
      name: "ongoingexamination",
      component: OngoingExaminationView,
      meta: { requiresAuth: true, requiresDoctor: true },
    },
    {
      path: "/patientExaminations/:date",
      name: "patientExaminations",
      component: PatientExaminationView,
      meta: { requiresAuth: true, requiresPatient: true },
    },
    {
      path: "/doctorExaminations/:patient/:date",
      name: "doctorExaminations",
      component: DoctorExaminationView,
      meta: { requiresAuth: true, requiresDoctor: true },
    },
    {
      path: "/receptionistVisits",
      name: "receptionistVisits",
      component: ReceptionistVisitsView,
      meta: { requiresAuth: true, requiresReceptionist: true },
    },
  ],
});

router.beforeEach(async (to) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  const requiresDoctor = to.matched.some((record) => record.meta.requiresDoctor);
  const requiresPatient = to.matched.some((record) => record.meta.requiresPatient);
  const requiresReceptionist = to.matched.some((record) => record.meta.requiresReceptionist);
  // The server validates the cookie, including expiry and current role, on every navigation.
  try {
    const { data } = await getUser();
    if (!data || typeof data.role !== "string" || typeof data.email !== "string") throw new Error("Invalid session");
    await store.dispatch("setAuth", true);
    await store.dispatch("setUser", data.role);
    await store.dispatch("setEmail", data.email);
  } catch {
    await store.dispatch("setAuth", false);
    await store.dispatch("setUser", "");
    await store.dispatch("setEmail", "");
  }

  if (requiresAuth && !store.state.authenticated) {
    return { name: "login" };
  } else if (requiresDoctor && store.state.user !== "Doctor") {
    return { name: "home" };
  } else if (requiresPatient && store.state.user !== "Patient") {
    return { name: "home" };
  } else if (requiresReceptionist && store.state.user !== "Receptionist") {
    return { name: "home" };
  }
  return true;
});

export default router;
