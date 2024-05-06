<template>
  <header class="p-3 bg-dark text-white">
    <div class="container">
      <div class="d-flex flex-wrap align-items-center justify-content-center justify-content-lg-start">
        <span class="d-flex align-items-center mb-2 mb-lg-0 text-white text-decoration-none">
          <ThemeIcon />
        </span>
        <ul class="nav col-12 col-lg-auto me-lg-auto mb-2 justify-content-center mb-md-0">
          <NavLink
            link="/"
            text="Home"
          />
          <NavLink
            link="/visits"
            text="Visits"
          />
          <NavLink
            link="/patients"
            text="Patients"
          />
          <NavLink
            link="/forms"
            text="Forms"
          />
          <NavLink
            link="/about"
            text="About"
          />
        </ul>
        <div class="text-end">
          <router-link to="/createaccount">
            <button
              type="button"
              class="btn btn-light"
            >
              Create account
            </button>
          </router-link>
        </div>
      </div>
    </div>
  </header>
</template>
<script setup lang="ts">
  import { onMounted, ref } from "vue";
  import NavLink from "@/components/main/NavLink.vue";
  import ThemeIcon from "@/components/Icons/ThemeIcon.vue";

  onMounted(() => {
    isLoading.value = false;
  });

  const isLoading = ref(true);
</script>
<style scoped lang="scss"></style>
