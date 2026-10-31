<template>
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5
          class="modal-title"
          id="exmainationSavedModalLabel"
        >
          Examination saved
        </h5>
        <button
          type="button"
          class="btn-close"
          data-bs-dismiss="modal"
          aria-label="Close"
        ></button>
      </div>
      <div class="modal-body">
        <p>Examination saved successfully.</p>
        <p v-if="deliveryStatus === 'submitted'">The email was submitted to the provider. Delivery is not guaranteed.</p>
        <p v-else-if="deliveryStatus === 'failed'" class="text-danger">The examination was saved, but the email could not be sent.</p>
        <p v-else class="text-muted">Email delivery is disabled or unavailable.</p>
        <p class="text-muted">The generated PDF is saved with the examination.</p>
      </div>
      <div class="modal-footer">
        <button
          type="button"
          class="btn btn-success"
          @click="goBack()"
          data-bs-dismiss="modal"
        >
          Continue
        </button>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
  import { ref } from "vue";
  import { useRouter } from "vue-router";

  defineProps<{ deliveryStatus: string }>();

  const router = useRouter();
  const modalVisible = ref<boolean>();

  const goBack = async () => {
    modalVisible.value = false;
    await router.push({ name: "doctorvisits" });
  };
</script>
