import { ExaminationDto } from "@/models/ExaminationDto";
import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5184/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

export const getDoctorExaminations = async (doctorEmail: string) => {
  return await apiClient.get("/Examination/GetDoctorExaminations?doctorEmail=" + doctorEmail);
};

export const getPatientExaminations = async (patientEmail: string) => {
  return await apiClient.get("/Examination/GetPatientExaminations?patientEmail=" + patientEmail);
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
  examination.images.forEach((image) => {
    formData.append("images", image);
  });

  return await apiClient.post("/Examination/AddExamination", formData, {
    headers: {
      "Content-Type": "multipart/form-data",
    },
  });
};
