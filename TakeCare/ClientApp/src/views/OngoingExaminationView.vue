<template>
  <div>
    <h1>Ongoing Examination</h1>
    <div v-if="errorMessage !== ''">
      <p class="text-danger">{{ errorMessage }}</p>
    </div>
    <div v-else-if="loadingEnded">
      <div class="border rounded mb-2 p-2">
        <h5>Visit details</h5>
        <div><strong>Patient: </strong>{{ visit.patientFirstName }} {{ visit.patientLastName }}</div>
        <div><strong>Patient Email: </strong>{{ visit.patientEmail }}</div>
        <div><strong>Reason: </strong> {{ visit.reason }}</div>
        <div><strong>Slot: </strong> {{ formattedSlot(visit).date }} {{ formattedSlot(visit).time }}</div>
        <div><strong>Doctor: </strong>{{ visit.doctorFirstName }} {{ visit.doctorLastName }} ({{ visit.doctorSpecialization }})</div>
        <div><strong>Doctor Email: </strong>{{ visit.doctorEmail }}</div>
      </div>
      <div>
        <form class="row">
          <div class="col-6">
            <label
              for="name"
              class="form-label required"
              >Examination name</label
            >
            <input
              v-model="name"
              v-bind="nameAttrs"
              id="name"
              name="name"
              class="form-control"
              type="text"
            />
            <div class="validation-error">{{ errors.name }}</div>
          </div>
          <div class="col-6">
            <label
              for="type"
              class="form-label"
              >Examination type</label
            >
            <input
              v-model="type"
              v-bind="typeAttrs"
              id="type"
              name="type"
              class="form-control"
              type="text"
            />
            <div class="validation-error">{{ errors.type }}</div>
          </div>
          <div class="col-6 mt-4">
            <label
              for="description"
              class="form-label"
              >Examination description</label
            >
            <textarea
              v-model="description"
              v-bind="descriptionAttrs"
              id="description"
              name="description"
              class="form-control"
              type="textarea"
            ></textarea>
            <div class="validation-error">{{ errors.description }}</div>
          </div>
          <div class="col-6 mt-4">
            <label
              for="result"
              class="form-label"
              >Examination result</label
            >
            <textarea
              v-model="result"
              v-bind="resultAttrs"
              id="result"
              name="result"
              class="form-control"
              type="textarea"
            ></textarea>
            <div class="validation-error">{{ errors.result }}</div>
          </div>
          <div class="col-12 mt-4">
            <label
              for="image"
              class="form-label"
              >Upload Images</label
            >
            <input
              type="file"
              id="image"
              name="image"
              class="form-control"
              @change="handleImageUpload"
              multiple
              accept=".jpg, .jpeg, .png"
            />
            <div class="validation-error">{{ errorMessage }}</div>
            <div
              v-if="imagePreviews.length > 0"
              class="mt-2"
            >
              <h6>Image Previews:</h6>
              <div
                v-for="(image, index) in imagePreviews"
                :key="index"
                class="mb-2"
              >
                <img
                  :src="image"
                  alt="Image Preview"
                  class="img-thumbnail"
                />
              </div>
            </div>
          </div>
          <div class="col-12 mt-4 d-flex justify-content-end">
            <button
              class="btn btn-primary me-2"
              @click="save"
            >
              Save
            </button>
            <button
              class="btn btn-outline-primary"
              @click="preview"
            >
              Preview
            </button>
          </div>
        </form>
        <div class="request-error">{{ errorMessage }}</div>
      </div>
    </div>
    <div
      id="modal1"
      class="modal fade"
    >
      <SuccessExaminationModal></SuccessExaminationModal>
    </div>
    <div
      id="modal2"
      class="modal fade"
    >
      <PdfPreviewModal
        v-if="previewVisible"
        :executed-visit="executedVisit"
      ></PdfPreviewModal>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, nextTick } from "vue";
  import * as yup from "yup";
  import { useForm } from "vee-validate";
  import { useRoute } from "vue-router";
  import { AxiosError } from "axios";
  import { Modal } from "bootstrap";
  import html2pdf from "html2pdf.js";

  import PdfPreviewModal from "@/modals/PdfPreviewModal.vue";
  import SuccessExaminationModal from "@/modals/SuccessExaminationModal.vue";

  import { VisitDto } from "@/models/VisitDto";
  import { ExaminationDto } from "@/models/ExaminationDto";

  import { getVisit } from "@/api/visitsApi";
  import { addExamination } from "@/api/examinationApi";

  const routerParams = useRoute();
  const visitId = ref<number>();
  const visit = ref<VisitDto>();
  const loadingEnded = ref(false);
  const errorMessage = ref("");
  const examination = ref<ExaminationDto>();
  const executedVisit = ref<ExecutedVisitDto>();
  const previewVisible = ref<boolean>(false);
  const imageFiles = ref<File[]>([]);
  const imagePreviews = ref<string[]>([]);
  const base64Images = ref<string[]>([]);

  interface ExecutedVisitDto {
    examination: ExaminationDto;
    visit: VisitDto;
    imagePreviews?: string[];
  }

  onMounted(async () => {
    await getVisitsData();
    loadingEnded.value = true;
  });

  const examinationSchema = yup.object({
    name: yup.string().required("Name is required").max(100, "Name must be at most 100 characters"),
    type: yup.string().required("Type is required").max(50, "Type must be at most 50 characters"),
    description: yup.string().required("Description is required").max(500, "Description must be at most 500 characters"),
    result: yup.string().required("Result is required").max(500, "Result must be at most 500 characters"),
  });

  const { errors, handleSubmit, defineField, resetForm } = useForm({
    validationSchema: examinationSchema,
  });

  const [name, nameAttrs] = defineField("name");
  const [type, typeAttrs] = defineField("type");
  const [description, descriptionAttrs] = defineField("description");
  const [result, resultAttrs] = defineField("result");

  const getVisitsData = async () => {
    visitId.value = Number(routerParams.params.visitId);
    if (Number.isNaN(visitId.value)) {
      errorMessage.value = "Invalid visit ID";
    } else {
      try {
        const response = await getVisit(visitId.value);
        visit.value = response.data;
      } catch (error) {
        if (error instanceof AxiosError && error.response) {
          errorMessage.value = error.response.data;
        } else {
          errorMessage.value = "An unexpected error occurred";
        }
      }
    }
  };

  // Function to convert image to Base64
  const convertImageToBase64 = (file: File): Promise<string> => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = () => {
        resolve(reader.result as string);
      };
      reader.onerror = reject;
      reader.readAsDataURL(file);
    });
  };

  const handleImageUpload = async (event: Event) => {
    const files = (event.target as HTMLInputElement).files;
    const validTypes = ["image/jpeg", "image/png"]; // Allowed file types
    const errorMessages: string[] = [];

    if (files) {
      const selectedFiles = Array.from(files);

      // Filter out valid image files and generate error messages for invalid ones
      const validFiles = selectedFiles.filter((file) => {
        if (!validTypes.includes(file.type)) {
          errorMessages.push(`${file.name} is not a valid image file. Only JPG and PNG are allowed.`);
          return false;
        }
        return true;
      });

      // Set the valid image files
      imageFiles.value = validFiles;

      // Convert images to Base64 and set previews
      imagePreviews.value = validFiles.map((file) => URL.createObjectURL(file));
      base64Images.value = await Promise.all(validFiles.map((file) => convertImageToBase64(file)));

      // Display error messages if there are invalid files
      if (errorMessages.length > 0) {
        errorMessage.value = errorMessages.join(" ");
      } else {
        errorMessage.value = "";
      }
    }
  };

  const fillExecutedVisit = () => {
    executedVisit.value = {
      examination: examination.value,
      visit: visit.value,
      imagePreviews: base64Images.value,
    };
    console.log(executedVisit.value);
  };

  const preview = handleSubmit(async (values) => {
    previewVisible.value = true;
    fillExamination(values);
    fillExecutedVisit();
    await nextTick();
    showPreview();
  });

  const createPDF = async () => {
    await nextTick();

    fillExecutedVisit();

    const modalBodyElement = document.querySelector("#modal2 .modal-body");
    if (modalBodyElement) {
      const pdfBlob = await html2pdf().from(modalBodyElement).save().outputPdf("blob");

      const formData = new FormData();
      formData.append("file", pdfBlob, `Examination-${examination.value.id}.pdf`);

      imageFiles.value.forEach((file, index) => {
        formData.append(`image_${index}`, file);
      });

      return formData;
    } else {
      throw new Error("Modal body element not found");
    }
  };

  const save = handleSubmit(async (values) => {
    previewVisible.value = true;
    errorMessage.value = "";
    try {
      fillExamination(values);
      fillExecutedVisit();

      await nextTick();
      showPreview();

      const formData = await createPDF();

      if (examination.value && Object.keys(examination.value).length > 0) {
        await addExamination(examination.value, formData);
      } else {
        throw new Error("Invalid examination data");
      }

      resetForm();
      showModal();
    } catch (error) {
      if (error instanceof AxiosError && error.response) {
        errorMessage.value = error.response.data;
      } else {
        console.error(error);
        errorMessage.value = "An unexpected error occurred";
      }
    }
  });

  const showPreview = () => {
    const modalElement = document.getElementById("modal2");
    if (modalElement) {
      const myModal = new Modal(modalElement);
      myModal.show();
    } else {
      console.error("Modal element not found");
    }
  };

  const showModal = () => {
    const modalElement = document.getElementById("modal1");
    if (modalElement) {
      const myModal = new Modal(modalElement);
      myModal.show();
    } else {
      console.error("Modal element not found");
    }
  };

  const fillExamination = (values) => {
    if (visit.value) {
      examination.value = {
        id: visit.value.id,
        name: values.name,
        type: values.type,
        description: values.description,
        result: values.result,
        date: new Date().toISOString(),
        doctorEmail: visit.value.doctorEmail || "",
        patientEmail: visit.value.patientEmail || "",
        visitId: visit.value.id,
        images: base64Images.value,
      };
    } else {
      errorMessage.value = "Visit details are missing, cannot save examination.";
    }
  };

  const formattedSlot = (bookedVisit: VisitDto) => {
    if (bookedVisit.slot) {
      const [date, time] = bookedVisit.slot.split("T");
      const [hour, minute] = time.split(":");
      return {
        date,
        time: `${hour}:${minute}`,
      };
    }
    return { date: "", time: "" };
  };
</script>

<style scoped>
  .validation-error {
    color: red;
    font-size: small;
  }
  .request-error {
    color: red;
    font-weight: bold;
  }
</style>
