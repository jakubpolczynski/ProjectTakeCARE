import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/components/home/HomeView.vue";
import AboutView from "@/components/about/AboutView.vue";
import RegisterView from "@/components/register/RegisterView.vue";
import VisitsView from "@/components/visits/VisitsView.vue";
import FormsView from "@/components/forms/FormsView.vue";
import CreateAccountView from "@/components/create account/CreateAccountView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {
      path: "/createaccount",
      name: "createaccount",
      component: CreateAccountView,
    },
    {
      path: "/register",
      name: "register",
      component: RegisterView,
    },
    {
      path: "/about",
      name: "about",
      component: AboutView,
    },
    {
      path: "/patients",
      name: "patients",
      component: RegisterView,
    },
    {
      path: "/visits",
      name: "visits",
      component: VisitsView,
    },
    {
      path: "/forms",
      name: "forms",
      component: FormsView,
    },
  ],
});

export default router;
