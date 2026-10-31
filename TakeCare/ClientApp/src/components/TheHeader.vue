<template>
  <header class="p-3 bg-dark text-white">
    <div class="container">
      <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
        <span class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none">
          <ThemeIcon />
        </span>
        <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
          <NavLink
            v-if="auth"
            link="/"
            text="Home"
          />
          <NavLink
            v-if="auth && userRole === 'Doctor'"
            link="/doctorvisits"
            text="Visits"
          />
          <NavLink
            v-if="auth && userRole === 'Patient'"
            link="/patientvisits"
            text="Visits"
          />
          <NavLink
            v-if="auth && userRole === 'Receptionist'"
            link="/receptionistvisits"
            text="Visits"
          />
          <NavLink
            v-if="auth && userRole === 'Doctor'"
            link="/patients"
            text="Patients"
          />
        </ul>
        <span v-if="logoutError" class="text-warning">{{ logoutError }}</span>
        <ul class="nav d-flex justify-content-end">
          <NavLink
            v-if="auth && userRole === 'Admin'"
            link="/createaccount"
            text="Create staff account"
          />
          <NavLink
            v-if="!auth"
            link="/login"
            text="Login"
          >
          </NavLink>
          <NavLink
            v-if="!auth"
            link="/createaccount"
            text="Create account"
          >
          </NavLink>
          <span
            v-if="auth"
            class="px-2 text-white text-decoration-none nav-link"
            role="button"
            @click="logout"
          >
            Logout
          </span>
        </ul>
      </div>
    </div>
  </header>
</template>
<script setup lang="ts">
  import { computed, ref } from "vue";
  import { AxiosError } from "axios";
  import { useRouter } from "vue-router";
  import { useStore } from "vuex";

  import { logoutUser } from "@/api/loginApi";

  import NavLink from "@/components/NavLink.vue";
  import ThemeIcon from "@/Icons/ThemeIcon.vue";

  const router = useRouter();
  const store = useStore();
  const logoutError = ref("");
  const auth = computed(() => {
    return store.state.authenticated;
  });
  const userRole = computed(() => {
    return store.state.user;
  });

  const logout = async () => {
    logoutError.value = "";
    try {
      await logoutUser();
      store.dispatch("setAuth", false);
      store.dispatch("setUser", "");
      store.dispatch("setEmail", "");
      await router.push("/login");
    } catch (error) {
      if (error instanceof AxiosError && error.response?.status === 401) {
        await store.dispatch("setAuth", false);
        await store.dispatch("setUser", "");
        await store.dispatch("setEmail", "");
        await router.push("/login");
      } else {
        logoutError.value = "Could not sign out. Please try again.";
      }
    }
  };
</script>
<style scoped>
  .nav-link:hover {
    background-color: rgba(255, 255, 255, 0.05);
  }
</style>
