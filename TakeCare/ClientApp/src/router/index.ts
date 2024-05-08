import { createRouter, createWebHistory } from "vue-router";
import HomeView from "@/components/home/HomeView.vue";
import AboutView from "@/components/about/AboutView.vue";
import VisitsView from "@/components/visits/VisitsView.vue";
import FormsView from "@/components/forms/FormsView.vue";
import CreateAccountView from "@/components/create_account/CreateAccountView.vue";
import LoginView from "@/components/login/LoginView.vue";

const router = createRouter({
  history: createWebHistory(),
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
      path: "/login",
      name: "login",
      component: LoginView,
    },
    {
      path: "/about",
      name: "about",
      component: AboutView,
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

router.beforeEach((to, from, next) => {
  const publicPages = ["/login", "/createaccount"];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("token");

  if (authRequired && !loggedIn) {
    return next("/login");
  } else {
    next();
  }

  if (authRequired) {
    next("/login");
  } else {
    next();
  }
});

export default router;
