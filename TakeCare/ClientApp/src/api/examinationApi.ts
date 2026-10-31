import { ExaminationDto } from "@/models/ExaminationDto";
import { apiClient } from "./client";


export const getDoctorExaminations = async (doctorEmail: string) => {
  return await apiClient.get("/Examination/GetDoctorExaminations", { params: { doctorEmail } });
};

export const getPatientExaminations = async (patientEmail: string) => {
  return await apiClient.get("/Examination/GetPatientExaminations", { params: { patientEmail } });
};

export const downloadExaminationPdf = async (id: number) => {
  if (!Number.isSafeInteger(id) || id <= 0) throw new Error("Invalid examination ID");
  const response = await apiClient.get(`/Examination/${id}/pdf`, { responseType: "blob" });
  if (!(response.data instanceof Blob) || response.data.type.toLowerCase().split(";")[0] !== "application/pdf")
    throw new Error("Invalid PDF response");
  const objectUrl = URL.createObjectURL(response.data);
  const revokeObjectUrl = URL.revokeObjectURL.bind(URL);
  try {
    const link = document.createElement("a");
    link.href = objectUrl;
    link.download = `examination-${id}.pdf`;
    document.body.appendChild(link);
    link.click();
    link.remove();
  } finally {
    // Defer revocation until the browser has started the download.
    window.setTimeout(() => revokeObjectUrl(objectUrl), 1000);
  }
};

export const addExamination = async (examination: ExaminationDto, formData: FormData) => {
  formData.append("id", examination.id.toString());
  formData.append("name", examination.name);
  formData.append("type", examination.type);
  formData.append("description", examination.description);
  formData.append("result", examination.result);
  formData.append("date", examination.date);
  formData.append("doctorEmail", examination.doctorEmail);
  formData.append("patientEmail", examination.patientEmail);
  formData.append("visitId", examination.visitId.toString());
  return await apiClient.post("/Examination/AddExamination", formData);
};
