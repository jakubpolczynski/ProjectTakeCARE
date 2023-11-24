import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/components/home/HomeView.vue";
import AboutView from "@/components/about/AboutView.vue";
import RegisterView from "@/components/register/RegisterView.vue";
import VisitsView from "@/components/visits/VisitsView.vue";
import FormsView from "@/components/forms/FormsView.vue";
import LoginView from "@/components/login/LoginView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView
    },
    {
      path: "/login",
      name: "login",
      component: LoginView
    },
    {
      path: "/register",
      name: "register",
      component: RegisterView
    },
    {
      path: "/about",
      name: "about",
      component: AboutView
    },
    {
      path: "/patients",
      name: "patients",
      component: RegisterView
    },
    {
      path: "/visits",
      name: "visits",
      component: VisitsView
    },
    {
      path: "/forms",
      name: "forms",
      component: FormsView
    }
  ]
});

export default router;
