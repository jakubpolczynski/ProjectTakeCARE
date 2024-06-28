import { createRouter, createWebHistory } from "vue-router";
import { store } from "@/store/store";
import HomeView from "@/views/HomeView.vue";
import FormsView from "@/views/FormsView.vue";
import CreateAccountView from "@/views/CreateAccountView.vue";
import LoginView from "@/views/LoginView.vue";
import PatientsView from "@/views/PatientsView.vue";
import DoctorVisitsView from "@/views/DoctorVisitsView.vue";
import PatientVisitsView from "@/views/PatientVisitsView.vue";
import OngoingExaminationView from "@/views/OngoingExaminationView.vue";

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
  ],
});

router.beforeEach((to, from, next) => {
  const requiresAuth = to.matched.some((record) => record.meta.requiresAuth);
  const requiresDoctor = to.matched.some((record) => record.meta.requiresDoctor);
  const requiresPatient = to.matched.some((record) => record.meta.requiresPatient);
  const isAuthenticated = store.state.authenticated;

  if (requiresAuth && !isAuthenticated) {
    next({ name: "login" });
  } else if (requiresDoctor && store.state.user !== "Doctor") {
    next({ name: "home" });
  } else if (requiresPatient && store.state.user !== "Patient") {
    next({ name: "home" });
  } else {
    next();
  }
});

export default router;
