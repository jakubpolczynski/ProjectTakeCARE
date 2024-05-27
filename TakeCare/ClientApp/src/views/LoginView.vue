<template>
  <div class="d-flex justify-content-center">
    <div class="d-felx flex-column align-items-start">
      <form
        class="form-group"
        @submit="login"
      >
        <ThemeIcon class="mb-2" />
        <span class="h3 ms-2 mb-3 fw-normal">Please sign in</span>

        <div class="mt-2">
          <label for="email"> Email </label>
          <input
            id="email"
            type="email"
            class="form-control"
            v-model="email"
            v-bind="emailAttrs"
            placeholder="Email address"
          />
          <span class="validation-error">{{ errors.email }}</span>
        </div>
        <div class="mt-2">
          <label for="password"> Password </label>
          <input
            id="password"
            type="password"
            class="form-control"
            v-model="password"
            v-bind="passwordAttrs"
            placeholder="Password"
          />
          <span class="validation-error">{{ errors.password }}</span>
        </div>
        <div class="mt-2">
          <button
            type="submit"
            class="btn btn-primary w-100"
          >
            Sign in
          </button>
        </div>
      </form>
      <div class="mt-2 w-100">
        <button class="btn btn-secondary w-100">
          <NavLink
            link="/createaccount"
            text="Create account"
          />
        </button>
        <span class="text-danger">{{ errorMessage }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from "vue";
  import ThemeIcon from "@/Icons/ThemeIcon.vue";
  import NavLink from "@/components/NavLink.vue";
  import { LoginDto } from "@/models/LoginDto";
  import * as yup from "yup";
  import { useForm } from "vee-validate";
  import { loginUser, getUser } from "@/api/loginApi";
  import { store } from "@/store/store";
  import { useRouter } from "vue-router";
  import { AxiosError } from "axios";

  const router = useRouter();

  const errorMessage = ref("");

  const user = ref<LoginDto>({
    email: "",
    password: "",
  });

  const userSchema = yup.object({
    email: yup.string().email("Must be a valid email").required("Email is required"),
    password: yup.string().required("Password is required"),
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: userSchema,
  });

  const [email, emailAttrs] = defineField("email");
  const [password, passwordAttrs] = defineField("password");

  const login = handleSubmit(async (values) => {
    errorMessage.value = "";
    fillLoginDto();
    try {
      await loginUser(user.value);
      const response = await getUser();
      await store.dispatch("setAuth", true);
      await store.dispatch("setUser", response.data.role);
      await store.dispatch("setEmail", response.data.email);
      resetForm();
      await router.push("/");
    } catch (error) {
      if (error instanceof AxiosError && error.response) {
        errorMessage.value = error.response.data;
      } else {
        errorMessage.value = "An unexpected error occurred";
      }
      await store.dispatch("setAuth", false);
      await store.dispatch("setUser", "");
      await store.dispatch("setEmail", "");
    }
  });

  const fillLoginDto = () => {
    user.value.email = email.value;
    user.value.password = password.value;
  };
</script>

<style scoped>
  .validation-error {
    color: red;
    font-size: small;
  }
</style>
